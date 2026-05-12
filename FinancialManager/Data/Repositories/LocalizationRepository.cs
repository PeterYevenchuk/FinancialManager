using FinancialManager.Models;

namespace FinancialManager.Data.Repositories;

public interface ILocalizationRepository : IRepository<Localization> { }

public class LocalizationRepository : BaseRepository<Localization>, ILocalizationRepository
{
    
}
