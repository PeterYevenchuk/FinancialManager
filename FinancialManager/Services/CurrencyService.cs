using FinancialManager.Helpers;
using FinancialManager.Services.Contracts;
using System.Text.Json;

namespace FinancialManager.Services;

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;

    public CurrencyService()
    {
        _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
    }

    public async Task<Dictionary<string, double>> GetLatestRatesAsync()
    {
        var rates = new Dictionary<string, double> { { StaticData.UahCurrency, 1.0 } };

        try
        {
            var response = await _httpClient.GetStringAsync(StaticData.NbuExchangeRateUrl);

            using var doc = JsonDocument.Parse(response);
            foreach (var element in doc.RootElement.EnumerateArray())
            {
                string cc = element.GetProperty(StaticData.CurrencyCodeProperty).GetString(); // "USD", "EUR"
                double rate = element.GetProperty(StaticData.RateProperty).GetDouble();

                if (cc == StaticData.UsdCode) rates[StaticData.UsdCurrency] = rate;
                if (cc == StaticData.EurCode) rates[StaticData.EurCurrency] = rate;
            }
        }
        catch
        {

        }

        return rates;
    }
}
