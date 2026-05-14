# Cutover Gate Criteria

- Phase: F3
- Date: 2026-05-15
- Status: Approved

## Objective
Define objective pass/fail criteria for migration cutover progression.

## Mandatory Gate Checks
1. Baseline invariant compliance check
2. Deterministic behavior evidence check
3. Audit completeness check
4. Conflict severity threshold check
5. Rollback readiness check

## Gate Outcomes
- Pass: all mandatory checks satisfied
- Fail: one or more checks unsatisfied

## Blocking Conditions
1. Critical or high-severity unresolved conflicts
2. Missing mandatory evidence
3. Invariant violation

## Gate Result
- F3: Closed
