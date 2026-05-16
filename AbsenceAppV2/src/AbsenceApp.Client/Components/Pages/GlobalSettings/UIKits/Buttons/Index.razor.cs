/*
===============================================================================
 File        : Index.razor.cs
 Namespace   : AbsenceApp.Client.Components.Pages.GlobalSettings.UIKits.Buttons
 Author      : GitHub Copilot
 Version     : 6.1.0
 Created     : 2026-05-14
 Updated     : 2026-05-16
-------------------------------------------------------------------------------
 Purpose     : Buttons3 UX model with full Ki-Admin button sets (one set per row).
               - Accordion sections
               - Real button selectors
               - Click variant -> CSS editor updates
               - Edit / Save, Cancel, Preview workflow
               - All accordions collapsed by default
===============================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AbsenceApp.Client.Components.Pages.GlobalSettings.UIKits.Buttons;

public partial class Index : ComponentBase
{
    public sealed record ButtonVariantDef(
        string Key,
        string Label,
        string CssClass,
        string PreviewCssClass,
        string Mode,
        string[] TokenKeys)
    {
        public bool IsEditable => TokenKeys.Length > 0;
    }

    public sealed class ButtonGroupState
    {
        public required string GroupKey { get; init; }
        public required string GroupTitle { get; init; }
        public required List<ButtonVariantDef> Variants { get; init; }

        public string SelectedVariantKey { get; set; } = string.Empty;
        public bool AccordionOpen { get; set; }
        public bool IsEditing { get; set; }
        public bool IsSaving { get; set; }

        public string EditorText { get; set; } = string.Empty;
        public string? PreviewCss { get; set; }
        public string? StatusMessage { get; set; }
        public bool StatusIsError { get; set; }
    }

    private static readonly List<ButtonVariantDef> BasicVariants =
    [
        new("basic-primary", "Primary", "dsv2-btn--primary", "dsv2-btn--primary", "custom", []),
        new("basic-secondary", "Secondary", "dsv2-btn--secondary", "dsv2-btn--secondary", "custom", []),
        new("basic-success", "Success", "dsv2-btn--success", "dsv2-btn--success", "custom", []),
        new("basic-danger", "Danger", "dsv2-btn--danger", "dsv2-btn--danger", "custom", []),
        new("basic-warning", "Warning", "dsv2-btn--warning", "dsv2-btn--warning", "custom", []),
        new("basic-info", "Info", "dsv2-btn--info", "dsv2-btn--info", "custom", []),
        new("basic-light", "Light", "dsv2-btn--light", "dsv2-btn--light", "custom", []),
        new("basic-dark", "Dark", "dsv2-btn--dark", "dsv2-btn--dark", "custom", []),
        new("basic-link", "Link", "dsv2-btn--link", "dsv2-btn--link", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> OutlineVariants =
    [
        new("outline-primary", "Primary", "dsv2-btn--outline-primary", "dsv2-btn--outline-primary", "custom", []),
        new("outline-secondary", "Secondary", "dsv2-btn--outline-secondary", "dsv2-btn--outline-secondary", "custom", []),
        new("outline-success", "Success", "dsv2-btn--outline-success", "dsv2-btn--outline-success", "custom", []),
        new("outline-danger", "Danger", "dsv2-btn--outline-danger", "dsv2-btn--outline-danger", "custom", []),
        new("outline-warning", "Warning", "dsv2-btn--outline-warning", "dsv2-btn--outline-warning", "custom", []),
        new("outline-info", "Info", "dsv2-btn--outline-info", "dsv2-btn--outline-info", "custom", []),
        new("outline-light", "Light", "dsv2-btn--outline-light", "dsv2-btn--outline-light", "custom", []),
        new("outline-dark", "Dark", "dsv2-btn--outline-dark", "dsv2-btn--outline-dark", "custom", []),
        new("outline-link", "Link", "dsv2-btn--outline-link", "dsv2-btn--outline-link", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> SoftVariants =
    [
        new("soft-primary", "Primary", "dsv2-btn--soft-primary", "dsv2-btn--soft-primary", "custom", []),
        new("soft-secondary", "Secondary", "dsv2-btn--soft-secondary", "dsv2-btn--soft-secondary", "custom", []),
        new("soft-success", "Success", "dsv2-btn--soft-success", "dsv2-btn--soft-success", "custom", []),
        new("soft-danger", "Danger", "dsv2-btn--soft-danger", "dsv2-btn--soft-danger", "custom", []),
        new("soft-warning", "Warning", "dsv2-btn--soft-warning", "dsv2-btn--soft-warning", "custom", []),
        new("soft-info", "Info", "dsv2-btn--soft-info", "dsv2-btn--soft-info", "custom", []),
        new("soft-light", "Light", "dsv2-btn--soft-light", "dsv2-btn--soft-light", "custom", []),
        new("soft-dark", "Dark", "dsv2-btn--soft-dark", "dsv2-btn--soft-dark", "custom", []),
        new("soft-link", "Link", "dsv2-btn--soft-link", "dsv2-btn--soft-link", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> IconVariants =
    [
        new("icon-fill-primary", "Download", "btp-icon-fill-primary", "btp-icon-btn btp-icon-fill-primary", "custom", []),
        new("icon-fill-secondary", "Bell", "btp-icon-fill-secondary", "btp-icon-btn btp-icon-fill-secondary", "custom", []),
        new("icon-outline-primary", "Settings", "btp-icon-outline-primary", "btp-icon-btn btp-icon-outline-primary", "custom", []),
        new("icon-outline-secondary", "Alarm", "btp-icon-outline-secondary", "btp-icon-btn btp-icon-outline-secondary", "custom", []),
        new("icon-soft-primary", "Camera", "btp-icon-soft-primary", "btp-icon-btn btp-icon-soft-primary", "custom", []),
        new("icon-soft-secondary", "Image", "btp-icon-soft-secondary", "btp-icon-btn btp-icon-soft-secondary", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> SocialVariants =
    [
        new("social-facebook", "Facebook", "btp-social-facebook", "btp-social-btn btp-social-facebook", "custom", []),
        new("social-twitter", "Twitter", "btp-social-twitter", "btp-social-btn btp-social-twitter", "custom", []),
        new("social-instagram", "Instagram", "btp-social-instagram", "btp-social-btn btp-social-instagram", "custom", []),
        new("social-reddit", "Reddit", "btp-social-reddit", "btp-social-btn btp-social-reddit", "custom", []),
        new("social-whatsapp", "WhatsApp", "btp-social-whatsapp", "btp-social-btn btp-social-whatsapp", "custom", []),
        new("social-linkedin", "LinkedIn", "btp-social-linkedin", "btp-social-btn btp-social-linkedin", "custom", []),
        new("social-telegram", "Telegram", "btp-social-telegram", "btp-social-btn btp-social-telegram", "custom", []),
        new("social-youtube", "YouTube", "btp-social-youtube", "btp-social-btn btp-social-youtube", "custom", []),
        new("social-behance", "Behance", "btp-social-behance", "btp-social-btn btp-social-behance", "custom", []),
        new("social-dribbble", "Dribbble", "btp-social-dribbble", "btp-social-btn btp-social-dribbble", "custom", []),
        new("social-snapchat", "Snapchat", "btp-social-snapchat", "btp-social-btn btp-social-snapchat", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> RadiusVariants =
    [
        new("radius-primary", "Primary", "btp-radius-primary", "dsv2-btn btp-radius-primary", "custom", []),
        new("radius-secondary", "Secondary", "btp-radius-secondary", "dsv2-btn btp-radius-secondary", "custom", []),
        new("radius-success", "Success", "btp-radius-success", "dsv2-btn btp-radius-success", "custom", []),
        new("radius-danger", "Danger", "btp-radius-danger", "dsv2-btn btp-radius-danger", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> ActiveVariants =
    [
        new("active-primary", "Primary", "btp-active-primary", "dsv2-btn btp-active-primary", "custom", []),
        new("active-secondary", "Secondary", "btp-active-secondary", "dsv2-btn btp-active-secondary", "custom", []),
        new("active-outline-primary", "Primary", "btp-active-outline-primary", "dsv2-btn btp-active-outline-primary", "custom", []),
        new("active-outline-secondary", "Secondary", "btp-active-outline-secondary", "dsv2-btn btp-active-outline-secondary", "custom", []),
        new("active-soft-primary", "Primary", "btp-active-soft-primary", "dsv2-btn btp-active-soft-primary", "custom", []),
        new("active-soft-secondary", "Secondary", "btp-active-soft-secondary", "dsv2-btn btp-active-soft-secondary", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> DisabledVariants =
    [
        new("disabled-primary", "Primary", "btp-disabled-primary", "dsv2-btn btp-disabled-primary", "custom", []),
        new("disabled-secondary", "Secondary", "btp-disabled-secondary", "dsv2-btn btp-disabled-secondary", "custom", []),
        new("disabled-outline-primary", "Primary", "btp-disabled-outline-primary", "dsv2-btn btp-disabled-outline-primary", "custom", []),
        new("disabled-outline-secondary", "Secondary", "btp-disabled-outline-secondary", "dsv2-btn btp-disabled-outline-secondary", "custom", []),
        new("disabled-soft-primary", "Primary", "btp-disabled-soft-primary", "dsv2-btn btp-disabled-soft-primary", "custom", []),
        new("disabled-soft-secondary", "Secondary", "btp-disabled-soft-secondary", "dsv2-btn btp-disabled-soft-secondary", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> LoadingVariants =
    [
        new("loading-primary", "Loading", "btp-loading-primary", "dsv2-btn btp-loading-primary", "custom", []),
        new("loading-secondary", "Wait", "btp-loading-secondary", "dsv2-btn btp-loading-secondary", "custom", []),
        new("loading-success", "Spin", "btp-loading-success", "dsv2-btn btp-loading-success", "custom", []),
        new("loading-danger", "Pulse", "btp-loading-danger", "dsv2-btn btp-loading-danger", "custom", []),
        new("loading-outline-primary", "Loading", "btp-loading-outline-primary", "dsv2-btn btp-loading-outline-primary", "custom", []),
        new("loading-outline-secondary", "Wait", "btp-loading-outline-secondary", "dsv2-btn btp-loading-outline-secondary", "custom", []),
        new("loading-outline-success", "Spin", "btp-loading-outline-success", "dsv2-btn btp-loading-outline-success", "custom", []),
        new("loading-outline-danger", "Pulse", "btp-loading-outline-danger", "dsv2-btn btp-loading-outline-danger", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> BlockVariants =
    [
        new("block-primary", "Button", "btp-block-primary", "dsv2-btn btp-block-btn btp-block-primary", "custom", []),
        new("block-secondary", "Button", "btp-block-secondary", "dsv2-btn btp-block-btn btp-block-secondary", "custom", []),
        new("block-outline-primary", "Button", "btp-block-outline-primary", "dsv2-btn btp-block-btn btp-block-outline-primary", "custom", []),
        new("block-outline-secondary", "Button", "btp-block-outline-secondary", "dsv2-btn btp-block-btn btp-block-outline-secondary", "custom", []),
        new("block-soft-primary", "Button", "btp-block-soft-primary", "dsv2-btn btp-block-btn btp-block-soft-primary", "custom", []),
        new("block-soft-secondary", "Button", "btp-block-soft-secondary", "dsv2-btn btp-block-btn btp-block-soft-secondary", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> SizeVariants =
    [
        new("size-sm", "Primary", "btp-size-sm", "dsv2-btn btp-size-primary btp-size-sm", "custom", []),
        new("size-md", "Primary", "btp-size-md", "dsv2-btn btp-size-primary btp-size-md", "custom", []),
        new("size-lg", "Primary", "btp-size-lg", "dsv2-btn btp-size-primary btp-size-lg", "custom", []),
        new("size-secondary", "Secondary", "btp-size-secondary", "dsv2-btn btp-size-secondary", "custom", []),
        new("size-success", "Success", "btp-size-success", "dsv2-btn btp-size-success", "custom", []),
        new("size-danger", "Danger", "btp-size-danger", "dsv2-btn btp-size-danger", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> GroupVariants =
    [
        new("group-solid", "Group", "btp-group-solid", "btp-group-demo btp-group-solid", "custom", []),
        new("group-outline", "Group", "btp-group-outline", "btp-group-demo btp-group-outline", "custom", []),
        new("group-soft", "Group", "btp-group-soft", "btp-group-demo btp-group-soft", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> NestedVariants =
    [
        new("nested-solid", "Nested", "btp-nested-solid", "btp-nested-demo btp-nested-solid", "custom", []),
        new("nested-outline", "Nested", "btp-nested-outline", "btp-nested-demo btp-nested-outline", "custom", []),
        new("nested-soft", "Nested", "btp-nested-soft", "btp-nested-demo btp-nested-soft", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> CheckboxRadioVariants =
    [
        new("checkbox-line", "Checkbox", "btp-checkbox-line", "btp-checkradio-demo btp-checkbox-line", "custom", []),
        new("radio-line", "Radio", "btp-radio-line", "btp-checkradio-demo btp-radio-line", "custom", []),
        new("pager-line", "Pager", "btp-pager-line", "btp-checkradio-demo btp-pager-line", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> VerticalVariants =
    [
        new("vertical-radio", "Radio Stack", "btp-vertical-radio", "btp-vertical-demo btp-vertical-radio", "custom", []),
        new("vertical-solid", "Solid Stack", "btp-vertical-solid", "btp-vertical-demo btp-vertical-solid", "custom", []),
        new("vertical-outline", "Outline Stack", "btp-vertical-outline", "btp-vertical-demo btp-vertical-outline", "custom", []),
        new("vertical-soft", "Soft Stack", "btp-vertical-soft", "btp-vertical-demo btp-vertical-soft", "custom", [])
    ];

    private static readonly Dictionary<string, string> StaticVariantCss = new(StringComparer.OrdinalIgnoreCase)
    {
        ["basic-primary"] = BuildCss(".dsv2-btn--primary", "#0f626a", "#ffffff", "#0f626a", "#0f626a"),
        ["basic-secondary"] = BuildCss(".dsv2-btn--secondary", "#626262", "#ffffff", "#626262", "#626262"),
        ["basic-success"] = BuildCss(".dsv2-btn--success", "#0ab964", "#ffffff", "#0ab964", "#0ab964"),
        ["basic-danger"] = BuildCss(".dsv2-btn--danger", "#e14e5a", "#ffffff", "#e14e5a", "#e14e5a"),
        ["basic-warning"] = BuildCss(".dsv2-btn--warning", "#f9c123", "#212529", "#f9c123", "#f9c123"),
        ["basic-info"] = BuildCss(".dsv2-btn--info", "#4196fa", "#ffffff", "#4196fa", "#4196fa"),
        ["basic-light"] = BuildCss(".dsv2-btn--light", "#c8b9d2", "#212529", "#c8b9d2", "#c8b9d2"),
        ["basic-dark"] = BuildCss(".dsv2-btn--dark", "#28232d", "#ffffff", "#28232d", "#28232d"),
        ["basic-link"] = BuildLinkCss(".dsv2-btn--link", "#2167f3", "#1a54c4"),

        ["outline-primary"] = BuildOutlineCss(".dsv2-btn--outline-primary", "#0f626a", "#0f626a", "#ffffff"),
        ["outline-secondary"] = BuildOutlineCss(".dsv2-btn--outline-secondary", "#626262", "#626262", "#ffffff"),
        ["outline-success"] = BuildOutlineCss(".dsv2-btn--outline-success", "#0ab964", "#0ab964", "#ffffff"),
        ["outline-danger"] = BuildOutlineCss(".dsv2-btn--outline-danger", "#e14e5a", "#e14e5a", "#ffffff"),
        ["outline-warning"] = BuildOutlineCss(".dsv2-btn--outline-warning", "#f9c123", "#f9c123", "#212529"),
        ["outline-info"] = BuildOutlineCss(".dsv2-btn--outline-info", "#4196fa", "#4196fa", "#ffffff"),
        ["outline-light"] = BuildOutlineCss(".dsv2-btn--outline-light", "#c8b9d2", "#c8b9d2", "#212529"),
        ["outline-dark"] = BuildOutlineCss(".dsv2-btn--outline-dark", "#28232d", "#28232d", "#ffffff"),
        ["outline-link"] = BuildLinkCss(".dsv2-btn--outline-link", "#2167f3", "#1a54c4"),

        ["soft-primary"] = BuildCss(".dsv2-btn--soft-primary", "#d7e7e8", "#0f626a", "#d7e7e8", "#dbe9ea"),
        ["soft-secondary"] = BuildCss(".dsv2-btn--soft-secondary", "#e0e0e0", "#626262", "#e0e0e0", "#e4e4e4"),
        ["soft-success"] = BuildCss(".dsv2-btn--soft-success", "#d1f1e0", "#0ab964", "#d1f1e0", "#d8f3e4"),
        ["soft-danger"] = BuildCss(".dsv2-btn--soft-danger", "#f7dde0", "#e14e5a", "#f7dde0", "#f9e2e5"),
        ["soft-warning"] = BuildCss(".dsv2-btn--soft-warning", "#f9f1d7", "#e1a900", "#f9f1d7", "#f4e9c7"),
        ["soft-info"] = BuildCss(".dsv2-btn--soft-info", "#d6e9ff", "#4196fa", "#d6e9ff", "#deedff"),
        ["soft-light"] = BuildCss(".dsv2-btn--soft-light", "#ece5f1", "#7f7191", "#ece5f1", "#f1eaf5"),
        ["soft-dark"] = BuildCss(".dsv2-btn--soft-dark", "#dedde0", "#28232d", "#dedde0", "#e6e5e8"),
        ["soft-link"] = BuildLinkCss(".dsv2-btn--soft-link", "#2167f3", "#1a54c4"),

        ["icon-fill-primary"] = BuildCss(".btp-icon-fill-primary", "#0f626a", "#ffffff", "#0f626a", "#0f626a"),
        ["icon-fill-secondary"] = BuildCss(".btp-icon-fill-secondary", "#626262", "#ffffff", "#626262", "#626262"),
        ["icon-outline-primary"] = BuildOutlineCss(".btp-icon-outline-primary", "#0f626a", "#0f626a", "#ffffff"),
        ["icon-outline-secondary"] = BuildOutlineCss(".btp-icon-outline-secondary", "#626262", "#626262", "#ffffff"),
        ["icon-soft-primary"] = BuildCss(".btp-icon-soft-primary", "#d7e7e8", "#0f626a", "#d7e7e8", "#dbe9ea"),
        ["icon-soft-secondary"] = BuildCss(".btp-icon-soft-secondary", "#e0e0e0", "#626262", "#e0e0e0", "#e4e4e4"),

        ["social-facebook"] = BuildCss(".btp-social-facebook", "#3B5998", "#ffffff", "#3B5998", "#3B5998"),
        ["social-twitter"] = BuildCss(".btp-social-twitter", "#55ACEE", "#ffffff", "#55ACEE", "#55ACEE"),
        ["social-instagram"] = BuildCss(".btp-social-instagram", "#E1306C", "#ffffff", "#E1306C", "#c62a5d"),
        ["social-reddit"] = BuildCss(".btp-social-reddit", "#FF4500", "#ffffff", "#FF4500", "#e23c00"),
        ["social-whatsapp"] = BuildCss(".btp-social-whatsapp", "#43D854", "#ffffff", "#43D854", "#43D854"),
        ["social-linkedin"] = BuildCss(".btp-social-linkedin", "#0077B5", "#ffffff", "#0077B5", "#00679c"),
        ["social-telegram"] = BuildCss(".btp-social-telegram", "#00405D", "#ffffff", "#00405D", "#00405D"),
        ["social-youtube"] = BuildCss(".btp-social-youtube", "#CD201F", "#ffffff", "#CD201F", "#CD201F"),
        ["social-behance"] = BuildCss(".btp-social-behance", "#1769FF", "#ffffff", "#1769FF", "#1159d8"),
        ["social-dribbble"] = BuildCss(".btp-social-dribbble", "#EA4C89", "#ffffff", "#EA4C89", "#d7437a"),
        ["social-snapchat"] = BuildCss(".btp-social-snapchat", "#FFFC00", "#212529", "#FFFC00", "#ede900"),

        ["radius-primary"] = BuildCss(".btp-radius-primary", "#0f626a", "#ffffff", "#0f626a", "#0f626a"),
        ["radius-secondary"] = BuildCss(".btp-radius-secondary", "#626262", "#ffffff", "#626262", "#626262"),
        ["radius-success"] = BuildCss(".btp-radius-success", "#0ab964", "#ffffff", "#0ab964", "#0ab964"),
        ["radius-danger"] = BuildCss(".btp-radius-danger", "#e14e5a", "#ffffff", "#e14e5a", "#e14e5a"),

        ["active-primary"] = BuildCss(".btp-active-primary", "#0f626a", "#ffffff", "#0f626a", "#0f626a"),
        ["active-secondary"] = BuildCss(".btp-active-secondary", "#626262", "#ffffff", "#626262", "#626262"),
        ["active-outline-primary"] = BuildOutlineCss(".btp-active-outline-primary", "#0f626a", "#0f626a", "#ffffff"),
        ["active-outline-secondary"] = BuildOutlineCss(".btp-active-outline-secondary", "#626262", "#626262", "#ffffff"),
        ["active-soft-primary"] = BuildCss(".btp-active-soft-primary", "#d7e7e8", "#0f626a", "#d7e7e8", "#dbe9ea"),
        ["active-soft-secondary"] = BuildCss(".btp-active-soft-secondary", "#e0e0e0", "#626262", "#e0e0e0", "#e4e4e4"),

        ["disabled-primary"] = BuildCss(".btp-disabled-primary", "#77aeb4", "#eaf3f4", "#77aeb4", "#77aeb4"),
        ["disabled-secondary"] = BuildCss(".btp-disabled-secondary", "#a9aaac", "#ececed", "#a9aaac", "#a9aaac"),
        ["disabled-outline-primary"] = BuildOutlineCss(".btp-disabled-outline-primary", "#7bb5bc", "#7bb5bc", "#ffffff"),
        ["disabled-outline-secondary"] = BuildOutlineCss(".btp-disabled-outline-secondary", "#a5a7aa", "#a5a7aa", "#ffffff"),
        ["disabled-soft-primary"] = BuildCss(".btp-disabled-soft-primary", "#d8e6e8", "#77aeb4", "#d8e6e8", "#d8e6e8"),
        ["disabled-soft-secondary"] = BuildCss(".btp-disabled-soft-secondary", "#e4e5e6", "#a9aaac", "#e4e5e6", "#e4e5e6"),

        ["loading-primary"] = BuildCss(".btp-loading-primary", "#0f626a", "#ffffff", "#0f626a", "#0f626a"),
        ["loading-secondary"] = BuildCss(".btp-loading-secondary", "#626262", "#ffffff", "#626262", "#626262"),
        ["loading-success"] = BuildCss(".btp-loading-success", "#0ab964", "#ffffff", "#0ab964", "#0ab964"),
        ["loading-danger"] = BuildCss(".btp-loading-danger", "#e14e5a", "#ffffff", "#e14e5a", "#e14e5a"),
        ["loading-outline-primary"] = BuildOutlineCss(".btp-loading-outline-primary", "#0f626a", "#0f626a", "#ffffff"),
        ["loading-outline-secondary"] = BuildOutlineCss(".btp-loading-outline-secondary", "#626262", "#626262", "#ffffff"),
        ["loading-outline-success"] = BuildOutlineCss(".btp-loading-outline-success", "#0ab964", "#0ab964", "#ffffff"),
        ["loading-outline-danger"] = BuildOutlineCss(".btp-loading-outline-danger", "#e14e5a", "#e14e5a", "#ffffff"),

        ["block-primary"] = BuildCss(".btp-block-primary", "#0f626a", "#ffffff", "#0f626a", "#0f626a"),
        ["block-secondary"] = BuildCss(".btp-block-secondary", "#626262", "#ffffff", "#626262", "#626262"),
        ["block-outline-primary"] = BuildOutlineCss(".btp-block-outline-primary", "#0f626a", "#0f626a", "#ffffff"),
        ["block-outline-secondary"] = BuildOutlineCss(".btp-block-outline-secondary", "#626262", "#626262", "#ffffff"),
        ["block-soft-primary"] = BuildCss(".btp-block-soft-primary", "#d7e7e8", "#0f626a", "#d7e7e8", "#dbe9ea"),
        ["block-soft-secondary"] = BuildCss(".btp-block-soft-secondary", "#e0e0e0", "#626262", "#e0e0e0", "#e4e4e4"),

        ["size-sm"] = BuildCss(".btp-size-primary.btp-size-sm", "#0f626a", "#ffffff", "#0f626a", "#0f626a"),
        ["size-md"] = BuildCss(".btp-size-primary.btp-size-md", "#0f626a", "#ffffff", "#0f626a", "#0f626a"),
        ["size-lg"] = BuildCss(".btp-size-primary.btp-size-lg", "#0f626a", "#ffffff", "#0f626a", "#0f626a"),
        ["size-secondary"] = BuildCss(".btp-size-secondary", "#626262", "#ffffff", "#626262", "#626262"),
        ["size-success"] = BuildCss(".btp-size-success", "#0ab964", "#ffffff", "#0ab964", "#0ab964"),
        ["size-danger"] = BuildCss(".btp-size-danger", "#e14e5a", "#ffffff", "#e14e5a", "#e14e5a"),

        ["group-solid"] = "/* Button Group (Solid) */",
        ["group-outline"] = "/* Button Group (Outline) */",
        ["group-soft"] = "/* Button Group (Soft) */",

        ["nested-solid"] = "/* Nested Buttons (Solid) */",
        ["nested-outline"] = "/* Nested Buttons (Outline) */",
        ["nested-soft"] = "/* Nested Buttons (Soft) */",

        ["checkbox-line"] = "/* Checkbox/Radio (Checkbox line) */",
        ["radio-line"] = "/* Checkbox/Radio (Radio line) */",
        ["pager-line"] = "/* Checkbox/Radio (Pager line) */",

        ["vertical-radio"] = "/* Vertical Buttons (Radio stack) */",
        ["vertical-solid"] = "/* Vertical Buttons (Solid stack) */",
        ["vertical-outline"] = "/* Vertical Buttons (Outline stack) */",
        ["vertical-soft"] = "/* Vertical Buttons (Soft stack) */"
    };

    private static string BuildCss(string selector, string bg, string text, string border, string hover) =>
        $"{selector} {{\n  background: {bg};\n  color: {text};\n  border-color: {border};\n}}\n{selector}:hover {{\n  background: {hover};\n  border-color: {hover};\n  color: {text};\n}}";

    private static string BuildOutlineCss(string selector, string color, string border, string hoverText) =>
        $"{selector} {{\n  background: transparent;\n  color: {color};\n  border-color: {border};\n}}\n{selector}:hover {{\n  background: {color};\n  border-color: {color};\n  color: {hoverText};\n}}";

    private static string BuildLinkCss(string selector, string color, string hover) =>
        $"{selector} {{\n  background: transparent;\n  color: {color};\n  border-color: transparent;\n  text-decoration: none;\n}}\n{selector}:hover {{\n  background: transparent;\n  color: {hover};\n  border-color: transparent;\n  text-decoration: underline;\n}}";

    private ButtonGroupState _basicState = default!;
    private ButtonGroupState _outlineState = default!;
    private ButtonGroupState _softState = default!;
    private ButtonGroupState _iconState = default!;
    private ButtonGroupState _socialState = default!;
    private ButtonGroupState _radiusState = default!;
    private ButtonGroupState _activeState = default!;
    private ButtonGroupState _disabledState = default!;
    private ButtonGroupState _loadingState = default!;
    private ButtonGroupState _blockState = default!;
    private ButtonGroupState _sizeState = default!;
    private ButtonGroupState _groupState = default!;
    private ButtonGroupState _nestedState = default!;
    private ButtonGroupState _checkRadioState = default!;
    private ButtonGroupState _verticalState = default!;

    private readonly Dictionary<string, string> _workingCopies = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string> _customSavedCss = new(StringComparer.OrdinalIgnoreCase);

    private bool _isLoading;
    private string? _initError;

    private IEnumerable<ButtonGroupState> GroupStates =>
    [
        _basicState,
        _outlineState,
        _softState,
        _iconState,
        _socialState,
        _radiusState,
        _activeState,
        _disabledState,
        _loadingState,
        _blockState,
        _sizeState,
        _groupState,
        _nestedState,
        _checkRadioState,
        _verticalState
    ];

    protected override void OnInitialized()
    {
        _isLoading = false;
        _initError = null;

        _basicState = CreateState("basic", "Basic Buttons", BasicVariants);
        _outlineState = CreateState("outline", "Outline Buttons", OutlineVariants);
        _softState = CreateState("soft", "Light / Soft Buttons", SoftVariants);
        _iconState = CreateState("icon", "Icon Buttons", IconVariants);
        _socialState = CreateState("social", "Social Buttons", SocialVariants);
        _radiusState = CreateState("radius", "Radius Buttons", RadiusVariants);
        _activeState = CreateState("active", "Active Buttons", ActiveVariants);
        _disabledState = CreateState("disabled", "Disabled Buttons", DisabledVariants);
        _loadingState = CreateState("loading", "Loading Buttons", LoadingVariants);
        _blockState = CreateState("block", "Block Buttons", BlockVariants);
        _sizeState = CreateState("sizes", "Button Sizes (sm, md, lg)", SizeVariants);
        _groupState = CreateState("groups", "Button Groups", GroupVariants);
        _nestedState = CreateState("nested", "Nested Buttons", NestedVariants);
        _checkRadioState = CreateState("checkradio", "Checkbox / Radio Buttons", CheckboxRadioVariants);
        _verticalState = CreateState("vertical", "Vertical Buttons", VerticalVariants);

        foreach (var state in GroupStates)
        {
            state.EditorText = SynthesizeCss(state, state.SelectedVariantKey);
        }
    }

    private static ButtonGroupState CreateState(string key, string title, List<ButtonVariantDef> variants)
        => new()
        {
            GroupKey = key,
            GroupTitle = title,
            Variants = variants,
            SelectedVariantKey = variants[0].Key,
            AccordionOpen = false
        };

    private void OnToggleAccordion(ButtonGroupState state)
    {
        if (state.AccordionOpen && state.IsEditing)
        {
            CancelEditing(state);
        }

        state.AccordionOpen = !state.AccordionOpen;

        if (state.AccordionOpen && string.IsNullOrWhiteSpace(state.EditorText))
        {
            state.EditorText = SynthesizeCss(state, state.SelectedVariantKey);
        }
    }

    private void OnSelectedVariantChanged(ButtonGroupState state, string variantKey)
    {
        if (state.IsEditing)
        {
            _workingCopies[$"{state.GroupKey}:{state.SelectedVariantKey}"] = state.EditorText;
        }

        state.SelectedVariantKey = variantKey;
        state.StatusMessage = null;

        if (state.IsEditing && _workingCopies.TryGetValue($"{state.GroupKey}:{variantKey}", out var saved))
        {
            state.EditorText = saved;
            return;
        }

        state.EditorText = SynthesizeCss(state, variantKey);
    }

    private Task OnEditSaveClickedAsync(ButtonGroupState state)
    {
        if (!state.IsEditing)
        {
            state.IsEditing = true;
            state.StatusMessage = null;
            state.StatusIsError = false;

            if (string.IsNullOrWhiteSpace(state.EditorText))
            {
                state.EditorText = SynthesizeCss(state, state.SelectedVariantKey);
            }

            return Task.CompletedTask;
        }

        return SaveAsync(state);
    }

    private Task SaveAsync(ButtonGroupState state)
    {
        state.StatusMessage = null;
        state.StatusIsError = false;

        var (valid, error) = ValidateCss(state.EditorText);
        if (!valid)
        {
            state.StatusMessage = error ?? "Invalid CSS.";
            state.StatusIsError = true;
            return Task.CompletedTask;
        }

        state.IsSaving = true;

        var selected = GetSelectedVariant(state);
        if (selected is null)
        {
            state.StatusMessage = "Variant not found.";
            state.StatusIsError = true;
            state.IsSaving = false;
            return Task.CompletedTask;
        }

        _customSavedCss[$"{state.GroupKey}:{selected.Key}"] = state.EditorText;

        state.IsEditing = false;
        state.PreviewCss = null;
        ClearWorkingCopies(state.GroupKey);
        state.EditorText = SynthesizeCss(state, state.SelectedVariantKey);
        state.StatusMessage = "CSS Code Successfully Updated";
        state.StatusIsError = false;
        state.IsSaving = false;

        return Task.CompletedTask;
    }

    private void OnCancelClicked(ButtonGroupState state)
    {
        CancelEditing(state);
    }

    private void OnPreviewClicked(ButtonGroupState state)
    {
        state.StatusMessage = null;
        state.StatusIsError = false;

        var (valid, error) = ValidateCss(state.EditorText);
        if (!valid)
        {
            state.StatusMessage = error ?? "Invalid CSS.";
            state.StatusIsError = true;
            return;
        }

        state.PreviewCss = BuildScopedPreviewCssFromRaw(state, state.EditorText);
        state.StatusMessage = "Preview applied to selected variant.";
        state.StatusIsError = false;
    }

    private void OnEditorInput(ButtonGroupState state, string text)
    {
        state.EditorText = text;
    }

    private void CancelEditing(ButtonGroupState state)
    {
        state.IsEditing = false;
        state.PreviewCss = null;
        state.StatusMessage = null;
        state.StatusIsError = false;
        ClearWorkingCopies(state.GroupKey);
        state.EditorText = SynthesizeCss(state, state.SelectedVariantKey);
    }

    private void ClearWorkingCopies(string groupKey)
    {
        var prefix = groupKey + ":";
        foreach (var key in _workingCopies.Keys.Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList())
        {
            _workingCopies.Remove(key);
        }
    }

    private static bool IsSelected(ButtonGroupState state, ButtonVariantDef variant)
        => string.Equals(state.SelectedVariantKey, variant.Key, StringComparison.OrdinalIgnoreCase);

    private ButtonVariantDef? GetSelectedVariant(ButtonGroupState state)
        => state.Variants.FirstOrDefault(v => string.Equals(v.Key, state.SelectedVariantKey, StringComparison.OrdinalIgnoreCase));

    private string SynthesizeCss(ButtonGroupState state, string variantKey)
    {
        var selected = state.Variants.FirstOrDefault(v => string.Equals(v.Key, variantKey, StringComparison.OrdinalIgnoreCase));
        if (selected is null)
        {
            return string.Empty;
        }

        var customKey = $"{state.GroupKey}:{selected.Key}";
        if (_customSavedCss.TryGetValue(customKey, out var savedCss))
        {
            return savedCss;
        }

        if (StaticVariantCss.TryGetValue(selected.Key, out var staticCss))
        {
            if (!string.IsNullOrWhiteSpace(staticCss) && !staticCss.StartsWith("/*", StringComparison.Ordinal))
            {
                return staticCss;
            }
        }

        return $".{selected.CssClass} {{\n  /* Ki-Admin style block for {selected.Label} */\n}}";
    }

    private string BuildScopedPreviewCssFromRaw(ButtonGroupState state, string cssText)
    {
        var wrapperId = $"#btn-demo-{state.GroupKey}";
        var sb = new StringBuilder();

        foreach (Match rule in RulePattern.Matches(cssText))
        {
            var selector = rule.Groups[1].Value.Trim();
            var body = rule.Groups[2].Value.Trim();
            if (string.IsNullOrWhiteSpace(selector) || string.IsNullOrWhiteSpace(body))
            {
                continue;
            }

            var scopedSelectors = selector
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.StartsWith(wrapperId, StringComparison.OrdinalIgnoreCase) ? s : $"{wrapperId} {s}");

            var selectorText = string.Join(", ", scopedSelectors);
            sb.AppendLine($"{selectorText} {{");

            foreach (var decl in ExtractDeclarations(body))
            {
                sb.AppendLine($"  {decl.Key}: {decl.Value} !important;");
            }

            sb.AppendLine("}");
        }

        return sb.ToString();
    }

    private static readonly Regex RulePattern =
        new(@"([^{]+)\{([^}]+)\}", RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly HashSet<string> KnownProperties =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "background", "color", "border-color", "border", "opacity", "text-decoration",
            "border-radius", "padding", "font-size", "display", "gap", "width", "min-width",
            "height", "align-items", "justify-content", "cursor", "pointer-events"
        };

    private static readonly Regex HexColorRegex =
        new(@"^#[0-9a-fA-F]{3}(?:[0-9a-fA-F]{3})?(?:[0-9a-fA-F]{2})?$", RegexOptions.Compiled);

    private static readonly Regex RgbColorRegex = new(@"^rgba?\s*\(", RegexOptions.Compiled);
    private static readonly Regex HslColorRegex = new(@"^hsla?\s*\(", RegexOptions.Compiled);
    private static readonly Regex RuleBodyRegex = new(@"\{([^{}]+)\}", RegexOptions.Singleline | RegexOptions.Compiled);
    private static readonly Regex DeclLineRegex = new(@"^([a-zA-Z-]+)\s*:\s*(.+?);?\s*$", RegexOptions.Compiled);

    private static readonly HashSet<string> NamedColors =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "transparent", "inherit", "initial", "currentcolor",
            "none", "white", "black", "red", "green", "blue",
            "yellow", "orange", "purple", "gray", "grey"
        };

    private static (bool Valid, string? Error) ValidateCss(string cssText)
    {
        if (string.IsNullOrWhiteSpace(cssText))
        {
            return (false, "CSS cannot be empty.");
        }

        var ruleMatches = RuleBodyRegex.Matches(cssText);
        if (ruleMatches.Count == 0)
        {
            return (false, "No CSS rule body found (expected { } block).");
        }

        var lineNum = 0;
        foreach (Match ruleMatch in ruleMatches)
        {
            var body = ruleMatch.Groups[1].Value;
            var lines = body.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawLine in lines)
            {
                lineNum++;
                var line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("/*", StringComparison.Ordinal))
                {
                    continue;
                }

                var declMatch = DeclLineRegex.Match(line);
                if (!declMatch.Success)
                {
                    return (false, $"Line {lineNum}: Expected 'property: value;' — found '{line}'.");
                }

                var property = declMatch.Groups[1].Value.Trim();
                var value = declMatch.Groups[2].Value.Trim();

                if (!KnownProperties.Contains(property))
                {
                    return (false, $"Line {lineNum}: Unknown CSS property '{property}'.");
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    return (false, $"Line {lineNum}: Empty value for property '{property}'.");
                }

                if (property is "background" or "color" or "border-color" or "border")
                {
                    if (!IsValidColorValue(value))
                    {
                        return (false, $"Line {lineNum}: Invalid color value '{value}'.");
                    }
                }
            }
        }

        return (true, null);
    }

    private static bool IsValidColorValue(string value)
    {
        if (NamedColors.Contains(value)) return true;
        if (HexColorRegex.IsMatch(value)) return true;
        if (RgbColorRegex.IsMatch(value)) return true;
        if (HslColorRegex.IsMatch(value)) return true;
        if (value.StartsWith("var(", StringComparison.OrdinalIgnoreCase)) return true;
        if (value.StartsWith("color-mix(", StringComparison.OrdinalIgnoreCase)) return true;
        return false;
    }

    private static Dictionary<string, string> ExtractDeclarations(string ruleBody)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var rawLine in ruleBody.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var line = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("/*", StringComparison.Ordinal))
            {
                continue;
            }

            var colonIndex = line.IndexOf(':', StringComparison.Ordinal);
            if (colonIndex < 0)
            {
                continue;
            }

            var property = line[..colonIndex].Trim();
            var value = line[(colonIndex + 1)..].TrimEnd(';', ' ', '\t').Trim();

            if (!string.IsNullOrWhiteSpace(property) && !string.IsNullOrWhiteSpace(value))
            {
                result[property] = value;
            }
        }

        return result;
    }
}
