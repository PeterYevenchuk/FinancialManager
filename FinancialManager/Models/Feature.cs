using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace FinancialManager.Models;

public partial class Feature : ObservableObject
{
    public Feature()
    {
        Id = Guid.NewGuid();
    }

    [PrimaryKey]
    public Guid Id { get; set; }

    [Indexed(Unique = true)]
    public string Key { get; set; } = string.Empty;

    [ObservableProperty]
    private bool isEnabled;

    [Ignore]
    public string LocalizedName { get; set; } = string.Empty;

    [Ignore]
    public string LocalizedDescription { get; set; } = string.Empty;
}
