# TabbedPage Bottom Gap Repro (Android)

A .NET MAUI project that reproduces a TabbedPage bottom spacing bug on Android.

## The Bug

On Android, there is a visible blank space (same color as the page background) between the bottom of the page content and the top of the bottom tab bar. This does **NOT** happen on iOS.

### Root Cause

When a `TabbedPage` with `android:TabbedPage.ToolbarPlacement="Bottom"` is wrapped inside a `NavigationPage` (i.e., `Window.Page = new NavigationPage(new MainTabbedPage())`), the native Android view tree shows the `FragmentContainerView` holding the `ViewPager2` has:
- `paddingBottom = 72px`
- `bottomMargin = 168px` (matching the tab bar height)

This causes the content area to be pushed up away from the tab bar, creating a visible gap.

## Architecture: No AppShell, Custom Stack-Based NavigationService

The app has **zero Shell usage**. All navigation is managed by a custom `NavigationService` that operates on `NavigationPage` push/pop/root-reset.

### Navigation Flow

1. **App starts** → `App.xaml.cs` creates `Window(new NavigationPage(new LoginPage()))`
2. **User logs in** → `LoginPage` calls `NavigationService.SetRootPage(new MainTabbedPage())`
3. **Service resets root** → `Window.Page = new NavigationPage(mainTabbedPage)` ← **BUG triggers here**

### Key Components

- **`INavigationService`** — interface with `PushAsync`, `PopAsync`, `PopToRootAsync`, `SetRootPage`, `SetRootPageNoWrapper`
- **`NavigationService`** — concrete implementation; holds a reference to `Window` and the current `NavigationPage` root; manages the full navigation stack
- **`LoginPage`** — entry point; simulates login then calls `SetRootPage()` (bug) or `SetRootPageNoWrapper()` (workaround)
- **`MainTabbedPage`** — `TabbedPage` with bottom tabs, each child wrapped in `NavigationPage`
- **`App.xaml`** — defines global implicit styles only (no Shell)
- **`App.xaml.cs`** — receives `INavigationService` and `LoginPage` via DI; initializes the service with the `Window`

### How to Verify

The `MainTabbedPage` code-behind includes a `DumpTree` method that logs the Android view hierarchy on `OnHandlerChanged`. Run the app on Android and check the Debug output — the `FragmentContainerView` parent of `ViewPager2` will show non-zero `paddingBottom` and `marginBottom`.

### Workaround

On the login screen, tap **"Login (no wrapper — workaround)"** instead of the primary login button. This calls `SetRootPageNoWrapper()` which sets `Window.Page = new MainTabbedPage()` directly — the gap disappears.

## Project Structure

- **TabbedPageBottomGapRepro/** — .NET MAUI project targeting `net10.0-android` and `net10.0-ios`
- **MainTabbedPage** — `TabbedPage` subclass with bottom toolbar placement and `DumpTree` diagnostics
- **Pages/** — `LoginPage` (entry point), `Tab1Page`, `Tab2Page`, `Tab3Page`
  - `Tab3Page` includes a `DevExpress.Maui.CollectionView.DXCollectionView` with dummy data
- **Services/** — `INavigationService` interface + `NavigationService` (custom stack-based, no Shell)
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
- All pages and services registered in DI container
