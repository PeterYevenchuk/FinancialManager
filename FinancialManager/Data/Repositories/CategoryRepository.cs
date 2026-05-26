using FinancialManager.Models;
using SQLite;

namespace FinancialManager.Data.Repositories;

public interface ICategoryRepository : IRepository<Category> 
{
    Task DeleteCategoryAsync(Category category);
}

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(SQLiteAsyncConnection database) : base(database) { }

    public async Task DeleteCategoryAsync(Category category)
    {
        if (category.IsSystem)
        {
            throw new InvalidOperationException(Resources.Strings.SystemCategoryDeleteForbidden);
        }

        var defaultCategory = await _database.Table<Category>()
            .FirstOrDefaultAsync(c => c.IsSystem && c.Icon == "📦");

        if (defaultCategory != null)
        {
            var affectedTransactions = await _database.Table<Transaction>()
                .Where(t => t.CategoryId == category.Id)
                .ToListAsync();

            foreach (var transaction in affectedTransactions)
            {
                transaction.CategoryId = defaultCategory.Id;
                await _database.UpdateAsync(transaction);
            }
        }

        await _database.DeleteAsync(category);
    }
}
