using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace FinancialManager.Models;

public partial class TransactionType : ObservableObject, ILocalizable
{
    public TransactionType()
    {
        Id = Guid.NewGuid();
    }

    [PrimaryKey]
    public Guid Id { get; set; }

    public bool IsSystem { get; set; }

    public string Icon { get; set; }

    [Ignore]
    public string LocalizedName { get; set; }

    [ObservableProperty]
    [property: Ignore]
    private bool isSelected;
}
