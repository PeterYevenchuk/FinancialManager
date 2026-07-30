using FinancialManager.Helpers;
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
        await DatabaseSchema.EnsureCreatedAsync(_connection);

        var typeCount = await _connection.Table<TransactionType>().CountAsync();
        if (typeCount == 0)
        {
            var defaultTypes = new List<(string Icon, string Uk, string En)>
            {
                (StaticData.IncomeIcon, "Прибуток", "Income"),
                (StaticData.ExpenseIcon, "Витрати", "Expenses"),
                (StaticData.SavingsIcon, "Заощадження", "Savings"),
                (StaticData.OtherTypeIcon, "Інше", "Other")
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
                (StaticData.DefaultCategoryIcon, "Інше", "Other")
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

        var defaultFeatures = new List<(string Key, string UkName, string EnName, string UkDesc, string EnDesc)>
        {
            (
                "ExportData",
                "Експорт даних",
                "Export Data",
                "Можливість експортувати інформацію з головної сторінки у CSV або Excel для аналізу.",
                "Export main page data into CSV or Excel format for analysis."
            )
        };

        foreach (var feat in defaultFeatures)
        {
            var existing = await _connection.Table<Feature>().FirstOrDefaultAsync(f => f.Key == feat.Key);
            if (existing == null)
            {
                var newFeature = new Feature
                {
                    Key = feat.Key,
                    IsEnabled = false
                };
                await _connection.InsertAsync(newFeature);

                var locUk = new Localization { ParentId = newFeature.Id, LanguageCode = "uk", Value = feat.UkName };
                var locEn = new Localization { ParentId = newFeature.Id, LanguageCode = "en", Value = feat.EnName };

                await _connection.InsertAsync(locUk);
                await _connection.InsertAsync(locEn);

                var locDescUk = new Localization { ParentId = newFeature.Id, LanguageCode = "uk", Value = feat.UkDesc };
                var locDescEn = new Localization { ParentId = newFeature.Id, LanguageCode = "en", Value = feat.EnDesc };

                await _connection.InsertAsync(locDescUk);
                await _connection.InsertAsync(locDescEn);
            }
        }
    }
}
