using FinancialManager.Models;

namespace FinancialManager.Data.Contracts;

public interface ICategoryRepository : IRepository<Category>
{
    Task DeleteCategoryAsync(Category category);
}
