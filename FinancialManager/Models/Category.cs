using SQLite;

namespace FinancialManager.Models;

public class Category
{
    public Category()
    {
        Id = Guid.NewGuid();
    }

    [PrimaryKey]
    public Guid Id { get; set; }

    public string Icon { get; set; }

    [Ignore]
    public string LocalizedName { get; set; }
}
