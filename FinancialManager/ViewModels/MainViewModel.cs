using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinancialManager.Data.Contracts;
using FinancialManager.Helpers;
using FinancialManager.Models;
using FinancialManager.Services.Contracts;
using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FinancialManager.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITransactionTypeRepository _typeRepository;
    private readonly ILocalizationRepository _localizationRepository;
    private readonly ILocalizationService _localizationService;
    private readonly ICurrencyService _currencyService;
    private readonly IFeatureRepository _featureRepository;
    private readonly IExportService _exportService;

    [ObservableProperty] private string currentBalance = StaticData.BalancePlaceholderUah;
    [ObservableProperty] private string totalIncome = StaticData.BalancePlaceholderUah;
    [ObservableProperty] private string totalExpenses = StaticData.BalancePlaceholderUah;
    [ObservableProperty] private string totalSavings = StaticData.BalancePlaceholderUah;
    [ObservableProperty] private string totalOthers = StaticData.BalancePlaceholderUah;

    [ObservableProperty] private DateTime selectedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    [ObservableProperty] private string selectedMonthDisplay = string.Empty;
    [ObservableProperty] private bool canGoNextMonth;

    [ObservableProperty] private DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    [ObservableProperty] private DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

    [ObservableProperty] private ObservableCollection<TransactionType> transactionTypes = new();
    [ObservableProperty] private ObservableCollection<Category> categories = new();
    [ObservableProperty] private ObservableCollection<Transaction> filteredTransactions = new();
    [ObservableProperty] private List<ChartLegendItem> chartLegendItems = new();
    [ObservableProperty] private string displayCurrency = StaticData.UahCurrency;
    [ObservableProperty] private bool isCurrencySelectionEnabled = true;
    [ObservableProperty] private string _selectedSortOption;
    [ObservableProperty] private ChartLegendItem selectedCategory;
    [ObservableProperty] private bool isCategoryFilterActive;
    [ObservableProperty] private bool hasChartData;
    [ObservableProperty] private bool isExportFeatureEnabled;

    private List<Transaction> _allTransactions = new();
    private Dictionary<string, double> _currentRates = new() { { StaticData.UahCurrency, 1.0 } };
    private List<Transaction> _fullyFilteredTransactions = new();
    private Chart _categoryChart;

    public Chart CategoryChart
    {
        get => _categoryChart;
        set
        {
            _categoryChart = value;
            OnPropertyChanged();
        }
    }

    public List<string> AvailableDisplayCurrencies { get; } = new() { StaticData.UahCurrency, StaticData.UsdCurrency, StaticData.EurCurrency };
    public List<string> SortOptions { get; } = new()
    {
        Resources.Strings.SortOption_DateNewest,
        Resources.Strings.SortOption_DateOldest,
        Resources.Strings.SortOption_PriceLower,
        Resources.Strings.SortOption_PriceHigher
    };

    public MainViewModel(
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository,
        ITransactionTypeRepository typeRepository,
        ILocalizationRepository localizationRepository,
        ILocalizationService localizationService,
        ICurrencyService currencyService,
        IFeatureRepository featureRepository,
        IExportService exportService)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _typeRepository = typeRepository;
        _localizationRepository = localizationRepository;
        _localizationService = localizationService;
        _currencyService = currencyService;
        _featureRepository = featureRepository;
        _exportService = exportService;

        SelectedSortOption = SortOptions[0];
        UpdateMonthState();
    }

    public async Task InitializeAsync()
    {
        var types = await _typeRepository.GetAsync();
        var cats = await _categoryRepository.GetAsync();
        var locs = await _localizationRepository.GetAsync();

        IsExportFeatureEnabled = await _featureRepository.IsFeatureEnabledAsync("ExportData");

        try
        {
            _currentRates = await _currencyService.GetLatestRatesAsync();
            IsCurrencySelectionEnabled = _currentRates != null && _currentRates.Count > 1;
        }
        catch (Exception)
        {
            _currentRates = new Dictionary<string, double> { { StaticData.UahCurrency, 1.0 } };
            IsCurrencySelectionEnabled = false;
        }

        string currentLang = _localizationService.CurrentLanguage;

        foreach (var type in types)
        {
            var loc = locs.FirstOrDefault(l => l.ParentId == type.Id && l.LanguageCode == currentLang)
                      ?? locs.FirstOrDefault(l => l.ParentId == type.Id && l.LanguageCode == StaticData.EnCode);
            type.LocalizedName = loc?.Value ?? Resources.Strings.NoName;
        }

        foreach (var cat in cats)
        {
            var loc = locs.FirstOrDefault(l => l.ParentId == cat.Id && l.LanguageCode == currentLang)
                      ?? locs.FirstOrDefault(l => l.ParentId == cat.Id && l.LanguageCode == StaticData.EnCode);
            cat.LocalizedName = loc?.Value ?? Resources.Strings.NoName;
        }

        TransactionTypes = new ObservableCollection<TransactionType>(types);
        Categories = new ObservableCollection<Category>(cats);

        _allTransactions = await _transactionRepository.GetTransactionsWithDetailsAsync();

        foreach (var t in _allTransactions)
        {
            if (t.Category != null)
            {
                var loc = locs.FirstOrDefault(l => l.ParentId == t.Category.Id && l.LanguageCode == currentLang)
                          ?? locs.FirstOrDefault(l => l.ParentId == t.Category.Id && l.LanguageCode == StaticData.EnCode);
                t.Category.LocalizedName = loc?.Value ?? Resources.Strings.NoName;
            }

            if (t.TransactionType != null)
            {
                var loc = locs.FirstOrDefault(l => l.ParentId == t.TransactionType.Id && l.LanguageCode == currentLang)
                          ?? locs.FirstOrDefault(l => l.ParentId == t.TransactionType.Id && l.LanguageCode == StaticData.EnCode);
                t.TransactionType.LocalizedName = loc?.Value ?? Resources.Strings.NoType;
            }
        }

        ApplyFilters();
    }

    [RelayCommand]
    private void PreviousMonth()
    {
        SelectedMonth = SelectedMonth.AddMonths(-1);
        UpdateMonthState();
    }

    [RelayCommand]
    private void NextMonth()
    {
        if (!CanGoNextMonth) return;

        SelectedMonth = SelectedMonth.AddMonths(1);
        UpdateMonthState();
    }

    private void UpdateMonthState()
    {
        var now = DateTime.Now;
        var currentMonthStart = new DateTime(now.Year, now.Month, 1);

        CanGoNextMonth = SelectedMonth < currentMonthStart;

        StartDate = new DateTime(SelectedMonth.Year, SelectedMonth.Month, 1);
        EndDate = new DateTime(SelectedMonth.Year, SelectedMonth.Month, DateTime.DaysInMonth(SelectedMonth.Year, SelectedMonth.Month));

        UpdatePeriodDisplay();
        ApplyFilters();
    }

    private void UpdatePeriodDisplay()
    {
        var culture = CultureInfo.CurrentCulture;

        if (StartDate.Year == EndDate.Year && StartDate.Month == EndDate.Month)
        {
            string monthName = StartDate.ToString("MMMM yyyy", culture);
            SelectedMonthDisplay = char.ToUpper(monthName[0]) + monthName.Substring(1);
        }
        else
        {
            string startStr = StartDate.ToString("MMM yyyy", culture);
            string endStr = EndDate.ToString("MMM yyyy", culture);

            string startFormatted = char.ToUpper(startStr[0]) + startStr.Substring(1);
            string endFormatted = char.ToUpper(endStr[0]) + endStr.Substring(1);

            SelectedMonthDisplay = $"{startFormatted} – {endFormatted}";
        }
    }

    public void UpdateChartData(List<Transaction> transactions)
    {
        if (transactions == null || !transactions.Any())
        {
            CategoryChart = null;
            ChartLegendItems = new List<ChartLegendItem>();
            HasChartData = false;
            return;
        }

        double targetRate = _currentRates.TryGetValue(DisplayCurrency, out double r) ? r : 1.0;
        var tempLegendItems = new List<ChartLegendItem>();

        var entries = transactions
            .GroupBy(t => t.Category?.Icon ?? "📦")
            .Select(group =>
            {
                var totalAmountInUah = group.Sum(t => t.GetAmountInUah(_currentRates));
                var totalAmountConverted = totalAmountInUah / targetRate;

                var icon = group.Key;
                var colorHex = icon switch
                {
                    "🛒" => "#2196F3",
                    "🚗" => "#FF9800",
                    "💡" => "#FFEB3B",
                    "💰" => "#4CAF50",
                    "🍔" => "#F44336",
                    "💊" => "#E91E63",
                    "🎬" => "#9C27B0",
                    "🛍️" => "#00BCD4",
                    "🏠" => "#795548",
                    "✈️" => "#03A9F4",
                    "🎓" => "#607D8B",
                    "📦" => "#444444",
                    _ => "#444444"
                };

                var localizedName = group.First().Category?.LocalizedName ?? "Other";

                tempLegendItems.Add(new ChartLegendItem
                {
                    Icon = icon,
                    Label = localizedName,
                    ValueLabel = $"{totalAmountConverted:N0} {DisplayCurrency}",
                    ColorHex = colorHex
                });

                return new ChartEntry((float)totalAmountConverted)
                {
                    Color = SKColor.Parse(colorHex)
                };
            })
            .ToList();

        ChartLegendItems = tempLegendItems.OrderByDescending(x => x.ValueLabel).ToList();

        CategoryChart = new DonutChart
        {
            Entries = entries,
            BackgroundColor = SKColors.Transparent,
            HoleRadius = 0.6f,
            LabelMode = LabelMode.None,
            GraphPosition = GraphPosition.Center
        };

        HasChartData = true;
    }

    partial void OnStartDateChanged(DateTime value)
    {
        if (value > EndDate)
        {
            EndDate = value;
        }
        UpdatePeriodDisplay();
        ApplyFilters();
    }

    partial void OnEndDateChanged(DateTime value)
    {
        if (value < StartDate)
        {
            StartDate = value;
        }
        UpdatePeriodDisplay();
        ApplyFilters();
    }

    partial void OnDisplayCurrencyChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            DisplayCurrency = StaticData.UahCurrency;
            return;
        }

        if (value != StaticData.UahCurrency && (_currentRates == null || !_currentRates.ContainsKey(value)))
        {
            DisplayCurrency = StaticData.UahCurrency;
            return;
        }
        ApplyFilters();
    }

    partial void OnSelectedSortOptionChanged(string value)
    {
        ApplyFilters();
    }

    [RelayCommand]
    private void SelectType(TransactionType type)
    {
        if (type == null) return;

        type.IsSelected = !type.IsSelected;

        ApplyFilters();
    }

    [RelayCommand]
    private void SelectCategory(ChartLegendItem item)
    {
        if (item == null) return;

        if (SelectedCategory == item)
        {
            SelectedCategory = null;
            IsCategoryFilterActive = false;
        }
        else
        {
            SelectedCategory = item;
            IsCategoryFilterActive = true;
        }

        ApplyFilters();
    }

    [RelayCommand]
    private void ClearCategoryFilter()
    {
        SelectedCategory = null;
        IsCategoryFilterActive = false;
        ApplyFilters();
    }

    [RelayCommand]
    private void ClearFilters()
    {
        foreach (var t in TransactionTypes)
        {
            t.IsSelected = false;
        }

        SelectedSortOption = SortOptions[0];
        SelectedCategory = null;
        IsCategoryFilterActive = false;

        DisplayCurrency = StaticData.UahCurrency;

        SelectedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        UpdateMonthState();
    }

    private void ApplyFilters()
    {
        var filtered = _allTransactions.Where(t => t.Date.Date >= StartDate.Date && t.Date.Date <= EndDate.Date);

        var selectedTypeIds = TransactionTypes.Where(t => t.IsSelected).Select(t => t.Id).ToList();
        if (selectedTypeIds.Any())
        {
            filtered = filtered.Where(t => selectedTypeIds.Contains(t.TransactionTypeId));
        }

        var filteredList = filtered.ToList();

        if (SelectedCategory != null)
        {
            filteredList = filteredList.Where(t => (t.Category?.Icon ?? "📦") == SelectedCategory.Icon).ToList();
        }

        filteredList = SelectedSortOption switch
        {
            var x when x == Resources.Strings.SortOption_DateOldest => filteredList.OrderBy(t => t.Date).ToList(),
            var x when x == Resources.Strings.SortOption_PriceLower => filteredList.OrderBy(t => t.GetAmountInUah(_currentRates)).ToList(),
            var x when x == Resources.Strings.SortOption_PriceHigher => filteredList.OrderByDescending(t => t.GetAmountInUah(_currentRates)).ToList(),
            _ => filteredList.OrderByDescending(t => t.Date).ToList()
        };

        _fullyFilteredTransactions = filteredList;
        FilteredTransactions = new ObservableCollection<Transaction>(_fullyFilteredTransactions.Take(20));

        var incomeInUah = filteredList.Where(t => t.TransactionType?.Icon == "📥").Sum(t => t.GetAmountInUah(_currentRates));
        var expenseInUah = filteredList.Where(t => t.TransactionType?.Icon == "📤").Sum(t => t.GetAmountInUah(_currentRates));
        var savingsInUah = filteredList.Where(t => t.TransactionType?.Icon == "🐷").Sum(t => t.GetAmountInUah(_currentRates));
        var othersInUah = filteredList.Where(t => t.TransactionType?.Icon == "🔄").Sum(t => t.GetAmountInUah(_currentRates));

        double targetRate = _currentRates.TryGetValue(DisplayCurrency, out double r) ? r : 1.0;

        var income = incomeInUah / targetRate;
        var expense = expenseInUah / targetRate;
        var savings = savingsInUah / targetRate;
        var others = othersInUah / targetRate;

        TotalIncome = $"+{income:N2} {DisplayCurrency}";
        TotalExpenses = $"-{expense:N2} {DisplayCurrency}";
        TotalSavings = $"{savings:N2} {DisplayCurrency}";
        TotalOthers = $"{others:N2} {DisplayCurrency}";

        CurrentBalance = $"{income - expense - savings:N2} {DisplayCurrency}";

        UpdateChartData(filteredList);
    }

    [RelayCommand]
    private async Task AddTransaction() => await Shell.Current.GoToAsync("TransactionAddPage");

    [RelayCommand]
    private void LoadMoreTransactions()
    {
        if (FilteredTransactions == null || _fullyFilteredTransactions == null)
            return;

        int currentlyLoaded = FilteredTransactions.Count;
        int totalAvailable = _fullyFilteredTransactions.Count;

        if (currentlyLoaded >= totalAvailable)
            return;

        var nextItems = _fullyFilteredTransactions
            .Skip(currentlyLoaded)
            .Take(20);

        foreach (var item in nextItems)
        {
            FilteredTransactions.Add(item);
        }
    }

    [RelayCommand]
    private async Task ExportData()
    {
        if (_fullyFilteredTransactions == null || !_fullyFilteredTransactions.Any())
        {
            await Shell.Current.DisplayAlert(Resources.Strings.Warning, Resources.Strings.ExportNoData, Resources.Strings.Ok);
            return;
        }

        string fileName = $"Transactions_{StartDate:yyyyMMdd}_{EndDate:yyyyMMdd}.csv";
        await _exportService.ExportTransactionsAsync(_fullyFilteredTransactions, fileName);
    }
}