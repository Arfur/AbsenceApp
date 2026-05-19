// ===========================================================================
//  File        : Index.razor.cs
//  Namespace   : AbsenceApp.Client.Components.Pages.GlobalSettings.UIKits.Dropdown
//  Author      : Michael
//  Version     : 1.0.0
//  Created     : 2026-05-19
// ---------------------------------------------------------------------------
//  Purpose     : Code-behind for the Dropdown UI Kit showcase page.
//                Defines the DropdownGroup model and any page-level logic.
// ===========================================================================

using Microsoft.AspNetCore.Components;

namespace AbsenceApp.Client.Components.Pages.GlobalSettings.UIKits.Dropdown;

/// <summary>Represents a single group card on the Dropdown UI Kit page.</summary>
public sealed class DropdownGroup
{
    public string Key         { get; }
    public string Title       { get; }
    public RenderFragment DemoContent { get; }
    public bool   Open        { get; set; }

    public DropdownGroup(string key, string title, RenderFragment demoContent, bool open = false)
    {
        Key         = key;
        Title       = title;
        DemoContent = demoContent;
        Open        = open;
    }
}
