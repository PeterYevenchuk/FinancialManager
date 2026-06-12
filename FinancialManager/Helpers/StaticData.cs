namespace FinancialManager.Helpers;

public static class StaticData
{
    // Language codes
    public static readonly string EnCode = "en";
    public static readonly string UkCode = "uk";
    public static readonly string EnName = "🇺🇸 English";
    public static readonly string UkName = "🇺🇦 Українська";
    public static readonly string DefaultLanguage = "en";
    public static readonly string LanguageKey = "selected_language";

    // Currency symbols
    public static readonly string UahCurrency = "₴";
    public static readonly string UsdCurrency = "$";
    public static readonly string EurCurrency = "€";

    // Currency codes
    public static readonly string UsdCode = "USD";
    public static readonly string EurCode = "EUR";

    // API endpoints
    public static readonly string NbuExchangeRateUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";

    // JSON property names
    public static readonly string CurrencyCodeProperty = "cc";
    public static readonly string RateProperty = "rate";

    // Icons
    public static readonly string DefaultIcon = "✨";

    // Balance placeholder
    public static readonly string BalancePlaceholderUah = "0.00 ₴";

    // Exchange rate
    public static readonly string ExchangeDefaultRate = "1.0";
}
