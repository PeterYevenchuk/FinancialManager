using FinancialManager.Models;
using SQLite;

namespace FinancialManager.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class, new()
    {
        protected SQLiteAsyncConnection _database;
        private readonly string _dbPath = Path.Combine(FileSystem.AppDataDirectory, "FinData.db");

        protected async Task Init()
        {
            if (_database is not null)
            {
                return;
            }

            _database = new SQLiteAsyncConnection(_dbPath);

            await _database.CreateTablesAsync<TransactionType, Category, Transaction>();
        }

        public virtual async Task<List<T>> GetAsync()
        {
            await Init();
            return await _database.Table<T>().ToListAsync();
        }

        public virtual async Task<T> GetAsync(int id)
        {
            await Init();
            return await _database.FindAsync<T>(id);
        }

        public virtual async Task<int> SaveAsync(T entity)
        {
            await Init();
            var info = _database.GetConnection().GetTableInfo(typeof(T).Name);
            return await _database.InsertOrReplaceAsync(entity);
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            await Init();
            return await _database.DeleteAsync(entity);
        }
    }
}
