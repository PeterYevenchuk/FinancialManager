namespace FinancialManager.Data.Contracts;

public interface IRepository<T> where T : class, new()
{
    Task<List<T>> GetAsync();
    Task<int> SaveAsync(T entity);
    Task<int> DeleteAsync(T entity);
}
