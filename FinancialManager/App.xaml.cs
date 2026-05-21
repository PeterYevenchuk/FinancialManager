using FinancialManager.Services;

namespace FinancialManager;

public partial class App : Application
{
    private readonly AppShell _appShell;

    public App(ILocalizationManager localizationManager, AppShell appShell)
    {
        InitializeComponent();
        _appShell = appShell;
        localizationManager?.Initialize();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_appShell);
    }
}