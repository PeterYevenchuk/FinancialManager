using FinancialManager.Models;
using FinancialManager.Services.Contracts;
using System.Text;

namespace FinancialManager.Services;

public class ExportService : IExportService
{
    public async Task ExportTransactionsAsync(IEnumerable<Transaction> transactions, string fileName = "transactions_export.csv")
    {
        if (transactions == null || !transactions.Any())
            return;

        var sb = new StringBuilder();

        sb.AppendLine(Resources.Strings.ExportCsvHeader);

        foreach (var t in transactions)
        {
            string dateStr = t.Date.ToString("dd.MM.yyyy");
            string category = t.Category?.LocalizedName ?? Resources.Strings.ExportNoCategory;
            string type = t.TransactionType?.LocalizedName ?? Resources.Strings.ExportNoType;
            string amount = t.Amount.ToString("F2");
            string currency = t.Currency ?? string.Empty;
            string exchangeRateToUah = t.ExchangeRateToUahDisplay ?? string.Empty;
            string desc = t.Description ?? string.Empty;

            category = EscapeCsvField(category);
            type = EscapeCsvField(type);

            sb.AppendLine($"{dateStr};{category};{type};{desc};{amount};{currency};{exchangeRateToUah}");
        }

        string tempFilePath = Path.Combine(FileSystem.CacheDirectory, fileName);

        await File.WriteAllTextAsync(tempFilePath, sb.ToString(), new UTF8Encoding(true));

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = Resources.Strings.ExportTitle,
            File = new ShareFile(tempFilePath)
        });
    }

    private static string EscapeCsvField(string field)
    {
        if (field.Contains(';') || field.Contains('"') || field.Contains('\n'))
        {
            field = field.Replace("\"", "\"\"");
            return $"\"{field}\"";
        }
        return field;
    }
}
