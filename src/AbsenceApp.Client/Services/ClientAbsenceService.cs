/*
===============================================================================
 File        : ClientAbsenceService.cs
 Namespace   : AbsenceApp.Client.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Blazor-facing wrapper around IAbsenceService.
               Provides the Blazor component layer with an injected service
               for absence-record operations without coupling pages directly
               to the core interface.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Registered as a singleton in MauiProgram; injected directly into Blazor
     pages via @inject.
   - All methods are simple pass-throughs; business logic lives in
     AbsenceApp.Core.Services.AbsenceService.
===============================================================================
*/

using AbsenceApp.Core.Interfaces;
using AbsenceApp.Core.Models;

namespace AbsenceApp.Client.Services;

// =========================================================================
// Client absence service — thin Blazor-layer wrapper around IAbsenceService
// =========================================================================

public class ClientAbsenceService
{
    // =========================================================================
    // Dependencies — core absence service injected via constructor
    // =========================================================================

    private readonly IAbsenceService _absenceService;

    public ClientAbsenceService(IAbsenceService absenceService)
    {
        _absenceService = absenceService;
    }

    // =========================================================================
    // Public operations — absence CRUD delegated to IAbsenceService
    // =========================================================================

    public Task<IEnumerable<AbsenceRecord>> GetAbsencesForStudentAsync(string studentId)
    {
        return _absenceService.GetAbsencesForStudentAsync(studentId);
    }

    public Task AddAbsenceAsync(AbsenceRecord record)
    {
        return _absenceService.AddAbsenceAsync(record);
    }
}