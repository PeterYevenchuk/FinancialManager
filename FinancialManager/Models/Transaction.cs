using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FinancialManager.Models;

public class Transaction
{
    public Transaction()
    {
        Id = Guid.NewGuid();
    }

    [PrimaryKey]
    public Guid Id { get; set; }

    public double Amount { get; set; }

    public string Description { get; set; }

    public DateTime Date { get; set; }

    [ForeignKey(typeof(Category))]
    public Guid CategoryId { get; set; }

    [ManyToOne]
    public Category Category { get; set; }

    [ForeignKey(typeof(TransactionType))]
    public Guid TransactionTypeId { get; set; }

    [ManyToOne]
    public TransactionType TransactionType { get; set; }
}
