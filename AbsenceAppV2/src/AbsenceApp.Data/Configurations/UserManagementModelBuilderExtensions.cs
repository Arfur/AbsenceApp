/*
===============================================================================
 File        : UserManagementModelBuilderExtensions.cs
 Namespace   : AbsenceApp.Data.Configurations
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-04-11
 Updated     : 2026-04-19
-------------------------------------------------------------------------------
 Purpose     : ModelBuilder extension that configures the four E15 entities:
               AppPage, RoleDefaultPagePermission, UserPageOverride, and
               UserPagePermission. Called from AppDbContext.OnModelCreating
               via modelBuilder.ConfigureUserManagement().

               All four entities use SQL Server IDENTITY PKs and are excluded
               from the ValueGeneratedNever loop in AppDbContext by type checks.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
   - 1.1.0  2026-04-11  E16 Pages Registry: extended AppPage configuration
                         with Slug / CategoryKey / MenuKey / IconKey /
                         Description / SupportsXxx / CreatedAt / UpdatedAt.
                         Updated HasData seed to include all 9 pages (added
                         "Pages" registry page at sort order 85).
   - 1.2.0  2026-04-19  E19 Navigation Audit remediations (FIX-02/05/06/09/11):
                         FIX-02: Audit Log route+slug "/v2/audit-log" → "/v2/auditlog".
                         FIX-05: Classes CategoryKey "ACADEMIC" → "ACADEMICS".
                         FIX-06: Dashboard CategoryKey "ADMIN" → "MAIN SIDEBAR";
                                 Audit Log → "CONFIGURATION"; Settings → "SETTINGS";
                                 Users → "CONFIGURATION"; Pages → "CONFIGURATION".
                         FIX-09: Added dashboard sub-page AppPages (ids 10–12).
                         FIX-11: Added diagnostics/site AppPages (ids 13–14).
-------------------------------------------------------------------------------
 Notes     :
   - app_pages is seeded with the canonical set of V2 application routes.
   - HasData documents the intended seed state; migrations contain the actual
     SQL that drives the database to this state.
   - RoleDefaultPagePermission uses a lazy-evaluated string RoleTypeName (no
     FK to role_types) to avoid cascade complexity and enable upsert by name.
   - Unique indexes mirror the spec: (RoleTypeName, PageId) on role defaults;
     (UserId, PageId) on user permissions and overrides.
   - CategoryKey values must match the dbo.MenuItems Category column exactly
     (case-sensitive match performed by NavigationApiServiceV2).
===============================================================================
*/

using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Configurations;

public static class UserManagementModelBuilderExtensions
{
    // -------------------------------------------------------------------------
    // Fixed seed timestamp â€” must be a compile-time constant for HasData().
    // -------------------------------------------------------------------------
    private static readonly DateTime SeedDate =
        new(2026, 4, 11, 0, 0, 0, DateTimeKind.Utc);

