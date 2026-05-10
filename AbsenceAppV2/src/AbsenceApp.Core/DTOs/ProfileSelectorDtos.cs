/*
===============================================================================
 File        : ProfileSelectorDtos.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-10
 Updated     : 2026-05-10
-------------------------------------------------------------------------------
 Purpose     : Shared DTOs for the unified V2 profile chrome. These contracts
               support the reusable profile banner, profile tab strip, and
               searchable profile-name selector used by User, Student, and
               Staff profile pages.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-10  Initial creation. Added ProfileNameSelectorItemDto,
                         ProfileBannerFieldDto, and ProfileTabItemDto.
-------------------------------------------------------------------------------
 Notes       :
   - Pure UI contracts only; no EF or service dependencies.
   - Route values are carried so pages can navigate on item selection without
     reconstructing URLs from guessed entity metadata.
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

public sealed class ProfileNameSelectorItemDto
{
    public long   Id            { get; set; }
    public string DisplayName   { get; set; } = string.Empty;
    public string SecondaryText { get; set; } = string.Empty;
    public string Route         { get; set; } = string.Empty;
    public string EntityType    { get; set; } = string.Empty;
    public string? Status       { get; set; }
    public string? AvatarUrl    { get; set; }
}

public sealed class ProfileBannerFieldDto
{
    public string Label    { get; set; } = string.Empty;
    public string Value    { get; set; } = string.Empty;
    public bool   IsBadge  { get; set; }
    public string CssClass { get; set; } = string.Empty;
}

public sealed class ProfileTabItemDto
{
    public int    Index   { get; set; }
    public string Label   { get; set; } = string.Empty;
    public string Icon    { get; set; } = string.Empty;
    public bool   Visible { get; set; } = true;
    public bool   Enabled { get; set; } = true;
}
