/*
===============================================================================
 File        : AbsenceRecord.cs
 Namespace   : AbsenceApp.Core.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Domain model representing a single recorded student absence.
-------------------------------------------------------------------------------
 Description :
   Properties:
     Id         — unique identifier (GUID string).
     StudentId  — ID of the associated student.
     Date       — calendar date of the absence.
     Reason     — free-text reason supplied by the recorder.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial model created.
-------------------------------------------------------------------------------
 Notes       :
   - Id is a GUID in string form; new records generate it via
     Guid.NewGuid().ToString() in AddAbsenceAsync.
   - Reason is free text with no length constraint at the model level.
===============================================================================
*/

namespace AbsenceApp.Core.Models;

// =========================================================================
// Domain model — absence record; one row per student absence event
// =========================================================================

public class AbsenceRecord
{
    public string Id { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Reason { get; set; } = string.Empty;
}