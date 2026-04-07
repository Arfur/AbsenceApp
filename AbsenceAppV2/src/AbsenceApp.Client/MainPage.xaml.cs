/*
===============================================================================
 File        : MainPage.xaml.cs
 Namespace   : AbsenceApp.Client
 Author      : Michael
 Version     : 1.4.0
 Created     : 2026-03-13
 Updated     : 2026-04-07
-------------------------------------------------------------------------------
 Purpose     : Code-behind for MainPage — the single ContentPage that hosts
               the BlazorWebView.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
   - 1.1.0  2026-04-05  Injected EntitlementsService — superseded.
   - 1.2.0  2026-04-05  Reverted to parameterless constructor.
   - 1.3.0  2026-04-07  Added WebView history clearing (initial attempt).
   - 1.3.1  2026-04-07  Added x:Name to XAML (incorrect assumption).
   - 1.3.2  2026-04-07  Corrected WebView reference to match actual XAML.
   - 1.4.0  2026-04-07  Removed unsupported WebView history-clearing logic.
                         MAUI Hybrid fix now handled entirely in Routes.razor.
-------------------------------------------------------------------------------
 Notes       :
   - Shell navigation relies on the route "/students" registered via the
     @page directive in StudentsPage.razor.
===============================================================================
*/

namespace AbsenceApp.Client;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        // No additional logic required here.
        // MAUI Hybrid stale-navigation handling is now fully implemented
        // in Routes.razor (v2.11.0).
    }
}
