/*
===============================================================================
 File        : App.xaml.cs
 Namespace   : AbsenceApp.Client
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Code-behind for the root MAUI Application class.
               Sets MainPage to the Blazor host ContentPage on startup.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - MainPage is set in the constructor; no XAML code-behind logic required.
   - The root ContentPage hosts the BlazorWebView declared in MainPage.xaml.
===============================================================================
*/

namespace AbsenceApp.Client;

// =========================================================================
// Application entry — sets the root ContentPage on application launch
// =========================================================================

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new MainPage();
	}
}
