namespace TabbedPageBottomGapRepro.Services;

/// <summary>
/// Navigation service that performs absolute navigation by replacing Window.Page.
/// Always wraps the target page in a NavigationPage — this is part of the bug reproduction.
/// </summary>
public class NavigationService
{
    /// <summary>
    /// Navigate to a page using absolute navigation (replaces Window.Page).
    /// The page is always wrapped in a NavigationPage.
    /// </summary>
    public void NavigateToAbsolute(Window window, Page page)
    {
        window.Page = new NavigationPage(page);
    }

    /// <summary>
    /// Navigate to the main TabbedPage (wrapped in NavigationPage).
    /// This is the configuration that triggers the bottom gap bug on Android.
    /// </summary>
    public void NavigateToMain(Window window)
    {
        window.Page = new NavigationPage(new MainTabbedPage());
    }

    /// <summary>
    /// Navigate to the main TabbedPage WITHOUT NavigationPage wrapper.
    /// This is the workaround — the gap disappears when TabbedPage is not wrapped.
    /// </summary>
    public void NavigateToMainNoWrapper(Window window)
    {
        window.Page = new MainTabbedPage();
    }
}
