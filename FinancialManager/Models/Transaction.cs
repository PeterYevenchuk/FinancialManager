using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FinancialManager.Models;

public partial class Transaction : ObservableObject
{
    public Transaction()
    {
        Id = Guid.NewGuid();
        Date = DateTime.Now;
        Currency = "₴";
    }

    [PrimaryKey]
    public Guid Id { get; set; }

    public double Amount { get; set; }
    public string Currency { get; set; } = "₴";

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

    [Ignore]
    public string AmountDisplay => $"{Amount:N2} {Currency ?? "₴"}";

    [ObservableProperty]
    [property: Ignore]
    private bool isSelected;
}
