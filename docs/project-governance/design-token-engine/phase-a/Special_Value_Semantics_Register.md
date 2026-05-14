# Special-Value Semantics Register

- Phase: A5
- Date: 2026-05-14
- Status: Approved

## Purpose
Defines schema-declared special-value semantics so behavior is data-defined and deterministic.

## Registered Semantic Classes

| Semantic Key | Scope | Interpretation Intent | Validation Class |
|---|---|---|---|
| `semantic.transparent-surface` | property/variant | Represents explicit transparent treatment where applicable | Strict |
| `semantic.inherit-explicit` | property | Explicit inheritance from resolved parent value | Strict |
| `semantic.none-explicit` | property | Explicit absence semantic where allowed by schema | Strict |
| `semantic.fallback-only` | property | Force fallback behavior when primary value is omitted | Strict |

## Policy Rules
1. A special-value behavior is valid only when declared via semantic key.
2. Undeclared special behavior is invalid.
3. Semantic keys are versioned governance artifacts.
4. Semantic interpretation must be consistent across synthesis, parsing, and editor state intent.

## Validation Outcomes
- Declared and allowed semantic: valid
- Declared but disallowed for scope: invalid
- Undeclared semantic usage: invalid

## Gate Result
- A5: Closed
