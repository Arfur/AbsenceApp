/*
===============================================================================
 File        : UserConfiguration.cs
 Namespace   : AbsenceApp.Data.Configurations
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-04-11
 Updated     : 2026-04-12
-------------------------------------------------------------------------------
 Purpose     : EF Core configuration for the User entity.
===============================================================================
*/

using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsenceApp.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // ---------------------------------------------------------------------
        // Table + Primary Key
        // ---------------------------------------------------------------------
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);   // REQUIRED 

        // ---------------------------------------------------------------------
        // Required fields
        // ---------------------------------------------------------------------
        builder.Property(u => u.Username).IsRequired();
        builder.Property(u => u.Email).IsRequired();
    }
}
