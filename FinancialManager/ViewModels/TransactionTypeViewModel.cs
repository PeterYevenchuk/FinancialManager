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

public partial class TransactionTypeViewModel : ObservableObject
{
    private readonly ITransactionTypeRepository _transactionTypeRepository;
    private readonly ILocalizationRepository _localizationRepository;
    private readonly ILocalizationApplier _localizationApplier;

    [ObservableProperty] private ObservableCollection<TransactionType> transactionTypes = new();
    [ObservableProperty] private TransactionType? selectedType;

    public Command LoadTransactionTypesCommand { get; }

    public TransactionTypeViewModel(ITransactionTypeRepository transactionTypeRepository, ILocalizationRepository localizationRepository, ILocalizationApplier localizationApplier)
    {
        _localizationRepository = localizationRepository;
        _transactionTypeRepository = transactionTypeRepository;
        _localizationApplier = localizationApplier;
        LoadTransactionTypesCommand = new Command(async () => await LoadTransactionTypes());

        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, async (r, m) =>
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await RefreshTransactionTypes();
            });
        });
    }

    public async Task RefreshTransactionTypes()
    {
        var allTransactionTypes = await _transactionTypeRepository.GetAsync();

        var resolver = await _localizationApplier.CreateResolverAsync();
        resolver.Apply(allTransactionTypes, Resources.Strings.NoName);

        TransactionTypes = new ObservableCollection<TransactionType>(allTransactionTypes);
    }

    private async Task LoadTransactionTypes()
    {
        var items = await _transactionTypeRepository.GetAsync();
        TransactionTypes = new ObservableCollection<TransactionType>(items);
    }

    [RelayCommand]
    private void SelectType(TransactionType? type)
    {
        if (type == null || type.IsSystem) return;

        var previouslySelected = TransactionTypes.FirstOrDefault(t => t != type && t.IsSelected);
        if (previouslySelected != null)
        {
            previouslySelected.IsSelected = false;
        }

        type.IsSelected = !type.IsSelected;

        SelectedType = type.IsSelected ? type : null;
    }

    [RelayCommand]
    private async Task AddTransactionTypes()
    {
        await Shell.Current.GoToAsync("TransactionTypeAddPage");
    }

    [RelayCommand]
    private async Task EditTransactionType(TransactionType type)
    {
        if (type == null || type.IsSystem) return;

        var navigationParameter = new Dictionary<string, object>
        {
            { "TransactionTypeToEdit", type }
        };
        await Shell.Current.GoToAsync("TransactionTypeAddPage", navigationParameter);
    }

    [RelayCommand]
    private async Task DeleteCategory(TransactionType type)
    {
        if (type == null || type.IsSystem) return;

        bool confirm = await Shell.Current.DisplayAlert(Resources.Strings.DeleteTitle, string.Format(Resources.Strings.DeleteTransactionTypeMessage, type.LocalizedName), 
            Resources.Strings.Yes, Resources.Strings.No);

        if (confirm)
        {
            var allLocs = await _localizationRepository.GetAsync();
            var transactionTypeLocs = allLocs.Where(l => l.ParentId == type.Id).ToList();

            foreach (var loc in transactionTypeLocs)
            {
                await _localizationRepository.DeleteAsync(loc);
            }

            await _transactionTypeRepository.DeleteTransactionTypeAsync(type);

            await RefreshTransactionTypes();

            SelectedType = null;
        }
    }
}