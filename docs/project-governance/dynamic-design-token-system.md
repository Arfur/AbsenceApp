# Dynamic Token System — Full Workflow Documentation

## Overview
The Dynamic Token System ensures that all design tokens used by the application are generated, resolved, and applied dynamically based on the `designtokens` database table, which acts as the single source of truth. The system supports both Global Tokens and Component Tokens, including inheritance, overrides, and real‑time regeneration when tokens are updated.

---

# 1. System Startup Workflow

## 1.1 Load Tokens from Database
When the application starts, it loads all rows from the `designtokens` table, including:
- Global Tokens (colors, spacing, radius, typography, etc.)
- Component Tokens (buttons, cards, forms, stats, etc.)
- Variant Tokens (primary, danger, outline-danger, soft-success, etc.)
- State Tokens (default, hover, active, disabled)

This table is the **single source of truth**.

---

## 1.2 Build In‑Memory Token Model
The system groups tokens into a structured model:

### Global Token Groups
- Colors
- Typography
- Spacing
- Radius
- Layout
- Shadows

### Component Token Groups
- Buttons
- Cards
- Forms
- Stats
- Navigation
- Tables

### Variant Groups
- primary
- secondary
- danger
- outline-danger
- soft-danger
- social-facebook
- etc.

### Property Groups
- background
- border
- radius
- padding
- font-size
- font-weight

---

## 1.3 Resolve Global Token References
If a component token references a global token, the system resolves it:

Example:

The system:
- Replaces the component token’s value with the global token’s value
- Marks the component token as **inherited**
- Stores the global reference so the UI can display:
  - “Using Global Token: --global-color-danger”

---

## 1.4 Generate Dynamic `components.json`
The system generates a complete `components.json` dynamically, containing:

### Component Groups
- Basic Buttons
- Outline Buttons
- Soft Buttons
- Icon Buttons
- Social Buttons
- Radius Buttons
- Size Buttons
- Action Buttons
- Button Groups
- Cards
- Forms
- Stats

### Variant Definitions
All variants defined in the database.

### Token Mappings
Resolved values for:
- background
- border
- radius
- spacing
- typography

### Inheritance Metadata
Each token includes:
- `value`
- `source` (global or local)
- `globalRef` (if inherited)

### Accordion Definitions
Each component group includes accordion sections for editing.

---

## 1.5 Write Dynamic JSON to LocalState
The generated `components.json` is written to:


This is the file the Blazor UI loads.

---

## 1.6 Notify UI of Regeneration
The system triggers a UI refresh event so Blazor reloads the new JSON and rehydrates all component states.

---

# 2. Real‑Time Regeneration Workflow

The system supports **instant regeneration** when tokens change.

---

## 2.1 User Updates a Component Token
Example:
User changes the background color of the **Outline Danger Button** on the Buttons page.

The UI writes the new value into the `designtokens` table:
- Either as a **local override**
- Or as a change to the **referenced global token**

---

## 2.2 Trigger Dynamic Regeneration
Immediately after saving, the system calls:


This triggers:
- Reloading tokens from the database
- Rebuilding the in‑memory model
- Re‑resolving global references
- Regenerating the full `components.json`
- Writing the updated file to LocalState

---

## 2.3 UI Reloads Updated Design System
The Blazor UI receives a “designSystemUpdated” event and reloads the dynamic JSON.

The Buttons page rehydrates:
- Variant mappings
- Token mappings
- Accordion sections
- Preview components

---

## 2.4 Component Updates Instantly
The Outline Danger Button now displays the updated background color.

The UI shows:
- Whether the token is inherited from a global token
- Whether the token is a local override
- The resolved value
- The original global reference (if applicable)

---

# 3. Global Token Override Logic

## 3.1 Component Uses a Global Token
If a component token references a global token:
- The component inherits the global value
- The UI displays the global reference
- The component token is marked as `source: "global"`

---

## 3.2 Component Overrides a Global Token
If the user overrides the component token:
- The component token becomes `source: "local"`
- The UI displays:
  - “Local override (was using --global-color-danger)”
- The global token remains unchanged

---

## 3.3 UI Display Example

| Token Name                     | Value      | Source | Global Reference            |
|--------------------------------|------------|--------|-----------------------------|
| --ds-btn-outline-danger-bg     | #FF0000    | local  | --global-color-danger       |
| --ds-btn-outline-danger-border | var(--global-color-danger) | global | --global-color-danger |

---

# 4. Summary

