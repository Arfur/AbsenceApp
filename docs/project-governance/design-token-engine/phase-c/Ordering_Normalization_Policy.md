# Ordering and Normalization Policy

- Phase: C2
- Date: 2026-05-15
- Status: Approved

## Ordering Policy
1. Component order: deterministic canonical order.
2. Variant order: deterministic within component.
3. Property order: deterministic within variant/component scope.
4. Domain output assembly follows fixed domain ordering.

## Normalization Policy
1. Canonical key normalization must be applied before mapping.
2. Canonical value normalization must be applied before output intent classification.
3. Formatting normalization must be stable and repeatable.

## Deterministic Constraints
1. Equal inputs must produce equal ordered mapping outcomes.
2. Any ordering ambiguity is invalid and escalated.

## Gate Result
- C2 dependency closure: confirmed
