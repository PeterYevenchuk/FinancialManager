using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinancialManager.Data.Repositories;
using FinancialManager.Models;
using FinancialManager.Services;

namespace FinancialManager.ViewModels;

[QueryProperty(nameof(CategoryToEdit), "CategoryToEdit")]
public partial class CategoryAddViewModel : ObservableObject
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILocalizationRepository _localizationRepository;
    private readonly ILocalizationService _localizationService;

    [ObservableProperty] private string icon = "✨";
    [ObservableProperty] private string nameEng;
    [ObservableProperty] private string nameUkr;

    [ObservableProperty] private Category? categoryToEdit;

    public CategoryAddViewModel(ICategoryRepository categoryRepository, ILocalizationRepository localizationRepository, ILocalizationService localizationService)
    {
        _categoryRepository = categoryRepository;
        _localizationRepository = localizationRepository;
        _localizationService = localizationService;
    }

    partial void OnCategoryToEditChanged(Category? value)
    {
        if (value != null)
        {
            Icon = value.Icon;
            LoadLocalizations(value.Id);
        }
    }

    private async void LoadLocalizations(Guid categoryId)
    {
        var allLocs = await _localizationRepository.GetAsync();

        NameEng = allLocs.FirstOrDefault(l => l.ParentId == categoryId && l.LanguageCode == "en")?.Value ?? string.Empty;
        NameUkr = allLocs.FirstOrDefault(l => l.ParentId == categoryId && l.LanguageCode == "uk")?.Value ?? string.Empty;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(NameEng))
        {
            await Shell.Current.DisplayAlert(Resources.Strings.Error, Resources.Strings.EnglishNameRequired, Resources.Strings.Ok);
            return;
        }

        var category = CategoryToEdit ?? new Category();
        category.Icon = Icon;

        await _categoryRepository.SaveAsync(category);

        await SaveOrUpdateLocalization(category.Id, "en", NameEng);
        await SaveOrUpdateLocalization(category.Id, "uk", NameUkr);

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