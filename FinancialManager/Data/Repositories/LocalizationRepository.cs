using FinancialManager.Models;
using SQLite;

namespace FinancialManager.Data.Repositories;

public interface ILocalizationRepository : IRepository<Localization> { }

public class LocalizationRepository : BaseRepository<Localization>, ILocalizationRepository
{
    public LocalizationRepository(SQLiteAsyncConnection database) : base(database) { }
}
