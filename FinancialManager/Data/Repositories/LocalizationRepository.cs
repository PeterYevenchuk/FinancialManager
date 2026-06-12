using FinancialManager.Data.Contracts;
using FinancialManager.Models;
using SQLite;

namespace FinancialManager.Data.Repositories;

public class LocalizationRepository : BaseRepository<Localization>, ILocalizationRepository
{
    public LocalizationRepository(SQLiteAsyncConnection database) : base(database) { }
}
