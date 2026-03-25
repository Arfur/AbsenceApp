/*
===============================================================================
 File        : StudentsViewModel.cs
 Namespace   : AbsenceApp.Core.ViewModels
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-13
 Updated     : 2026-03-18
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the Students list page. Loads all students via
               IStudentFullViewService and exposes StudentFullViewDto rows for
               data-binding in the Full View table.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
   - 1.1.0  2026-03-18  Switched to StudentFullViewDto and
                        IStudentFullViewService for Full View UI binding.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class StudentsViewModel
{
    private readonly IStudentFullViewService _studentService;

    public StudentsViewModel(IStudentFullViewService studentService)
    {
        _studentService = studentService;
    }

    public IEnumerable<StudentFullViewDto> Students { get; private set; } = Enumerable.Empty<StudentFullViewDto>();
    public bool    IsLoading    { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task LoadAsync()
    {
        IsLoading    = true;
        ErrorMessage = null;
        try
        {
            Students = await _studentService.GetAllAsync();
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
