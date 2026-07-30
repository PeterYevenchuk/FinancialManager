namespace FinancialManager.Helpers;

public static class StaticData
{
    // Language codes
    public const string EnCode = "en";
    public const string UkCode = "uk";
    public const string EnName = "🇺🇸 English";
    public const string UkName = "🇺🇦 Українська";
    public const string DefaultLanguage = "en";
    public const string LanguageKey = "selected_language";

    // Currency symbols
    public const string UahCurrency = "₴";
    public const string UsdCurrency = "$";
    public const string EurCurrency = "€";

    // Currency codes
    public const string UsdCode = "USD";
    public const string EurCode = "EUR";

    // API endpoints
    public const string NbuExchangeRateUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";

    // JSON property names
    public const string CurrencyCodeProperty = "cc";
    public const string RateProperty = "rate";

    // Icons
    public const string DefaultIcon = "✨";
    public const string DefaultCategoryIcon = "📦";

    // Balance placeholder
    public const string BalancePlaceholderUah = "0.00 ₴";

    // Exchange rate
    public const string ExchangeDefaultRate = "1.0";
}
