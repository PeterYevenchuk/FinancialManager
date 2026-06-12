using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinancialManager.Data.Contracts;
using FinancialManager.Helpers;
using FinancialManager.Models;
using FinancialManager.Services.Contracts;
using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace FinancialManager.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITransactionTypeRepository _typeRepository;
    private readonly ILocalizationRepository _localizationRepository;
    private readonly ILocalizationService _localizationService;
    private readonly ICurrencyService _currencyService;

    [ObservableProperty] private string currentBalance = StaticData.BalancePlaceholderUah;
    [ObservableProperty] private string totalIncome = StaticData.BalancePlaceholderUah;
    [ObservableProperty] private string totalExpenses = StaticData.BalancePlaceholderUah;
    [ObservableProperty] private string totalSavings = StaticData.BalancePlaceholderUah;
    [ObservableProperty] private string totalOthers = StaticData.BalancePlaceholderUah;
    [ObservableProperty] private DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    [ObservableProperty] private DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
    [ObservableProperty] private ObservableCollection<TransactionType> transactionTypes = new();
    [ObservableProperty] private ObservableCollection<Category> categories = new();
    [ObservableProperty] private ObservableCollection<Transaction> filteredTransactions = new();
    [ObservableProperty] private List<ChartLegendItem> chartLegendItems = new();
    [ObservableProperty] private string displayCurrency = StaticData.UahCurrency;
    [ObservableProperty] private bool isCurrencySelectionEnabled = true;
    [ObservableProperty] private bool isSortedAscending;
    [ObservableProperty] private bool isSortedDescending;

    private List<Transaction> _allTransactions = new();
    private Dictionary<string, double> _currentRates = new() { { StaticData.UahCurrency, 1.0 } };
    public List<string> AvailableDisplayCurrencies { get; } = new() { StaticData.UahCurrency, StaticData.UsdCurrency, StaticData.EurCurrency };

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

    public MainViewModel(
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository,
        ITransactionTypeRepository typeRepository,
        ILocalizationRepository localizationRepository,
        ILocalizationService localizationService,
        ICurrencyService currencyService)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _typeRepository = typeRepository;
        _localizationRepository = localizationRepository;
        _localizationService = localizationService;
        _currencyService = currencyService;
    }

    public async Task InitializeAsync()
    {
        var types = await _typeRepository.GetAsync();
        var cats = await _categoryRepository.GetAsync();
        var locs = await _localizationRepository.GetAsync();

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

    public async void UpdateChartData(List<Transaction> transactions)
    {
        CategoryChart = null;
        ChartLegendItems = new List<ChartLegendItem>();
        await Task.Delay(30);

        if (transactions == null || !transactions.Any())
            return;

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
    }

    partial void OnStartDateChanged(DateTime value)
    {
        if (value > EndDate)
        {
            EndDate = value;
        }
        ApplyFilters();
    }

    partial void OnEndDateChanged(DateTime value)
    {
        if (value < StartDate)
        {
            StartDate = value;
        }
        ApplyFilters();
    }

    partial void OnDisplayCurrencyChanged(string value)
    {
        if (value != StaticData.UahCurrency && (_currentRates == null || !_currentRates.ContainsKey(value)))
        {
            DisplayCurrency = StaticData.UahCurrency;
            return;
        }
        ApplyFilters();
    }

    [RelayCommand]
    private void ToggleSortAscending()
    {
        if (IsSortedAscending)
        {
            IsSortedAscending = false;
        }
        else
        {
            IsSortedAscending = true;
            IsSortedDescending = false;
        }
        ApplyFilters();
    }

    [RelayCommand]
    private void ToggleSortDescending()
    {
        if (IsSortedDescending)
        {
            IsSortedDescending = false;
        }
        else
        {
            IsSortedDescending = true;
            IsSortedAscending = false;
        }
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
    private void ClearFilters()
    {
        foreach (var t in TransactionTypes)
        {
            t.IsSelected = false;
        }

        IsSortedAscending = false;
        IsSortedDescending = false;
        DisplayCurrency = StaticData.UahCurrency;
        StartDate = DateTime.Now.AddDays(-30);
        EndDate = DateTime.Now;
        ApplyFilters();
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

        if (IsSortedAscending)
        {
            filteredList = filteredList.OrderBy(t => t.GetAmountInUah(_currentRates)).ToList();
        }
        else if (IsSortedDescending)
        {
            filteredList = filteredList.OrderByDescending(t => t.GetAmountInUah(_currentRates)).ToList();
        }

        FilteredTransactions = new ObservableCollection<Transaction>(filteredList);

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
}