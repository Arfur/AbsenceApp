/* =============================================================================
   File        : Index.razor.cs
   Namespace   : AbsenceApp.Client.Components.Pages.GlobalSettings.UIKits.Dropdown
   Author      : GitHub Copilot (Refactored by Michael + Copilot)
   Version     : 3.3.0
   Created     : 2026-05-19
   Updated     : 2026-05-19
   Changes     : v3.0.0 — Cut‑down reset version. Removed all old groups,
                 variants, static CSS, and rendering logic. Clean base ready
                 for adding KI‑Admin dropdown groups one at a time.
                 v3.3.0 — Added token support to all dropdown variants.
-------------------------------------------------------------------------------
   Purpose     : Minimal dropdown UI Kit page with Buttons‑style accordion
                 + Edit / Save / Cancel / Preview workflow. No groups yet.
============================================================================= */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AbsenceApp.Client.Components.Pages.GlobalSettings.UIKits.Dropdown;

public partial class Index : ComponentBase
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Variant + Group Models (kept minimal)
    // ─────────────────────────────────────────────────────────────────────────

    public sealed record DropdownVariantDef(
        string Key,
        string Label,
        string CssClass,
        string PreviewCssClass,
        string[] TokenKeys
    )
    {
        public bool IsEditable => TokenKeys.Length > 0;
    }

    public sealed class DropdownGroupState
    {
        public required string GroupKey { get; init; }
        public required string GroupTitle { get; init; }
        public required List<DropdownVariantDef> Variants { get; init; }

        public string SelectedVariantKey { get; set; } = string.Empty;
        public bool AccordionOpen { get; set; }
        public bool IsEditing { get; set; }
        public bool IsSaving { get; set; }

        public string EditorText { get; set; } = string.Empty;
        public string? PreviewCss { get; set; }
        public string? StatusMessage { get; set; }
        public bool StatusIsError { get; set; }

        // ⭐ Step 3.1 — Add this property here
        public string? OpenMenuForVariant { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  State
    // ─────────────────────────────────────────────────────────────────────────

    private readonly List<DropdownGroupState> _groups = [];
    private readonly Dictionary<string, string> _workingCopies = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string> _customSavedCss = new(StringComparer.OrdinalIgnoreCase);

    private bool _isLoading;
    private string? _initError;

    private IEnumerable<DropdownGroupState> GroupStates => _groups;

    // ─────────────────────────────────────────────────────────────────────────
    //  Init (Step 6 — Add all dropdown groups)
    // ─────────────────────────────────────────────────────────────────────────

    protected override void OnInitialized()
    {
        _isLoading = false;
        _initError = null;

        _groups.Clear();

        // Group 1 — Single Button & Link Dropdown
        _groups.Add(new DropdownGroupState
        {
            GroupKey = "single",
            GroupTitle = "Single Button & Link Dropdown",
            Variants =
            [
                new("button", "Dropdown Button", "ddp-single-button", "ddp-single-button",
                    ["dropdown.button.bg", "dropdown.button.text", "dropdown.button.border"]),
                new("link", "Dropdown Link", "ddp-single-link", "ddp-single-link",
                    ["dropdown.link.text", "dropdown.link.hover.text"])
            ],
            SelectedVariantKey = "button",
            AccordionOpen = false
        });

        // Group 2 — Split Button Dropdowns
        _groups.Add(new DropdownGroupState
        {
            GroupKey = "split",
            GroupTitle = "Split Button Dropdowns",
            Variants =
            [
                new("primary", "Primary", "ddp-split-primary", "ddp-split-primary",
                    ["dropdown.split.primary.bg", "dropdown.split.primary.text", "dropdown.split.primary.border"]),
                new("secondary", "Secondary", "ddp-split-secondary", "ddp-split-secondary",
                    ["dropdown.split.secondary.bg", "dropdown.split.secondary.text", "dropdown.split.secondary.border"]),
                new("success", "Success", "ddp-split-success", "ddp-split-success",
                    ["dropdown.split.success.bg", "dropdown.split.success.text", "dropdown.split.success.border"])
            ],
            SelectedVariantKey = "primary",
            AccordionOpen = false
        });

        // Group 3 — Directional Dropdowns
        _groups.Add(new DropdownGroupState
        {
            GroupKey = "directional",
            GroupTitle = "Directional Dropdowns",
            Variants =
            [
                new("dropup", "Dropup", "ddp-dropup", "ddp-dropup",
                    ["dropdown.directional.dropup.bg", "dropdown.directional.dropup.text"]),
                new("dropstart", "Dropstart", "ddp-dropstart", "ddp-dropstart",
                    ["dropdown.directional.dropstart.bg", "dropdown.directional.dropstart.text"]),
                new("dropend", "Dropend", "ddp-dropend", "ddp-dropend",
                    ["dropdown.directional.dropend.bg", "dropdown.directional.dropend.text"])
            ],
            SelectedVariantKey = "dropup",
            AccordionOpen = false
        });
    }

    private void OnToggleAccordion(DropdownGroupState state)
    {
        if (state.AccordionOpen && state.IsEditing)
            CancelEditing(state);

        state.AccordionOpen = !state.AccordionOpen;

        if (state.AccordionOpen && string.IsNullOrWhiteSpace(state.EditorText))
            state.EditorText = SynthesizeCss(state, state.SelectedVariantKey);
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Variant Selection
    // ─────────────────────────────────────────────────────────────────────────

    private void OnSelectedVariantChanged(DropdownGroupState state, string variantKey)
    {
        if (state.IsEditing)
            _workingCopies[$"{state.GroupKey}:{state.SelectedVariantKey}"] = state.EditorText;

        state.SelectedVariantKey = variantKey;
        state.StatusMessage = null;

        if (state.IsEditing &&
            _workingCopies.TryGetValue($"{state.GroupKey}:{variantKey}", out var saved))
        {
            state.EditorText = saved;
            return;
        }

        state.EditorText = SynthesizeCss(state, variantKey);
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Edit / Save / Cancel / Preview
    // ─────────────────────────────────────────────────────────────────────────

    private Task OnEditSaveClickedAsync(DropdownGroupState state)
    {
        if (!state.IsEditing)
        {
            state.IsEditing = true;
            state.StatusMessage = null;
            state.StatusIsError = false;

            if (string.IsNullOrWhiteSpace(state.EditorText))
                state.EditorText = SynthesizeCss(state, state.SelectedVariantKey);

            return Task.CompletedTask;
        }

        return SaveAsync(state);
    }

    private Task SaveAsync(DropdownGroupState state)
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

    private void OnCancelClicked(DropdownGroupState state)
    {
        CancelEditing(state);
    }

    private void OnPreviewClicked(DropdownGroupState state)
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

    private void OnEditorInput(DropdownGroupState state, string text)
    {
        state.EditorText = text;
    }

    private void CancelEditing(DropdownGroupState state)
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
        foreach (var key in _workingCopies.Keys.Where(k => k.StartsWith(prefix)).ToList())
            _workingCopies.Remove(key);
    }

    private void OnDropdownClicked(DropdownGroupState group, DropdownVariantDef variant)
    {
        // Close all menus in all groups
        foreach (var g in _groups)
            g.OpenMenuForVariant = null;

        // Toggle this one
        if (group.OpenMenuForVariant == variant.Key)
        {
            group.OpenMenuForVariant = null; // close
        }
        else
        {
            group.OpenMenuForVariant = variant.Key; // open
            group.SelectedVariantKey = variant.Key; // becomes active for this group
        }
    }

    private void OnSplitDropdownClicked(DropdownGroupState group, DropdownVariantDef variant)
    {
        // Close all menus globally
        foreach (var g in _groups)
            g.OpenMenuForVariant = null;

        // Toggle this one
        if (group.OpenMenuForVariant == variant.Key)
        {
            group.OpenMenuForVariant = null; // close
        }
        else
        {
            group.OpenMenuForVariant = variant.Key; // open
            group.SelectedVariantKey = variant.Key; // active for this group
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  CSS Helpers (kept minimal)
    // ─────────────────────────────────────────────────────────────────────────

    private static bool IsSelected(DropdownGroupState state, DropdownVariantDef variant)
        => string.Equals(state.SelectedVariantKey, variant.Key, StringComparison.OrdinalIgnoreCase);

    private DropdownVariantDef? GetSelectedVariant(DropdownGroupState state)
        => state.Variants.FirstOrDefault(v => IsSelected(state, v));

    private string SynthesizeCss(DropdownGroupState state, string variantKey)
    {
        var variant = state.Variants.First(v => v.Key == variantKey);

        var sb = new StringBuilder();
        sb.AppendLine($".{variantKey} {{");

        foreach (var token in variant.TokenKeys)
            sb.AppendLine($"  /* uses: var(--{token}) */");

        sb.AppendLine("}");
        return sb.ToString();
    }

    private string BuildScopedPreviewCssFromRaw(DropdownGroupState state, string cssText)
    {
        var wrapperId = $"#dd-demo-{state.GroupKey}";
        var sb = new StringBuilder();

        foreach (Match rule in RulePattern.Matches(cssText))
        {
            var selector = rule.Groups[1].Value.Trim();
            var body = rule.Groups[2].Value.Trim();
            if (string.IsNullOrWhiteSpace(selector) || string.IsNullOrWhiteSpace(body))
                continue;

            var scopedSelectors = selector
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => $"{wrapperId} {s.Trim()}");

            sb.AppendLine($"{string.Join(", ", scopedSelectors)} {{");

            foreach (var decl in ExtractDeclarations(body))
                if (decl.Value.StartsWith("var(--"))
                    sb.AppendLine($"  {decl.Key}: {decl.Value} !important;");
                else
                    sb.AppendLine($"  {decl.Key}: {decl.Value} !important;");

            sb.AppendLine("}");
        }

        return sb.ToString();
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  CSS Validation (unchanged)
    // ─────────────────────────────────────────────────────────────────────────

    private static readonly Regex RulePattern =
        new(@"([^{]+)\{([^}]+)\}", RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex RuleBodyRegex =
        new(@"\{([^{}]+)\}", RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex DeclLineRegex =
        new(@"^([a-zA-Z-]+)\s*:\s*(.+?);?\s*$", RegexOptions.Compiled);

    private static readonly HashSet<string> KnownProperties =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "background", "color", "border-color", "border", "opacity",
            "text-decoration", "border-radius", "padding", "font-size",
            "display", "gap", "width", "min-width", "height", "align-items",
            "justify-content", "cursor", "pointer-events", "box-shadow",
            "transition", "line-height", "outline", "vertical-align",
            "white-space", "font-weight"
        };

    private static (bool Valid, string? Error) ValidateCss(string cssText)
    {
        if (string.IsNullOrWhiteSpace(cssText))
            return (false, "CSS cannot be empty.");

        var ruleMatches = RuleBodyRegex.Matches(cssText);
        if (ruleMatches.Count == 0)
            return (false, "No CSS rule body found.");

        var lineNum = 0;
        foreach (Match ruleMatch in ruleMatches)
        {
            var body = ruleMatch.Groups[1].Value;
            var lines = body.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawLine in lines)
            {
                lineNum++;
                var line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("/*"))
                    continue;

                var declMatch = DeclLineRegex.Match(line);
                if (!declMatch.Success)
                    return (false, $"Line {lineNum}: Expected 'property: value;'");

                var property = declMatch.Groups[1].Value.Trim();
                var value = declMatch.Groups[2].Value.Trim();

                if (!KnownProperties.Contains(property))
                    return (false, $"Line {lineNum}: Unknown CSS property '{property}'.");

                if (string.IsNullOrWhiteSpace(value))
                    return (false, $"Line {lineNum}: Empty value for '{property}'.");
            }
        }

        return (true, null);
    }

    private static Dictionary<string, string> ExtractDeclarations(string ruleBody)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var rawLine in ruleBody.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var line = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("/*"))
                continue;

            var colonIndex = line.IndexOf(':');
            if (colonIndex < 0)
                continue;

            var property = line[..colonIndex].Trim();
            var value = line[(colonIndex + 1)..].TrimEnd(';').Trim();

            if (!string.IsNullOrWhiteSpace(property) && !string.IsNullOrWhiteSpace(value))
                result[property] = value;
        }

        return result;
    }
}
