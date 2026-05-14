using FinancialManager.ViewModels;

namespace FinancialManager.Views;

public partial class TransactionTypePage : ContentPage
{
	public TransactionTypePage(TransactionTypeViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is TransactionTypeViewModel viewModel)
        {
            await viewModel.RefreshTransactionTypes();
        }
    }
}