# Schema-Driven Navigation & Grouping Model

- Phase: D2
- Date: 2026-05-15
- Status: Approved

## Model Objective
Define deterministic navigation, grouping, and ordering semantics for universal editor rendering from schema metadata.

## Navigation Structure
1. Component selection context
2. Group selection context
3. Variant selection context
4. Property presentation context

## Grouping Rules
1. Groups are schema-declared and ordered deterministically.
2. Variant membership in groups is schema-declared.
3. Property visibility per variant/group is schema-declared.

## Ordering Rules
1. Group order is deterministic.
2. Variant order within group is deterministic.
3. Property order within variant scope is deterministic.

## Invalid Conditions
1. Undeclared group membership.
2. Duplicate ordering rank in same scope.
3. Orphaned variant/property references.

## Gate Result
- D2: Closed
