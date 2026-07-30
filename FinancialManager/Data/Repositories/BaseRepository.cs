using FinancialManager.Data.Contracts;
using SQLite;

namespace FinancialManager.Data.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : class, new()
{
    protected readonly SQLiteAsyncConnection _database;

    protected BaseRepository(SQLiteAsyncConnection database)
    {
        _database = database;
    }

    protected Task Init()
    {
        return DatabaseSchema.EnsureCreatedAsync(_database);
    }

    public virtual async Task<List<T>> GetAsync()
    {
        await Init();
        return await _database.Table<T>().ToListAsync();
    }

    public virtual async Task<int> SaveAsync(T entity)
    {
        await Init();
        return await _database.InsertOrReplaceAsync(entity);
    }

    public virtual async Task<int> DeleteAsync(T entity)
    {
        await Init();
        return await _database.DeleteAsync(entity);
    }
}
