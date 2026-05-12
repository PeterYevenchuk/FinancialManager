using FinancialManager.Models;

namespace FinancialManager.Data.Repositories;

public interface ITransactionTypeRepository : IRepository<TransactionType> { }

public class TransactionTypeRepository : BaseRepository<TransactionType>, ITransactionTypeRepository
{

}
