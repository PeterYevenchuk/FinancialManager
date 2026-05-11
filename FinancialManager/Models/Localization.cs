using SQLite;

namespace FinancialManager.Models
{
    public class Localization
    {
        [PrimaryKey, AutoIncrement]
        public Guid Id { get; set; }

        [Indexed]
        public Guid ParentId { get; set; }

        public string LanguageCode { get; set; } // "uk", "en"

        public string Value { get; set; }
    }
}
