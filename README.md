# TabbedPage Bottom Gap Repro (Android)

A .NET MAUI project that reproduces a TabbedPage bottom spacing bug on Android.

## The Bug

On Android, there is a visible blank space (same color as the page background) between the bottom of the page content and the top of the bottom tab bar. This does **NOT** happen on iOS.

### Root Cause

When a `TabbedPage` with `android:TabbedPage.ToolbarPlacement="Bottom"` is wrapped inside a `NavigationPage` (i.e., `Window.Page = new NavigationPage(new MainTabbedPage())`), the native Android view tree shows the `FragmentContainerView` holding the `ViewPager2` has:
- `paddingBottom = 72px`
- `bottomMargin = 168px` (matching the tab bar height)

This causes the content area to be pushed up away from the tab bar, creating a visible gap.

### How to Verify

The `MainTabbedPage` code-behind includes a `DumpTree` method that logs the Android view hierarchy on `OnHandlerChanged`. Run the app on Android and check the Debug output — the `FragmentContainerView` parent of `ViewPager2` will show non-zero `paddingBottom` and `marginBottom`.

### Workaround

In `App.xaml.cs`, change:
```csharp
// BUG: Gap appears
return new Window(new NavigationPage(new MainTabbedPage()));
```
to:
```csharp
// WORKAROUND: Gap disappears
return new Window(new MainTabbedPage());
```

## Project Structure

- **TabbedPageBottomGapRepro/** — .NET MAUI project targeting `net10.0-android` and `net10.0-ios`
- **MainTabbedPage** — `TabbedPage` subclass with bottom toolbar placement and `DumpTree` diagnostics
- **Pages/** — Three tab content pages (`Tab1Page`, `Tab2Page`, `Tab3Page`)
- **Tab3Page** includes a `DevExpress.Maui.CollectionView.DXCollectionView` with dummy data
- **Services/NavigationService.cs** — Absolute navigation via `Window.Page = new NavigationPage(page)`
- **Models/DummyItem.cs** — Simple model for the collection view

## Dependencies

- Microsoft.Maui.Controls 10.0.70
- DevExpress.Maui.Controls (latest)
- DevExpress.Maui.CollectionView (latest)
- DevExpress.Maui.Editors (latest)
- DevExpress.Maui.Core (latest)

## Key Configuration

- `SupportedOSPlatformVersion` for Android: **26.0**
- `MainActivity` uses `Theme = "@style/Maui.SplashTheme"`
- Global implicit `NavigationPage` style sets `HasNavigationBar = False`
- `TabbedPage` style sets bar colors
- Each tab child is wrapped in `NavigationPage`
- Each content page sets `NavigationPage.HasNavigationBar="False"`
