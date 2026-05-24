using FinancialManager.ViewModels;

namespace FinancialManager.Views;

public partial class BackupPage : ContentPage
{
    public BackupPage(BackupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}