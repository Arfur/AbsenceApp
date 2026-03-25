/*
===============================================================================
 File        : AuditLogViewModel.cs
 Namespace   : AbsenceApp.Core.ViewModels
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the Audit Log page. Loads all audit entries or
               filters by user ID via IAuditLogService.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation in AbsenceApp.Client.
   - 1.1.0  2026-03-14  Moved to AbsenceApp.Core for testability.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

// ============================================================================
// AuditLogViewModel — backing state for an audit log viewer page
// ============================================================================
public class AuditLogViewModel
{
    // =========================================================================
    // Dependencies
    // =========================================================================
    private readonly IAuditLogService _auditLogService;

    public AuditLogViewModel(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    // =========================================================================
    // Bindable state
    // =========================================================================
    public IEnumerable<AuditLogDto> Entries { get; private set; } = Enumerable.Empty<AuditLogDto>();
    public bool IsLoading { get; private set; }
    public string? ErrorMessage { get; private set; }

    // =========================================================================
    // Commands
    // =========================================================================

    /// <summary>Loads all audit log entries.</summary>
    public async Task LoadAllAsync()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            Entries = await _auditLogService.GetAllAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>Loads audit log entries for a specific user.</summary>
    public async Task LoadByUserAsync(int userId)
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            Entries = await _auditLogService.GetByUserAsync(userId);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }
}
