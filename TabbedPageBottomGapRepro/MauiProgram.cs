using DevExpress.Maui;
using Microsoft.Extensions.Logging;
using TabbedPageBottomGapRepro.Pages;
using TabbedPageBottomGapRepro.Services;

namespace TabbedPageBottomGapRepro;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseDevExpressControls()
            .UseDevExpressEditors()
            .UseDevExpressCollectionView()
            .UseDevExpress()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Custom stack-based navigation service (no Shell anywhere)
        builder.Services.AddSingleton<NavigationService>();
        builder.Services.AddSingleton<INavigationService>(sp => sp.GetRequiredService<NavigationService>());

        // Register pages for DI so they can receive INavigationService
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<Tab1Page>();
        builder.Services.AddTransient<Tab2Page>();
        builder.Services.AddTransient<Tab3Page>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
