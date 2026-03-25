/*
===============================================================================
 File        : ClassDetailViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : ViewModel for ClassDetailPageV2. Loads the full class detail
               record via ClassesApiServiceV2. Drives ClassDetailPageV2.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 7).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in DI (Phase 10).
===============================================================================
*/

using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.ViewModels.V2;

/// <summary>
/// Drives the Class detail page V2. Register as Scoped (Phase 10).
/// </summary>
public sealed class ClassDetailViewModelV2
{
    private readonly ClassesApiServiceV2 _api;

    public ClassDetailViewModelV2(ClassesApiServiceV2 api) => _api = api;

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------

    public ClassFullViewDto? Item { get; private set; }
    public bool IsLoading { get; private set; }
    public string? Error { get; private set; }

    // -------------------------------------------------------------------------
    // Data loading
    // -------------------------------------------------------------------------

    public async Task LoadAsync(long id, CancellationToken ct = default)
    {
        IsLoading = true;
        Error = null;
        Item = null;

        var result = await _api.GetDetailAsync(id, ct);
        if (result.Success)
            Item = result.Data;
        else
            Error = result.ErrorMessage ?? "Failed to load class.";

        IsLoading = false;
    }
}
