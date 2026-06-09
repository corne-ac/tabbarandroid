using TabbedPageBottomGapRepro.Pages;
using TabbedPageBottomGapRepro.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TabbedPageBottomGapRepro;

public partial class App : Application
{
    private readonly INavigationService _navigationService;
    private readonly IServiceProvider _services;

    public App(INavigationService navigationService, IServiceProvider services)
    {
        InitializeComponent();
        _navigationService = navigationService;
        _services = services;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Resolve the first page after App resources have been loaded.
        var loginPage = _services.GetRequiredService<LoginPage>();

        // No AppShell — the app starts with a NavigationPage wrapping LoginPage.
        // After login, the NavigationService resets the root to MainTabbedPage.
        var window = new Window(new NavigationPage(loginPage));

        // Initialize the navigation service with the window so it can manage
        // all future push/pop/root-reset operations.
        ((NavigationService)_navigationService).Initialize(window);

        return window;
    }
}
