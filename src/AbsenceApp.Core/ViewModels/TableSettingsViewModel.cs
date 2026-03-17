/*
===============================================================================
 File        : TableSettingsViewModel.cs
 Namespace   : AbsenceApp.Core.ViewModels
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Backing ViewModel for the Table Settings page.  Holds the
               working copy of field-level display settings for the selected
               page.  Changes are staged in memory and only written to the
               database when SaveAsync() is called.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial implementation.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class TableSettingsViewModel
{
    private readonly ITableSettingsService _service;

    public string                    SelectedPage { get; private set; } = "students";
    public List<TablePageSettingDto> Settings     { get; private set; } = [];
    public bool                      IsLoading    { get; private set; }
    public bool                      IsSaving     { get; private set; }
    public bool                      IsSaved      { get; private set; }
    public string?                   ErrorMessage { get; private set; }

    public TableSettingsViewModel(ITableSettingsService service) => _service = service;

    // =========================================================================
    // LoadAsync — fetch settings for pageName; stages results in Settings list
    // =========================================================================

    public async Task LoadAsync(string pageName)
    {
        SelectedPage = pageName;
        IsLoading    = true;
        IsSaved      = false;
        ErrorMessage = null;

        try
        {
            Settings = (await _service.GetSettingsAsync(pageName)).ToList();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            Settings     = [];
        }
        finally
        {
            IsLoading = false;
        }
    }

    // =========================================================================
    // SaveAsync — persist the current working copy to the database
    // =========================================================================

    public async Task SaveAsync()
    {
        IsSaving     = true;
        IsSaved      = false;
        ErrorMessage = null;

        try
        {
            await _service.SaveSettingsAsync(SelectedPage, Settings);
            IsSaved = true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsSaving = false;
        }
    }

    // =========================================================================
    // ResetToDefaultsAsync — remove persisted rows; next Load returns defaults
    // =========================================================================

    public async Task ResetToDefaultsAsync()
    {
        await _service.ResetToDefaultsAsync(SelectedPage);
        await LoadAsync(SelectedPage);
    }

    // =========================================================================
    // ClearSaved — dismiss the success banner without reloading
    // =========================================================================

    public void ClearSaved() => IsSaved = false;
}
