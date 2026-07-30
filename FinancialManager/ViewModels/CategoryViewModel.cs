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

public partial class CategoryViewModel : ObservableObject
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILocalizationRepository _localizationRepository;
    private readonly ILocalizationApplier _localizationApplier;

    [ObservableProperty] private ObservableCollection<Category> categories = new();
    [ObservableProperty] private Category? selectedCategory;

    public Command LoadCategoriesCommand { get; }

    public CategoryViewModel(ICategoryRepository categoryRepository, ILocalizationRepository localizationRepository, ILocalizationApplier localizationApplier)
    {
        _categoryRepository = categoryRepository;
        LoadCategoriesCommand = new Command(async () => await LoadCategories());
        _localizationRepository = localizationRepository;
        _localizationApplier = localizationApplier;

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

        var resolver = await _localizationApplier.CreateResolverAsync();
        resolver.Apply(allCategories, Resources.Strings.NoName);

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

        bool confirm = await Shell.Current.DisplayAlert(Resources.Strings.DeleteTitle, string.Format(Resources.Strings.DeleteCategoryMessage, category.LocalizedName), 
            Resources.Strings.Yes, Resources.Strings.No);

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