The Dynamic Token System ensures:
- The database is the single source of truth
- All tokens are resolved dynamically
- Global tokens can override component tokens
- Components can override global tokens locally
- The UI always shows where values come from
- Regeneration happens instantly after any change
- The UI updates immediately without restarting the app

This architecture provides a fully dynamic, transparent, and flexible design system.

## 5. Json Schems (dynamic design system)
{
  "globalTokens": {
    "colors": {
      "global-color-primary": {
        "value": "#0052CC",
        "category": "color"
      },
      "global-color-danger": {
        "value": "#FF3B30",
        "category": "color"
      }
    },
    "radius": {
      "global-radius-sm": {
        "value": "4px",
        "category": "radius"
      },
      "global-radius-lg": {
        "value": "12px",
        "category": "radius"
      }
    },
    "spacing": {
      "global-space-2": {
        "value": "0.5rem",
        "category": "spacing"
      }
    }
  },
  "components": {
    "button": {
      "groups": {
        "basic": {
          "variants": {
            "primary": {
              "tokens": {
                "background": {
                  "value": "var(--global-color-primary)",
                  "source": "global",
                  "globalRef": "global-color-primary",
                  "cssVar": "--ds-btn-basic-primary-bg"
                },
                "radius": {
                  "value": "var(--global-radius-sm)",
                  "source": "global",
                  "globalRef": "global-radius-sm",
                  "cssVar": "--ds-btn-basic-primary-radius"
                }
              }
            },
            "danger": {
              "tokens": {
                "background": {
                  "value": "var(--global-color-danger)",
                  "source": "global",
                  "globalRef": "global-color-danger",
                  "cssVar": "--ds-btn-basic-danger-bg"
                }
              }
            }
          }
        },
        "outline": {
          "variants": {
            "danger": {
              "tokens": {
                "background": {
                  "value": "#FF0000",
                  "source": "local",
                  "globalRef": "global-color-danger",
                  "cssVar": "--ds-btn-outline-danger-bg"
                },
                "border": {
                  "value": "var(--global-color-danger)",
                  "source": "global",
                  "globalRef": "global-color-danger",
                  "cssVar": "--ds-btn-outline-danger-border"
                }
              }
            }
          }
        }
      }
    },
    "card": {
      "groups": {
        "basic": {
          "variants": {
            "default": {
              "tokens": {
                "background": {
                  "value": "#FFFFFF",
                  "source": "local",
                  "cssVar": "--ds-card-basic-bg"
                }
              }
            }
          }
        }
      }
    }
  },
  "editor": {
    "groups": {
      "button-outline-danger": {
        "title": "Outline Danger Button",
        "component": "button",
        "groupKey": "outline",
        "variantKey": "danger",
        "tokenMappings": [
          {
            "tokenName": "--ds-btn-outline-danger-bg",
            "property": "background",
            "source": "local",
            "globalRef": "global-color-danger"
          },
          {
            "tokenName": "--ds-btn-outline-danger-border",
            "property": "border",
            "source": "global",
            "globalRef": "global-color-danger"
          }
        ]
      }
    }
  }
}

## 6. C# regeneration pipeline (dynamic tokens → components.json):
public class DesignSystemRegenerator
{
    private readonly ITokenRepository _tokenRepository;
    private readonly string _componentsJsonPath;

    public DesignSystemRegenerator(ITokenRepository tokenRepository, string componentsJsonPath)
    {
        _tokenRepository = tokenRepository;
        _componentsJsonPath = componentsJsonPath;
    }

