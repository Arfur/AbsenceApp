/*
===============================================================================
 File        : App.xaml.cs
 Namespace   : AbsenceApp.Client.WinUI
 Platform    : Windows (WinUI 3)
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Windows (WinUI 3) application code-behind.
               Initialises the WinUI application component and delegates
               MauiApp creation to MauiProgram.CreateMauiApp().
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - InitializeComponent() wires up the partial class generated from
     Platforms/Windows/App.xaml by the WinUI toolchain.
   - All cross-platform DI and Blazor setup occurs inside MauiProgram.
===============================================================================
*/

using Microsoft.UI.Xaml;

namespace AbsenceApp.Client.WinUI;

// =========================================================================
// WinUI application class — initialises component and forwards CreateMauiApp
// =========================================================================

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
	public App()
	{
		this.InitializeComponent();
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

