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
        ExchangeRateToUah = 1.0;
    }

    [PrimaryKey]
    public Guid Id { get; set; }

    public double Amount { get; set; }

    public string Currency { get; set; } = "₴";

    public double ExchangeRateToUah { get; set; } = 1.0;

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

    [Ignore]
    public string ExchangeRateToUahDisplay => $"{ExchangeRateToUah:N2}";

    [Ignore]
    public bool ShowExchangeRate => Currency != "₴" ? true : false;

    public double GetAmountInUah(Dictionary<string, double> currentRatesFallback)
    {
        if (Currency == "₴") return Amount;
        if (ExchangeRateToUah > 0) return Amount * ExchangeRateToUah;
        if (currentRatesFallback != null && currentRatesFallback.TryGetValue(Currency, out double currentRate))
        {
            return Amount * currentRate;
        }
        return Amount;
    }

    [ObservableProperty]
    [property: Ignore]
    private bool isSelected;
}
