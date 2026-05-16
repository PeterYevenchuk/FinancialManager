namespace FinancialManager.Views;

public partial class TransactionAddPage : ContentPage
{
    public TransactionAddPage(ViewModels.TransactionAddViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewModels.TransactionAddViewModel vm)
        {
            vm.ResetForm();
            await vm.LoadDataAsync();
        }
    }
}