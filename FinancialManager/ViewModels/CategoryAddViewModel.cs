using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinancialManager.Data.Repositories;
using FinancialManager.Models;

namespace FinancialManager.ViewModels;

public partial class CategoryAddViewModel : ObservableObject
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IRepository<Localization> _localizationRepository;

    [ObservableProperty] private string icon = "📁";
    [ObservableProperty] private string nameEng;
    [ObservableProperty] private string nameUkr;

    public CategoryAddViewModel(ICategoryRepository categoryRepository, IRepository<Localization> localizationRepository)
    {
        _categoryRepository = categoryRepository;
        _localizationRepository = localizationRepository;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(NameEng))
        {
            await Shell.Current.DisplayAlert("Error", "English name is required!", "OK");
            return;
        }

        var category = new Category { Icon = Icon };
        await _categoryRepository.SaveAsync(category);

        await _localizationRepository.SaveAsync(new Localization
        {
            ParentId = category.Id,
            LanguageCode = "en",
            Value = NameEng
        });

        if (!string.IsNullOrWhiteSpace(NameUkr))
        {
            await _localizationRepository.SaveAsync(new Localization
            {
                ParentId = category.Id,
                LanguageCode = "uk",
                Value = NameUkr
            });
        }

        await Shell.Current.GoToAsync("..");
    }
}
