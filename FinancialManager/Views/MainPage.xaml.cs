using FinancialManager.ViewModels;

namespace FinancialManager.Views;

public partial class MainPage : ContentPage
{
    // Horizontal travel (device-independent units) required to treat a pan as a month swipe.
    private const double MonthSwipeThreshold = 40;

    private double _panTotalX;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MainViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }

    // PanGestureRecognizer is used instead of SwipeGestureRecognizer because the dashboard
    // lives inside a scrolling CollectionView, where swipe gestures are unreliable: the
    // vertical scroll frequently wins. Pan events fire across the whole card and let us act
    // only when the horizontal movement clearly dominates the gesture.
    private void OnDashboardPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (BindingContext is not MainViewModel viewModel)
        {
            return;
        }

        switch (e.StatusType)
        {
            case GestureStatus.Running:
                // Android reports TotalX == 0 on Completed, so remember the last running value.
                _panTotalX = e.TotalX;
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                if (_panTotalX <= -MonthSwipeThreshold && viewModel.NextMonthCommand.CanExecute(null))
                {
                    viewModel.NextMonthCommand.Execute(null);
                }
                else if (_panTotalX >= MonthSwipeThreshold && viewModel.PreviousMonthCommand.CanExecute(null))
                {
                    viewModel.PreviousMonthCommand.Execute(null);
                }

                _panTotalX = 0;
                break;
        }
    }
}