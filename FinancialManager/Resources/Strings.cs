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
        public static string FromLabel => CurrentLanguage == "uk" ? "З" : "From";
        public static string ToLabel => CurrentLanguage == "uk" ? "По" : "To";
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
    }
}
