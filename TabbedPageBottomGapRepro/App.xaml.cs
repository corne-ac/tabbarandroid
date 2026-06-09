using TabbedPageBottomGapRepro.Pages;
using TabbedPageBottomGapRepro.Services;

namespace TabbedPageBottomGapRepro;

public partial class App : Application
{
    private readonly INavigationService _navigationService;

    public App(INavigationService navigationService, LoginPage loginPage)
    {
        InitializeComponent();
        _navigationService = navigationService;
        _loginPage = loginPage;
    }

    private readonly LoginPage _loginPage;

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // No AppShell — the app starts with a NavigationPage wrapping LoginPage.
        // After login, the NavigationService resets the root to MainTabbedPage.
        var window = new Window(new NavigationPage(_loginPage));

        // Initialize the navigation service with the window so it can manage
        // all future push/pop/root-reset operations.
        ((NavigationService)_navigationService).Initialize(window);

        return window;
    }
}
