using CommunityToolkit.Mvvm.Messaging;
using FinancialManager.Services.Messages;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

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
            d["NameEnglishLabel"] = Resources.Strings.NameEnglishLabel;
            d["NameUkrLabel"] = Resources.Strings.NameUkrLabel;
            d["PlaceholderRequired"] = Resources.Strings.PlaceholderRequired;
            d["PlaceholderOptional"] = Resources.Strings.PlaceholderOptional;

            if (Application.Current?.Resources == null)
            {
                Application.Current.Resources = d;
            }
        });
    }
}
