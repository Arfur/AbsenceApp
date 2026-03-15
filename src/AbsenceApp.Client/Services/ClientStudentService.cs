/*
===============================================================================
 File        : ClientStudentService.cs
 Namespace   : AbsenceApp.Client.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Blazor-facing wrapper around IStudentService.
               Provides the Blazor component layer with an injected service
               for student-record operations without coupling pages directly
               to the core interface.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Registered as a singleton in MauiProgram; injected directly into Blazor
     pages via @inject.
   - All methods are simple pass-throughs; business logic lives in
     AbsenceApp.Core.Services.StudentService.
===============================================================================
*/

using AbsenceApp.Core.Interfaces;
using AbsenceApp.Core.Models;

namespace AbsenceApp.Client.Services;

// =========================================================================
// Client student service — thin Blazor-layer wrapper around IStudentService
// =========================================================================

public class ClientStudentService
{
    // =========================================================================
    // Dependencies — core student service injected via constructor
    // =========================================================================

    private readonly IStudentService _studentService;

    public ClientStudentService(IStudentService studentService)
    {
        _studentService = studentService;
    }

    // =========================================================================
    // Public operations — student queries delegated to IStudentService
    // =========================================================================

    public Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        return _studentService.GetAllStudentsAsync();
    }

    public Task<Student?> GetStudentByIdAsync(string id)
    {
        return _studentService.GetStudentByIdAsync(id);
    }
}