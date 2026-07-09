using Microsoft.Maui.Storage;

namespace Resources
{
    public static class Strings
    {
        private static string CurrentLanguage => Preferences.Default.Get("selected_language", "en");

        // Existing
        public static string DeleteTitle => CurrentLanguage == "uk" ? "Видалення" : "Delete";
        public static string DeleteTransactionMessage => CurrentLanguage == "uk" ? "Видалити цю транзакцію?" : "Delete this transaction?";
        public static string DeleteCategoryMessage => CurrentLanguage == "uk" ? "Ви впевнені, що хочете видалити категорію {0}?" : "Are you sure you want to delete category {0}?";
        public static string DeleteTransactionTypeMessage => CurrentLanguage == "uk" ? "Ви впевнені, що хочете видалити тип транзакції {0}?" : "Are you sure you want to delete transaction type {0}?";
        public static string Yes => CurrentLanguage == "uk" ? "Так" : "Yes";
        public static string No => CurrentLanguage == "uk" ? "Ні" : "No";
        public static string NoName => CurrentLanguage == "uk" ? "Без назви" : "No Name";
        public static string NoType => CurrentLanguage == "uk" ? "Без типу" : "No Type";
        public static string Error => CurrentLanguage == "uk" ? "Помилка" : "Error";
        public static string EnglishNameRequired => CurrentLanguage == "uk" ? "Потрібна англійська назва!" : "English name is required!";
        public static string Ok => CurrentLanguage == "uk" ? "ОК" : "OK";

        // UI strings
        public static string WelcomeMessage => CurrentLanguage == "uk" ? "Ласкаво просимо в .NET MAUI!" : "Welcome to .NET MAUI!";

        public static string TransactionsTitle => CurrentLanguage == "uk" ? "Транзакції" : "Transactions";
        public static string CategoriesTitle => CurrentLanguage == "uk" ? "Категорії" : "Categories";
        public static string TransactionTypesTitle => CurrentLanguage == "uk" ? "Типи транзакцій" : "Transaction Types";

        public static string CategoryLabel => CurrentLanguage == "uk" ? "Категорія" : "Category";
        public static string TypeLabel => CurrentLanguage == "uk" ? "Тип" : "Type";
        public static string FromLabel => CurrentLanguage == "uk" ? "З:" : "From:";
        public static string ToLabel => CurrentLanguage == "uk" ? "По:" : "To:";
        public static string Apply => CurrentLanguage == "uk" ? "Застосувати" : "Apply";
        public static string ClearFilters => CurrentLanguage == "uk" ? "Очистити фільтри" : "Clear Filters";
        public static string Edit => CurrentLanguage == "uk" ? "✏️ Редагувати" : "✏️ Edit";
        public static string Delete => CurrentLanguage == "uk" ? "🗑️ Видалити" : "🗑️ Delete";

        public static string Plus => "+";

        // TransactionAdd / CategoryAdd / TypeAdd
        public static string ManageTransactionTitle => CurrentLanguage == "uk" ? "Керування транзакцією" : "Manage Transaction";
        public static string AmountLabel => CurrentLanguage == "uk" ? "Сума *" : "Amount *";
        public static string PlaceholderAmount => "0.00";
        public static string AmountMustBeGreaterThanZero => CurrentLanguage == "uk" ? "Сума повинна бути більшою за 0" : "Amount must be greater than 0";
        public static string SelectCategoryAndType => CurrentLanguage == "uk" ? "Виберіть категорію та тип транзакції" : "Select category and transaction type";
        public static string DescriptionLabel => CurrentLanguage == "uk" ? "Опис" : "Description";
        public static string PlaceholderDescription => CurrentLanguage == "uk" ? "наприклад: обід, бензин" : "e.g. Dinner, Gas";
        public static string SelectCategory => CurrentLanguage == "uk" ? "Вибрати категорію" : "Select Category";
        public static string SelectType => CurrentLanguage == "uk" ? "Вибрати тип" : "Select Type";
        public static string DateLabel => CurrentLanguage == "uk" ? "Дата" : "Date";
        public static string Save => CurrentLanguage == "uk" ? "Зберегти" : "Save";

        public static string ManageTypeTitle => CurrentLanguage == "uk" ? "Керування типом" : "Manage Type";

