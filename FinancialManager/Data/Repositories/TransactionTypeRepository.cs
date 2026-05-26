using FinancialManager.Models;
using SQLite;

namespace FinancialManager.Data.Repositories;

public interface ITransactionTypeRepository : IRepository<TransactionType> 
{
    Task DeleteTransactionTypeAsync(TransactionType type);
}

public class TransactionTypeRepository : BaseRepository<TransactionType>, ITransactionTypeRepository
{
    public TransactionTypeRepository(SQLiteAsyncConnection database) : base(database) { }

    public async Task DeleteTransactionTypeAsync(TransactionType type)
    {
        if (type.IsSystem)
        {
            throw new InvalidOperationException(Resources.Strings.SystemTransactionTypeDeleteForbidden);
        }

        var defaultType = await _database.Table<TransactionType>()
            .FirstOrDefaultAsync(t => t.IsSystem && t.Icon == "🔄");

        if (defaultType != null)
        {
            var affectedTransactions = await _database.Table<Transaction>()
                .Where(t => t.TransactionTypeId == type.Id)
                .ToListAsync();

            foreach (var transaction in affectedTransactions)
            {
                transaction.TransactionTypeId = defaultType.Id;
                await _database.UpdateAsync(transaction);
            }
        }

        await _database.DeleteAsync(type);
    }
}
