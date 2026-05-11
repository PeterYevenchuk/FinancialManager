using SQLite;

namespace FinancialManager.Models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public Guid Id { get; set; }

        public string Icon { get; set; }

        [Ignore]
        public string LocalizedName { get; set; }
    }
}