    // -------------------------------------------------------------------------
    // Default seed data — all canonical V2 application pages.
    // Tuple layout: (Id, Name, Slug, Route, Category, Menu, Icon,
    //                SupRead, SupWrite, SupCreate, SupDelete, SupImport, SupExport, Sort)
    //
    // CategoryKey must match dbo.MenuItems.Category exactly (case-sensitive).
    // -------------------------------------------------------------------------
    private static readonly (
        int     Id,
        string  Name,
        string  Slug,
        string  Route,
        string  CategoryKey,
        string  MenuKey,
        string  IconKey,
        bool    SupportsRead,
        bool    SupportsWrite,
        bool    SupportsCreate,
        bool    SupportsDelete,
        bool    SupportsImport,
        bool    SupportsExport,
        int     Sort
    )[] DefaultPages =
    [
        // ── Core pages (ids 1–9) ────────────────────────────────────────────
        // FIX-06: Dashboard CategoryKey "ADMIN" → "MAIN SIDEBAR"
        ( 1, "Dashboard",   "dashboard",   "/v2/dashboard",   "MAIN SIDEBAR", "Dashboard",  "bi-speedometer2",      true,  false, false, false, false, false, 10 ),
        ( 2, "Students",    "students",    "/v2/students",    "PEOPLE",       "Students",   "bi-mortarboard",       true,  true,  true,  true,  true,  true,  20 ),
        ( 3, "Staff",       "staff",       "/v2/staff",       "PEOPLE",       "Staff",      "bi-person-badge",      true,  true,  true,  true,  true,  true,  30 ),
        // FIX-05: Classes CategoryKey "ACADEMIC" → "ACADEMICS"
        ( 4, "Classes",     "classes",     "/v2/classes",     "ACADEMICS",    "Classes",    "bi-journal-bookmark",  true,  true,  true,  true,  false, true,  40 ),
        ( 5, "Attendance",  "attendance",  "/v2/attendance",  "ATTENDANCE",   "Attendance", "bi-calendar-check",    true,  true,  true,  false, false, true,  50 ),
        // FIX-02: Audit Log route+slug "/v2/audit-log" → "/v2/auditlog" (no hyphen)
        // FIX-06: Audit Log CategoryKey "ADMIN" → "CONFIGURATION"
        ( 6, "Audit Log",   "auditlog",    "/v2/auditlog",    "CONFIGURATION","Audit Log",  "bi-journal-text",      true,  false, false, false, false, true,  60 ),
        // FIX-06: Settings CategoryKey "ADMIN" → "SETTINGS"
        ( 7, "Settings",    "settings",    "/v2/settings",    "SETTINGS",     "Settings",   "bi-gear",              true,  true,  false, false, false, false, 70 ),
        // FIX-06: Users CategoryKey "ADMIN" → "CONFIGURATION"
        ( 8, "Users",       "users",       "/v2/users",       "CONFIGURATION","Users",      "bi-people",            true,  true,  true,  true,  false, false, 80 ),
        // FIX-06: Pages CategoryKey "ADMIN" → "CONFIGURATION"
        ( 9, "Pages",       "pages",       "/v2/pages",       "CONFIGURATION","Pages",      "bi-file-earmark-text", true,  true,  true,  true,  false, false, 85 ),

        // ── FIX-09: Dashboard sub-pages (ids 10–12) ─────────────────────────
        (10, "Dashboard Overview",     "dashboard-overview",     "/v2/dashboard/overview",     "MAIN SIDEBAR","Dashboard","bi-bar-chart",    true, false, false, false, false, false, 11 ),
        (11, "Dashboard Trends",       "dashboard-trends",       "/v2/dashboard/trends",       "MAIN SIDEBAR","Dashboard","bi-graph-up",     true, false, false, false, false, false, 12 ),
        (12, "Dashboard Safeguarding", "dashboard-safeguarding", "/v2/dashboard/safeguarding", "MAIN SIDEBAR","Dashboard","bi-shield-check", true, false, false, false, false, false, 13 ),

        // ── FIX-11: Settings sub-pages (ids 13–14) ──────────────────────────
        (13, "Diagnostics", "diagnostics", "/v2/diagnostics", "SETTINGS","Diagnostics","bi-activity", true, false, false, false, false, false, 75 ),
        (14, "Site",        "site",        "/v2/site",        "SETTINGS","Site",        "bi-globe",    true, false, false, false, false, false, 76 ),
    ];

