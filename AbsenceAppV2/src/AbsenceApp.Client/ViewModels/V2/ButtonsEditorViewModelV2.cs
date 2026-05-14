/*
===============================================================================
 File        : ButtonsEditorViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-12
 Updated     : 2026-05-12
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the Buttons Editor page (/globalsettings/buttons).
               Provides token loading, edit buffering, live validation, save,
               and reset operations backed by DesignTokenApiServiceV2.

               Scoped lifetime: one instance per page navigation. The Singleton
               DesignTokenApiServiceV2 is injected safely — a Scoped service
               receiving a Singleton dependency is always safe.

               StateChanged event: fired after any state mutation so the Blazor
               component can call StateHasChanged without direct VM→Component
               coupling.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-12  Initial creation (Phase B — Buttons Editor).
-------------------------------------------------------------------------------
 Validation rules :
   - All values:        required (not empty / whitespace).
   - All values:        no CSS injection characters (;  {  }  <  >).
   - Color tokens:      hex, rgb/rgba, hsl/hsla, transparent, named keyword,
                        or a CSS var() reference.
   - Structure tokens:  CSS dimension (e.g. 6px, 0.875rem) or var().
===============================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Client.ViewModels.V2;

public sealed class ButtonsEditorViewModelV2
{
    private readonly DesignTokenApiServiceV2 _tokenService;

    public ButtonsEditorViewModelV2(DesignTokenApiServiceV2 tokenService)
        => _tokenService = tokenService;

    // =========================================================================
    // Events
    // =========================================================================

    /// <summary>
    /// Fired after any state change so the component can call StateHasChanged.
    /// </summary>
    public event Action? StateChanged;

    // =========================================================================
    // State
    // =========================================================================

    /// <summary>All tokens for the "btn" component group, loaded from the DB.</summary>
    public List<DesignToken> AllTokens { get; private set; } = [];

    /// <summary>Edit buffer: CssVariable → value currently being edited.</summary>
    public Dictionary<string, string> EditBuffer { get; private set; } = new();

    /// <summary>Validation errors: CssVariable → error message.</summary>
    public Dictionary<string, string> Errors { get; private set; } = new();

    public bool   IsSaving    { get; private set; }
    public bool   IsLoaded    { get; private set; }
    public string SaveMessage { get; private set; } = string.Empty;

    /// <summary>Currently selected variant key (null = nothing selected).</summary>
    public string? SelectedVariant { get; private set; }

    /// <summary>
    /// True when any visible token's EditBuffer value differs from the DB value.
    /// </summary>
    public bool IsDirty => VisibleTokens.Any(t =>
        EditBuffer.TryGetValue(t.CssVariable, out var v) &&
        v != (t.CurrentValue ?? t.DefaultValue));

    /// <summary>Tokens visible in the right editor panel for the current variant.</summary>
    public IEnumerable<DesignToken> VisibleTokens => SelectedVariant switch
    {
        null         => [],
        "structural" => AllTokens.Where(t => t.Category == "structure"),
        var key      => AllTokens.Where(t => t.TokenKey.StartsWith(key + "-", StringComparison.Ordinal)),
    };

    // =========================================================================
    // Initialisation
    // =========================================================================

    /// <summary>
    /// Loads all btn tokens from the database into AllTokens and resets the
    /// EditBuffer. Safe to call multiple times; skips if already initialised.
    /// </summary>
    public async Task InitialiseAsync()
    {
        if (IsLoaded) return;
        AllTokens = await _tokenService.GetGroupAsync("btn");
        RebuildBuffer();
        IsLoaded = true;
    }

    // =========================================================================
    // Variant selection
    // =========================================================================

    public void SelectVariant(string variant)
    {
        SelectedVariant = variant;
        Errors.Clear();
        SaveMessage = string.Empty;
        StateChanged?.Invoke();
    }

    // =========================================================================
    // Token editing
    // =========================================================================

    public void SetTokenValue(string cssVar, string value)
    {
        EditBuffer[cssVar] = value;
        ValidateSingle(cssVar, value);
        SaveMessage = string.Empty;
        StateChanged?.Invoke();
    }

    // =========================================================================
    // Save
    // =========================================================================

    public async Task SaveAsync()
    {
        if (!Validate())
        {
            StateChanged?.Invoke();
            return;
        }

        IsSaving = true;
        StateChanged?.Invoke();

        try
        {
            var updates = VisibleTokens.ToDictionary(
                t => t.CssVariable,
                t => (string?)EditBuffer.GetValueOrDefault(t.CssVariable, t.DefaultValue));

            await _tokenService.UpdateTokensAsync(updates);

            // Refresh from DB so IsDirty and RebuildBuffer reflect the saved state.
            AllTokens = await _tokenService.GetGroupAsync("btn");
            RebuildBuffer();
            SaveMessage = "Changes saved successfully.";
        }
        finally
        {
            IsSaving = false;
            StateChanged?.Invoke();
        }
    }

    // =========================================================================
    // Reset
    // =========================================================================

    public async Task ResetToDefaultsAsync()
    {
        IsSaving = true;
        StateChanged?.Invoke();

        try
        {
            await _tokenService.ResetGroupAsync("btn");
            AllTokens = await _tokenService.GetGroupAsync("btn");
            RebuildBuffer();
            Errors.Clear();
            SaveMessage = "Reset to defaults.";
        }
        finally
        {
            IsSaving = false;
            StateChanged?.Invoke();
        }
    }

    // =========================================================================
    // Validation
    // =========================================================================

    /// <summary>
    /// Validates all visible tokens. Populates Errors. Returns true if clean.
    /// </summary>
    public bool Validate()
    {
        Errors.Clear();
        foreach (var token in VisibleTokens)
        {
            var val = EditBuffer.GetValueOrDefault(token.CssVariable, string.Empty);
            ValidateSingle(token.CssVariable, val);
        }
        return Errors.Count == 0;
    }

    private void ValidateSingle(string cssVar, string value)
    {
        Errors.Remove(cssVar);

        if (string.IsNullOrWhiteSpace(value))
        {
            Errors[cssVar] = "Value cannot be empty.";
            return;
        }

        // Security: prevent CSS injection
        if (value.IndexOfAny([';', '{', '}', '<', '>']) >= 0)
        {
            Errors[cssVar] = "Value contains invalid characters (;  {  }  <  >).";
            return;
        }

        var token = AllTokens.FirstOrDefault(t => t.CssVariable == cssVar);
        if (token is null) return;

        if (token.Category == "color")
        {
            var v = value.Trim();
            bool valid = v == "transparent"
                || v.StartsWith('#')
                || v.StartsWith("rgb(",  StringComparison.OrdinalIgnoreCase)
                || v.StartsWith("rgba(", StringComparison.OrdinalIgnoreCase)
                || v.StartsWith("hsl(",  StringComparison.OrdinalIgnoreCase)
                || v.StartsWith("hsla(", StringComparison.OrdinalIgnoreCase)
                || v.StartsWith("var(",  StringComparison.OrdinalIgnoreCase)
                || Regex.IsMatch(v, @"^[a-zA-Z]+$"); // named CSS colour keyword

            if (!valid)
                Errors[cssVar] = "Must be a valid CSS colour (e.g. #0d6efd, rgba(…), or a colour name).";
        }
        else if (token.Category == "structure")
        {
            var v = value.Trim();
            bool valid = v.StartsWith("var(", StringComparison.OrdinalIgnoreCase)
                || Regex.IsMatch(v, @"^\d+(\.\d+)?(px|rem|em|%|vh|vw)$")
                || Regex.IsMatch(v, @"^\d+$");

            if (!valid)
                Errors[cssVar] = "Must be a valid CSS dimension (e.g. 6px, 0.875rem).";
        }
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    /// <summary>
    /// Resets EditBuffer to the current DB values (CurrentValue ?? DefaultValue)
    /// for every token in AllTokens. Discards any unsaved edits.
    /// </summary>
    private void RebuildBuffer()
    {
        EditBuffer = AllTokens.ToDictionary(
            t => t.CssVariable,
            t => t.CurrentValue ?? t.DefaultValue);
    }
}
