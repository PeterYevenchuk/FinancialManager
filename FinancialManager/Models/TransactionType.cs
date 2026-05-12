using SQLite;

namespace FinancialManager.Models;

public class TransactionType
{
    public TransactionType()
    {
        Id = Guid.NewGuid();
    }

    [PrimaryKey]
    public Guid Id { get; set; }

    [Ignore]
    public string LocalizedName { get; set; }
}
