# Identity and Naming Rulebook

- Phase: A3
- Date: 2026-05-14
- Status: Approved

## Identity Keys
1. `componentKey` — globally unique
2. `variantKey` — unique within `componentKey`
3. `propertyKey` — unique within its declared scope
4. Composite identity for bindings: `(componentKey, variantKey, propertyKey)`

## Normalization Policy
1. Keys are lowercase canonical forms.
2. Word separators use hyphen (`-`) only.
3. No whitespace, punctuation, or mixed separator styles.
4. Canonical comparison is case-insensitive after normalization.

## Uniqueness Constraints
1. Duplicate canonical keys are invalid.
2. Alias keys are not permitted in Phase A.
3. Identity collisions fail validation and are not auto-corrected.

## Naming Governance
1. Display labels may vary without affecting identity.
2. Identity keys are immutable once approved, except through explicit versioned governance change.
3. Renames require compatibility classification and migration notice.

## Gate Result
- A3: Closed
