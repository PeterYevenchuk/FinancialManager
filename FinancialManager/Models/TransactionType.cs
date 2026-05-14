using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace FinancialManager.Models;

public partial class TransactionType : ObservableObject
{
    public TransactionType()
    {
        Id = Guid.NewGuid();
    }

    [PrimaryKey]
    public Guid Id { get; set; }

    [Ignore]
    public string LocalizedName { get; set; }

    [ObservableProperty]
    [property: Ignore]
    private bool isSelected;
}
