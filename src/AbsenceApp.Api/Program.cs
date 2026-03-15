/*
===============================================================================
 File        : Program.cs
 Namespace   : AbsenceApp.Api
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : ASP.NET Core Minimal API startup. Registers the data layer,
               maps REST endpoints for Users, Roles, Classes, Attendance, and
               Audit Logs, and configures Swagger UI for development.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial endpoint set.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------
// Services
// ---------------------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? "Server=(localdb)\\MSSQLLocalDB;Database=AbsenceAppDev;Trusted_Connection=True;";

builder.Services.AddDataLayer(connectionString);

var app = builder.Build();

// ---------------------------------------------------------------------------
// Middleware
// ---------------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ===========================================================================
// Classes endpoints
// ===========================================================================
var classes = app.MapGroup("/api/classes").WithTags("Classes");

classes.MapGet("/", async (IClassService svc) =>
    Results.Ok(await svc.GetAllAsync()))
    .WithName("GetAllClasses");

classes.MapGet("/{id:int}", async (int id, IClassService svc) =>
{
    var dto = await svc.GetByIdAsync(id);
    return dto is null ? Results.NotFound() : Results.Ok(dto);
}).WithName("GetClassById");

classes.MapPost("/", async (ClassDto dto, IClassService svc) =>
{
    var created = await svc.CreateAsync(dto);
    return Results.Created($"/api/classes/{created.ClassId}", created);
}).WithName("CreateClass");

classes.MapPut("/{id:int}", async (int id, ClassDto dto, IClassService svc) =>
{
    dto.ClassId = id;
    await svc.UpdateAsync(dto);
    return Results.NoContent();
}).WithName("UpdateClass");

classes.MapDelete("/{id:int}", async (int id, IClassService svc) =>
{
    await svc.DeleteAsync(id);
    return Results.NoContent();
}).WithName("DeleteClass");

// ===========================================================================
// Roles endpoints
// ===========================================================================
var roles = app.MapGroup("/api/roles").WithTags("Roles");

roles.MapGet("/", async (IRoleService svc) =>
    Results.Ok(await svc.GetAllAsync()))
    .WithName("GetAllRoles");

roles.MapGet("/{id:int}", async (int id, IRoleService svc) =>
{
    var dto = await svc.GetByIdAsync(id);
    return dto is null ? Results.NotFound() : Results.Ok(dto);
}).WithName("GetRoleById");

roles.MapPost("/", async (RoleDto dto, IRoleService svc) =>
{
    var created = await svc.CreateAsync(dto);
    return Results.Created($"/api/roles/{created.RoleId}", created);
}).WithName("CreateRole");

roles.MapPut("/{id:int}", async (int id, RoleDto dto, IRoleService svc) =>
{
    dto.RoleId = id;
    await svc.UpdateAsync(dto);
    return Results.NoContent();
}).WithName("UpdateRole");

roles.MapDelete("/{id:int}", async (int id, IRoleService svc) =>
{
    await svc.DeleteAsync(id);
    return Results.NoContent();
}).WithName("DeleteRole");

// ===========================================================================
// Audit logs endpoints (append-only write)
// ===========================================================================
var auditLogs = app.MapGroup("/api/auditlogs").WithTags("AuditLogs");

auditLogs.MapGet("/", async (IAuditLogService svc) =>
    Results.Ok(await svc.GetAllAsync()))
    .WithName("GetAllAuditLogs");

auditLogs.MapGet("/{id:int}", async (int id, IAuditLogService svc) =>
{
    var dto = await svc.GetByIdAsync(id);
    return dto is null ? Results.NotFound() : Results.Ok(dto);
}).WithName("GetAuditLogById");

auditLogs.MapGet("/user/{userId:int}", async (int userId, IAuditLogService svc) =>
    Results.Ok(await svc.GetByUserAsync(userId)))
    .WithName("GetAuditLogsByUser");

auditLogs.MapPost("/", async (LogRequest req, IAuditLogService svc) =>
{
    var created = await svc.LogAsync(req.UserId, req.Action);
    return Results.Created($"/api/auditlogs/{created.AuditId}", created);
}).WithName("CreateAuditLog");

app.Run();

// ---------------------------------------------------------------------------
// Request body types
// ---------------------------------------------------------------------------

/// <summary>Body model for POST /api/auditlogs.</summary>
record LogRequest(int UserId, string Action);
