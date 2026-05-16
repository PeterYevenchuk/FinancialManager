using FinancialManager.ViewModels;
using FinancialManager.Views;

namespace FinancialManager
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            Routing.RegisterRoute(nameof(CategoryAddPage), typeof(CategoryAddPage));
            Routing.RegisterRoute(nameof(TransactionTypeAddPage), typeof(TransactionTypeAddPage));
            Routing.RegisterRoute(nameof(TransactionAddPage), typeof(TransactionAddPage));
        }
    }
}
