using FinancialManager.Models;

namespace FinancialManager.Data.Contracts;

public interface ITransactionTypeRepository : IRepository<TransactionType>
{
    Task DeleteTransactionTypeAsync(TransactionType type);
}
