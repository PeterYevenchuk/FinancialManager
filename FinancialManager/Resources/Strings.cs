using Microsoft.Maui.Storage;

namespace Resources
{
    public static class Strings
    {
        private static string CurrentLanguage => Preferences.Default.Get("selected_language", "en");

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
    }
}
