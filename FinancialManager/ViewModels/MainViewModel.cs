using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinancialManager.Data.Repositories;
using FinancialManager.Models;
using FinancialManager.Services;
using System.Collections.ObjectModel;

namespace FinancialManager.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITransactionTypeRepository _typeRepository;
    private readonly ILocalizationRepository _localizationRepository;
    private readonly ILocalizationService _localizationService;

    private List<Transaction> _allTransactions = new();

    [ObservableProperty] private string currentBalance = "0.00 ₴";
    [ObservableProperty] private string totalIncome = "0.00 ₴";
    [ObservableProperty] private string totalExpenses = "0.00 ₴";
    [ObservableProperty] private string totalSavings = "0.00 ₴";
    [ObservableProperty] private string totalOthers = "0.00 ₴";

    [ObservableProperty] private DateTime startDate = DateTime.Now.AddDays(-30);
    [ObservableProperty] private DateTime endDate = DateTime.Now;

    [ObservableProperty] private ObservableCollection<TransactionType> transactionTypes = new();
    [ObservableProperty] private ObservableCollection<Category> categories = new();

    [ObservableProperty] private ObservableCollection<Transaction> filteredTransactions = new();

    public MainViewModel(
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository,
        ITransactionTypeRepository typeRepository,
        ILocalizationRepository localizationRepository,
        ILocalizationService localizationService)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _typeRepository = typeRepository;
        _localizationRepository = localizationRepository;
        _localizationService = localizationService;
    }

    public async Task InitializeAsync()
    {
        var types = await _typeRepository.GetAsync();
        var cats = await _categoryRepository.GetAsync();
        var locs = await _localizationRepository.GetAsync();

        string currentLang = _localizationService.CurrentLanguage;

        foreach (var type in types)
        {
            var loc = locs.FirstOrDefault(l => l.ParentId == type.Id && l.LanguageCode == currentLang)
                      ?? locs.FirstOrDefault(l => l.ParentId == type.Id && l.LanguageCode == "en");
            type.LocalizedName = loc?.Value ?? "No Name";
        }

        foreach (var cat in cats)
        {
            var loc = locs.FirstOrDefault(l => l.ParentId == cat.Id && l.LanguageCode == currentLang)
                      ?? locs.FirstOrDefault(l => l.ParentId == cat.Id && l.LanguageCode == "en");
            cat.LocalizedName = loc?.Value ?? "No Name";
        }

        TransactionTypes = new ObservableCollection<TransactionType>(types);
        Categories = new ObservableCollection<Category>(cats);

        _allTransactions = await _transactionRepository.GetTransactionsWithDetailsAsync();

        foreach (var t in _allTransactions)
        {
            if (t.Category != null)
            {
                var loc = locs.FirstOrDefault(l => l.ParentId == t.Category.Id && l.LanguageCode == currentLang)
                          ?? locs.FirstOrDefault(l => l.ParentId == t.Category.Id && l.LanguageCode == "en");
                t.Category.LocalizedName = loc?.Value ?? "No Name";
            }

            if (t.TransactionType != null)
            {
                var loc = locs.FirstOrDefault(l => l.ParentId == t.TransactionType.Id && l.LanguageCode == currentLang)
                          ?? locs.FirstOrDefault(l => l.ParentId == t.TransactionType.Id && l.LanguageCode == "en");
                t.TransactionType.LocalizedName = loc?.Value ?? "No Type";
            }
        }

        ApplyFilters();
    }

    partial void OnStartDateChanged(DateTime value) => ApplyFilters();
    partial void OnEndDateChanged(DateTime value) => ApplyFilters();

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
        FilteredTransactions = new ObservableCollection<Transaction>(filteredList);

        var income = filteredList.Where(t => t.TransactionType?.Icon == "📥").Sum(t => t.Amount);
        var expense = filteredList.Where(t => t.TransactionType?.Icon == "📤").Sum(t => t.Amount);
        var savings = filteredList.Where(t => t.TransactionType?.Icon == "🐷").Sum(t => t.Amount);
        var others = filteredList.Where(t => t.TransactionType?.Icon == "🔄").Sum(t => t.Amount);

        TotalIncome = $"+{income:N2} ₴";
        TotalExpenses = $"-{expense:N2} ₴";
        TotalSavings = $"{savings:N2} ₴";
        TotalOthers = $"{others:N2} ₴";

        CurrentBalance = $"{income - expense - savings:N2} ₴";

        UpdateChartData(filteredList);
    }

    private void UpdateChartData(List<Transaction> transactions)
    {
        // Логіка передачі даних у LiveCharts
    }
}