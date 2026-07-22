using FinancialManager.Models;


namespace FinancialManager.Services.Contracts;

public interface IExportService
{
    Task ExportTransactionsAsync(IEnumerable<Transaction> transactions, string fileName = "transactions_export.csv");
}
