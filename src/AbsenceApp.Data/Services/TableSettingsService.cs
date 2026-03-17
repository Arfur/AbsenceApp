/*
===============================================================================
 File        : TableSettingsService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Reads and writes per-table field-level display configuration
               from the table_page_settings database table.
-------------------------------------------------------------------------------
 Notes  (v2 — schema-driven):
   - The available field list for each page is derived directly from EF Core
     model metadata (IEntityType.GetProperties()), NOT from a manually
     maintained hard-coded list.  This ensures:
       • All real DB columns appear in Table Settings automatically.
       • Fabricated fields (e.g. "student_name", "full_name") are impossible.
       • Adding a new column to an entity causes it to appear in the Table
         Settings UI on next load — no code changes required.
   - GetSettingsAsync merges the schema field list with any saved DB rows:
       • Saved settings (label, visibility, etc.) are preserved per field.
       • New schema fields with no saved row get sensible defaults.
       • DB rows for fields no longer in the schema are silently dropped.
   - PageEntityMap is the single source of truth mapping the UI page name
     to the CLR entity type whose properties define the field list.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial hard-coded defaults implementation.
   - 2.0.0  2026-03-17  Replaced hard-coded defaults with EF schema reflection.
                        Added PageEntityMap for all 13 supported pages.
                        Fixed staff → Staff entity (was using users columns).
                        Fixed students → removed student_name / year_group.
                        Fixed classes → removed class_name fabrication.
                        Added field definitions for all 10 previously empty pages.
===============================================================================
*/

using System.Text;
using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

public class TableSettingsService : ITableSettingsService
{
    private readonly AppDbContext _db;

    public TableSettingsService(AppDbContext db) => _db = db;

    // =========================================================================
    // Page name → CLR entity type mapping.
    // UI page name keys must match the values in TableSettingsPage.razor's
    // TableOptions array exactly.  The CLR type drives EF metadata lookup.
    // =========================================================================

    private static readonly Dictionary<string, Type> PageEntityMap = new()
    {
        ["students"]             = typeof(Student),
        ["staff"]                = typeof(Staff),
        ["classes"]              = typeof(Class),
        ["year_groups"]          = typeof(YearGroup),
        ["departments"]          = typeof(Department),
        ["houses"]               = typeof(House),
        ["devices"]              = typeof(StaffDevice),
        ["external_accounts"]    = typeof(StaffExternalAccount),
        ["absence_types"]        = typeof(AbsenceType),
        ["attendance_registers"] = typeof(AttendanceRegister),
        ["attendance_marks"]     = typeof(AttendanceMark),
        ["safeguarding_flags"]   = typeof(StudentFlag),
        ["medical_records"]      = typeof(StudentMedical),
    };

    // =========================================================================
    // GetSettingsAsync — schema-driven merge.
    //
    // 1. Derive the canonical field list from EF metadata (real schema).
    // 2. Load any saved rows from table_page_settings, keyed by FieldName.
    // 3. For each schema field: use saved settings if present, else defaults.
    //    Fields saved in the DB but absent from the schema are dropped.
    // =========================================================================

    public async Task<IReadOnlyList<TablePageSettingDto>> GetSettingsAsync(string pageName)
    {
        try
        {
            var schemaFields = GetSchemaFields(pageName);
            if (schemaFields.Count == 0) return [];

            var saved = await _db.TablePageSettings
                .Where(x => x.PageName == pageName)
                .ToDictionaryAsync(x => x.FieldName);

            var result = schemaFields
                .Select(sf => saved.TryGetValue(sf.FieldName, out var row) ? ToDto(row) : sf)
                .ToList();

            return result.AsReadOnly();
        }
        catch
        {
            // DB unavailable — return schema-driven defaults so the page still renders.
            return GetSchemaFields(pageName);
        }
    }

    // =========================================================================
    // SaveSettingsAsync — upsert one row per (pageName, fieldName) pair
    // =========================================================================

