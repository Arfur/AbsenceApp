# Rollback & Contingency Policy

- Phase: F4
- Date: 2026-05-15
- Status: Approved

## Objective
Define deterministic rollback triggers, fallback outcomes, and contingency controls.

## Rollback Triggers
1. Invariant breach detected
2. Failed cutover gate with blocking severity
3. Incomplete or invalid evidence set
4. Determinism failure in governed outputs

## Rollback Rules
1. Trigger classification determines rollback severity path.
2. Rollback execution must preserve audit traceability.
3. Rollback completion requires explicit success evidence.

## Contingency Controls
1. Stage freeze on unresolved critical/high issues
2. Mandatory remediation record before re-attempt
3. Re-entry requires full gate re-evaluation

## Gate Result
- F4: Closed
