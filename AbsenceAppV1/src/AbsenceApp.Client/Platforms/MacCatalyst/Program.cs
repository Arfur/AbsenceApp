/*
===============================================================================
 File        : Program.cs
 Namespace   : AbsenceApp.Client
 Platform    : Mac Catalyst
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Mac Catalyst application entry point.
               Starts the UIKit run loop, passing AppDelegate as the
               application delegate class.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Structurally identical to the iOS Program.cs; Mac Catalyst re-uses
     the UIKit run-loop model.
   - This is the lowest-level Mac Catalyst entry before any MAUI code runs.
===============================================================================
*/

using ObjCRuntime;
using UIKit;

namespace AbsenceApp.Client;

// =========================================================================
// Mac Catalyst entry point — invokes UIApplication.Main with AppDelegate
// =========================================================================

public class Program
{
	static void Main(string[] args)
	{
		UIApplication.Main(args, null, typeof(AppDelegate));
	}
}