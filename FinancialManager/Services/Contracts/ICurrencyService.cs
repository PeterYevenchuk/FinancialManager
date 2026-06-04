namespace FinancialManager.Services.Contracts;

public interface ICurrencyService
{
    Task<Dictionary<string, double>> GetLatestRatesAsync();
}
