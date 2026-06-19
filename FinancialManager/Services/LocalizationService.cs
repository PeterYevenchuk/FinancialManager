using System.Globalization;
using FinancialManager.Helpers;
using FinancialManager.Services.Contracts;

namespace FinancialManager.Services;

public class LocalizationService : ILocalizationService
{
    public string CurrentLanguage
    {
        get => field ??= Preferences.Default.Get(StaticData.LanguageKey, StaticData.DefaultLanguage);
        set
        {
            if (field != value)
            {
                field = value;
                Preferences.Default.Set(StaticData.LanguageKey, value);
                ApplyCulture(value);
            }
        }
    }

    public void Init()
    {
        ApplyCulture(CurrentLanguage);
    }

    private void ApplyCulture(string languageCode)
    {
        var culture = new CultureInfo(languageCode);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}