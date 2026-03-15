/*
===============================================================================
 File        : MainApplication.cs
 Namespace   : AbsenceApp.Client
 Platform    : Android
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Android Application subclass.
               Delegates MauiApp creation to MauiProgram.CreateMauiApp().
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Required by .NET MAUI on Android to provide the standard Application
     lifecycle integration; without it the MAUI bridge cannot initialise.
   - Constructor parameters are mandated by the Android JNI bridge.
===============================================================================
*/

using Android.App;
using Android.Runtime;

namespace AbsenceApp.Client;

// =========================================================================
// Android application class — forwards CreateMauiApp to shared MauiProgram
// =========================================================================

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