        public static string AddNewCategoryTitle => CurrentLanguage == "uk" ? "Додати нову категорію" : "Add New Category";
        public static string CategoryIconLabel => CurrentLanguage == "uk" ? "Іконка категорії (Emoji або текст)" : "Category Icon (Emoji or Text)";
        public static string PlaceholderIcon => "e.g. 🍕";
        public static string NameEnglishLabel => CurrentLanguage == "uk" ? "Назва (англійською) *" : "Name (English) *";
        public static string NameUkrLabel => CurrentLanguage == "uk" ? "Назва (Українська)" : "Назва (Українська)";
        public static string PlaceholderRequired => CurrentLanguage == "uk" ? "Обов'язково" : "Required";
        public static string PlaceholderOptional => CurrentLanguage == "uk" ? "Необов'язково" : "Optional";
        // Repository messages
        public static string SystemCategoryDeleteForbidden => CurrentLanguage == "uk" ? "Системні категорії видаляти заборонено!" : "System categories cannot be deleted!";
        public static string SystemTransactionTypeDeleteForbidden => CurrentLanguage == "uk" ? "Системні типи транзакцій видаляти заборонено!" : "System transaction types cannot be deleted!";

        // AppShell / Home
        public static string HomeTitle => CurrentLanguage == "uk" ? "Головна" : "Home";
        public static string LanguageLabel => CurrentLanguage == "uk" ? "Мова" : "Language";
        public static string BackupTitle => CurrentLanguage == "uk" ? "Бекап даних" : "Backup";

        // Backup page texts
        public static string Backup_SecurityHeading => CurrentLanguage == "uk" ? "Безпека ваших даних" : "Your data security";
        public static string Backup_SecurityDesc => CurrentLanguage == "uk" ? "Керуйте резервними копіями вашого гаманця. Усі дані експортуються у безпечному текстовому форматі JSON." : "Manage backups of your wallet. All data is exported in a safe JSON text format.";

        public static string Backup_ExportTitle => CurrentLanguage == "uk" ? "Створити копію" : "Create backup";
        public static string Backup_ExportDesc => CurrentLanguage == "uk" ? "Зберегти поточні транзакції, категорії та локалізації у файл." : "Save current transactions, categories and localizations to a file.";
        public static string Backup_ExportButton => CurrentLanguage == "uk" ? "Експортувати в JSON" : "Export to JSON";

        public static string Backup_ImportTitle => CurrentLanguage == "uk" ? "Відновити дані" : "Restore data";
        public static string Backup_ImportDesc => CurrentLanguage == "uk" ? "Завантажити дані з раніше створеного файлу бекапу." : "Upload data from a previously created backup file.";
        public static string Backup_ImportButton => CurrentLanguage == "uk" ? "Імпортувати файл" : "Import file";

        public static string Backup_Processing => CurrentLanguage == "uk" ? "Обробка даних..." : "Processing data...";
        // TransactionType icon
        public static string TransactionType_IconLabel => CurrentLanguage == "uk" ? "Іконка типу (Emoji або текст)" : "Type Icon (Emoji or Text)";

