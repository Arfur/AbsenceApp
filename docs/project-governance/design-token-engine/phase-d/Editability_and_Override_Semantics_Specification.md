# Editability & Override Semantics Specification

- Phase: D3
- Date: 2026-05-15
- Status: Approved

## Semantic Classes
1. Editable explicit value
2. Inherited value
3. Static/read-only value
4. Derived/computed value

## Editability Rules
1. Editability class is schema-declared and deterministic.
2. Inherited class may be overridden only when schema policy permits.
3. Static/read-only class is non-editable.
4. Derived/computed class follows declared derivation policy.

## Override Rules
1. Override precedence follows approved inheritance/precedence governance.
2. Override intent must be explicitly represented in editor state semantics.
3. Illegal override attempts are rejected and classified.

## User-Visible Semantics
1. Each class must have explicit status representation.
2. Status representation must be consistent across components.

## Gate Result
- D3: Closed
