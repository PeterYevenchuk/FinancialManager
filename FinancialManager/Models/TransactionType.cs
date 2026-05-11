using SQLite;

namespace FinancialManager.Models
{
    public class TransactionType
    {
        [PrimaryKey, AutoIncrement]
        public Guid Id { get; set; }

        [Ignore]
        public string LocalizedName { get; set; }
    }
}
