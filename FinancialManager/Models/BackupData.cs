namespace FinancialManager.Models;

public class BackupData
{
    public List<Category> Categories { get; set; } = new();
    public List<TransactionType> TransactionTypes { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
    public List<Localization> Localizations { get; set; } = new();
}
