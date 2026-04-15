/*
===============================================================================
 File        : PageFormViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : ViewModel shared by PagesListPageV2 for the Add/Edit page form.
               Manages form field binding and delegates persistence to
               PagesApiServiceV2 (direct DB via IServiceScopeFactory).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E16 Pages Registry).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in V2ServiceCollectionExtensions.cs.
   - IsNew=true → CreatePageAsync; IsNew=false → UpdatePageAsync.
   - CategoryOptions is a static list of the four known category keys.
===============================================================================
*/

using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.ViewModels.V2;

public sealed class PageFormViewModelV2
{
    // =========================================================================
    // Construction
    // =========================================================================

    private readonly PagesApiServiceV2 _svc;

    public PageFormViewModelV2(PagesApiServiceV2 svc) => _svc = svc;

    // =========================================================================
    // Mode
    // =========================================================================

    public bool IsNew { get; private set; } = true;

    // =========================================================================
    // Form fields
    // =========================================================================

    public int     PageId        { get; set; }
    public string  Name          { get; set; } = string.Empty;
    public string  Slug          { get; set; } = string.Empty;
    public string  Route         { get; set; } = string.Empty;
    public string  CategoryKey   { get; set; } = string.Empty;
    public string  MenuKey       { get; set; } = string.Empty;
    public string  IconKey       { get; set; } = string.Empty;
    public string  Description   { get; set; } = string.Empty;
    public bool    IsActive      { get; set; } = true;
    public int     SortOrder     { get; set; }

    // Capability flags
    public bool    SupportsRead   { get; set; }
    public bool    SupportsWrite  { get; set; }
    public bool    SupportsCreate { get; set; }
    public bool    SupportsDelete { get; set; }
    public bool    SupportsImport { get; set; }
    public bool    SupportsExport { get; set; }

    // =========================================================================
    // Reference data
    // =========================================================================

    public static IReadOnlyList<string> CategoryOptions =>
        ["ADMIN", "PEOPLE", "ACADEMIC", "ATTENDANCE"];

    // =========================================================================
    // State
    // =========================================================================

    public bool   IsLoading { get; private set; }
    public bool   IsSaving  { get; private set; }
    public string? Error    { get; private set; }
    public string? Success  { get; private set; }

    // =========================================================================
    // Initialise for Add
    // =========================================================================

    public void InitNew()
    {
        IsNew       = true;
        PageId      = 0;
        Name        = string.Empty;
        Slug        = string.Empty;
        Route       = string.Empty;
        CategoryKey = string.Empty;
        MenuKey     = string.Empty;
        IconKey     = string.Empty;
        Description = string.Empty;
        IsActive    = true;
        SortOrder   = 0;
        SupportsRead   = false;
        SupportsWrite  = false;
        SupportsCreate = false;
        SupportsDelete = false;
        SupportsImport = false;
        SupportsExport = false;
        Error   = null;
        Success = null;
    }

    // =========================================================================
    // Initialise for Edit
    // =========================================================================

    public async Task InitEditAsync(int id, CancellationToken ct = default)
    {
        IsNew     = false;
        IsLoading = true;
        Error     = null;
        Success   = null;
        try
        {
            var dto = await _svc.GetPageForEditAsync(id, ct);
            if (dto is null)
            {
                Error = "Page not found.";
                return;
            }

            PageId         = dto.Id;
            Name           = dto.Name;
            Slug           = dto.Slug;
            Route          = dto.Route;
            CategoryKey    = dto.CategoryKey;
            MenuKey        = dto.MenuKey;
            IconKey        = dto.IconKey  ?? string.Empty;
            Description    = dto.Description ?? string.Empty;
            IsActive       = dto.IsActive;
            SortOrder      = dto.SortOrder;
            SupportsRead   = dto.SupportsRead;
            SupportsWrite  = dto.SupportsWrite;
            SupportsCreate = dto.SupportsCreate;
            SupportsDelete = dto.SupportsDelete;
            SupportsImport = dto.SupportsImport;
            SupportsExport = dto.SupportsExport;
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsLoading = false; }
    }

    // =========================================================================
    // Save
    // =========================================================================

    public async Task<bool> SaveAsync(CancellationToken ct = default)
    {
        Error    = null;
        Success  = null;
        IsSaving = true;

        try
        {
            if (IsNew)
            {
                var (ok, err, _) = await _svc.CreatePageAsync(new PageCreateDto
                {
                    Name           = Name.Trim(),
                    Slug           = Slug.Trim(),
                    Route          = Route.Trim(),
                    CategoryKey    = CategoryKey.Trim(),
                    MenuKey        = MenuKey.Trim(),
                    IconKey        = string.IsNullOrWhiteSpace(IconKey) ? null : IconKey.Trim(),
                    Description    = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim(),
                    IsActive       = IsActive,
                    SortOrder      = SortOrder,
                    SupportsRead   = SupportsRead,
                    SupportsWrite  = SupportsWrite,
                    SupportsCreate = SupportsCreate,
                    SupportsDelete = SupportsDelete,
                    SupportsImport = SupportsImport,
                    SupportsExport = SupportsExport,
                }, ct);

                if (!ok) { Error = err; return false; }
                Success = "Page created successfully.";
                return true;
            }
            else
            {
                var (ok, err) = await _svc.UpdatePageAsync(new PageUpdateDto
                {
                    Id             = PageId,
                    Name           = Name.Trim(),
                    Slug           = Slug.Trim(),
                    Route          = Route.Trim(),
                    CategoryKey    = CategoryKey.Trim(),
                    MenuKey        = MenuKey.Trim(),
                    IconKey        = string.IsNullOrWhiteSpace(IconKey) ? null : IconKey.Trim(),
                    Description    = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim(),
                    IsActive       = IsActive,
                    SortOrder      = SortOrder,
                    SupportsRead   = SupportsRead,
                    SupportsWrite  = SupportsWrite,
                    SupportsCreate = SupportsCreate,
                    SupportsDelete = SupportsDelete,
                    SupportsImport = SupportsImport,
                    SupportsExport = SupportsExport,
                }, ct);

                if (!ok) { Error = err; return false; }
                Success = "Page updated successfully.";
                return true;
            }
        }
        catch (Exception ex)
        {
            Error = ex.Message;
            return false;
        }
        finally
        {
            IsSaving = false;
        }
    }
}
