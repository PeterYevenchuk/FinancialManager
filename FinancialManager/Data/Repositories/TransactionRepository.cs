using FinancialManager.Models;
using SQLiteNetExtensionsAsync.Extensions;

namespace FinancialManager.Data.Repositories;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<List<Transaction>> GetTransactionsWithDetailsAsync();
}

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public async Task<List<Transaction>> GetTransactionsWithDetailsAsync()
    {
        await Init();
        return await _database.GetAllWithChildrenAsync<Transaction>();
    }
}
