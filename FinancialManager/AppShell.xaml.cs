namespace FinancialManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.CategoryAddPage), typeof(Views.CategoryAddPage));
        }
    }
}
