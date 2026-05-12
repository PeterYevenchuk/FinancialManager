using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinancialManager.Data.Repositories;
using FinancialManager.Models;
using System.Collections.ObjectModel;

namespace FinancialManager.ViewModels;

public partial class CategoryViewModel : ObservableObject
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILocalizationRepository _localizationRepository;

    [ObservableProperty]
    private ObservableCollection<Category> categories = new();

    public CategoryViewModel(ICategoryRepository categoryRepository, ILocalizationRepository localizationRepository)
    {
        _categoryRepository = categoryRepository;
        LoadCategoriesCommand = new Command(async () => await LoadCategories());
        _localizationRepository = localizationRepository;
    }

    public Command LoadCategoriesCommand { get; }

    private async Task LoadCategories()
    {
        var items = await _categoryRepository.GetAsync();
        Categories = new ObservableCollection<Category>(items);
    }

    [RelayCommand]
    private async Task AddCategory()
    {
        await Shell.Current.GoToAsync("CategoryAddPage");
    }

    public async Task RefreshCategories()
    {
        var allCategories = await _categoryRepository.GetAsync();
        var allLocalizations = await _localizationRepository.GetAsync();

        string currentLang = "uk";

        foreach (var cat in allCategories)
        {
            var loc = allLocalizations.FirstOrDefault(l => l.ParentId == cat.Id && l.LanguageCode == currentLang);

            if (loc == null)
            {
                loc = allLocalizations.FirstOrDefault(l => l.ParentId == cat.Id && l.LanguageCode == "en");
            }

            cat.LocalizedName = loc?.Value ?? "No Name";
        }

        Categories = new ObservableCollection<Category>(allCategories);
    }
}
