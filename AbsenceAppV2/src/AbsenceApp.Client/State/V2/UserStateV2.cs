/*
===============================================================================
 File        : UserStateV2.cs
 Namespace   : AbsenceApp.Client.State.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Immutable record holding the authenticated user's identity and role claims. Used inside AppStateStoreV2 to drive role-based UI visibility and personalisation.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 6 model. No DI registration required; owned by AppStateStoreV2.
===============================================================================
*/

namespace AbsenceApp.Client.State.V2;

/// <summary>
/// Immutable snapshot of the current user session for V2 components.
/// </summary>
public sealed record UserStateV2
{
    /// <summary>Database ID of the current user. Zero when not authenticated.</summary>
    public long UserId { get; init; }

    /// <summary>Display name of the current user.</summary>
    public string UserName { get; init; } = string.Empty;

    /// <summary>Role of the current user (e.g. "Admin", "Teacher").</summary>
    public string UserRole { get; init; } = string.Empty;

    /// <summary>User's email address.</summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>True when the user is authenticated.</summary>
    public bool IsAuthenticated { get; init; }

    // -------------------------------------------------------------------------

    // Factory

    // -------------------------------------------------------------------------
    /// <summary>Represents an anonymous / not-authenticated user.</summary>
    public static UserStateV2 Anonymous() => new();

    /// <summary>Creates an authenticated user state record.</summary>
    public static UserStateV2 Authenticated(long id, string name, string role, string email = "") =>
        new() { UserId = id, UserName = name, UserRole = role, Email = email, IsAuthenticated = true };
}
