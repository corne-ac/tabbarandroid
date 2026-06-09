using TabbedPageBottomGapRepro.Services;

namespace TabbedPageBottomGapRepro.Pages;

public partial class LoginPage : ContentPage
{
    private readonly INavigationService _navigationService;

    public LoginPage(INavigationService navigationService)
    {
        InitializeComponent();
        _navigationService = navigationService;
    }

    /// <summary>
    /// Navigates to MainTabbedPage wrapped in NavigationPage.
    /// This is the path that triggers the bottom gap bug on Android.
    /// </summary>
    private void OnLoginClicked(object? sender, EventArgs e)
    {
        _navigationService.SetRootPage(new MainTabbedPage());
    }

    /// <summary>
    /// Navigates to MainTabbedPage WITHOUT NavigationPage wrapper.
    /// This is the workaround — the gap disappears.
    /// </summary>
    private void OnLoginNoWrapperClicked(object? sender, EventArgs e)
    {
        _navigationService.SetRootPageNoWrapper(new MainTabbedPage());
    }
}
