using FinancialManager.Models;
using SQLite;

namespace FinancialManager.Data;

/// <summary>
/// Creates the SQLite schema exactly once per process. Repositories and the seeder
/// share this gate so tables are created a single time instead of on every data call,
/// while still guarding against the fire-and-forget seeding at app startup.
/// </summary>
internal static class DatabaseSchema
{
    private static readonly object Gate = new();
    private static Task? _initialization;

    public static Task EnsureCreatedAsync(SQLiteAsyncConnection database)
    {
        if (_initialization != null)
        {
            return _initialization;
        }

        lock (Gate)
        {
            _initialization ??= database.CreateTablesAsync<TransactionType, Category, Transaction, Localization, Feature>();
        }

        return _initialization;
    }
}
