/*
===============================================================================
 File        : MainPage.xaml.cs
 Namespace   : AbsenceApp.Client
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-03-13
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : Code-behind for MainPage — the single ContentPage that hosts
               the BlazorWebView.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
   - 1.1.0  2026-04-05  Injected EntitlementsService — superseded.
   - 1.2.0  2026-04-05  Reverted to parameterless constructor. The XAML
                         already declares the RootComponent; programmatic
                         duplication caused CS0103/CS0246 build errors.
                         EntitlementsService is available to all Razor
                         components via Blazor's own DI container.
-------------------------------------------------------------------------------
 Notes       :
   - Shell navigation relies on the route "/students" registered via the
     @page directive in StudentsPage.razor.
===============================================================================
*/

namespace AbsenceApp.Client;

// =========================================================================
// Page code-behind
// =========================================================================

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
}
