/*
===============================================================================
 File        : StaffDeviceAudit.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the staff_device_audit table.
               Captures every create / update / delete action on a staff device record.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - No navigation properties; FK integrity is enforced at the database layer.
   - All required string properties use = default! to satisfy the nullable compiler.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class StaffDeviceAudit
{
    public int Id { get; set; }
    public int StaffDeviceId { get; set; }
    public int StaffId { get; set; }
    public string Action { get; set; } = default!;
    public string? Details { get; set; }
    public DateTime CreatedAt { get; set; }
}
