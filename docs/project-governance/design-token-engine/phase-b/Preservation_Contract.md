# Preservation Contract

- Phase: B3
- Date: 2026-05-15
- Status: Approved

## Preservation Principle
Runtime/user override intent is preserved across reconciliation runs.

## Contract Terms
1. Structural reconciliation must not destructively overwrite runtime override values.
2. Default-intent alignment updates are confined to schema-managed default fields.
3. Runtime override clearing is out of scope for structural reconciliation and requires explicit lifecycle-governed action.

## Invariants
1. User intent continuity invariant.
2. Non-destructive synchronization invariant.
3. Deterministic replay invariant under unchanged inputs.

## Contract Violation Classification
- Any destructive overwrite of runtime override intent is a critical policy breach.

## Gate Result
- B3: Closed
