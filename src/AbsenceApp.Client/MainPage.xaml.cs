/*
===============================================================================
 File        : MainPage.xaml.cs
 Namespace   : AbsenceApp.Client
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Code-behind for MainPage — the single ContentPage that hosts
               the BlazorWebView.  Handles the Students quick-launch button
               event by navigating the Shell to /students.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Shell navigation relies on the route "/students" registered via the
     @page directive in StudentsPage.razor.
   - OnStudentsClicked is wired up in MainPage.xaml via the Clicked attribute.
===============================================================================
*/

namespace AbsenceApp.Client;

// =========================================================================
// Page code-behind — event handlers wired from MainPage.xaml
// =========================================================================

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}
}
