using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FinancialManager.Services;
using FinancialManager.Services.Messages;
using System.Collections.ObjectModel;

namespace FinancialManager.ViewModels;

public partial class AppShellViewModel : ObservableObject
{
    private readonly ILocalizationService _localizationService;

    public ObservableCollection<LanguageModel> Languages { get; } = new()
    {
        new LanguageModel { Code = "en", Name = "🇺🇸 English" }, //Defolt language
        new LanguageModel { Code = "uk", Name = "🇺🇦 Українська" }
    };

    [ObservableProperty]
    private LanguageModel? selectedLanguage;

    public AppShellViewModel(ILocalizationService localizationService)
    {
        _localizationService = localizationService;

        var currentCode = _localizationService.CurrentLanguage;

        var languageToSelect = Languages.FirstOrDefault(l => l.Code == currentCode);

        SelectedLanguage = languageToSelect ?? Languages[0];
    }

    partial void OnSelectedLanguageChanged(LanguageModel? value)
    {
        if (value != null && _localizationService.CurrentLanguage != value.Code)
        {
            _localizationService.CurrentLanguage = value.Code;

            WeakReferenceMessenger.Default.Send(new LanguageChangedMessage(value.Code));
        }
    }
}

public class LanguageModel
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