    // -------------------------------------------------------------------------
    // ConfigureUserManagement
    // -------------------------------------------------------------------------
    public static ModelBuilder ConfigureUserManagement(this ModelBuilder modelBuilder)
    {
        // â”€â”€ AppPage â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        modelBuilder.Entity<AppPage>(b =>
        {
            b.ToTable("app_pages");
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedOnAdd();
            b.Property(e => e.Slug).IsRequired().HasMaxLength(200).HasDefaultValue(string.Empty);
            b.Property(e => e.Route).IsRequired().HasMaxLength(500);
            b.Property(e => e.CategoryKey).IsRequired().HasMaxLength(100).HasDefaultValue(string.Empty);
            b.Property(e => e.MenuKey).IsRequired().HasMaxLength(200).HasDefaultValue(string.Empty);
            b.Property(e => e.IconKey).HasMaxLength(100);
            b.Property(e => e.Description).HasMaxLength(1000);
            b.Property(e => e.IsActive).HasDefaultValue(true);
            b.Property(e => e.SupportsRead).HasDefaultValue(false);
            b.Property(e => e.SupportsWrite).HasDefaultValue(false);
            b.Property(e => e.SupportsCreate).HasDefaultValue(false);
            b.Property(e => e.SupportsDelete).HasDefaultValue(false);
            b.Property(e => e.SupportsImport).HasDefaultValue(false);
            b.Property(e => e.SupportsExport).HasDefaultValue(false);
            b.Property(e => e.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP(6)");
            b.Property(e => e.UpdatedAt).HasDefaultValueSql("UTC_TIMESTAMP(6)");
            b.HasIndex(e => e.Route).IsUnique();
            b.HasIndex(e => e.Slug).IsUnique();
        });

        // Seed default pages
        foreach (var p in DefaultPages)
        {
            modelBuilder.Entity<AppPage>().HasData(new AppPage
            {
                Id             = p.Id,
                Name           = p.Name,
                Slug           = p.Slug,
                Route          = p.Route,
                CategoryKey    = p.CategoryKey,
                MenuKey        = p.MenuKey,
                IconKey        = p.IconKey,
                IsActive       = true,
                SortOrder      = p.Sort,
                SupportsRead   = p.SupportsRead,
                SupportsWrite  = p.SupportsWrite,
                SupportsCreate = p.SupportsCreate,
                SupportsDelete = p.SupportsDelete,
                SupportsImport = p.SupportsImport,
                SupportsExport = p.SupportsExport,
                CreatedAt      = SeedDate,
                UpdatedAt      = SeedDate,
            });
        }

        // â”€â”€ RoleDefaultPagePermission â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        modelBuilder.Entity<RoleDefaultPagePermission>(b =>
        {
            b.ToTable("role_default_page_permissions");
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedOnAdd();
            b.Property(e => e.RoleTypeName).IsRequired().HasMaxLength(100);
            b.Property(e => e.CanRead).HasDefaultValue(false);
            b.Property(e => e.CanWrite).HasDefaultValue(false);
            b.Property(e => e.CanCreate).HasDefaultValue(false);
            b.Property(e => e.CanDelete).HasDefaultValue(false);
            b.Property(e => e.CanImport).HasDefaultValue(false);
            b.Property(e => e.CanExport).HasDefaultValue(false);
            b.HasIndex(e => new { e.RoleTypeName, e.PageId }).IsUnique();
            b.HasOne<AppPage>()
                .WithMany()
                .HasForeignKey(e => e.PageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // â”€â”€ UserPageOverride â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        modelBuilder.Entity<UserPageOverride>(b =>
        {
            b.ToTable("user_page_overrides");
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedOnAdd();
            b.Property(e => e.OverrideType).IsRequired().HasMaxLength(10);
            b.HasIndex(e => new { e.UserId, e.PageId }).IsUnique();
            b.HasOne<AppPage>()
                .WithMany()
                .HasForeignKey(e => e.PageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // â”€â”€ UserPagePermission â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        modelBuilder.Entity<UserPagePermission>(b =>
        {
            b.ToTable("user_page_permissions");
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedOnAdd();
            b.Property(e => e.CanRead).HasDefaultValue(false);
            b.Property(e => e.CanWrite).HasDefaultValue(false);
            b.Property(e => e.CanCreate).HasDefaultValue(false);
            b.Property(e => e.CanDelete).HasDefaultValue(false);
            b.Property(e => e.CanImport).HasDefaultValue(false);
            b.Property(e => e.CanExport).HasDefaultValue(false);
            b.HasIndex(e => new { e.UserId, e.PageId }).IsUnique();
            b.HasOne<AppPage>()
                .WithMany()
                .HasForeignKey(e => e.PageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        return modelBuilder;
    }
}
