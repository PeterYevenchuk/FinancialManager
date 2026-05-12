using FinancialManager.ViewModels;

namespace FinancialManager.Views;

public partial class CategoryPage : ContentPage
{
    public CategoryPage(CategoryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CategoryViewModel viewModel)
        {
            await viewModel.RefreshCategories();
        }
    }
}