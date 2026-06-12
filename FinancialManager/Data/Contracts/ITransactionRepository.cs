using FinancialManager.Models;

namespace FinancialManager.Data.Contracts;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<List<Transaction>> GetTransactionsWithDetailsAsync();
}
