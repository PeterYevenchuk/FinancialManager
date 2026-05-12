using FinancialManager.ViewModels;

namespace FinancialManager.Views;

public partial class CategoryAddPage : ContentPage
{
    public CategoryAddPage(CategoryAddViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}