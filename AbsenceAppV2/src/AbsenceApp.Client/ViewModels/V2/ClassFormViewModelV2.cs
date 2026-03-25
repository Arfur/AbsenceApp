/*
===============================================================================
 File        : ClassFormViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : ViewModel for ClassFormPageV2 (add and edit). Manages class
               form field state and delegates create/update calls to
               ClassesApiServiceV2.
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
/// Drives the Class add/edit form page V2. Register as Scoped (Phase 10).
/// </summary>
public sealed class ClassFormViewModelV2
{
    private readonly ClassesApiServiceV2 _api;

    public ClassFormViewModelV2(ClassesApiServiceV2 api) => _api = api;

    // -------------------------------------------------------------------------
    // Mode
    // -------------------------------------------------------------------------

    public bool IsNew { get; private set; } = true;
    public long EditId { get; private set; }

    // -------------------------------------------------------------------------
    // UI state
    // -------------------------------------------------------------------------

    public bool IsBusy { get; private set; }
    public string? Error { get; private set; }
    public bool SaveSuccess { get; private set; }

    // -------------------------------------------------------------------------
    // Form fields
    // -------------------------------------------------------------------------

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Initialise
    // -------------------------------------------------------------------------

    public void LoadForCreate()
    {
        IsNew = true;
        EditId = 0;
        Name = string.Empty;
        Description = string.Empty;
    }

    public async Task LoadForEditAsync(long id, CancellationToken ct = default)
    {
        IsNew = false;
        EditId = id;
        IsBusy = true;
        Error = null;

        var result = await _api.GetDetailAsync(id, ct);
        if (result.Success && result.Data is not null)
        {
            Name        = result.Data.Name;
            Description = result.Data.Description ?? string.Empty;
        }
        else
        {
            Error = result.ErrorMessage ?? "Failed to load class for editing.";
        }

        IsBusy = false;
    }

    // -------------------------------------------------------------------------
    // Actions
    // -------------------------------------------------------------------------

    public async Task<bool> SaveAsync(CancellationToken ct = default)
    {
        IsBusy = true;
        Error = null;
        SaveSuccess = false;

        var dto = new ClassDto
        {
            Id          = IsNew ? 0 : (int)EditId,
            Name        = Name,
            Description = string.IsNullOrWhiteSpace(Description) ? null : Description,
        };

        if (IsNew)
        {
            var result = await _api.CreateAsync(dto, ct);
            SaveSuccess = result.Success;
            if (!result.Success) Error = result.ErrorMessage ?? "Failed to create class.";
        }
        else
        {
            var result = await _api.UpdateAsync(EditId, dto, ct);
            SaveSuccess = result.Success;
            if (!result.Success) Error = result.ErrorMessage ?? "Failed to update class.";
        }

        IsBusy = false;
        return SaveSuccess;
    }
}
