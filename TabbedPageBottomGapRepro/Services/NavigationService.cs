namespace TabbedPageBottomGapRepro.Services;

/// <summary>
/// Contract for the custom stack-based navigation service.
/// The real app has zero Shell usage — all navigation goes through this service.
/// </summary>
public interface INavigationService
{
    /// <summary>Push a page onto the navigation stack.</summary>
    Task PushAsync(Page page, bool animated = true);

    /// <summary>Pop the top page from the navigation stack.</summary>
    Task PopAsync(bool animated = true);

    /// <summary>Pop to the root of the navigation stack.</summary>
    Task PopToRootAsync(bool animated = true);

    /// <summary>
    /// Reset the entire navigation stack and set a new root page.
    /// The page is wrapped in a NavigationPage (this is what triggers the bug).
    /// </summary>
    void SetRootPage(Page page);

    /// <summary>
    /// Reset the entire navigation stack and set a new root page WITHOUT
    /// a NavigationPage wrapper. This is the workaround for the bug.
    /// </summary>
    void SetRootPageNoWrapper(Page page);

    /// <summary>The underlying NavigationPage (if any) that manages the stack.</summary>
    NavigationPage? NavigationRoot { get; }
}

/// <summary>
/// Custom stack-based navigation service — mirrors the real app's architecture.
/// 
/// Flow:
///   App starts → NavigationPage(LoginPage) → user taps "Login" →
///   NavigationService.SetRootPage(MainTabbedPage) →
///   Window.Page = new NavigationPage(MainTabbedPage)   ← BUG triggers here
///
/// The service holds a reference to the current Window and manages all
/// push / pop / root-reset operations. No Shell is used anywhere.
/// </summary>
public class NavigationService : INavigationService
{
    private Window? _window;

    public NavigationPage? NavigationRoot { get; private set; }

    /// <summary>
    /// Called once at startup to bind the service to the application window.
    /// </summary>
    public void Initialize(Window window)
    {
        _window = window;
        NavigationRoot = window.Page as NavigationPage;
    }

    public async Task PushAsync(Page page, bool animated = true)
    {
        if (NavigationRoot is not null)
            await NavigationRoot.Navigation.PushAsync(page, animated);
    }

    public async Task PopAsync(bool animated = true)
    {
        if (NavigationRoot is not null)
            await NavigationRoot.Navigation.PopAsync(animated);
    }

    public async Task PopToRootAsync(bool animated = true)
    {
        if (NavigationRoot is not null)
            await NavigationRoot.Navigation.PopToRootAsync(animated);
    }

    /// <summary>
    /// Replaces Window.Page with a new NavigationPage wrapping the given page.
    /// This is how the real app navigates after login — and triggers the bottom gap bug.
    /// </summary>
    public void SetRootPage(Page page)
    {
        if (_window is null) return;

        var navPage = new NavigationPage(page);
        _window.Page = navPage;
        NavigationRoot = navPage;
    }

    /// <summary>
    /// Replaces Window.Page directly (no NavigationPage wrapper).
    /// Workaround: the gap disappears when TabbedPage is not wrapped.
    /// </summary>
    public void SetRootPageNoWrapper(Page page)
    {
        if (_window is null) return;

        _window.Page = page;
        NavigationRoot = null;
    }
}
