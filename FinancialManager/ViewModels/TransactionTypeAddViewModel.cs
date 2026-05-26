using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FinancialManager.Data.Repositories;
using FinancialManager.Models;
using FinancialManager.Services;
using FinancialManager.Services.Messages;

namespace FinancialManager.ViewModels;

[QueryProperty(nameof(TransactionTypeToEdit), "TransactionTypeToEdit")]
public partial class TransactionTypeAddViewModel : ObservableObject
{
    private readonly ITransactionTypeRepository _transactionTypeRepository;
    private readonly ILocalizationRepository _localizationRepository;
    private readonly ILocalizationService _localizationService;

    [ObservableProperty] private string nameEng;
    [ObservableProperty] private string icon = "✨";
    [ObservableProperty] private string nameUkr;

    [ObservableProperty] private TransactionType? transactionTypeToEdit;

    public bool IsUkrVisible => _localizationService.CurrentLanguage == "uk";

    public TransactionTypeAddViewModel(ITransactionTypeRepository transactionTypeRepository, ILocalizationRepository localizationRepository, ILocalizationService localizationService)
    {
        _transactionTypeRepository = transactionTypeRepository;
        _localizationRepository = localizationRepository;
        _localizationService = localizationService;

        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
        {
            OnPropertyChanged(nameof(IsUkrVisible));
        });
    }

    partial void OnTransactionTypeToEditChanged(TransactionType? value)
    {
        if (value != null)
        {
            LoadLocalizations(value.Id);
            Icon = value.Icon;
        }
    }

    private async void LoadLocalizations(Guid transactionTypeId)
    {
        var allLocs = await _localizationRepository.GetAsync();

        NameEng = allLocs.FirstOrDefault(l => l.ParentId == transactionTypeId && l.LanguageCode == "en")?.Value ?? string.Empty;
        NameUkr = allLocs.FirstOrDefault(l => l.ParentId == transactionTypeId && l.LanguageCode == "uk")?.Value ?? string.Empty;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(NameEng))
        {
            await Shell.Current.DisplayAlert(Resources.Strings.Error, Resources.Strings.EnglishNameRequired, Resources.Strings.Ok);
            return;
        }

        var transactionType = TransactionTypeToEdit ?? new TransactionType();
        transactionType.Icon = Icon;

        await _transactionTypeRepository.SaveAsync(transactionType);

        await SaveOrUpdateLocalization(transactionType.Id, "en", NameEng);
        await SaveOrUpdateLocalization(transactionType.Id, "uk", NameUkr);

        await Shell.Current.GoToAsync("..");
    }

    private async Task SaveOrUpdateLocalization(Guid parentId, string langCode, string value)
    {
        var allLocs = await _localizationRepository.GetAsync();
        var existingLoc = allLocs.FirstOrDefault(l => l.ParentId == parentId && l.LanguageCode == langCode);

        if (existingLoc != null)
        {
            existingLoc.Value = value;
            await _localizationRepository.SaveAsync(existingLoc);
        }
        else if (!string.IsNullOrWhiteSpace(value))
        {
            await _localizationRepository.SaveAsync(new Localization
            {
                ParentId = parentId,
                LanguageCode = langCode,
                Value = value
            });
        }
    }
}
