using TabbedPageBottomGapRepro.Pages;

namespace TabbedPageBottomGapRepro;

public partial class MainTabbedPage : TabbedPage
{
    public MainTabbedPage()
    {
        InitializeComponent();

        // Each tab child is wrapped in a NavigationPage — this is part of the bug reproduction
        Children.Add(new NavigationPage(new Tab1Page()) { Title = "Home", IconImageSource = "dotnet_bot.png" });
        Children.Add(new NavigationPage(new Tab2Page()) { Title = "Browse", IconImageSource = "dotnet_bot.png" });
        Children.Add(new NavigationPage(new Tab3Page()) { Title = "About", IconImageSource = "dotnet_bot.png" });
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

#if ANDROID
        if (Handler?.PlatformView is Android.Views.View pv)
        {
            pv.PostDelayed(() =>
            {
                var decor = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity?.Window?.DecorView;
                DumpTree(decor, 0);
            }, 500);
        }
#endif
    }

#if ANDROID
    private void DumpTree(Android.Views.View? view, int depth)
    {
        if (view == null) return;

        var indent = new string(' ', depth * 2);
        var lp = view.LayoutParameters as Android.Views.ViewGroup.MarginLayoutParams;
        int marginBottom = lp?.BottomMargin ?? 0;

        System.Diagnostics.Debug.WriteLine(
            $"{indent}{view.GetType().Name} " +
            $"w={view.Width} h={view.Height} " +
            $"paddingBottom={view.PaddingBottom} marginBottom={marginBottom}");

        if (view is Android.Views.ViewGroup vg)
        {
            for (int i = 0; i < vg.ChildCount; i++)
            {
                DumpTree(vg.GetChildAt(i), depth + 1);
            }
        }
    }
#endif
}
