# Phase A Scope Record

- Phase: A (Schema Format)
- Mode: EXECUTION MODE
- Date: 2026-05-14
- Status: Approved
- Decision authority: Pre-approved by project owner

## Scope In
1. `btn` component domain
2. `card` component domain
3. Schema governance for:
   - component/variant/property structure
   - default values
   - inheritance references
   - special-value semantics metadata
   - editor grouping metadata

## Scope Out (Phase A)
1. `tab` component domain
2. All other UIKit domains not explicitly listed in Scope In
3. Implementation code changes, runtime wiring, and CSS runtime behavior changes

## Fixed Constraints
1. Schema is authoritative for structure and behavioral intent metadata.
2. Runtime behavior must not be inferred from hardcoded variant naming conventions.
3. Special behavior must be declared in schema semantics.
4. This phase delivers governance artifacts only.

## Gate Result
- A1: Closed
- Blocking ambiguity: None
