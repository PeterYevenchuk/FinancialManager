namespace FinancialManager.Services.Contracts;

public interface ILocalizationService
{
    string CurrentLanguage { get; set; }
    void Init();
}
