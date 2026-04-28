/*
===============================================================================
 File        : UserProfileConfiguration.cs
 Namespace   : AbsenceApp.Data.Configurations
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-22
 Updated     : 2026-04-22
-------------------------------------------------------------------------------
 Purpose     : EF Core configuration for the UserProfile entity.
               Provides deterministic table mapping, column definitions,
               and MySQL-compatible type mappings for profile fields.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-22  Initial creation. Added explicit table mapping for
                        userprofiles and defined column types, including
                        MySQL DATE mapping for DateOfBirth.
-------------------------------------------------------------------------------
 Notes       :
   - UserProfile has no navigation properties; FK integrity is enforced at
     the database layer.
   - All string properties use = default! in the model to satisfy nullable
     reference type rules.
===============================================================================
*/

using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsenceApp.Data.Configurations;

public sealed class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        // ---------------------------------------------------------------------
        // Table + Primary Key
        // ---------------------------------------------------------------------
        builder.ToTable("userprofiles");
        builder.HasKey(p => p.Id);

        // ---------------------------------------------------------------------
        // Column Types
        // ---------------------------------------------------------------------
        builder.Property(p => p.DisplayName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.ThemePreference)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.ProfilePictureUrl)
            .HasMaxLength(500);

        builder.Property(p => p.Bio)
            .HasMaxLength(2000);

        builder.Property(p => p.Timezone)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LanguageCode)
            .IsRequired()
            .HasMaxLength(10);

        // ---------------------------------------------------------------------
        // Timestamps
        // ---------------------------------------------------------------------
        builder.Property(p => p.CreatedAt)
            .HasColumnType("datetime(6)");

        builder.Property(p => p.UpdatedAt)
            .HasColumnType("datetime(6)");
    }
}
