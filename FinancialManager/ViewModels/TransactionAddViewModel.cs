using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FinancialManager.Data.Repositories;
using FinancialManager.Models;
using FinancialManager.Services;
using FinancialManager.Services.Contracts;
using FinancialManager.Services.Messages;
using System.Collections.ObjectModel;

namespace FinancialManager.ViewModels;

[QueryProperty(nameof(TransactionToEdit), "TransactionToEdit")]
public partial class TransactionAddViewModel : ObservableObject
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITransactionTypeRepository _typeRepository;
    private readonly ILocalizationRepository _localizationRepository;
    private readonly ILocalizationService _localizationService;
    private readonly ICurrencyService _currencyService;

    [ObservableProperty] private double amount;
    [ObservableProperty] private string description = string.Empty;
    [ObservableProperty] private DateTime date = DateTime.Now;
    [ObservableProperty] private Category? selectedCategory;
    [ObservableProperty] private TransactionType? selectedType;
    [ObservableProperty] private string selectedCurrency = "₴";
    [ObservableProperty] private string exchangeRate = "1.0";
    [ObservableProperty] private bool isRateFieldsVisible;

    [ObservableProperty] private ObservableCollection<Category> categories = new();
    [ObservableProperty] private ObservableCollection<TransactionType> transactionTypes = new();

    [ObservableProperty] private Transaction? transactionToEdit;

    public List<string> Currencies { get; } = new() { "₴", "$", "€" };

    public TransactionAddViewModel(
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

        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, async (r, m) =>
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await RefreshLocalizationOnlyAsync();
            });
        });
    }

    public async Task LoadDataAsync()
    {
        await RefreshLocalizationOnlyAsync();

        if (TransactionToEdit != null)
        {
            Amount = TransactionToEdit.Amount;
            Description = TransactionToEdit.Description;
            Date = TransactionToEdit.Date;
            SelectedCategory = Categories.FirstOrDefault(c => c.Id == TransactionToEdit.CategoryId);
            SelectedType = TransactionTypes.FirstOrDefault(t => t.Id == TransactionToEdit.TransactionTypeId);
            SelectedCurrency = Currencies.Contains(TransactionToEdit.Currency) ? TransactionToEdit.Currency : "₴";
            ExchangeRate = TransactionToEdit.ExchangeRateToUah.ToString("F2");
        }
        else
        {
            SelectedCurrency = "₴";
            ExchangeRate = "1.0";
        }
    }

    private async Task RefreshLocalizationOnlyAsync()
    {
        var allCats = await _categoryRepository.GetAsync();
        var allTypes = await _typeRepository.GetAsync();
        var allLocs = await _localizationRepository.GetAsync();

        string currentLang = _localizationService.CurrentLanguage;

        foreach (var cat in allCats)
        {
            var loc = allLocs.FirstOrDefault(l => l.ParentId == cat.Id && l.LanguageCode == currentLang)
                      ?? allLocs.FirstOrDefault(l => l.ParentId == cat.Id && l.LanguageCode == "en");
            cat.LocalizedName = loc?.Value ?? "No Name";
        }

        foreach (var type in allTypes)
        {
            var loc = allLocs.FirstOrDefault(l => l.ParentId == type.Id && l.LanguageCode == currentLang)
                      ?? allLocs.FirstOrDefault(l => l.ParentId == type.Id && l.LanguageCode == "en");
            type.LocalizedName = loc?.Value ?? "No Name";
        }

        var currentSelectedCatId = SelectedCategory?.Id;
        var currentSelectedTypeId = SelectedType?.Id;

        Categories = new ObservableCollection<Category>(allCats);
        TransactionTypes = new ObservableCollection<TransactionType>(allTypes);

        if (currentSelectedCatId != null)
            SelectedCategory = Categories.FirstOrDefault(c => c.Id == currentSelectedCatId);

        if (currentSelectedTypeId != null)
            SelectedType = TransactionTypes.FirstOrDefault(t => t.Id == currentSelectedTypeId);
    }

    public void ResetForm()
    {
        if (TransactionToEdit == null)
        {
            Amount = 0;
            Description = string.Empty;
            Date = DateTime.Now;
            SelectedCategory = null;
            SelectedType = null;
            SelectedCurrency = "₴";
            ExchangeRate = "1.0";
        }
    }

    [RelayCommand]
    private async Task Save()
    {
        if (Amount <= 0)
        {
            await Shell.Current.DisplayAlert(Resources.Strings.Error, Resources.Strings.AmountMustBeGreaterThanZero, Resources.Strings.Ok);
            return;
        }
        if (SelectedCategory == null || SelectedType == null)
        {
            await Shell.Current.DisplayAlert(Resources.Strings.Error, Resources.Strings.SelectCategoryAndType, Resources.Strings.Ok);
            return;
        }

        double rateValue = 1.0;
        if (SelectedCurrency != "₴")
        {
            if (string.IsNullOrWhiteSpace(ExchangeRate) || !double.TryParse(ExchangeRate, out rateValue) || rateValue <= 0)
            {
                await Shell.Current.DisplayAlert(Resources.Strings.InvalidExchangeRateTitle, Resources.Strings.InvalidExchangeRateMessage, Resources.Strings.Ok);
                return;
            }
        }

        var transaction = TransactionToEdit ?? new Transaction();
        transaction.Amount = Amount;
        transaction.Description = Description;
        transaction.Date = Date;
        transaction.CategoryId = SelectedCategory.Id;
        transaction.TransactionTypeId = SelectedType.Id;
        transaction.Currency = SelectedCurrency;
        transaction.ExchangeRateToUah = rateValue;

        await _transactionRepository.SaveAsync(transaction);
        await Shell.Current.GoToAsync("..");
    }

    partial void OnSelectedCurrencyChanged(string value)
    {
        IsRateFieldsVisible = value != "₴";
        if (value == "₴")
        {
            ExchangeRate = "1.0";
        }
        else
        {
            _ = AutoFetchRateAsync(value);
        }
    }

    private async Task AutoFetchRateAsync(string currencySymbol)
    {
        var rates = await _currencyService.GetLatestRatesAsync();
        if (rates != null && rates.TryGetValue(currencySymbol, out double fetchedRate))
        {
            ExchangeRate = fetchedRate.ToString("F2");
        }
        else
        {
            ExchangeRate = "";
        }
    }
}