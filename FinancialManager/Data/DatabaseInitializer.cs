using FinancialManager.Models;
using SQLite;

namespace FinancialManager.Data;

public class DatabaseInitializer
{
    private readonly SQLiteAsyncConnection _connection;

    public DatabaseInitializer(SQLiteAsyncConnection connection)
    {
        _connection = connection;
    }

    public async Task InitializeAndSeedAsync()
    {
        await _connection.CreateTablesAsync<TransactionType, Category, Transaction, Localization>();

        var typeCount = await _connection.Table<TransactionType>().CountAsync();
        if (typeCount == 0)
        {
            var defaultTypes = new List<(string Icon, string Uk, string En)>
            {
                ("📥", "Прибуток", "Income"),
                ("📤", "Витрати", "Expenses"),
                ("🐷", "Заощадження", "Savings"),
                ("🔄", "Інше", "Other")
            };

            foreach (var typeData in defaultTypes)
            {
                var newType = new TransactionType 
                {
                    Icon = typeData.Icon,
                    IsSystem = true 
                };

                await _connection.InsertAsync(newType);

                var locUk = new Localization { ParentId = newType.Id, LanguageCode = "uk", Value = typeData.Uk };
                var locEn = new Localization { ParentId = newType.Id, LanguageCode = "en", Value = typeData.En };

                await _connection.InsertAsync(locUk);
                await _connection.InsertAsync(locEn);
            }
        }

        var categoryCount = await _connection.Table<Category>().CountAsync();
        if (categoryCount == 0)
        {
            var defaultCategories = new List<(string Icon, string Uk, string En)>
            {
                ("🛒", "Продукти", "Groceries"),
                ("🚗", "Транспорт", "Transport"),
                ("💡", "Комунальні послуги", "Utilities"),
                ("💰", "Зарплата", "Salary"),
                ("🍔", "Кафе та ресторани", "Cafes & Restaurants"),
                ("💊", "Здоров'я та медицина", "Health & Medical"),
                ("🎬", "Розваги та дозвілля", "Entertainment"),
                ("🛍️", "Покупки", "Shopping"),
                ("🏠", "Житло / Оренда", "Housing / Rent"),
                ("✈️", "Подорожі", "Travel"),
                ("🎓", "Освіта", "Education"),
                ("📦", "Інше", "Other")
            };

            foreach (var catData in defaultCategories)
            {
                var newCategory = new Category
                {
                    Icon = catData.Icon,
                    IsSystem = true
                };
                await _connection.InsertAsync(newCategory);

                var locUk = new Localization { ParentId = newCategory.Id, LanguageCode = "uk", Value = catData.Uk };
                var locEn = new Localization { ParentId = newCategory.Id, LanguageCode = "en", Value = catData.En };

                await _connection.InsertAsync(locUk);
                await _connection.InsertAsync(locEn);
            }
        }
    }
}
