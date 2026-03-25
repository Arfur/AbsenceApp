/*
===============================================================================
 File        : MainActivity.cs
 Namespace   : AbsenceApp.Client
 Platform    : Android
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Android platform entry activity.
               Declares the splash theme, sets the launcher flag, and lists
               all configuration changes handled without restart.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - The Activity attribute is the only required customisation; all other
     Android behaviour is inherited from MauiAppCompatActivity.
   - ConfigurationChanges list prevents activity restarts during rotation,
     dark-mode toggle, and display density changes.
===============================================================================
*/

using Android.App;
using Android.Content.PM;
using Android.OS;

namespace AbsenceApp.Client;

// =========================================================================
// Android activity — sole ActivityAttribute entry point for the application
// =========================================================================

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
