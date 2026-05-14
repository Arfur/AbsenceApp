# Failure Severity Matrix

- Phase: B5
- Date: 2026-05-15
- Status: Approved

| Severity | Classification | Required Outcome |
|---|---|---|
| Critical | Policy breach / destructive behavior risk | Stop + escalate |
| High | Determinism compromised / ambiguous state | Stop + escalate |
| Medium | Partial evidence or recoverable classification issue | Continue with explicit warning and remediation record |
| Low | Non-blocking advisory issue | Continue with advisory logging |

## Severity Rules
1. Critical and High severities block successful run closure.
2. Medium and Low severities require explicit evidence annotation.
3. Silent failure is prohibited for all severities.

## Gate Result
- B5 closure dependency: complete
