namespace FinancialManager.Services;

public interface ILocalizationService
{
    string CurrentLanguage { get; set; }
    void Init();
}

public class LocalizationService : ILocalizationService
{
    private const string LanguageKey = "selected_language";
    private const string DefaultLanguage = "en";
    private string _currentLanguage = DefaultLanguage;

    public string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage != value)
            {
                _currentLanguage = value;
                Preferences.Default.Set(LanguageKey, value);
            }
        }
    }

    public void Init()
    {
        _currentLanguage = Preferences.Default.Get(LanguageKey, DefaultLanguage);
    }
}
