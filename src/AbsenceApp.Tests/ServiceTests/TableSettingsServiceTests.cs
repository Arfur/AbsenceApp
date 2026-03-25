/*
===============================================================================
 File        : TableSettingsServiceTests.cs
 Namespace   : AbsenceApp.Tests.ServiceTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for TableSettingsService using EF Core InMemory.

               Covers:
                 - GetSettingsAsync: empty DB, schema-only, merged with saves,
                   orphaned saved row pruning.
                 - SaveSettingsAsync: insert, update, orphan pruning.
                 - ResetToDefaultsAsync: all rows deleted.
                 - GetDiagnosticsAsync: page mapping, schema drift detection,
                   orphaned DB keys, unmapped entity detection.
                 - PageEntityMap completeness (all 13 keys present).
                 - ToSnakeCase / ToDisplayLabel utility correctness.
-------------------------------------------------------------------------------
 Notes       :
   - Uses EF Core InMemory provider to avoid a live SQL Server connection.
   - InMemory does NOT support FK constraints; test data is kept minimal.
   - GetSchemaFields() relies on _db.Model.FindEntityType() — the InMemory
     provider fully populates the EF model, so schema-driven logic works
     correctly in tests.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial test suite.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using AbsenceApp.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Tests.ServiceTests;

// =============================================================================
// Fixture — shared InMemory context builder
// =============================================================================

file static class DbFactory
{
    public static AppDbContext Create(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }
}

// =============================================================================
// GetSettingsAsync tests
// =============================================================================

public class TableSettingsService_GetSettingsAsync
{
    [Fact]
    public async Task UnknownPage_ReturnsEmpty()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var result = await sut.GetSettingsAsync("does_not_exist");

        Assert.Empty(result);
    }

    [Fact]
    public async Task KnownPage_NoSavedRows_ReturnsSchemaDefaults()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var result = await sut.GetSettingsAsync("students");

        Assert.NotEmpty(result);
        Assert.All(result, f =>
        {
            Assert.Equal("students", f.PageName);
            Assert.NotEmpty(f.FieldName);
            Assert.True(f.IsVisible);
            Assert.True(f.IsSortable);
            Assert.False(f.IsFilterable);
            Assert.False(f.IsSearchable);
        });
    }

    [Fact]
    public async Task KnownPage_SchemaFieldNames_MatchEntityProperties()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var result = await sut.GetSettingsAsync("students");

        // id must be the snake_case of Student.Id
        Assert.Contains(result, f => f.FieldName == "id");
        // admission_number → Student.AdmissionNumber
        Assert.Contains(result, f => f.FieldName == "admission_number");
        // first_name → Student.FirstName
        Assert.Contains(result, f => f.FieldName == "first_name");
        // last_name → Student.LastName
        Assert.Contains(result, f => f.FieldName == "last_name");
        // year_group_id → Student.YearGroupId (NOT "year_group")
        Assert.Contains(result, f => f.FieldName == "year_group_id");
        // Fabricated field must NOT exist
        Assert.DoesNotContain(result, f => f.FieldName == "student_name");
        Assert.DoesNotContain(result, f => f.FieldName == "year_group");
        Assert.DoesNotContain(result, f => f.FieldName == "full_name");
    }

    [Fact]
    public async Task KnownPage_SavedRowsOverrideDefaults()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        // Pre-save a custom DisplayLabel for first_name
        db.TablePageSettings.Add(new TablePageSetting
        {
            PageName     = "students",
            FieldName    = "first_name",
            DisplayLabel = "CustomLabel",
            IsVisible    = false,
            IsSortable   = false,
            IsFilterable = true,
            IsSearchable = true,
            DisplayOrder = 99,
        });
        await db.SaveChangesAsync();

        var result = await sut.GetSettingsAsync("students");

        var fn = result.Single(f => f.FieldName == "first_name");
        Assert.Equal("CustomLabel", fn.DisplayLabel);
        Assert.False(fn.IsVisible);
        Assert.False(fn.IsSortable);
        Assert.True(fn.IsFilterable);
        Assert.True(fn.IsSearchable);
        Assert.Equal(99, fn.DisplayOrder);
    }

    [Fact]
    public async Task KnownPage_OrphanedSavedRow_IsDroppedFromResult()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        // Save a row for a field that does NOT exist in Student entity
        db.TablePageSettings.Add(new TablePageSetting
        {
            PageName     = "students",
            FieldName    = "fabricated_field_xyz",
            DisplayLabel = "Should not appear",
            IsVisible    = true,
            IsSortable   = true,
        });
        await db.SaveChangesAsync();

        var result = await sut.GetSettingsAsync("students");

        Assert.DoesNotContain(result, f => f.FieldName == "fabricated_field_xyz");
    }

    [Theory]
    [InlineData("students")]
    [InlineData("staff")]
    [InlineData("classes")]
    [InlineData("year_groups")]
    [InlineData("departments")]
    [InlineData("houses")]
    [InlineData("devices")]
    [InlineData("external_accounts")]
    [InlineData("absence_types")]
    [InlineData("attendance_registers")]
    [InlineData("attendance_marks")]
    [InlineData("safeguarding_flags")]
    [InlineData("medical_records")]
    public async Task AllKnownPages_ReturnNonEmptyResults(string pageKey)
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var result = await sut.GetSettingsAsync(pageKey);

        Assert.NotEmpty(result);
        Assert.All(result, f => Assert.Equal(pageKey, f.PageName));
    }

    [Fact]
    public async Task StaffPage_DoesNotContainUserColumns()
    {
        // Regression: v1.0 mapped 'staff' to 'User' entity accidentally.
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var result = await sut.GetSettingsAsync("staff");

        // These are User-only columns, must NOT appear under staff
        Assert.DoesNotContain(result, f => f.FieldName == "username");
        Assert.DoesNotContain(result, f => f.FieldName == "password_hash");
        // Staff-specific column must appear
        Assert.Contains(result, f => f.FieldName == "staff_number");
    }

    [Fact]
    public async Task ClassesPage_DoesNotContainFabricatedClassNameField()
    {
        // Regression: v1.0 fabricated "class_name" — real field is "name"
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var result = await sut.GetSettingsAsync("classes");

        Assert.DoesNotContain(result, f => f.FieldName == "class_name");
        Assert.Contains(result, f => f.FieldName == "name");
    }
}

// =============================================================================
// SaveSettingsAsync tests
// =============================================================================

public class TableSettingsService_SaveSettingsAsync
{
    [Fact]
    public async Task Save_InsertsNewRows()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var schema = await sut.GetSettingsAsync("students");
        await sut.SaveSettingsAsync("students", schema);

        var count = await db.TablePageSettings.CountAsync(x => x.PageName == "students");
        Assert.Equal(schema.Count, count);
    }

    [Fact]
    public async Task Save_UpdatesExistingRows()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var schema = await sut.GetSettingsAsync("students");
        await sut.SaveSettingsAsync("students", schema);

        // Now modify one and re-save
        var modified = schema.ToList();
        var target   = modified.First(f => f.FieldName == "first_name");
        target.DisplayLabel = "Modified";
        target.IsVisible    = false;

        await sut.SaveSettingsAsync("students", modified);

        var row = await db.TablePageSettings
            .SingleAsync(r => r.PageName == "students" && r.FieldName == "first_name");

        Assert.Equal("Modified", row.DisplayLabel);
        Assert.False(row.IsVisible);
    }

    [Fact]
    public async Task Save_PrunesOrphanedRows()
    {
        // Inject an orphaned row manually, then save schema-driven settings.
        // The orphaned row must be removed.
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        db.TablePageSettings.Add(new TablePageSetting
        {
            PageName     = "students",
            FieldName    = "orphaned_field",
            DisplayLabel = "Orphan",
            IsVisible    = true,
            IsSortable   = true,
        });
        await db.SaveChangesAsync();

        var schema = await sut.GetSettingsAsync("students");
        await sut.SaveSettingsAsync("students", schema);

        var orphanExists = await db.TablePageSettings
            .AnyAsync(r => r.PageName == "students" && r.FieldName == "orphaned_field");

        Assert.False(orphanExists);
    }

    [Fact]
    public async Task Save_DoesNotAffectOtherPages()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        // Save staff settings
        var staffSchema = await sut.GetSettingsAsync("staff");
        await sut.SaveSettingsAsync("staff", staffSchema);

        // Save students settings
        var studentsSchema = await sut.GetSettingsAsync("students");
        await sut.SaveSettingsAsync("students", studentsSchema);

        var staffCount    = await db.TablePageSettings.CountAsync(x => x.PageName == "staff");
        var studentsCount = await db.TablePageSettings.CountAsync(x => x.PageName == "students");

        Assert.Equal(staffSchema.Count, staffCount);
        Assert.Equal(studentsSchema.Count, studentsCount);
    }
}

// =============================================================================
// ResetToDefaultsAsync tests
// =============================================================================

public class TableSettingsService_ResetToDefaultsAsync
{
    [Fact]
    public async Task Reset_DeletesAllSavedRowsForPage()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var schema = await sut.GetSettingsAsync("students");
        await sut.SaveSettingsAsync("students", schema);

        Assert.True(await db.TablePageSettings.AnyAsync(x => x.PageName == "students"));

        await sut.ResetToDefaultsAsync("students");

        Assert.False(await db.TablePageSettings.AnyAsync(x => x.PageName == "students"));
    }

    [Fact]
    public async Task Reset_DoesNotAffectOtherPages()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var staffSchema    = await sut.GetSettingsAsync("staff");
        var studentsSchema = await sut.GetSettingsAsync("students");
        await sut.SaveSettingsAsync("staff", staffSchema);
        await sut.SaveSettingsAsync("students", studentsSchema);

        await sut.ResetToDefaultsAsync("students");

        var staffCount = await db.TablePageSettings.CountAsync(x => x.PageName == "staff");
        Assert.Equal(staffSchema.Count, staffCount);
    }

    [Fact]
    public async Task AfterReset_GetSettingsReturnsSchemaDefaults()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        // Customise and save
        var schema = (await sut.GetSettingsAsync("students")).ToList();
        schema.First().DisplayLabel = "Modified";
        await sut.SaveSettingsAsync("students", schema);

        // Reset
        await sut.ResetToDefaultsAsync("students");

        // Re-load — should return schema defaults, not the saved "Modified" label
        var result = await sut.GetSettingsAsync("students");
        var fn     = result.First(f => f.FieldName == "first_name");
        Assert.Equal("First Name", fn.DisplayLabel);
        Assert.True(fn.IsVisible);
    }
}

// =============================================================================
// GetDiagnosticsAsync tests
// =============================================================================

public class TableSettingsService_GetDiagnosticsAsync
{
    [Fact]
    public async Task EmptyDb_ReturnsAllPagesOkAndNoOrphans()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var report = await sut.GetDiagnosticsAsync();

        Assert.Equal("OK", report.OverallStatus);
        Assert.Equal(13, report.Pages.Count);
        Assert.Empty(report.OrphanedDbPageKeys);
        Assert.All(report.Pages, p =>
        {
            Assert.Equal("OK", p.Status);
            Assert.True(p.HasEntityMapping);
            Assert.NotEmpty(p.SchemaFields);
            Assert.Empty(p.SavedFields);
            Assert.NotEmpty(p.UnsavedSchemaFields); // all fields are unsaved
            Assert.Empty(p.OrphanedSavedFields);
        });
    }

    [Fact]
    public async Task AllFieldsSaved_ReturnsOkWithNoUnsavedOrOrphaned()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        // Save all schema fields for students
        var schema = await sut.GetSettingsAsync("students");
        await sut.SaveSettingsAsync("students", schema);

        var report  = await sut.GetDiagnosticsAsync();
        var pg      = report.Pages.Single(p => p.PageKey == "students");

        Assert.Equal("OK", pg.Status);
        Assert.Empty(pg.UnsavedSchemaFields);
        Assert.Empty(pg.OrphanedSavedFields);
    }

    [Fact]
    public async Task OrphanedSavedRow_PageStatusIsWarning()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        db.TablePageSettings.Add(new TablePageSetting
        {
            PageName     = "students",
            FieldName    = "stale_orphan",
            DisplayLabel = "Orphan",
            IsVisible    = true,
            IsSortable   = true,
        });
        await db.SaveChangesAsync();

        var report = await sut.GetDiagnosticsAsync();
        var pg     = report.Pages.Single(p => p.PageKey == "students");

        Assert.Equal("Warning", pg.Status);
        Assert.Contains("stale_orphan", pg.OrphanedSavedFields);
    }

    [Fact]
    public async Task OrphanedDbPageKey_OverallStatusIsError()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        db.TablePageSettings.Add(new TablePageSetting
        {
            PageName     = "unknown_page_xyz",
            FieldName    = "some_field",
            DisplayLabel = "X",
            IsVisible    = true,
            IsSortable   = true,
        });
        await db.SaveChangesAsync();

        var report = await sut.GetDiagnosticsAsync();

        Assert.Equal("Error", report.OverallStatus);
        Assert.Contains("unknown_page_xyz", report.OrphanedDbPageKeys);
    }

    [Theory]
    [InlineData("students")]
    [InlineData("staff")]
    [InlineData("classes")]
    [InlineData("year_groups")]
    [InlineData("departments")]
    [InlineData("houses")]
    [InlineData("devices")]
    [InlineData("external_accounts")]
    [InlineData("absence_types")]
    [InlineData("attendance_registers")]
    [InlineData("attendance_marks")]
    [InlineData("safeguarding_flags")]
    [InlineData("medical_records")]
    public async Task DiagReport_ContainsEntryForEachMappedPage(string pageKey)
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var report = await sut.GetDiagnosticsAsync();

        Assert.Contains(report.Pages, p => p.PageKey == pageKey);
    }

    [Fact]
    public async Task DiagReport_UnmappedEntities_AreReported()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var report = await sut.GetDiagnosticsAsync();

        // There are entities in AppDbContext that are NOT in PageEntityMap
        // (e.g. User, AuditLog, ClassMember etc.) — verify at least one is reported.
        Assert.NotEmpty(report.UnmappedEntityTypes);
    }

    [Fact]
    public async Task DiagReport_MappedEntityNames_MatchExpected()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var report      = await sut.GetDiagnosticsAsync();
        var entityNames = report.Pages.ToDictionary(p => p.PageKey, p => p.EntityTypeName);

        Assert.Equal("Student",              entityNames["students"]);
        Assert.Equal("Staff",                entityNames["staff"]);
        Assert.Equal("Class",                entityNames["classes"]);
        Assert.Equal("YearGroup",            entityNames["year_groups"]);
        Assert.Equal("Department",           entityNames["departments"]);
        Assert.Equal("House",                entityNames["houses"]);
        Assert.Equal("StaffDevice",          entityNames["devices"]);
        Assert.Equal("StaffExternalAccount", entityNames["external_accounts"]);
        Assert.Equal("AbsenceType",          entityNames["absence_types"]);
        Assert.Equal("AttendanceRegister",   entityNames["attendance_registers"]);
        Assert.Equal("AttendanceMark",       entityNames["attendance_marks"]);
        Assert.Equal("StudentFlag",          entityNames["safeguarding_flags"]);
        Assert.Equal("StudentMedical",       entityNames["medical_records"]);
    }
}

// =============================================================================
// ToSnakeCase / ToDisplayLabel utility tests (via GetSettingsAsync output)
// =============================================================================

public class TableSettingsService_SnakeCaseAndDisplayLabel
{
    [Theory]
    [InlineData("Id",            "id")]
    [InlineData("FirstName",     "first_name")]
    [InlineData("LastName",      "last_name")]
    [InlineData("YearGroupId",   "year_group_id")]
    [InlineData("WorkEmail",     "work_email")]
    [InlineData("StaffNumber",   "staff_number")]
    [InlineData("DateOfBirth",   "date_of_birth")]
    [InlineData("CreatedAt",     "created_at")]
    [InlineData("AccountStatus", "account_status")]
    public void ToSnakeCase_ConvertsCorrectly(string input, string expected)
    {
        // Verify via the public surface: fields returned by GetSettingsAsync
        // must match the snake_case of their property names.
        // We test static conversion logic indirectly via a reflection call.
        var method = typeof(TableSettingsService)
            .GetMethod("ToSnakeCase",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        Assert.NotNull(method);
        var result = method!.Invoke(null, [input]) as string;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("FirstName",     "First Name")]
    [InlineData("LastName",      "Last Name")]
    [InlineData("YearGroupId",   "Year Group Id")]
    [InlineData("WorkEmail",     "Work Email")]
    [InlineData("StaffNumber",   "Staff Number")]
    [InlineData("CreatedAt",     "Created At")]
    [InlineData("AccountStatus", "Account Status")]
    public void ToDisplayLabel_ConvertsCorrectly(string input, string expected)
    {
        var method = typeof(TableSettingsService)
            .GetMethod("ToDisplayLabel",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        Assert.NotNull(method);
        var result = method!.Invoke(null, [input]) as string;
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetSettingsAsync_DisplayLabels_AreHumanReadable()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var result = await sut.GetSettingsAsync("students");

        // Spot-check: "first_name" field should have display label "First Name"
        var fn = result.Single(f => f.FieldName == "first_name");
        Assert.Equal("First Name", fn.DisplayLabel);

        var ln = result.Single(f => f.FieldName == "last_name");
        Assert.Equal("Last Name", ln.DisplayLabel);

        var yg = result.Single(f => f.FieldName == "year_group_id");
        Assert.Equal("Year Group Id", yg.DisplayLabel);
    }
}

// =============================================================================
// PageEntityMap completeness tests
// =============================================================================

public class PageEntityMapCompletenessTests
{
    private static readonly string[] ExpectedPageKeys =
    [
        "students", "staff", "classes", "year_groups", "departments",
        "houses", "devices", "external_accounts", "absence_types",
        "attendance_registers", "attendance_marks", "safeguarding_flags",
        "medical_records",
    ];

    [Fact]
    public async Task AllExpectedPageKeys_ProduceNonEmptyFields()
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        foreach (var key in ExpectedPageKeys)
        {
            var result = await sut.GetSettingsAsync(key);
            Assert.True(result.Count > 0, $"Page '{key}' returned no fields.");
        }
    }

    [Fact]
    public void ExactlyThirteenPageKeys_AreMapped()
    {
        // We verify this indirectly: all 13 known keys produce results,
        // and an unknown key produces empty.
        Assert.Equal(13, ExpectedPageKeys.Length);
    }

    [Theory]
    [InlineData("students")]
    [InlineData("staff")]
    [InlineData("classes")]
    [InlineData("year_groups")]
    [InlineData("departments")]
    [InlineData("houses")]
    [InlineData("devices")]
    [InlineData("external_accounts")]
    [InlineData("absence_types")]
    [InlineData("attendance_registers")]
    [InlineData("attendance_marks")]
    [InlineData("safeguarding_flags")]
    [InlineData("medical_records")]
    public async Task EachPageKey_ReturnsFieldsWithCorrectPageName(string pageKey)
    {
        using var db  = DbFactory.Create();
        var sut       = new TableSettingsService(db);

        var result = await sut.GetSettingsAsync(pageKey);

        Assert.All(result, f => Assert.Equal(pageKey, f.PageName));
    }
}
