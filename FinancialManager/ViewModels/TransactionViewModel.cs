using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FinancialManager.Data.Contracts;
using FinancialManager.Helpers;
using FinancialManager.Models;
using FinancialManager.Services.Contracts;
using FinancialManager.Services.Messages;
using System.Collections.ObjectModel;

namespace FinancialManager.ViewModels;

public partial class TransactionViewModel : ObservableObject
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITransactionTypeRepository _typeRepository;
    private readonly ILocalizationApplier _localizationApplier;

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
        ILocalizationApplier localizationApplier)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _typeRepository = typeRepository;
        _localizationApplier = localizationApplier;

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

    public async Task RefreshTransactions()
    {
        _allLoadedTransactions = await _transactionRepository.GetTransactionsWithDetailsAsync();

        var resolver = await _localizationApplier.CreateResolverAsync();
        resolver.ApplyToTransactions(_allLoadedTransactions, Resources.Strings.NoName, Resources.Strings.NoType);

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

    private async Task LoadFiltersData()
    {
        var cats = await _categoryRepository.GetAsync();
        var types = await _typeRepository.GetAsync();

        var resolver = await _localizationApplier.CreateResolverAsync();
        resolver.Apply(cats, Resources.Strings.NoName);
        resolver.Apply(types, Resources.Strings.NoName);

        Categories = new ObservableCollection<Category>(cats);
        TransactionTypes = new ObservableCollection<TransactionType>(types);
    }

    [RelayCommand]
    private void SelectTransaction(Transaction? transaction)
    {
        if (transaction == null) return;

        var previouslySelected = Transactions.FirstOrDefault(t => t != transaction && t.IsSelected);
        if (previouslySelected != null)
        {
            previouslySelected.IsSelected = false;
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
}