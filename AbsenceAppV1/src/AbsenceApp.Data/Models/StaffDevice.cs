/*
===============================================================================
 File        : StaffDevice.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the staff_devices table.
               Tracks hardware assets assigned to staff members.
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

public class StaffDevice
{
    public long Id { get; set; }
    public long StaffId { get; set; }
    public long DeviceTypeId { get; set; }
    public string SerialNumber { get; set; } = default!;
    public DateOnly AssignedDate { get; set; }
    public DateOnly? ReturnedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
