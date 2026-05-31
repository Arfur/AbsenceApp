/*
===============================================================================
 File        : Index.razor.cs
 Namespace   : AbsenceApp.Client.Components.Pages.GlobalSettings.UIKits.Buttons
 Author      : GitHub Copilot
 Version     : 7.0.0
 Created     : 2026-05-14
 Updated     : 2026-05-22 16:10
 Changes     : v7.0.0 — Hybrid Merge: preserved Buttons3 UI Kit layout and
               galleries while replacing legacy CSS synthesis logic with
               token-backed editor flow (ButtonsEditorViewModelV2 +
               DesignSystemConfigService + components.json mappings).
-------------------------------------------------------------------------------
 Purpose     : Buttons3 UX model with full Ki-Admin button sets (one set per row).
               - Accordion sections
               - Real button selectors
               - Token-backed Edit / Save, Cancel workflow
               - Token-driven preview CSS variable overlay
               - All accordions collapsed by default
===============================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AbsenceApp.Client.Services;
using AbsenceApp.Client.ViewModels.V2;
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
        string[] TokenKeys);

    public sealed class ButtonGroupUiState
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

    [Inject] private ButtonsEditorViewModelV2 ButtonsEditorViewModel { get; set; } = default!;
    [Inject] private DesignSystemConfigService ConfigService { get; set; } = default!;

    private sealed class ButtonConfigMetadata
    {
        public HashSet<string> Variants { get; } = new(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, string> TokenMappings { get; } = new(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, List<string>> GroupCssVariables { get; } = new(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> PreviewCssVariables { get; } = new(StringComparer.OrdinalIgnoreCase);
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
        new("basic-dark", "Dark", "dsv2-btn--dark", "dsv2-btn--dark", "custom", [])
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
        new("outline-dark", "Dark", "dsv2-btn--outline-dark", "dsv2-btn--outline-dark", "custom", [])
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
        new("soft-dark", "Dark", "dsv2-btn--soft-dark", "dsv2-btn--soft-dark", "custom", [])
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
        new("radius-primary",   "Primary",   "btp-radius-primary",   "dsv2-btn dsv2-btn--primary btp-radius-primary",   "custom", []),
        new("radius-secondary", "Secondary", "btp-radius-secondary", "dsv2-btn dsv2-btn--secondary btp-radius-secondary", "custom", []),
        new("radius-success",   "Success",   "btp-radius-success",   "dsv2-btn dsv2-btn--success btp-radius-success",   "custom", []),
        new("radius-danger",    "Danger",    "btp-radius-danger",    "dsv2-btn dsv2-btn--danger btp-radius-danger",    "custom", []),
        new("radius-warning",   "Warning",   "btp-radius-warning",   "dsv2-btn dsv2-btn--warning btp-radius-warning",   "custom", []),
        new("radius-info",      "Info",      "btp-radius-info",      "dsv2-btn dsv2-btn--info btp-radius-info",        "custom", []),
        new("radius-light",     "Light",     "btp-radius-light",     "dsv2-btn dsv2-btn--light btp-radius-light",      "custom", []),
        new("radius-dark",      "Dark",      "btp-radius-dark",      "dsv2-btn dsv2-btn--dark btp-radius-dark",        "custom", [])
    ];

    private static readonly List<ButtonVariantDef> ActiveVariants =
    [
        new("active-primary",   "Primary",   "btp-active-primary",   "dsv2-btn btp-active-primary",   "custom", []),
        new("active-secondary", "Secondary", "btp-active-secondary", "dsv2-btn btp-active-secondary", "custom", []),
        new("active-success",   "Success",   "btp-active-success",   "dsv2-btn btp-active-success",   "custom", []),
        new("active-danger",    "Danger",    "btp-active-danger",    "dsv2-btn btp-active-danger",    "custom", []),
        new("active-warning",   "Warning",   "btp-active-warning",   "dsv2-btn btp-active-warning",   "custom", []),
        new("active-info",      "Info",      "btp-active-info",      "dsv2-btn btp-active-info",      "custom", []),
        new("active-light",     "Light",     "btp-active-light",     "dsv2-btn btp-active-light",     "custom", []),
        new("active-dark",      "Dark",      "btp-active-dark",      "dsv2-btn btp-active-dark",      "custom", [])
    ];

    private static readonly List<ButtonVariantDef> DisabledVariants =
    [
        new("disabled-primary",   "Primary",   "btp-disabled-primary",   "dsv2-btn btp-disabled-primary",   "custom", []),
        new("disabled-secondary", "Secondary", "btp-disabled-secondary", "dsv2-btn btp-disabled-secondary", "custom", []),
        new("disabled-success",   "Success",   "btp-disabled-success",   "dsv2-btn btp-disabled-success",   "custom", []),
        new("disabled-danger",    "Danger",    "btp-disabled-danger",    "dsv2-btn btp-disabled-danger",    "custom", []),
        new("disabled-warning",   "Warning",   "btp-disabled-warning",   "dsv2-btn btp-disabled-warning",   "custom", []),
        new("disabled-info",      "Info",      "btp-disabled-info",      "dsv2-btn btp-disabled-info",      "custom", []),
        new("disabled-light",     "Light",     "btp-disabled-light",     "dsv2-btn btp-disabled-light",     "custom", []),
        new("disabled-dark",      "Dark",      "btp-disabled-dark",      "dsv2-btn btp-disabled-dark",      "custom", [])
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
        new("block-primary", "Button", "btp-block-primary", "dsv2-btn dsv2-btn--primary btp-block-btn btp-block-primary", "custom", []),
        new("block-secondary", "Button", "btp-block-secondary", "dsv2-btn dsv2-btn--secondary btp-block-btn btp-block-secondary", "custom", []),
        new("block-outline-primary", "Button", "btp-block-outline-primary", "dsv2-btn dsv2-btn--outline-primary btp-block-btn btp-block-outline-primary", "custom", []),
        new("block-outline-secondary", "Button", "btp-block-outline-secondary", "dsv2-btn dsv2-btn--outline-secondary btp-block-btn btp-block-outline-secondary", "custom", []),
        new("block-soft-primary", "Button", "btp-block-soft-primary", "dsv2-btn dsv2-btn--soft-primary btp-block-btn btp-block-soft-primary", "custom", []),
        new("block-soft-secondary", "Button", "btp-block-soft-secondary", "dsv2-btn dsv2-btn--soft-secondary btp-block-btn btp-block-soft-secondary", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> SizeVariants =
    [
        new("size-sm", "Small",  "btp-size-sm", "dsv2-btn--primary btp-size-sm", "custom", []),
        new("size-md", "Medium", "btp-size-md", "dsv2-btn--primary btp-size-md", "custom", []),
        new("size-lg", "Large",  "btp-size-lg", "dsv2-btn--primary btp-size-lg", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> GroupVariants =
    [
        new("group-solid", "Group", "btp-group-solid", "btp-group-demo btp-group-solid", "custom", []),
        new("group-outline", "Group", "btp-group-outline", "btp-group-demo btp-group-outline", "custom", []),
        new("group-soft", "Group", "btp-group-soft", "btp-group-demo btp-group-soft", "custom", [])
    ];

    private static readonly List<ButtonVariantDef> ActionVariants =
    [
        new("action-save",     "Save",     "dsv2-btn--primary",   "dsv2-btn dsv2-btn--primary",   "custom", []),
        new("action-cancel",   "Cancel",   "dsv2-btn--secondary", "dsv2-btn dsv2-btn--secondary", "custom", []),
        new("action-settings", "Settings", "dsv2-btn--primary",   "dsv2-btn dsv2-btn--primary",   "custom", []),
        new("action-more",     "More",     "dsv2-btn--secondary", "dsv2-btn dsv2-btn--secondary", "custom", [])
    ];

    private static readonly Regex CssVariableLineRegex =
        new(@"^(--[a-zA-Z0-9\-_]+)\s*:\s*(.+?)\s*;?$", RegexOptions.Compiled);

    private readonly Dictionary<string, string> _draftEditorCache = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string?> _variantToConfigVariant = new(StringComparer.OrdinalIgnoreCase);

    private ButtonConfigMetadata _buttonConfig = new();

    private ButtonGroupUiState _basicState = default!;
    private ButtonGroupUiState _outlineState = default!;
    private ButtonGroupUiState _softState = default!;
    private ButtonGroupUiState _iconState = default!;
    private ButtonGroupUiState _socialState = default!;
    private ButtonGroupUiState _radiusState = default!;
    private ButtonGroupUiState _activeState = default!;
    private ButtonGroupUiState _disabledState = default!;
    private ButtonGroupUiState _loadingState = default!;
    private ButtonGroupUiState _blockState = default!;
    private ButtonGroupUiState _sizeState = default!;
    private ButtonGroupUiState _groupState = default!;
    private ButtonGroupUiState _actionState = default!;

    private bool _isLoading;
    private string? _initError;

    private IEnumerable<ButtonGroupUiState> GroupStates =>
    [
        _basicState,
        _outlineState,
        _softState,
        _iconState,
        _radiusState,
        _socialState,
        _disabledState,
        _activeState,
        _loadingState,
        _blockState,
        _sizeState,
        _groupState,
        _actionState,
    ];

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _initError = null;

        try
        {
            await ButtonsEditorViewModel.InitialiseAsync();
            var components = await ConfigService.GetComponentsAsync();
            _buttonConfig = ParseButtonConfig(components);

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
            _sizeState = CreateState("sizes", "Button Sizes", SizeVariants);
            _groupState = CreateState("groups", "Button Groups", GroupVariants);
            _actionState = CreateState("action", "Action Buttons", ActionVariants);

            foreach (var state in GroupStates)
            {
                HydrateVariantMapping(state);
                RefreshStateFromTokens(state, state.SelectedVariantKey);
            }
        }
        catch (Exception ex)
        {
            _initError = $"Token editor setup failed: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private static ButtonConfigMetadata ParseButtonConfig(JsonObject components)
    {
        if (components["button"] is not JsonObject buttonNode)
            throw new InvalidOperationException("components.json is missing the 'button' section.");

        var result = new ButtonConfigMetadata();

        if (buttonNode["variants"] is JsonArray variantsArray)
        {
            foreach (var item in variantsArray.OfType<JsonValue>())
            {
                var value = item.GetValue<string>();
                if (!string.IsNullOrWhiteSpace(value))
                    result.Variants.Add(value.Trim());
            }
        }

        if (buttonNode["tokenMappings"] is JsonObject tokenMappings)
        {
            foreach (var kv in tokenMappings)
            {
                var cssVar = kv.Value?.GetValue<string>();
                if (!string.IsNullOrWhiteSpace(kv.Key) && !string.IsNullOrWhiteSpace(cssVar))
                {
                    result.TokenMappings[kv.Key] = cssVar.Trim();
                }
            }
        }

        if (buttonNode["editor"] is JsonObject editorNode &&
            editorNode["groups"] is JsonObject groupsNode)
        {
            foreach (var kv in groupsNode)
            {
                if (kv.Value is not JsonArray tokenKeyArray)
                    continue;

                var mappedCssVars = new List<string>();
                foreach (var tokenValue in tokenKeyArray.OfType<JsonValue>())
                {
                    var tokenKey = tokenValue.GetValue<string>();
                    if (string.IsNullOrWhiteSpace(tokenKey))
                        continue;

                    if (result.TokenMappings.TryGetValue(tokenKey.Trim(), out var cssVar) &&
                        !mappedCssVars.Contains(cssVar, StringComparer.OrdinalIgnoreCase))
                    {
                        mappedCssVars.Add(cssVar);
                    }
                }

                if (!string.IsNullOrWhiteSpace(kv.Key))
                    result.GroupCssVariables[kv.Key] = mappedCssVars;
            }
        }

        if (buttonNode["preview"] is JsonObject previewNode &&
            previewNode["cssVariables"] is JsonArray previewVars)
        {
            foreach (var item in previewVars.OfType<JsonValue>())
            {
                var cssVar = item.GetValue<string>();
                if (!string.IsNullOrWhiteSpace(cssVar))
                    result.PreviewCssVariables.Add(cssVar.Trim());
            }
        }

        return result;
    }

    private ButtonGroupUiState CreateState(string key, string title, List<ButtonVariantDef> variants)
    {
        return new ButtonGroupUiState
        {
            GroupKey = key,
            GroupTitle = title,
            Variants = variants,
            SelectedVariantKey = variants[0].Key,
            AccordionOpen = false,
            IsEditing = false,
            IsSaving = false
        };
    }

    private void HydrateVariantMapping(ButtonGroupUiState state)
    {
        foreach (var variant in state.Variants)
        {
            if (_variantToConfigVariant.ContainsKey(variant.Key))
                continue;

            _variantToConfigVariant[variant.Key] = ResolveConfigVariant(variant.Key);
        }
    }

    private string? ResolveConfigVariant(string uiVariantKey)
    {
        if (_buttonConfig.Variants.Count == 0)
            return null;

        if (_buttonConfig.Variants.Contains(uiVariantKey))
            return uiVariantKey;

        var firstDash = uiVariantKey.IndexOf('-', StringComparison.Ordinal);
        if (firstDash >= 0 && firstDash < uiVariantKey.Length - 1)
        {
            var suffix = uiVariantKey[(firstDash + 1)..];
            if (_buttonConfig.Variants.Contains(suffix))
                return suffix;
        }

        return _buttonConfig.Variants.FirstOrDefault(v =>
            uiVariantKey.EndsWith(v, StringComparison.OrdinalIgnoreCase));
    }

    private string? ResolveTokenGroupId(ButtonGroupUiState state)
    {
        if (state.GroupKey is "radius" or "sizes")
            return "structure";

        return ResolveConfigVariant(state.SelectedVariantKey);
    }

    private List<string> GetCssVariablesForUiVariant(string uiVariantKey)
    {
        if (!_variantToConfigVariant.TryGetValue(uiVariantKey, out var configVariant) ||
            string.IsNullOrWhiteSpace(configVariant))
        {
            return [];
        }

        return _buttonConfig.GroupCssVariables.TryGetValue(configVariant, out var vars)
            ? vars
            : [];
    }

    private string BuildEditorTextFromVariables(List<string> cssVariables)
    {
        if (cssVariables.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        foreach (var cssVar in cssVariables)
        {
            var value = ButtonsEditorViewModel.GetCurrentValue(cssVar);
            if (string.IsNullOrWhiteSpace(value))
                value = "initial";

            sb.AppendLine($"{cssVar}: {value};");
        }

        return sb.ToString().TrimEnd();
    }

    private static Dictionary<string, string> ParseEditorAssignments(string editorText)
    {
        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(editorText))
            return values;

        var lines = editorText
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrWhiteSpace(l));

        foreach (var line in lines)
        {
            var match = CssVariableLineRegex.Match(line);
            if (!match.Success)
                continue;

            values[match.Groups[1].Value.Trim()] = match.Groups[2].Value.Trim();
        }

        return values;
    }

    private string BuildPreviewCss(Dictionary<string, string> draftValues, List<string> scopedVars)
    {
        if (scopedVars.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine(":root {");

        foreach (var cssVar in scopedVars)
        {
            var value = draftValues.TryGetValue(cssVar, out var edited)
                ? edited
                : ButtonsEditorViewModel.GetCurrentValue(cssVar);

            if (string.IsNullOrWhiteSpace(value))
                value = "initial";

            sb.AppendLine($"  {cssVar}: {value};");
        }

        sb.AppendLine("}");
        return sb.ToString().TrimEnd();
    }

    private void RefreshStateFromTokens(ButtonGroupUiState state, string variantKey)
    {
        var cssVars = GetCssVariablesForUiVariant(variantKey);

        if (cssVars.Count == 0)
        {
            state.EditorText = string.Empty;
            state.PreviewCss = string.Empty;
            state.StatusMessage = "No token mapping exists in components.json for this variant.";
            state.StatusIsError = true;
            return;
        }

        var cacheKey = $"{state.GroupKey}:{variantKey}";
        if (state.IsEditing && _draftEditorCache.TryGetValue(cacheKey, out var draft))
        {
            state.EditorText = draft;
        }
        else
        {
            state.EditorText = BuildEditorTextFromVariables(cssVars);
        }

        var parsed = ParseEditorAssignments(state.EditorText);
        state.PreviewCss = BuildPreviewCss(parsed, cssVars);
        state.StatusMessage = null;
        state.StatusIsError = false;
    }

    private static ButtonVariantDef? GetSelectedVariant(ButtonGroupUiState state)
        => state.Variants.FirstOrDefault(v => string.Equals(v.Key, state.SelectedVariantKey, StringComparison.OrdinalIgnoreCase));

    private void OnToggleAccordion(ButtonGroupUiState state)
    {
        if (state.AccordionOpen && state.IsEditing)
        {
            OnCancelClicked(state);
        }

        state.AccordionOpen = !state.AccordionOpen;

        if (state.AccordionOpen)
        {
            RefreshStateFromTokens(state, state.SelectedVariantKey);
        }
    }

    private void OnSelectedVariantChanged(ButtonGroupUiState state, string variantKey)
    {
        var currentCacheKey = $"{state.GroupKey}:{state.SelectedVariantKey}";
        if (state.IsEditing)
        {
            _draftEditorCache[currentCacheKey] = state.EditorText;
        }

        state.SelectedVariantKey = variantKey;
        RefreshStateFromTokens(state, variantKey);
    }

    private Task OnEditSaveClickedAsync(ButtonGroupUiState state)
    {
        if (!state.IsEditing)
        {
            state.IsEditing = true;
            state.StatusMessage = null;
            state.StatusIsError = false;
            RefreshStateFromTokens(state, state.SelectedVariantKey);
            return Task.CompletedTask;
        }

        return SaveAsync(state);
    }

    private async Task SaveAsync(ButtonGroupUiState state)
    {
        state.StatusMessage = null;
        state.StatusIsError = false;
        state.IsSaving = true;

        try
        {
            var selected = GetSelectedVariant(state);
            if (selected is null)
            {
                state.StatusMessage = "Variant not found.";
                state.StatusIsError = true;
                return;
            }

            var targetCssVars = GetCssVariablesForUiVariant(selected.Key);
            if (targetCssVars.Count == 0)
            {
                state.StatusMessage = "Save blocked: selected variant has no token mapping in components.json.";
                state.StatusIsError = true;
                return;
            }

            var parsed = ParseEditorAssignments(state.EditorText);
            var updates = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var cssVar in targetCssVars)
            {
                if (!parsed.TryGetValue(cssVar, out var value) || string.IsNullOrWhiteSpace(value))
                {
                    state.StatusMessage = $"Missing token value for '{cssVar}'.";
                    state.StatusIsError = true;
                    return;
                }

                updates[cssVar] = value;
            }

            var result = await ButtonsEditorViewModel.SaveCssVariableValuesAsync(updates);
            if (!result.Success)
            {
                state.StatusMessage = result.Error ?? "Failed to save token values.";
                state.StatusIsError = true;
                return;
            }

            state.IsEditing = false;
            state.StatusMessage = "Token values saved successfully.";
            state.StatusIsError = false;
            _draftEditorCache.Remove($"{state.GroupKey}:{state.SelectedVariantKey}");
            RefreshStateFromTokens(state, state.SelectedVariantKey);
        }
        finally
        {
            state.IsSaving = false;
        }
    }

    private void OnEditorInput(ButtonGroupUiState state, string text)
    {
        state.EditorText = text;
        _draftEditorCache[$"{state.GroupKey}:{state.SelectedVariantKey}"] = text;
        state.PreviewCss = BuildPreviewCss(ParseEditorAssignments(text), GetCssVariablesForUiVariant(state.SelectedVariantKey));
    }

    private void OnCancelClicked(ButtonGroupUiState state)
    {
        state.IsEditing = false;
        state.StatusMessage = null;
        state.StatusIsError = false;
        _draftEditorCache.Remove($"{state.GroupKey}:{state.SelectedVariantKey}");
        RefreshStateFromTokens(state, state.SelectedVariantKey);
    }
}
