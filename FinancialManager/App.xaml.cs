using FinancialManager.Data;
using FinancialManager.Services.Contracts;

namespace FinancialManager;

public partial class App : Application
{
    private readonly AppShell _appShell;

    public App(ILocalizationManager localizationManager, AppShell appShell, DatabaseInitializer databaseInitializer)
    {
        InitializeComponent();
        _appShell = appShell;
        localizationManager?.Initialize();
        InitializeDatabase(databaseInitializer);
    }

    private async void InitializeDatabase(DatabaseInitializer databaseInitializer)
    {
        try
        {
            await databaseInitializer.InitializeAndSeedAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DB Initialization Error: {ex.Message}");
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_appShell);
    }
}