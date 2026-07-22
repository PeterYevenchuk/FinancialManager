using FinancialManager.ViewModels;

namespace FinancialManager.Views;

public partial class FeaturesPage : ContentPage
{
    private readonly FeaturesViewModel _viewModel;

    public FeaturesPage(FeaturesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}