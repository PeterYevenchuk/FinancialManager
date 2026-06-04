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
        var rates = new Dictionary<string, double> { { "₴", 1.0 } };

        try
        {
            string url = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";
            var response = await _httpClient.GetStringAsync(url);

            using var doc = JsonDocument.Parse(response);
            foreach (var element in doc.RootElement.EnumerateArray())
            {
                string cc = element.GetProperty("cc").GetString(); // "USD", "EUR"
                double rate = element.GetProperty("rate").GetDouble();

                if (cc == "USD") rates["$"] = rate;
                if (cc == "EUR") rates["€"] = rate;
            }
        }
        catch
        {

        }

        return rates;
    }
}
