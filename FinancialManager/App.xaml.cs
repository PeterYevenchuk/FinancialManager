using FinancialManager.Services;

namespace FinancialManager;

public partial class App : Application
{
    private readonly AppShell _appShell;

    public App(ILocalizationService localizationService, AppShell appShell)
    {
        InitializeComponent();

        localizationService.Init();
        _appShell = appShell;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_appShell);
    }
}