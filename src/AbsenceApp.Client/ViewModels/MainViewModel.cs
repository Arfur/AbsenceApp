/*
===============================================================================
 File        : MainViewModel.cs
 Namespace   : AbsenceApp.Client.ViewModels
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Shared view-model that Blazor pages inject to load and mutate
               student and absence data.
               Registered as a singleton so all pages share the same state
               within a session.
-------------------------------------------------------------------------------
 Description :
   Properties:
     Title            — page title string (not yet bound in pages).
     Students         — full student list loaded by LoadAsync.
     SelectedStudent  — single student loaded by LoadStudentAsync.
     Absences         — absence records for the selected student.

   Methods:
     LoadAsync          — loads all students.
     LoadStudentAsync   — loads one student by ID.
     LoadAbsencesAsync  — loads absences for a student.
     AddAbsenceAsync    — creates and persists a new absence record.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Singleton registration means Students/Absences persist for the app
     lifetime; pages should call LoadAsync on each navigation if freshness
     is required.
   - Properties are plain auto-properties; no INotifyPropertyChanged is
     implemented — pages call StateHasChanged manually after awaiting loads.
===============================================================================
*/

using AbsenceApp.Client.Services;
using AbsenceApp.Core.Models;

namespace AbsenceApp.Client.ViewModels;

public class MainViewModel
{
    // =========================================================================
    // Dependencies — client service wrappers injected via constructor
    // =========================================================================

    private readonly ClientStudentService _clientStudentService;
    private readonly ClientAbsenceService _clientAbsenceService;

    public MainViewModel(ClientStudentService clientStudentService, ClientAbsenceService clientAbsenceService)
    {
        _clientStudentService = clientStudentService;
        _clientAbsenceService = clientAbsenceService;
    }

    // =========================================================================
    // Bindable state — properties populated by the data loading methods
    // =========================================================================

    public string Title { get; set; } = string.Empty;
    public IEnumerable<Student> Students { get; set; } = Enumerable.Empty<Student>();
    public Student? SelectedStudent { get; set; }

    public IEnumerable<AbsenceRecord> Absences { get; set; } = Enumerable.Empty<AbsenceRecord>();

    // =========================================================================
    // Data loading operations — async methods called from page OnInitializedAsync
    // =========================================================================

    public async Task LoadAsync()
    {
        Students = await _clientStudentService.GetAllStudentsAsync();
    }

    public async Task LoadStudentAsync(string id)
    {
        SelectedStudent = await _clientStudentService.GetStudentByIdAsync(id);
    }

    public async Task LoadAbsencesAsync(string studentId)
    {
        Absences = await _clientAbsenceService.GetAbsencesForStudentAsync(studentId);
    }

    // =========================================================================
    // Absence mutation — builds a new AbsenceRecord and delegates to service
    // =========================================================================

    public async Task AddAbsenceAsync(string studentId, string reason)
    {
        var record = new AbsenceRecord { Id = Guid.NewGuid().ToString(), StudentId = studentId, Date = DateTime.Today, Reason = reason };
        await _clientAbsenceService.AddAbsenceAsync(record);
    }
}