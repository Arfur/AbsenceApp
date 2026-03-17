/*
===============================================================================
 File        : TableSettingsService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Reads and writes per-table field-level display configuration
               from the table_page_settings database table.  When no rows
               exist for a given page, returns hard-coded defaults so pages
               work correctly before an administrator has made any changes.
-------------------------------------------------------------------------------
 Notes       :
   - GetSettingsAsync returns defaults when the table is empty for pageName;
     it does NOT auto-seed the database.  Settings are only written on Save.
   - SaveSettingsAsync is an upsert: field rows are matched by FieldName.
   - Defaults are defined inline for: students, staff, classes.
     TODO: add default column definitions for the remaining pages listed in
     the Table Settings dropdown (year_groups, departments, houses, devices,
     external_accounts, absence_types, attendance_registers, attendance_marks,
     safeguarding_flags, medical_records).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial implementation.
===============================================================================
*/

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
    // GetSettingsAsync — persisted rows first; hard-coded defaults as fallback
    // =========================================================================

    public async Task<IReadOnlyList<TablePageSettingDto>> GetSettingsAsync(string pageName)
    {
        var rows = await _db.TablePageSettings
            .Where(x => x.PageName == pageName)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync();

        return rows.Count > 0
            ? rows.Select(ToDto).ToList().AsReadOnly()
            : GetDefaults(pageName);
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
    // ResetToDefaultsAsync — delete all rows; next GetSettings returns defaults
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
    // GetDefaults — hard-coded factory defaults per page
    // -------------------------------------------------------------------------

    private static IReadOnlyList<TablePageSettingDto> GetDefaults(string pageName) =>
        pageName switch
        {
            "students" => Build("students",
            [
                ("student_name", "Student Name",  true,  true,  false, true,  1),
                ("year_group",   "Year Group",    true,  true,  true,  false, 2),
            ]),

            "staff" => Build("staff",
            [
                ("full_name",  "Full Name",  true,  true,  false, true,  1),
                ("username",   "Username",   true,  true,  false, true,  2),
                ("email",      "Email",      true,  true,  false, true,  3),
                ("is_active",  "Status",     true,  true,  true,  false, 4),
            ]),

            "classes" => Build("classes",
            [
                ("class_name",  "Class Name",  true,  true,  false, true,  1),
                ("description", "Description", true,  false, false, true,  2),
            ]),

            // TODO: add defaults for year_groups, departments, houses, devices,
            // external_accounts, absence_types, attendance_registers,
            // attendance_marks, safeguarding_flags, medical_records
            _ => []
        };

    private static IReadOnlyList<TablePageSettingDto> Build(
        string pageName,
        (string field, string label, bool vis, bool sort, bool filter, bool search, int order)[] cols) =>
        cols.Select(c => new TablePageSettingDto
        {
            PageName     = pageName,
            FieldName    = c.field,
            DisplayLabel = c.label,
            IsVisible    = c.vis,
            IsSortable   = c.sort,
            IsFilterable = c.filter,
            IsSearchable = c.search,
            DisplayOrder = c.order,
        }).ToList().AsReadOnly();
}
