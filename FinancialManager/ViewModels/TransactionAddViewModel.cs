using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinancialManager.Data.Repositories;
using FinancialManager.Models;
using FinancialManager.Services;
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

    [ObservableProperty] private double amount;
    [ObservableProperty] private string description = string.Empty;
    [ObservableProperty] private DateTime date = DateTime.Now;
    [ObservableProperty] private Category? selectedCategory;
    [ObservableProperty] private TransactionType? selectedType;

    [ObservableProperty] private ObservableCollection<Category> categories = new();
    [ObservableProperty] private ObservableCollection<TransactionType> transactionTypes = new();

    [ObservableProperty] private Transaction? transactionToEdit;

    public TransactionAddViewModel(
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

    public async Task LoadDataAsync()
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

        Categories = new ObservableCollection<Category>(allCats);
        TransactionTypes = new ObservableCollection<TransactionType>(allTypes);

        if (TransactionToEdit != null)
        {
            Amount = TransactionToEdit.Amount;
            Description = TransactionToEdit.Description;
            Date = TransactionToEdit.Date;
            SelectedCategory = Categories.FirstOrDefault(c => c.Id == TransactionToEdit.CategoryId);
            SelectedType = TransactionTypes.FirstOrDefault(t => t.Id == TransactionToEdit.TransactionTypeId);
        }
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
        }
    }

    [RelayCommand]
    private async Task Save()
    {
        if (Amount <= 0)
        {
            await Shell.Current.DisplayAlert("Помилка", "Сума повинна бути більшою за 0", "OK");
            return;
        }
        if (SelectedCategory == null || SelectedType == null)
        {
            await Shell.Current.DisplayAlert("Помилка", "Виберіть категорію та тип транзакції", "OK");
            return;
        }

        var transaction = TransactionToEdit ?? new Transaction();
        transaction.Amount = Amount;
        transaction.Description = Description;
        transaction.Date = Date;
        transaction.CategoryId = SelectedCategory.Id;
        transaction.TransactionTypeId = SelectedType.Id;

        await _transactionRepository.SaveAsync(transaction);
        await Shell.Current.GoToAsync("..");
    }
}
