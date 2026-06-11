using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FinancialManager.Data.Repositories;
using FinancialManager.Models;
using FinancialManager.Services;
using FinancialManager.Services.Messages;
using System.Collections.ObjectModel;

namespace FinancialManager.ViewModels;

public partial class CategoryViewModel : ObservableObject
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILocalizationRepository _localizationRepository;
    private readonly ILocalizationService _localizationService;

    [ObservableProperty]
    private ObservableCollection<Category> categories = new();

    [ObservableProperty]
    private Category? selectedCategory;

    public Command LoadCategoriesCommand { get; }

    public CategoryViewModel(ICategoryRepository categoryRepository, ILocalizationRepository localizationRepository, ILocalizationService localizationService)
    {
        _categoryRepository = categoryRepository;
        LoadCategoriesCommand = new Command(async () => await LoadCategories());
        _localizationRepository = localizationRepository;
        _localizationService = localizationService;

        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, async (r, m) =>
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await RefreshCategories();
            });
        });
    }

    public async Task RefreshCategories()
    {
        var allCategories = await _categoryRepository.GetAsync();
        var allLocalizations = await _localizationRepository.GetAsync();

        string currentLang = _localizationService.CurrentLanguage;

        foreach (var cat in allCategories)
        {
            var loc = allLocalizations.FirstOrDefault(l => l.ParentId == cat.Id && l.LanguageCode == currentLang);

            if (loc == null)
            {
                loc = allLocalizations.FirstOrDefault(l => l.ParentId == cat.Id && l.LanguageCode == "en");
            }

            cat.LocalizedName = loc?.Value ?? Resources.Strings.NoName;
        }

        Categories = new ObservableCollection<Category>(allCategories);
    }

    private async Task LoadCategories()
    {
        var items = await _categoryRepository.GetAsync();
        Categories = new ObservableCollection<Category>(items);
    }

    [RelayCommand]
    private void SelectCategory(Category? category)
    {
        if (category == null || category.IsSystem) return;

        var previouslySelected = Categories.FirstOrDefault(c => c != category && c.IsSelected);
        if (previouslySelected != null)
        {
            previouslySelected.IsSelected = false;
        }

        category.IsSelected = !category.IsSelected;

        SelectedCategory = category.IsSelected ? category : null;
    }

    [RelayCommand]
    private async Task AddCategory()
    {
        await Shell.Current.GoToAsync("CategoryAddPage");
    }

    [RelayCommand]
    private async Task EditCategory(Category category)
    {
        if (category == null || category.IsSystem) return;

        var navigationParameter = new Dictionary<string, object>
        {
            { "CategoryToEdit", category }
        };
        await Shell.Current.GoToAsync("CategoryAddPage", navigationParameter);
    }

    [RelayCommand]
    private async Task DeleteCategory(Category category)
    {
        if (category == null || category.IsSystem) return;

        bool confirm = await Shell.Current.DisplayAlert(Resources.Strings.DeleteTitle, string.Format(Resources.Strings.DeleteCategoryMessage, category.LocalizedName), Resources.Strings.Yes, Resources.Strings.No);

        if (confirm)
        {
            var allLocs = await _localizationRepository.GetAsync();
            var categoryLocs = allLocs.Where(l => l.ParentId == category.Id).ToList();

            foreach (var loc in categoryLocs)
            {
                await _localizationRepository.DeleteAsync(loc);
            }

            await _categoryRepository.DeleteCategoryAsync(category);

            await RefreshCategories();

            SelectedCategory = null;
        }
    }
}