    public async Task SaveSettingsAsync(string pageName, IEnumerable<TablePageSettingDto> settings)
    {
        var existing = await _db.TablePageSettings
            .Where(x => x.PageName == pageName)
            .ToListAsync();

        foreach (var dto in settings)
        {
            var row = existing.FirstOrDefault(r => r.FieldName == dto.FieldName);
            if (row is null)
            {
                row = new TablePageSetting { PageName = pageName, FieldName = dto.FieldName };
                _db.TablePageSettings.Add(row);
            }

            row.DisplayLabel = dto.DisplayLabel;
            row.IsVisible    = dto.IsVisible;
            row.IsSortable   = dto.IsSortable;
            row.IsFilterable = dto.IsFilterable;
            row.IsSearchable = dto.IsSearchable;
            row.DisplayOrder = dto.DisplayOrder;
        }

        await _db.SaveChangesAsync();
    }

    // =========================================================================
    // ResetToDefaultsAsync — delete saved rows; next load returns schema defaults
    // =========================================================================

    public async Task ResetToDefaultsAsync(string pageName)
    {
        var rows = await _db.TablePageSettings
            .Where(x => x.PageName == pageName)
            .ToListAsync();

        _db.TablePageSettings.RemoveRange(rows);
        await _db.SaveChangesAsync();
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    private static TablePageSettingDto ToDto(TablePageSetting row) => new()
    {
        Id           = row.Id,
        PageName     = row.PageName,
        FieldName    = row.FieldName,
        DisplayLabel = row.DisplayLabel,
        IsVisible    = row.IsVisible,
        IsSortable   = row.IsSortable,
        IsFilterable = row.IsFilterable,
        IsSearchable = row.IsSearchable,
        DisplayOrder = row.DisplayOrder,
    };

    // -------------------------------------------------------------------------
    // GetSchemaFields — derives the canonical field list from EF Core metadata.
    //
    // Each scalar property on the mapped entity becomes one TablePageSettingDto.
    // FieldName is the snake_case version of the C# property name, matching the
    // DB column naming convention used throughout the application.
    // All fields default to Visible=true, Sortable=true; users can override via
    // the Table Settings UI.
    // -------------------------------------------------------------------------

    private IReadOnlyList<TablePageSettingDto> GetSchemaFields(string pageName)
    {
        if (!PageEntityMap.TryGetValue(pageName, out var clrType))
            return [];

        var entityType = _db.Model.FindEntityType(clrType);
        if (entityType is null) return [];

        return entityType.GetProperties()
            .Select((p, i) => new TablePageSettingDto
            {
                PageName     = pageName,
                FieldName    = ToSnakeCase(p.Name),
                DisplayLabel = ToDisplayLabel(p.Name),
                IsVisible    = true,
                IsSortable   = true,
                IsFilterable = false,
                IsSearchable = false,
                DisplayOrder = i + 1,
            })
            .ToList()
            .AsReadOnly();
    }

    // -------------------------------------------------------------------------
    // ToSnakeCase — "FirstName" → "first_name", "YearGroupId" → "year_group_id"
    // -------------------------------------------------------------------------

    private static string ToSnakeCase(string name)
    {
        var sb = new StringBuilder(name.Length + 4);
        for (var i = 0; i < name.Length; i++)
        {
            if (char.IsUpper(name[i]) && i > 0)
                sb.Append('_');
            sb.Append(char.ToLower(name[i]));
        }
        return sb.ToString();
    }

    // -------------------------------------------------------------------------
    // ToDisplayLabel — "FirstName" → "First Name", "YearGroupId" → "Year Group Id"
    // -------------------------------------------------------------------------

    private static string ToDisplayLabel(string propertyName)
    {
        var sb = new StringBuilder(propertyName.Length + 4);
        for (var i = 0; i < propertyName.Length; i++)
        {
            if (char.IsUpper(propertyName[i]) && i > 0)
                sb.Append(' ');
            sb.Append(propertyName[i]);
        }
        return sb.ToString();
    }

    /* === REMOVED in v2.0 ====================================================
     * GetDefaults() and Build() helpers — replaced by GetSchemaFields() above.
     * The old hard-coded switch contained:
     *   students: fabricated "student_name" and "year_group" (real: year_group_id)
     *   staff:    ALL 4 fields were from users (TABLE1), not staff (TABLE2)
     *   classes:  fabricated "class_name" (real field is "name")
     *   all other pages: returned [] silently
     ========================================================================*/
}
