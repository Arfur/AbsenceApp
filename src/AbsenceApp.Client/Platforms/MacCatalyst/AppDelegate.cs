/*
===============================================================================
 File        : AppDelegate.cs
 Namespace   : AbsenceApp.Client
 Platform    : Mac Catalyst
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Mac Catalyst application delegate.
               Delegates MauiApp creation to MauiProgram.CreateMauiApp().
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Identical in structure to the iOS AppDelegate; Mac Catalyst shares the
     UIKit application model and the same [Register] requirement.
   - All cross-platform DI and Blazor setup occurs inside MauiProgram.
===============================================================================
*/

using Foundation;

namespace AbsenceApp.Client;

// =========================================================================
// Mac Catalyst application delegate — forwards CreateMauiApp to shared MauiProgram
// =========================================================================

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
