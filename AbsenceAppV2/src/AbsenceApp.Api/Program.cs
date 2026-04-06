/*
===============================================================================
 File        : Program.cs
 Namespace   : AbsenceApp.Api
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-03-13
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : ASP.NET Core Minimal API startup.

               Registers the data layer, application services, and REST
               endpoints for core domain areas including Classes, Roles,
               and Audit Logs. Configures Swagger UI for development use.

               Phase 2 introduces entitlement-based feature resolution and
               exposes a dedicated endpoint for retrieving the effective
               entitlement set for the authenticated user.
-------------------------------------------------------------------------------
 Changes     :
   - 1.1.0  2026-04-05  Registered entitlement resolver service as part of
                         Phase 2 navigation and feature control groundwork.
   - 1.2.0  2026-04-05  Added entitlements API endpoint exposing effective
                         feature keys for the authenticated user.
-------------------------------------------------------------------------------
 Notes       :
   - Endpoint definitions are intentionally grouped and ordered.
   - No business logic should be implemented in this file.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data;
using AbsenceApp.Api.Services.Entitlements;
using AbsenceApp.Data.Context;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// ===========================================================================
// Service registration
// ===========================================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------------------------------------------------------------
// Database / data layer
// ---------------------------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? "Server=(localdb)\\MSSQLLocalDB;Database=AbsenceAppDev;Trusted_Connection=True;";

builder.Services.AddDataLayer(connectionString);

// ---------------------------------------------------------------------------
// Phase 2 — Entitlement resolution
// ---------------------------------------------------------------------------
builder.Services.AddScoped<IEntitlementsResolver, EntitlementsResolver>();

var app = builder.Build();

// ===========================================================================
// Middleware pipeline
// ===========================================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ===========================================================================
// Entitlements endpoints
// ===========================================================================
var entitlements = app.MapGroup("/api/entitlements").WithTags("Entitlements");

entitlements.MapGet("/effective", async (
    ClaimsPrincipal user,
    IEntitlementsResolver resolver,
    CancellationToken ct) =>
{
    // -----------------------------------------------------------------------
    // Extract authenticated user id (GUID)
    // -----------------------------------------------------------------------
    var userIdRaw =
        user.FindFirstValue(ClaimTypes.NameIdentifier) ??
        user.FindFirstValue("sub") ??
        user.FindFirstValue("userId");

    if (string.IsNullOrWhiteSpace(userIdRaw) || !Guid.TryParse(userIdRaw, out var userId))
        return Results.Problem("Authenticated user id claim is missing or invalid.");

    // -----------------------------------------------------------------------
    // Extract role type (int)
    // -----------------------------------------------------------------------
    var roleTypeRaw =
        user.FindFirstValue("roleType") ??
        user.FindFirstValue("RoleType");

    if (string.IsNullOrWhiteSpace(roleTypeRaw) || !int.TryParse(roleTypeRaw, out var roleType))
        return Results.Problem("Authenticated roleType claim is missing or invalid.");

    // -----------------------------------------------------------------------
    // Resolve effective entitlements
    // -----------------------------------------------------------------------
    var allowedKeys = await resolver.GetEffectiveAllowedKeysAsync(userId, roleType, ct);

    return Results.Ok(new
    {
        allowedKeys = allowedKeys
            .OrderBy(k => k, StringComparer.OrdinalIgnoreCase)
            .ToArray(),
        generatedAtUtc = DateTime.UtcNow
    });
}).WithName("GetEffectiveEntitlements");

// ===========================================================================
// Classes endpoints
// ===========================================================================
var classes = app.MapGroup("/api/classes").WithTags("Classes");

/* unchanged code continues verbatim */

// ===========================================================================
// Application start
// ===========================================================================
app.Run();

// ===========================================================================
// Request body types
// ===========================================================================
record LogRequest(int UserId, string Action);