    public async Task RegenerateDesignSystemAsync()
    {
        // 1. Load all tokens from DB
        var tokens = await _tokenRepository.GetAllTokensAsync();

        // 2. Build in-memory model
        var globalTokens = BuildGlobalTokens(tokens);
        var componentTokens = BuildComponentTokens(tokens);

        // 3. Resolve global references for component tokens
        ResolveGlobalReferences(componentTokens, globalTokens);

        // 4. Build dynamic JSON object
        var root = new
        {
            globalTokens = globalTokens,
            components = componentTokens,
            editor = BuildEditorGroups(componentTokens)
        };

        // 5. Serialize to JSON
        var json = JsonSerializer.Serialize(root, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        // 6. Write to LocalState/components.json
        Directory.CreateDirectory(Path.GetDirectoryName(_componentsJsonPath)!);
        await File.WriteAllTextAsync(_componentsJsonPath, json);

        // 7. Notify UI (e.g., via event, messaging, or interop)
        DesignSystemEvents.RaiseDesignSystemUpdated();
    }

    private object BuildGlobalTokens(IEnumerable<DesignToken> tokens)
    {
        // Group by category (colors, radius, spacing, etc.)
        // Return an anonymous object matching the JSON schema
        // Example: { colors = { ... }, radius = { ... } }
        // Implementation depends on your DesignToken model
        throw new NotImplementedException();
    }

    private object BuildComponentTokens(IEnumerable<DesignToken> tokens)
    {
        // Group by component (button, card, form, etc.)
        // Then by group (basic, outline, soft, etc.)
        // Then by variant (primary, danger, etc.)
        // Then by property (background, border, etc.)
        // Return an anonymous object matching the JSON schema
        throw new NotImplementedException();
    }

    private void ResolveGlobalReferences(object componentTokens, object globalTokens)
    {
        // Walk componentTokens structure:
        // - If a token value is a reference to a global token
        //   (e.g., "var(--global-color-danger)")
        // - Set source = "global"
        // - Set globalRef = "global-color-danger"
        // - Optionally resolve the actual value from globalTokens
        // Implementation depends on your internal model types
        throw new NotImplementedException();
    }

    private object BuildEditorGroups(object componentTokens)
    {
        // Build editor metadata:
        // - group titles
        // - component/group/variant keys
        // - tokenMappings with source/globalRef
        // Return an anonymous object matching the JSON schema
        throw new NotImplementedException();
    }
}

public static class DesignSystemEvents
{
    public static event Action? DesignSystemUpdated;

    public static void RaiseDesignSystemUpdated()
        => DesignSystemUpdated?.Invoke();
}


## 7. Blazor UI logic (showing global vs local, reacting to updates)
7.1 Component code-behind (C#)
public partial class ButtonTokenEditor : ComponentBase, IDisposable
{
    [Inject] public IDesignSystemLoader DesignSystemLoader { get; set; } = default!;

    private EditorGroupModel? _outlineDangerGroup;

    protected override async Task OnInitializedAsync()
    {
        DesignSystemEvents.DesignSystemUpdated += OnDesignSystemUpdated;
        await LoadDesignSystemAsync();
    }

    private async Task LoadDesignSystemAsync()
    {
        var designSystem = await DesignSystemLoader.LoadAsync();
        _outlineDangerGroup = designSystem.EditorGroups
            .FirstOrDefault(g => g.GroupKey == "button-outline-danger");
        StateHasChanged();
    }

    private async void OnDesignSystemUpdated()
    {
        await LoadDesignSystemAsync();
    }

    public void Dispose()
    {
        DesignSystemEvents.DesignSystemUpdated -= OnDesignSystemUpdated;
    }
}

public class EditorGroupModel
{
    public string Title { get; set; } = "";
    public string Component { get; set; } = "";
    public string GroupKey { get; set; } = "";
    public string VariantKey { get; set; } = "";
    public List<TokenMappingModel> TokenMappings { get; set; } = new();
}

public class TokenMappingModel
{
    public string TokenName { get; set; } = "";
    public string Property { get; set; } = "";
    public string Value { get; set; } = "";
    public string Source { get; set; } = ""; // "global" or "local"
    public string? GlobalRef { get; set; }
}


3.2 Razor UI (show inherited vs local tokens)
@inherits ButtonTokenEditor

@if (_outlineDangerGroup is null)
{
    <p>Loading Outline Danger Button tokens...</p>
}
else
{
    <h3>@_outlineDangerGroup.Title (@_outlineDangerGroup.Component)</h3>

    <table class="table table-sm">
        <thead>
            <tr>
                <th>Token</th>
                <th>Property</th>
                <th>Value</th>
                <th>Source</th>
                <th>Global Reference</th>
            </tr>
        </thead>
        <tbody>
        @foreach (var token in _outlineDangerGroup.TokenMappings)
        {
            <tr>
                <td>@token.TokenName</td>
                <td>@token.Property</td>
                <td>@token.Value</td>
                <td>
                    @if (token.Source == "global")
                    {
                        <span class="badge bg-info">Global</span>
                    }
                    else
                    {
                        <span class="badge bg-warning text-dark">Local override</span>
                    }
                </td>
                <td>
                    @if (!string.IsNullOrEmpty(token.GlobalRef))
                    {
                        <code>@token.GlobalRef</code>
                    }
                    else
                    {
                        <span class="text-muted">None</span>
                    }
                </td>
            </tr>
        }
        </tbody>
    </table>
}
