/*
===============================================================================
 File        : ClassMemberConfiguration.cs
 Namespace   : AbsenceApp.Data.Configurations
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-15
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : EF Core fluent configuration for the ClassMembers entity.
               Defines composite primary key.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation (no file header at that time).
   - 1.1.0  2026-05-04  Phase 4.13: Fixed composite PK cm.UserId → cm.StaffId
                         to match live DB column and updated ClassMembers model.
===============================================================================
*/
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsenceApp.Data.Configurations;

// Legacy stub — ClassMembers entity is retained only for backward compatibility.
public class ClassMemberConfiguration : IEntityTypeConfiguration<ClassMembers>
{
    public void Configure(EntityTypeBuilder<ClassMembers> builder)
    {
        builder.HasKey(cm => new { cm.ClassId, cm.StaffId });
    }
}
