using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FinancialManager.Data.Repositories;
using FinancialManager.Models;
using FinancialManager.Services;
using FinancialManager.Services.Messages;
using System.Collections.ObjectModel;

namespace FinancialManager.ViewModels;

public partial class TransactionViewModel : ObservableObject
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITransactionTypeRepository _typeRepository;
    private readonly ILocalizationRepository _localizationRepository;
    private readonly ILocalizationService _localizationService;

    private List<Transaction> _allLoadedTransactions = new();

    [ObservableProperty] private ObservableCollection<Transaction> transactions = new();
    [ObservableProperty] private ObservableCollection<Category> categories = new();
    [ObservableProperty] private ObservableCollection<TransactionType> transactionTypes = new();

    [ObservableProperty] private Category? selectedCategoryFilter;
    [ObservableProperty] private TransactionType? selectedTypeFilter;
    [ObservableProperty] private DateTime dateFrom;
    [ObservableProperty] private DateTime dateTo;

    [ObservableProperty] private Transaction? selectedTransaction;

    public TransactionViewModel(
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

        var now = DateTime.Now;
        DateFrom = new DateTime(now.Year, now.Month, 1);
        DateTo = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, async (r, m) =>
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await InitAsync();
            });
        });
    }

    public async Task InitAsync()
    {
        await LoadFiltersData();
        await RefreshTransactions();
    }

    private async Task LoadFiltersData()
    {
        var cats = await _categoryRepository.GetAsync();
        var types = await _typeRepository.GetAsync();
        var locs = await _localizationRepository.GetAsync();

        string currentLang = _localizationService.CurrentLanguage;

        foreach (var cat in cats)
        {
            var loc = locs.FirstOrDefault(l => l.ParentId == cat.Id && l.LanguageCode == currentLang)
                      ?? locs.FirstOrDefault(l => l.ParentId == cat.Id && l.LanguageCode == "en");
            cat.LocalizedName = loc?.Value ?? "No Name";
        }

        foreach (var type in types)
        {
            var loc = locs.FirstOrDefault(l => l.ParentId == type.Id && l.LanguageCode == currentLang)
                      ?? locs.FirstOrDefault(l => l.ParentId == type.Id && l.LanguageCode == "en");
            type.LocalizedName = loc?.Value ?? "No Name";
        }

        Categories = new ObservableCollection<Category>(cats);
        TransactionTypes = new ObservableCollection<TransactionType>(types);
    }

    public async Task RefreshTransactions()
    {
        _allLoadedTransactions = await _transactionRepository.GetTransactionsWithDetailsAsync();

        var locs = await _localizationRepository.GetAsync();
        string currentLang = _localizationService.CurrentLanguage;

        foreach (var t in _allLoadedTransactions)
        {
            if (t.Category != null)
            {
                var loc = locs.FirstOrDefault(l => l.ParentId == t.Category.Id && l.LanguageCode == currentLang)
                          ?? locs.FirstOrDefault(l => l.ParentId == t.Category.Id && l.LanguageCode == "en");
                t.Category.LocalizedName = loc?.Value ?? Resources.Strings.NoName;
            }

            if (t.TransactionType != null)
            {
                var loc = locs.FirstOrDefault(l => l.ParentId == t.TransactionType.Id && l.LanguageCode == currentLang)
                          ?? locs.FirstOrDefault(l => l.ParentId == t.TransactionType.Id && l.LanguageCode == "en");
                t.TransactionType.LocalizedName = loc?.Value ?? Resources.Strings.NoType;
            }
        }

        ApplyFilters();
    }

    [RelayCommand]
    public void ApplyFilters()
    {
        var filtered = _allLoadedTransactions.Where(t => t.Date.Date >= DateFrom.Date && t.Date.Date <= DateTo.Date);

        if (SelectedCategoryFilter != null)
            filtered = filtered.Where(t => t.CategoryId == SelectedCategoryFilter.Id);

        if (SelectedTypeFilter != null)
            filtered = filtered.Where(t => t.TransactionTypeId == SelectedTypeFilter.Id);

        Transactions = new ObservableCollection<Transaction>(filtered.OrderByDescending(t => t.Date));
    }

    [RelayCommand]
    public void ClearFilters()
    {
        SelectedCategoryFilter = null;
        SelectedTypeFilter = null;
        var now = DateTime.Now;
        DateFrom = new DateTime(now.Year, now.Month, 1);
        DateTo = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        ApplyFilters();
    }

    partial void OnSelectedTransactionChanged(Transaction? value)
    {
        foreach (var t in Transactions)
        {
            t.IsSelected = false;
        }

        if (value != null)
        {
            value.IsSelected = true;

            int index = Transactions.IndexOf(value);
            if (index != -1)
            {
                Transactions[index] = value;
            }
        }
    }

    [RelayCommand]
    private void SelectTransaction(Transaction? transaction)
    {
        if (transaction == null) return;

        foreach (var t in Transactions)
        {
            if (t != transaction)
                t.IsSelected = false;
        }

        transaction.IsSelected = !transaction.IsSelected;
    }

    [RelayCommand]
    private async Task AddTransaction() => await Shell.Current.GoToAsync("TransactionAddPage");

    [RelayCommand]
    private async Task EditTransaction(Transaction transaction)
    {
        var param = new Dictionary<string, object> { { "TransactionToEdit", transaction } };
        await Shell.Current.GoToAsync("TransactionAddPage", param);
    }

    [RelayCommand]
    private async Task DeleteTransaction(Transaction transaction)
    {
        if (transaction == null) return;
        bool confirm = await Shell.Current.DisplayAlert(Resources.Strings.DeleteTitle, Resources.Strings.DeleteTransactionMessage, Resources.Strings.Yes, Resources.Strings.No);
        if (confirm)
        {
            await _transactionRepository.DeleteAsync(transaction);
            await RefreshTransactions();
        }
    }

    partial void OnDateFromChanged(DateTime value)
    {
        if (value > DateTo)
        {
            DateTo = value;
        }
        ApplyFilters();
    }

    partial void OnDateToChanged(DateTime value)
    {
        if (value < DateFrom)
        {
            DateFrom = value;
        }
        ApplyFilters();
    }
}