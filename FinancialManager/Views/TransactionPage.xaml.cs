namespace FinancialManager.Views;

public partial class TransactionPage : ContentPage
{
    public TransactionPage(ViewModels.TransactionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewModels.TransactionViewModel vm)
        {
            await vm.InitAsync();
        }
    }
}