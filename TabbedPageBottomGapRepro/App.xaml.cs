namespace TabbedPageBottomGapRepro;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // BUG REPRODUCTION: Wrapping TabbedPage in NavigationPage causes
        // a visible blank gap between content and bottom tab bar on Android.
        // Change to: new MainTabbedPage() (without NavigationPage wrapper) to see the gap disappear.
        return new Window(new NavigationPage(new MainTabbedPage()));
    }
}
