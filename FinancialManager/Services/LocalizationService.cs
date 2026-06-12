using FinancialManager.Helpers;
using FinancialManager.Services.Contracts;

namespace FinancialManager.Services;

public class LocalizationService : ILocalizationService
{
    private string _currentLanguage = StaticData.DefaultLanguage;

    public string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage != value)
            {
                _currentLanguage = value;
                Preferences.Default.Set(StaticData.LanguageKey, value);
            }
        }
    }

    public void Init()
    {
        _currentLanguage = Preferences.Default.Get(StaticData.LanguageKey, StaticData.DefaultLanguage);
    }
}
