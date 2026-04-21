/*
===============================================================================
 File        : Program.cs
 Namespace   : AbsenceApp.Api
 Author      : Michael
 Version     : 1.4.0
 Created     : 2026-03-13
 Updated     : 2026-04-12
-------------------------------------------------------------------------------
 Purpose     : ASP.NET Core Minimal API startup.

               Registers the data layer, application services, and REST
               endpoints for core domain areas including Classes, Roles,
               and Audit Logs. Configures Swagger UI for development use.

               Phase 2 introduces entitlement-based feature resolution and
               exposes a dedicated endpoint for retrieving the effective
               entitlement set for the authenticated user.

               Phase 3 (Option A) introduces API-authoritative access control:
               - GET /api/menu returns the role-filtered, pruned menu tree.
               - GET /api/features/allowed returns a per-feature boolean.
-------------------------------------------------------------------------------
 Changes     :
   - 1.1.0  2026-04-05  Registered entitlement resolver service as part of
                         Phase 2 navigation and feature control groundwork.
   - 1.2.0  2026-04-05  Added entitlements API endpoint exposing effective
                         feature keys for the authenticated user.
   - 1.3.0  2026-04-06  Option A access control: registered IMenuResolver,
                         IFeaturePermissionResolver. Added GET /api/menu and
                         GET /api/features/allowed endpoints.
   - 1.4.0  2026-04-12  Added startup diagnostic output to print the resolved
                         connection string used by the API at runtime to
                         validate environment overrides and configuration
                         precedence during debugging.
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
using AbsenceApp.Api.Services.Navigation;
using AbsenceApp.Data.Context;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// ===========================================================================
// Diagnostic: print the resolved connection string used at runtime
// ===========================================================================
Console.WriteLine("### API USING DB: " + builder.Configuration.GetConnectionString("Default"));

// ===========================================================================
// Service registration
// ===========================================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------------------------------------------------------------
// Database / data layer
// ---------------------------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? "Server=(localdb)\\MSSQLLocalDB;Database=AbsenceApp;Trusted_Connection=True;";

builder.Services.AddDataLayer(connectionString);

// ---------------------------------------------------------------------------
// Phase 2 — Entitlement resolution
// ---------------------------------------------------------------------------
builder.Services.AddScoped<IEntitlementsResolver, EntitlementsResolver>();

// ---------------------------------------------------------------------------
// Phase 3 (Option A) — API-authoritative menu and feature permission
// ---------------------------------------------------------------------------
builder.Services.AddScoped<IMenuResolver, MenuResolver>();
builder.Services.AddScoped<IFeaturePermissionResolver, FeaturePermissionResolver>();

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
    var userIdRaw =
        user.FindFirstValue(ClaimTypes.NameIdentifier) ??
        user.FindFirstValue("sub") ??
        user.FindFirstValue("userId");

    if (string.IsNullOrWhiteSpace(userIdRaw) || !Guid.TryParse(userIdRaw, out var userId))
        return Results.Problem("Authenticated user id claim is missing or invalid.");

    var roleTypeRaw =
        user.FindFirstValue("roleType") ??
        user.FindFirstValue("RoleType");

    if (string.IsNullOrWhiteSpace(roleTypeRaw) || !int.TryParse(roleTypeRaw, out var roleType))
        return Results.Problem("Authenticated roleType claim is missing or invalid.");

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
// Menu endpoint
// ===========================================================================
var menu = app.MapGroup("/api/menu").WithTags("Menu");

menu.MapGet("/", async (
    ClaimsPrincipal user,
    IMenuResolver resolver,
    CancellationToken ct) =>
{
    var roleTypeRaw =
        user.FindFirstValue("roleType") ??
        user.FindFirstValue("RoleType");

    if (string.IsNullOrWhiteSpace(roleTypeRaw) || !int.TryParse(roleTypeRaw, out var roleType))
        return Results.Problem("Authenticated roleType claim is missing or invalid.");

    var result = await resolver.GetMenuAsync(roleType, ct);
    return Results.Ok(result);
}).WithName("GetMenu");

// ===========================================================================
// Feature permission endpoint
// ===========================================================================
var features = app.MapGroup("/api/features").WithTags("Features");

features.MapGet("/allowed", async (
    string key,
    ClaimsPrincipal user,
    IFeaturePermissionResolver resolver,
    CancellationToken ct) =>
{
    var roleTypeRaw =
        user.FindFirstValue("roleType") ??
        user.FindFirstValue("RoleType");

    if (string.IsNullOrWhiteSpace(roleTypeRaw) || !int.TryParse(roleTypeRaw, out var roleType))
        return Results.Problem("Authenticated roleType claim is missing or invalid.");

    if (string.IsNullOrWhiteSpace(key))
        return Results.BadRequest("Feature key is required.");

    var allowed = await resolver.IsAllowedAsync(roleType, key, ct);
    return Results.Ok(new { key, allowed });
}).WithName("IsFeatureAllowed");

// ===========================================================================
// Classes endpoints
// ===========================================================================
var classes = app.MapGroup("/api/classes").WithTags("Classes");

// ===========================================================================
// Absence lookup endpoints
// ===========================================================================
var absenceTypes = app.MapGroup("/api/absence-types").WithTags("Absences");

absenceTypes.MapGet("/", async (IAbsenceTypeService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetAllAsync()))
    .WithName("GetAbsenceTypes");

var absenceStatuses = app.MapGroup("/api/absence-statuses").WithTags("Absences");

absenceStatuses.MapGet("/", async (IAbsenceStatusService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetAllAsync()))
    .WithName("GetAbsenceStatuses");

// ===========================================================================
// Absence endpoints
// ===========================================================================
var absences = app.MapGroup("/api/absences").WithTags("Absences");

absences.MapGet("/staff/{staffId:long}", async (
    long staffId,
    IAbsenceService svc,
    CancellationToken ct) =>
    Results.Ok(await svc.GetByPersonAsync("Staff", staffId)))
    .WithName("GetStaffAbsences");

absences.MapGet("/students/{studentId:long}", async (
    long studentId,
    IAbsenceService svc,
    CancellationToken ct) =>
    Results.Ok(await svc.GetByPersonAsync("Student", studentId)))
    .WithName("GetStudentAbsences");

absences.MapGet("/{id:long}", async (
    long id,
    IAbsenceService svc,
    CancellationToken ct) =>
{
    var result = await svc.GetByIdAsync(id);
    return result is null ? Results.NotFound() : Results.Ok(result);
}).WithName("GetAbsenceById");

absences.MapPost("/", async (
    CreateAbsenceDto body,
    IAbsenceService svc,
    CancellationToken ct) =>
{
    var created = await svc.CreateAsync(body);
    return Results.Created($"/api/absences/{created.Id}", created);
}).WithName("CreateAbsence");

absences.MapPatch("/{id:long}/status", async (
    long id,
    UpdateAbsenceStatusDto body,
    IAbsenceService svc,
    CancellationToken ct) =>
{
    await svc.UpdateStatusAsync(id, body);
    return Results.NoContent();
}).WithName("UpdateAbsenceStatus");

// ===========================================================================
// Application start
// ===========================================================================
app.Run();

// ===========================================================================
// Request body types
// ===========================================================================
record LogRequest(int UserId, string Action);
