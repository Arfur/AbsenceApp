/*
===============================================================================
 File        : ThemeMode.cs
 Namespace   : AbsenceApp.Client.Models.Theming
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Enumeration of available V2 theme modes. Light and Dark are the
               two built-in modes; System follows the OS preference reported
               by the platform.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 8).
-------------------------------------------------------------------------------
 Notes       :
   - Phase 8 model. No DI registration required.
   - ThemeServiceV2 reads and writes this value via Preferences.
===============================================================================
*/

namespace AbsenceApp.Client.FrameworkV2.Models;

/// <summary>
/// Selects which V2 colour scheme the shell renders.
/// </summary>
public enum ThemeMode
{
    /// <summary>Always use the light colour scheme.</summary>
    Light = 0,

    /// <summary>Always use the dark colour scheme.</summary>
    Dark = 1,

    /// <summary>
    /// Follow the operating system dark/light preference.
    /// Falls back to <see cref="Light"/> if the preference cannot be read.
    /// </summary>
    System = 2
}
