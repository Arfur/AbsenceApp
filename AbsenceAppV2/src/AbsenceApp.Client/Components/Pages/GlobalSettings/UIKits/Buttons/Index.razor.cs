/*
===============================================================================
 File        : Index.razor.cs
 Namespace   : AbsenceApp.Client.Components.Pages.GlobalSettings.UIKits.Buttons
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-14
 Updated     : 2026-05-14
-------------------------------------------------------------------------------
 Purpose     : Code-behind for the UIKits / Buttons page
               (/globalsettings/ui-kits/buttons).

               Implements the full two-group Buttons page:
                 - Basic Buttons group (9 variants: primary – link)
                 - Outline Buttons group (8 variants: outline-primary – link)

               Safety-net CSS architecture:
                 Layer 1 — Live CSS  : DesignTokens DB via DesignTokenApiServiceV2
                 Layer 2 — Working copy : component state (never persisted until Save)
                 Layer 3 — Preview injection : scoped <style> block in demo div

               No JSInterop. No new shared components. No new migrations.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-14  Initial creation — UIKits Buttons page Phase 1.
===============================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Data.Models;
using Microsoft.AspNetCore.Components;

namespace AbsenceApp.Client.Components.Pages.GlobalSettings.UIKits.Buttons;

public partial class Index : ComponentBase
{
    // =========================================================================
    // Dependency injection
    // =========================================================================

    [Inject] private DesignTokenApiServiceV2 TokenService { get; set; } = default!;

    // =========================================================================
    // Inner types
    // =========================================================================

    /// <summary>
    /// Immutable definition of one button variant (Key, Label, CSS class, token keys).
    /// IsEditable is true when the variant has backing design tokens.
    /// </summary>
    public sealed record ButtonVariantDef(
        string Key,
        string Label,
        string CssClass,
        string[] TokenKeys)
    {
        public bool IsEditable => TokenKeys.Length > 0;
    }

    /// <summary>
    /// Mutable per-group UI state. One instance exists for Basic, one for Outline.
    /// All event handlers receive the specific group's state as a parameter.
    /// </summary>
    public sealed class ButtonGroupState
    {
        public required string GroupKey   { get; init; }   // "basic" | "outline"
        public required string GroupTitle { get; init; }   // "Basic Buttons" | "Outline Buttons"
        public required List<ButtonVariantDef> Variants { get; init; }

        public string  SelectedVariantKey { get; set; } = string.Empty;
        public bool    AccordionOpen      { get; set; }
        public bool    IsEditing          { get; set; }
        public string  EditorText         { get; set; } = string.Empty;
        public string? PreviewCss         { get; set; }
        public string? StatusMessage      { get; set; }
        public bool    StatusIsError      { get; set; }
    }

    // =========================================================================
    // Static variant definitions
    // =========================================================================

    private static readonly List<ButtonVariantDef> BasicVariants =
    [
        new("primary",   "Primary",   "dsv2-btn--primary",   ["primary-bg",   "primary-text",   "primary-border",   "primary-hover-bg"]),
        new("secondary", "Secondary", "dsv2-btn--secondary", ["secondary-bg", "secondary-text", "secondary-border", "secondary-hover-bg"]),
        new("success",   "Success",   "dsv2-btn--success",   ["success-bg",   "success-text",   "success-border",   "success-hover-bg"]),
        new("danger",    "Danger",    "dsv2-btn--danger",    ["danger-bg",    "danger-text",    "danger-border",    "danger-hover-bg"]),
        new("warning",   "Warning",   "dsv2-btn--warning",   ["warning-bg",   "warning-text",   "warning-border",   "warning-hover-bg"]),
        new("info",      "Info",      "dsv2-btn--info",      ["info-bg",      "info-text",      "info-border",      "info-hover-bg"]),
        new("light",     "Light",     "dsv2-btn--light",     []),
        new("dark",      "Dark",      "dsv2-btn--dark",      []),
        new("link",      "Link",      "dsv2-btn--link",      []),
    ];

    private static readonly List<ButtonVariantDef> OutlineVariants =
    [
        new("outline-primary",   "Primary",   "dsv2-btn--outline-primary",   ["primary-bg",   "primary-text",   "primary-border",   "primary-hover-bg"]),
        new("outline-secondary", "Secondary", "dsv2-btn--outline-secondary", ["secondary-bg", "secondary-text", "secondary-border", "secondary-hover-bg"]),
        new("outline-success",   "Success",   "dsv2-btn--outline-success",   ["success-bg",   "success-text",   "success-border",   "success-hover-bg"]),
        new("outline-danger",    "Danger",    "dsv2-btn--outline-danger",    ["danger-bg",    "danger-text",    "danger-border",    "danger-hover-bg"]),
        new("outline-warning",   "Warning",   "dsv2-btn--outline-warning",   ["warning-bg",   "warning-text",   "warning-border",   "warning-hover-bg"]),
        new("outline-info",      "Info",      "dsv2-btn--outline-info",      ["info-bg",      "info-text",      "info-border",      "info-hover-bg"]),
        new("dark",              "Dark",      "dsv2-btn--dark",              []),
        new("link",              "Link",      "dsv2-btn--link",              []),
    ];

    // Static CSS strings for non-token variants (light, dark, link).
    // These are display-only; the editor textarea is read-only for these variants.
    private static readonly Dictionary<string, string> StaticVariantCss = new()
    {
        ["light"] =
            ".dsv2-btn--light {\n" +
            "  background: #f8f9fa;\n" +
            "  color: #212529;\n" +
            "  border-color: #dee2e6;\n" +
            "}\n" +
            ".dsv2-btn--light:hover {\n" +
            "  background: #e2e6ea;\n" +
            "  border-color: #dae0e5;\n" +
            "  color: #212529;\n" +
            "}",

        ["dark"] =
            ".dsv2-btn--dark {\n" +
            "  background: #212529;\n" +
            "  color: #ffffff;\n" +
            "  border-color: #212529;\n" +
            "}\n" +
            ".dsv2-btn--dark:hover {\n" +
            "  background: #1c1f23;\n" +
            "  border-color: #1a1e21;\n" +
            "  color: #ffffff;\n" +
            "}",

        ["link"] =
            ".dsv2-btn--link {\n" +
            "  background: transparent;\n" +
            "  color: #0d6efd;\n" +
            "  border-color: transparent;\n" +
            "  text-decoration: none;\n" +
            "}\n" +
            ".dsv2-btn--link:hover {\n" +
            "  background: transparent;\n" +
            "  color: #0b5ed7;\n" +
            "  border-color: transparent;\n" +
            "  text-decoration: underline;\n" +
            "}",
    };

    // =========================================================================
    // Component state
    // =========================================================================

    private ButtonGroupState _basicState   = default!;
    private ButtonGroupState _outlineState = default!;

    /// <summary>All btn tokens indexed by TokenKey (e.g. "primary-bg").</summary>
    private Dictionary<string, DesignToken> _tokensByKey = new();

    /// <summary>
    /// Temporary working copies keyed "{groupKey}-{variantKey}".
    /// Populated when the user switches variants while in edit mode.
    /// Cleared on Save or Cancel.
    /// </summary>
    private readonly Dictionary<string, string> _workingCopies = new();

    private bool    _tokensLoaded;
    private string? _initError;

    // =========================================================================
    // Lifecycle
    // =========================================================================

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var tokens = await TokenService.GetGroupAsync("btn");
            _tokensByKey = tokens.ToDictionary(t => t.TokenKey, t => t);

            _basicState = new ButtonGroupState
            {
                GroupKey   = "basic",
                GroupTitle = "Basic Buttons",
                Variants   = BasicVariants,
            };
            _basicState.SelectedVariantKey = BasicVariants[0].Key;

            _outlineState = new ButtonGroupState
            {
                GroupKey   = "outline",
                GroupTitle = "Outline Buttons",
                Variants   = OutlineVariants,
            };
            _outlineState.SelectedVariantKey = OutlineVariants[0].Key;

            _tokensLoaded = true;
        }
        catch (Exception ex)
        {
            _initError = $"Failed to load design tokens: {ex.Message}";
        }
    }

    // =========================================================================
    // Event handlers — accordion & variant selection
    // =========================================================================

    private void OnToggleAccordion(ButtonGroupState state)
    {
        if (state.AccordionOpen && state.IsEditing)
            CancelEditing(state);   // silently discard working copy on close

        state.AccordionOpen = !state.AccordionOpen;

        if (state.AccordionOpen && string.IsNullOrEmpty(state.EditorText))
            state.EditorText = SynthesizeCss(state, state.SelectedVariantKey);
    }

    private void OnSelectedVariantChanged(ButtonGroupState state, string variantKey)
    {
        if (state.IsEditing)
        {
            // Persist working copy for the current variant before switching
            _workingCopies[$"{state.GroupKey}-{state.SelectedVariantKey}"] = state.EditorText;
        }

        state.SelectedVariantKey = variantKey;
        state.StatusMessage      = null;

        // Restore working copy if the user has previously edited this variant,
        // otherwise synthesize fresh CSS from the current token values.
        if (state.IsEditing && _workingCopies.TryGetValue($"{state.GroupKey}-{variantKey}", out var saved))
            state.EditorText = saved;
        else
            state.EditorText = SynthesizeCss(state, variantKey);
    }

    // =========================================================================
    // Event handlers — Edit / Save / Cancel / Preview
    // =========================================================================

    private async Task OnEditSaveClickedAsync(ButtonGroupState state)
    {
        if (!state.IsEditing)
        {
            // Enter edit mode
            state.IsEditing     = true;
            state.StatusMessage = null;

            if (string.IsNullOrEmpty(state.EditorText))
                state.EditorText = SynthesizeCss(state, state.SelectedVariantKey);
        }
        else
        {
            // Save
            await SaveAsync(state);
        }
    }

    private async Task SaveAsync(ButtonGroupState state)
    {
        state.StatusMessage = null;

        var (valid, error) = ValidateCss(state.EditorText);
        if (!valid)
        {
            state.StatusMessage = error ?? "Invalid CSS.";
            state.StatusIsError = true;
            return;
        }

        var updates = ParseCssToTokenValues(state, state.EditorText);
        if (updates.Count == 0)
        {
            state.StatusMessage = "No editable token values found in the CSS.";
            state.StatusIsError = true;
            return;
        }

        try
        {
            await TokenService.UpdateTokensAsync(updates);

            // Reload tokens to reflect the persisted state
            var refreshed = await TokenService.GetGroupAsync("btn");
            _tokensByKey  = refreshed.ToDictionary(t => t.TokenKey, t => t);

            state.IsEditing  = false;
            state.PreviewCss = null;
            ClearWorkingCopies(state.GroupKey);

            // Refresh the editor textarea with the newly saved values
            state.EditorText    = SynthesizeCss(state, state.SelectedVariantKey);
            state.StatusMessage = "CSS Code Successfully Updated";
            state.StatusIsError = false;
        }
        catch (Exception ex)
        {
            state.StatusMessage = $"Save failed: {ex.Message}";
            state.StatusIsError = true;
        }
    }

    private void OnCancelClicked(ButtonGroupState state)
    {
        CancelEditing(state);
    }

    private void OnPreviewClicked(ButtonGroupState state)
    {
        state.StatusMessage = null;

        var (valid, error) = ValidateCss(state.EditorText);
        if (!valid)
        {
            state.StatusMessage = error ?? "Invalid CSS.";
            state.StatusIsError = true;
            return;
        }

        var updates = ParseCssToTokenValues(state, state.EditorText);
        if (updates.Count == 0)
        {
            state.StatusMessage = "No token values found to preview.";
            state.StatusIsError = true;
            return;
        }

        state.PreviewCss = BuildScopedPreviewCss(state, updates);
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    private void CancelEditing(ButtonGroupState state)
    {
        state.IsEditing     = false;
        state.PreviewCss    = null;
        state.StatusMessage = null;
        ClearWorkingCopies(state.GroupKey);
        state.EditorText = SynthesizeCss(state, state.SelectedVariantKey);
    }

    private void ClearWorkingCopies(string groupKey)
    {
        var prefix   = groupKey + "-";
        var toRemove = _workingCopies.Keys
            .Where(k => k.StartsWith(prefix, StringComparison.Ordinal))
            .ToList();
        foreach (var key in toRemove)
            _workingCopies.Remove(key);
    }

    internal bool IsSelected(ButtonGroupState state, ButtonVariantDef variant)
        => state.SelectedVariantKey == variant.Key;

    // =========================================================================
    // CSS synthesis (Layer 2 — Working Copy generation)
    // =========================================================================

    private string SynthesizeCss(ButtonGroupState state, string variantKey)
    {
        var variant = state.Variants.FirstOrDefault(v => v.Key == variantKey);
        if (variant is null) return string.Empty;

        if (!variant.IsEditable)
        {
            // Non-token variants: return hardcoded static CSS
            var staticKey = variantKey.Replace("outline-", string.Empty);
            return StaticVariantCss.TryGetValue(staticKey, out var staticCss) ? staticCss : string.Empty;
        }

        var isOutline   = state.GroupKey == "outline";
        var baseVariant = variantKey.Replace("outline-", string.Empty);
        var cssClass    = variant.CssClass;

        var bg      = ResolveToken($"{baseVariant}-bg");
        var text    = ResolveToken($"{baseVariant}-text");
        var border  = ResolveToken($"{baseVariant}-border");
        var hoverBg = ResolveToken($"{baseVariant}-hover-bg");

        if (!isOutline)
        {
            return
                $"{cssClass} {{\n" +
                $"  background: {bg};\n" +
                $"  color: {text};\n" +
                $"  border-color: {border};\n" +
                $"}}\n" +
                $"{cssClass}:hover {{\n" +
                $"  background: {hoverBg};\n" +
                $"  border-color: {hoverBg};\n" +
                $"  color: {text};\n" +
                $"}}";
        }

        // Outline synthesis.
        // Secondary is special: its bg token is "transparent" (not a visible colour),
        // so the outline rule uses text/border/hover-bg tokens directly instead of bg.
        var bgIsTransparent = string.IsNullOrEmpty(bg) ||
                              string.Equals(bg, "transparent", StringComparison.OrdinalIgnoreCase);

        if (bgIsTransparent)
        {
            // Secondary-style outline
            return
                $"{cssClass} {{\n" +
                $"  background: transparent;\n" +
                $"  color: {text};\n" +
                $"  border-color: {border};\n" +
                $"}}\n" +
                $"{cssClass}:hover {{\n" +
                $"  background: {hoverBg};\n" +
                $"  border-color: {border};\n" +
                $"  color: {text};\n" +
                $"}}";
        }

        // Standard outline (primary, success, danger, warning, info)
        return
            $"{cssClass} {{\n" +
            $"  background: transparent;\n" +
            $"  color: {bg};\n" +
            $"  border-color: {border};\n" +
            $"}}\n" +
            $"{cssClass}:hover {{\n" +
            $"  background: {bg};\n" +
            $"  border-color: {border};\n" +
            $"  color: {text};\n" +
            $"}}";
    }

    private string ResolveToken(string tokenKey)
    {
        if (_tokensByKey.TryGetValue(tokenKey, out var token))
            return token.CurrentValue ?? token.DefaultValue;
        return string.Empty;
    }

    // =========================================================================
    // CSS Validation
    // =========================================================================

    private static readonly HashSet<string> KnownProperties =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "background", "color", "border-color", "border",
            "opacity", "font-size", "padding", "border-radius", "text-decoration",
        };

    private static readonly Regex HexColorRegex =
        new(@"^#[0-9a-fA-F]{3}(?:[0-9a-fA-F]{3})?(?:[0-9a-fA-F]{2})?$",
            RegexOptions.Compiled);

    private static readonly Regex RgbColorRegex  = new(@"^rgba?\s*\(", RegexOptions.Compiled);
    private static readonly Regex HslColorRegex  = new(@"^hsla?\s*\(", RegexOptions.Compiled);
    private static readonly Regex RuleBodyRegex  = new(@"\{([^{}]+)\}", RegexOptions.Singleline | RegexOptions.Compiled);
    private static readonly Regex DeclLineRegex  = new(@"^([a-zA-Z-]+)\s*:\s*(.+?);?\s*$", RegexOptions.Compiled);

    private static readonly HashSet<string> NamedColors =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "transparent", "inherit", "initial", "currentcolor",
            "none", "white", "black", "red", "green", "blue",
            "yellow", "orange", "purple", "gray", "grey",
        };

    private static (bool Valid, string? Error) ValidateCss(string cssText)
    {
        if (string.IsNullOrWhiteSpace(cssText))
            return (false, "CSS cannot be empty.");

        var ruleMatches = RuleBodyRegex.Matches(cssText);
        if (ruleMatches.Count == 0)
            return (false, "No CSS rule body found (expected { } block).");

        var lineNum = 0;
        foreach (Match ruleMatch in ruleMatches)
        {
            var body  = ruleMatch.Groups[1].Value;
            var lines = body.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawLine in lines)
            {
                lineNum++;
                var line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("/*", StringComparison.Ordinal))
                    continue;

                var declMatch = DeclLineRegex.Match(line);
                if (!declMatch.Success)
                    return (false, $"Line {lineNum}: Expected 'property: value;' — found '{line}'.");

                var property = declMatch.Groups[1].Value.Trim();
                var value    = declMatch.Groups[2].Value.Trim();

                if (!KnownProperties.Contains(property))
                    return (false, $"Line {lineNum}: Unknown CSS property '{property}'.");

                if (string.IsNullOrWhiteSpace(value))
                    return (false, $"Line {lineNum}: Empty value for property '{property}'.");

                if (property is "background" or "color" or "border-color" or "border")
                {
                    if (!IsValidColorValue(value))
                        return (false, $"Line {lineNum}: Invalid color value '{value}'. Use #rrggbb, rgb(), rgba(), hsl(), a named colour, or 'transparent'.");
                }
            }
        }

        return (true, null);
    }

    private static bool IsValidColorValue(string value)
    {
        if (NamedColors.Contains(value))                                    return true;
        if (HexColorRegex.IsMatch(value))                                   return true;
        if (RgbColorRegex.IsMatch(value))                                   return true;
        if (HslColorRegex.IsMatch(value))                                   return true;
        if (value.StartsWith("var(", StringComparison.OrdinalIgnoreCase))   return true;
        return false;
    }

    // =========================================================================
    // CSS Parsing — extract token values from edited CSS text
    // =========================================================================

    private static readonly Regex RulePattern =
        new(@"([^{]+)\{([^}]+)\}", RegexOptions.Singleline | RegexOptions.Compiled);

    /// <summary>
    /// Parses the synthesized CSS in the editor and extracts a dictionary of
    /// { CssVariable → newValue } suitable for DesignTokenApiServiceV2.UpdateTokensAsync.
    /// </summary>
    private Dictionary<string, string?> ParseCssToTokenValues(
        ButtonGroupState state, string cssText)
    {
        var result  = new Dictionary<string, string?>();
        var variant = state.Variants.FirstOrDefault(v => v.Key == state.SelectedVariantKey);

        if (variant is null || !variant.IsEditable)
            return result;

        var isOutline   = state.GroupKey == "outline";
        var baseVariant = state.SelectedVariantKey.Replace("outline-", string.Empty);

        // For secondary-style outline: bg token is transparent, so the synthesis
        // used text/border tokens directly — parsing must mirror that.
        var bg             = ResolveToken($"{baseVariant}-bg");
        var bgTransparent  = string.IsNullOrEmpty(bg) ||
                             string.Equals(bg, "transparent", StringComparison.OrdinalIgnoreCase);

        foreach (Match rule in RulePattern.Matches(cssText))
        {
            var selector = rule.Groups[1].Value.Trim();
            var body     = rule.Groups[2].Value;
            var isHover  = selector.Contains(":hover", StringComparison.Ordinal);
            var decls    = ExtractDeclarations(body);

            if (!isOutline)
            {
                // Basic button — direct token mapping
                if (!isHover)
                {
                    TrySetToken(result, $"--ds-btn-{baseVariant}-bg",     decls, "background");
                    TrySetToken(result, $"--ds-btn-{baseVariant}-text",   decls, "color");
                    TrySetToken(result, $"--ds-btn-{baseVariant}-border", decls, "border-color");
                }
                else
                {
                    TrySetToken(result, $"--ds-btn-{baseVariant}-hover-bg", decls, "background");
                }
            }
            else if (bgTransparent)
            {
                // Secondary-style outline: colour/border use text/border tokens
                if (!isHover)
                {
                    TrySetToken(result, $"--ds-btn-{baseVariant}-text",   decls, "color");
                    TrySetToken(result, $"--ds-btn-{baseVariant}-border", decls, "border-color");
                }
                else
                {
                    TrySetToken(result, $"--ds-btn-{baseVariant}-hover-bg", decls, "background");
                }
            }
            else
            {
                // Standard outline: colour in base rule = bg token; hover colour = text token
                if (!isHover)
                {
                    TrySetToken(result, $"--ds-btn-{baseVariant}-bg",     decls, "color");
                    TrySetToken(result, $"--ds-btn-{baseVariant}-border", decls, "border-color");
                }
                else
                {
                    TrySetToken(result, $"--ds-btn-{baseVariant}-text", decls, "color");
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Adds a CssVariable → value entry to the result dictionary, skipping
    /// empty or "transparent" values (which are not persisted as token overrides).
    /// </summary>
    private static void TrySetToken(
        Dictionary<string, string?> result,
        string cssVariable,
        Dictionary<string, string> decls,
        string property)
    {
        if (!decls.TryGetValue(property, out var value)) return;

        var trimmed = value.Trim();
        if (string.IsNullOrWhiteSpace(trimmed) ||
            string.Equals(trimmed, "transparent", StringComparison.OrdinalIgnoreCase))
            return;

        result[cssVariable] = trimmed;
    }

    /// <summary>
    /// Splits a CSS rule body into a case-insensitive property → value dictionary.
    /// </summary>
    private static Dictionary<string, string> ExtractDeclarations(string ruleBody)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var rawLine in ruleBody.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var line = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("/*", StringComparison.Ordinal))
                continue;

            var colonIdx = line.IndexOf(':', StringComparison.Ordinal);
            if (colonIdx < 0) continue;

            var property = line[..colonIdx].Trim();
            var value    = line[(colonIdx + 1)..].TrimEnd(';', ' ', '\t').Trim();

            if (!string.IsNullOrWhiteSpace(property) && !string.IsNullOrWhiteSpace(value))
                result[property] = value;
        }
        return result;
    }

    // =========================================================================
    // Preview CSS generation (Layer 3 — scoped style injection)
    // =========================================================================

    /// <summary>
    /// Builds a scoped CSS string targeting only the buttons inside the group's
    /// demo row div (#btn-demo-{groupKey}).  Uses !important to override both
    /// the static GlobalSettings.css rules and the global :root token overrides.
    /// </summary>
    private string BuildScopedPreviewCss(
        ButtonGroupState state,
        Dictionary<string, string?> tokenUpdates)
    {
        var variant = state.Variants.FirstOrDefault(v => v.Key == state.SelectedVariantKey);
        if (variant is null) return string.Empty;

        var cssClass    = variant.CssClass;
        var wrapperId   = $"btn-demo-{state.GroupKey}";
        var isOutline   = state.GroupKey == "outline";
        var baseVariant = state.SelectedVariantKey.Replace("outline-", string.Empty);

        // Resolve values: apply preview updates on top of current token values.
        // tokenUpdates is keyed by CssVariable (e.g. "--ds-btn-primary-bg").
        string GetPreviewValue(string tokenKey)
        {
            var cssVar = $"--ds-btn-{tokenKey}";
            if (tokenUpdates.TryGetValue(cssVar, out var updated) && updated is not null)
                return updated;
            return ResolveToken(tokenKey);
        }

        var bg      = GetPreviewValue($"{baseVariant}-bg");
        var text    = GetPreviewValue($"{baseVariant}-text");
        var border  = GetPreviewValue($"{baseVariant}-border");
        var hoverBg = GetPreviewValue($"{baseVariant}-hover-bg");

        var bgIsTransparent = string.IsNullOrEmpty(bg) ||
                              string.Equals(bg, "transparent", StringComparison.OrdinalIgnoreCase);

        var sb = new StringBuilder();

        if (!isOutline)
        {
            sb.AppendLine($"#{wrapperId} .{cssClass} {{");
            if (!string.IsNullOrEmpty(bg))     sb.AppendLine($"  background: {bg} !important;");
            if (!string.IsNullOrEmpty(text))   sb.AppendLine($"  color: {text} !important;");
            if (!string.IsNullOrEmpty(border)) sb.AppendLine($"  border-color: {border} !important;");
            sb.AppendLine("}");
            sb.AppendLine($"#{wrapperId} .{cssClass}:hover {{");
            if (!string.IsNullOrEmpty(hoverBg)) sb.AppendLine($"  background: {hoverBg} !important;");
            if (!string.IsNullOrEmpty(hoverBg)) sb.AppendLine($"  border-color: {hoverBg} !important;");
            if (!string.IsNullOrEmpty(text))    sb.AppendLine($"  color: {text} !important;");
            sb.AppendLine("}");
        }
        else if (bgIsTransparent)
        {
            // Secondary-style outline
            sb.AppendLine($"#{wrapperId} .{cssClass} {{");
            sb.AppendLine( "  background: transparent !important;");
            if (!string.IsNullOrEmpty(text))   sb.AppendLine($"  color: {text} !important;");
            if (!string.IsNullOrEmpty(border)) sb.AppendLine($"  border-color: {border} !important;");
            sb.AppendLine("}");
            sb.AppendLine($"#{wrapperId} .{cssClass}:hover {{");
            if (!string.IsNullOrEmpty(hoverBg)) sb.AppendLine($"  background: {hoverBg} !important;");
            if (!string.IsNullOrEmpty(border))  sb.AppendLine($"  border-color: {border} !important;");
            if (!string.IsNullOrEmpty(text))    sb.AppendLine($"  color: {text} !important;");
            sb.AppendLine("}");
        }
        else
        {
            // Standard outline
            sb.AppendLine($"#{wrapperId} .{cssClass} {{");
            sb.AppendLine( "  background: transparent !important;");
            if (!string.IsNullOrEmpty(bg))     sb.AppendLine($"  color: {bg} !important;");
            if (!string.IsNullOrEmpty(border)) sb.AppendLine($"  border-color: {border} !important;");
            sb.AppendLine("}");
            sb.AppendLine($"#{wrapperId} .{cssClass}:hover {{");
            if (!string.IsNullOrEmpty(bg))     sb.AppendLine($"  background: {bg} !important;");
            if (!string.IsNullOrEmpty(border)) sb.AppendLine($"  border-color: {border} !important;");
            if (!string.IsNullOrEmpty(text))   sb.AppendLine($"  color: {text} !important;");
            sb.AppendLine("}");
        }

        return sb.ToString();
    }
}
