/*
===============================================================================
 File        : Program.cs
 Namespace   : AbsenceApp.Client
 Platform    : iOS
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : iOS application entry point.
               Starts the UIKit run loop, passing AppDelegate as the
               application delegate class.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - UIApplication.Main must receive the concrete AppDelegate type; passing
     null for the principal class name lets the Info.plist override it.
   - This is the lowest-level iOS entry before any MAUI code runs.
===============================================================================
*/

using ObjCRuntime;
using UIKit;

namespace AbsenceApp.Client;

// =========================================================================
// iOS entry point — invokes UIApplication.Main with AppDelegate
// =========================================================================

public class Program
{
	static void Main(string[] args)
	{
		UIApplication.Main(args, null, typeof(AppDelegate));
	}
}
