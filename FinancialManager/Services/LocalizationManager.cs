using CommunityToolkit.Mvvm.Messaging;
using FinancialManager.Services.Messages;

namespace FinancialManager.Services;

public interface ILocalizationManager
{
    void Initialize();
}

public class LocalizationManager : ILocalizationManager
{
    private readonly ILocalizationService _localizationService;

    public LocalizationManager(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
        {
            UpdateResources();
        });
    }

    public void Initialize()
    {
        _localizationService.Init();
        UpdateResources();
    }

    private void UpdateResources()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var d = Application.Current?.Resources ?? new Microsoft.Maui.Controls.ResourceDictionary();

            d["WelcomeMessage"] = Resources.Strings.WelcomeMessage;
            d["TransactionsTitle"] = Resources.Strings.TransactionsTitle;
            d["CategoriesTitle"] = Resources.Strings.CategoriesTitle;
            d["TransactionTypesTitle"] = Resources.Strings.TransactionTypesTitle;
            d["FromLabel"] = Resources.Strings.FromLabel;
            d["ToLabel"] = Resources.Strings.ToLabel;

            d["CategoryLabel"] = Resources.Strings.CategoryLabel;
            d["TypeLabel"] = Resources.Strings.TypeLabel;
            d["FromLabel"] = Resources.Strings.FromLabel;
            d["ToLabel"] = Resources.Strings.ToLabel;
            d["Apply"] = Resources.Strings.Apply;
            d["ClearFilters"] = Resources.Strings.ClearFilters;
            d["Edit"] = Resources.Strings.Edit;
            d["Delete"] = Resources.Strings.Delete;
            d["Plus"] = Resources.Strings.Plus;

            d["ManageTransactionTitle"] = Resources.Strings.ManageTransactionTitle;
            d["AmountLabel"] = Resources.Strings.AmountLabel;
            d["PlaceholderAmount"] = Resources.Strings.PlaceholderAmount;
            d["DescriptionLabel"] = Resources.Strings.DescriptionLabel;
            d["PlaceholderDescription"] = Resources.Strings.PlaceholderDescription;
            d["SelectCategory"] = Resources.Strings.SelectCategory;
            d["SelectType"] = Resources.Strings.SelectType;
            d["DateLabel"] = Resources.Strings.DateLabel;
            d["Save"] = Resources.Strings.Save;

            d["ManageTypeTitle"] = Resources.Strings.ManageTypeTitle;

            d["AddNewCategoryTitle"] = Resources.Strings.AddNewCategoryTitle;
            d["CategoryIconLabel"] = Resources.Strings.CategoryIconLabel;
            d["PlaceholderIcon"] = Resources.Strings.PlaceholderIcon;
            d["TransactionType_IconLabel"] = Resources.Strings.TransactionType_IconLabel;
            d["NameEnglishLabel"] = Resources.Strings.NameEnglishLabel;
            d["NameUkrLabel"] = Resources.Strings.NameUkrLabel;
            d["PlaceholderRequired"] = Resources.Strings.PlaceholderRequired;
            d["PlaceholderOptional"] = Resources.Strings.PlaceholderOptional;

            // AppShell / general
            d["HomeTitle"] = Resources.Strings.HomeTitle;
            d["LanguageLabel"] = Resources.Strings.LanguageLabel;
            d["BackupTitle"] = Resources.Strings.BackupTitle;

            // Backup page
            d["Backup_SecurityHeading"] = Resources.Strings.Backup_SecurityHeading;
            d["Backup_SecurityDesc"] = Resources.Strings.Backup_SecurityDesc;
            d["Backup_ExportTitle"] = Resources.Strings.Backup_ExportTitle;
            d["Backup_ExportDesc"] = Resources.Strings.Backup_ExportDesc;
            d["Backup_ExportButton"] = Resources.Strings.Backup_ExportButton;
            d["Backup_ImportTitle"] = Resources.Strings.Backup_ImportTitle;
            d["Backup_ImportDesc"] = Resources.Strings.Backup_ImportDesc;
            d["Backup_ImportButton"] = Resources.Strings.Backup_ImportButton;
            d["Backup_Processing"] = Resources.Strings.Backup_Processing;

            // Main / Dashboard
            d["DashboardTitle"] = Resources.Strings.DashboardTitle;
            d["TotalBalanceLabel"] = Resources.Strings.TotalBalanceLabel;
            d["IncomeLabel"] = Resources.Strings.IncomeLabel;
            d["ExpenseLabel"] = Resources.Strings.ExpenseLabel;
            d["SavingsLabel"] = Resources.Strings.SavingsLabel;
            d["OthersLabel"] = Resources.Strings.OthersLabel;
            d["TransactionTypeHeader"] = Resources.Strings.TransactionTypeHeader;
            d["ChartPlaceholder"] = Resources.Strings.ChartPlaceholder;
            d["ChartSubtitle"] = Resources.Strings.ChartSubtitle;
            d["TransactionsPeriodLabel"] = Resources.Strings.TransactionsPeriodLabel;
            d["Reset"] = Resources.Strings.Reset;
            d["ReportCurrencyLabel"] = Resources.Strings.ReportCurrencyLabel;
            d["ExchangeRateLabel"] = Resources.Strings.ExchangeRateLabel;
            d["PlaceholderExchangeRate"] = Resources.Strings.PlaceholderExchangeRate;
            d["InvalidExchangeRateTitle"] = Resources.Strings.InvalidExchangeRateTitle;
            d["InvalidExchangeRateMessage"] = Resources.Strings.InvalidExchangeRateMessage;

            if (Application.Current?.Resources == null)
            {
                Application.Current.Resources = d;
            }
        });
    }
}