        // Main / Dashboard
        public static string DashboardTitle => CurrentLanguage == "uk" ? "Дашборд" : "Dashboard";
        public static string TotalBalanceLabel => CurrentLanguage == "uk" ? "Загальний баланс" : "Total balance";
        public static string IncomeLabel => CurrentLanguage == "uk" ? "📥 Прибутки" : "📥 Income";
        public static string ExpenseLabel => CurrentLanguage == "uk" ? "📤 Витрати" : "📤 Expenses";
        public static string SavingsLabel => CurrentLanguage == "uk" ? "🐷 Заощадження" : "🐷 Savings";
        public static string OthersLabel => CurrentLanguage == "uk" ? "🔄 Інше" : "🔄 Other";
        public static string TransactionTypeHeader => CurrentLanguage == "uk" ? "Тип транзакції" : "Transaction type";
        public static string ChartPlaceholder => CurrentLanguage == "uk" ? "📊 Тут буде твій графік" : "📊 Your chart will be here";
        public static string ChartSubtitle => CurrentLanguage == "uk" ? "(Згруповано за категоріями)" : "(Grouped by categories)";
        public static string TransactionsPeriodLabel => CurrentLanguage == "uk" ? "Транзакції за період" : "Transactions for period";
        public static string Reset => CurrentLanguage == "uk" ? "Скинути" : "Reset";
        public static string ReportCurrencyLabel => CurrentLanguage == "uk" ? "Валюта звіту:" : "Report currency:";
        public static string ExchangeRateLabel => CurrentLanguage == "uk" ? "Курс валюти до UAH" : "Exchange rate to UAH";
        public static string PlaceholderExchangeRate => CurrentLanguage == "uk" ? "Введіть курс" : "Enter rate";
        public static string InvalidExchangeRateTitle => CurrentLanguage == "uk" ? "Помилка" : "Error";
        public static string InvalidExchangeRateMessage => CurrentLanguage == "uk" ? "Будь ласка, введіть коректний курс валюти." : "Please enter a valid currency exchange rate.";
        // Backup related
        public static string Backup_SaveTitle => CurrentLanguage == "uk" ? "Зберегти бекап" : "Save backup";
        public static string Backup_PickTitle => CurrentLanguage == "uk" ? "Оберіть файл бекапу (.json)" : "Pick backup file (.json)";
        public static string Backup_ReplaceConfirmation => CurrentLanguage == "uk" ? "Це повністю замінить твої поточні дані. Продовжуємо?" : "This will completely replace your current data. Continue?";
        public static string Backup_RestoreSuccess => CurrentLanguage == "uk" ? "Дані відновлено!" : "Data restored!";
        public static string Warning => CurrentLanguage == "uk" ? "Увага" : "Warning";
        public static string Success => CurrentLanguage == "uk" ? "Успіх" : "Success";
        public static string ImportErrorTitle => CurrentLanguage == "uk" ? "Помилка імпорту" : "Import error";
        // Default seed translations for transaction types
        public static string DefaultType_Income_Uk => "Прибуток";
        public static string DefaultType_Income_En => "Income";
        public static string DefaultType_Expenses_Uk => "Витрати";
        public static string DefaultType_Expenses_En => "Expenses";
        public static string DefaultType_Savings_Uk => "Заощадження";
        public static string DefaultType_Savings_En => "Savings";
        public static string DefaultType_Other_Uk => "Інше";
        public static string DefaultType_Other_En => "Other";

        // Default seed translations for categories
        public static string DefaultCategory_Groceries_Uk => "Продукти";
        public static string DefaultCategory_Groceries_En => "Groceries";
        public static string DefaultCategory_Transport_Uk => "Транспорт";
        public static string DefaultCategory_Transport_En => "Transport";
        public static string DefaultCategory_Utilities_Uk => "Комунальні послуги";
        public static string DefaultCategory_Utilities_En => "Utilities";
        public static string DefaultCategory_Salary_Uk => "Зарплата";
        public static string DefaultCategory_Salary_En => "Salary";
        public static string DefaultCategory_Cafes_Uk => "Кафе та ресторани";
        public static string DefaultCategory_Cafes_En => "Cafes & Restaurants";
        public static string DefaultCategory_Health_Uk => "Здоров'я та медицина";
        public static string DefaultCategory_Health_En => "Health & Medical";
        public static string DefaultCategory_Entertainment_Uk => "Розваги та дозвілля";
        public static string DefaultCategory_Entertainment_En => "Entertainment";
        public static string DefaultCategory_Shopping_Uk => "Покупки";
        public static string DefaultCategory_Shopping_En => "Shopping";
        public static string DefaultCategory_Housing_Uk => "Житло / Оренда";
        public static string DefaultCategory_Housing_En => "Housing / Rent";
        public static string DefaultCategory_Travel_Uk => "Подорожі";
        public static string DefaultCategory_Travel_En => "Travel";
        public static string DefaultCategory_Education_Uk => "Освіта";
        public static string DefaultCategory_Education_En => "Education";
        public static string DefaultCategory_Other_Uk => "Інше";
        public static string DefaultCategory_Other_En => "Other";

        // Sort options
        public static string SortOption_DateNewest => CurrentLanguage == "uk" ? "Дата: спочатку нові" : "Date: newest first";
        public static string SortOption_DateOldest => CurrentLanguage == "uk" ? "Дата: спочатку старі" : "Date: oldest first";
        public static string SortOption_PriceLower => CurrentLanguage == "uk" ? "Ціна: від меншої" : "Price: lower first";
        public static string SortOption_PriceHigher => CurrentLanguage == "uk" ? "Ціна: від більшої" : "Price: higher first";
    }
}
