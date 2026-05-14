# Reconciliation Trigger Policy

- Phase: B4
- Date: 2026-05-15
- Status: Approved

## Trigger Policy
Reconciliation runs are policy-triggered and must execute under deterministic preconditions.

## Approved Trigger Classes
1. Controlled startup trigger
2. Controlled governance-triggered reconciliation event
3. Controlled maintenance reconciliation window

## Trigger Constraints
1. No ad hoc unmanaged trigger execution.
2. Trigger invocation must include run context metadata.
3. Trigger attempts without context metadata are invalid.

## Gate Result
- B4: Closed
