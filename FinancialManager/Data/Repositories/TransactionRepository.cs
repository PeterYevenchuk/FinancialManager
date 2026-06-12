using FinancialManager.Data.Contracts;
using FinancialManager.Models;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace FinancialManager.Data.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(SQLiteAsyncConnection database) : base(database) { }

    public async Task<List<Transaction>> GetTransactionsWithDetailsAsync()
    {
        await Init();
        return await _database.GetAllWithChildrenAsync<Transaction>();
    }
}
