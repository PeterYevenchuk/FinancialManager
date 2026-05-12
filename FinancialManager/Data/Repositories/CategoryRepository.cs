using FinancialManager.Models;

namespace FinancialManager.Data.Repositories;

public interface ICategoryRepository : IRepository<Category> { }

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    
}
