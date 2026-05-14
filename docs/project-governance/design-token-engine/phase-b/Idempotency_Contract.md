# Idempotency Contract

- Phase: B4
- Date: 2026-05-15
- Status: Approved

## Idempotency Definition
Given unchanged schema intent and unchanged persisted inputs, repeated reconciliation runs must converge to the same stable state and decision summary.

## Contract Conditions
1. State classification outcomes are repeatable.
2. Action assignments are repeatable.
3. No additional structural changes are introduced after convergence.

## Non-Idempotent Indicators
1. Repeated drift actions under unchanged inputs.
2. Divergent decision counts under unchanged inputs.
3. Inconsistent state assignments across repeated runs.

## Gate Result
- B4 closure evidence: complete
