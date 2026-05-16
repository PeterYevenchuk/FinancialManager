using FinancialManager.ViewModels;

namespace FinancialManager.Views;

public partial class TransactionTypeAddPage : ContentPage
{
	public TransactionTypeAddPage(TransactionTypeAddViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}