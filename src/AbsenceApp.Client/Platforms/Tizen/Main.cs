/*
===============================================================================
 File        : Main.cs
 Namespace   : AbsenceApp.Client
 Platform    : Tizen
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Tizen application entry point.
               Instantiates the Program class (MauiApplication) and starts
               the Tizen application run loop.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - The Tizen target is disabled by default in AbsenceApp.Client.csproj;
     uncomment the TargetFrameworks entry to enable it.
   - Requires the Tizen SDK to be installed separately.
===============================================================================
*/

using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace AbsenceApp.Client;

// =========================================================================
// Tizen entry point — creates the MauiApplication and starts the run loop
// =========================================================================

class Program : MauiApplication
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	static void Main(string[] args)
	{
		var app = new Program();
		app.Run(args);
	}
}
