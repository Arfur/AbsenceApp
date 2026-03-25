/*
===============================================================================
 File        : AppDelegate.cs
 Namespace   : AbsenceApp.Client
 Platform    : iOS
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : iOS application delegate.
               Delegates MauiApp creation to MauiProgram.CreateMauiApp().
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - The [Register] attribute maps this class to the Objective-C runtime
     name expected by the iOS UIKit application lifecycle.
   - All cross-platform DI and Blazor setup occurs inside MauiProgram.
===============================================================================
*/

using Foundation;

namespace AbsenceApp.Client;

// =========================================================================
// iOS application delegate — forwards CreateMauiApp to shared MauiProgram
// =========================================================================

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
