using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsenceApp.Data.Configurations;

// Legacy stub — Attendance entity is retained only for backward compatibility.
// Active attendance mapping is in AttendanceRegister / AttendanceMark.
public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.HasKey(a => a.AttendanceId);
        // Legacy stub: UserId and RecordedBy are int but User.Id is long.
        // Ignore navigation properties to avoid FK type-mismatch validation.
        builder.Ignore(a => a.User);
        builder.Ignore(a => a.Recorder);
        builder.Ignore(a => a.Class);
    }
}
