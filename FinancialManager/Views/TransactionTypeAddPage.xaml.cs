using FinancialManager.ViewModels;

namespace FinancialManager.Views;

public partial class TransactionTypeAddPage : ContentPage
{
	public TransactionTypeAddPage(TransactionTypeViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}