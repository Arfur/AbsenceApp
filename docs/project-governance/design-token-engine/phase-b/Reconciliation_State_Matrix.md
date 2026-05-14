# Reconciliation State Matrix

- Phase: B2
- Date: 2026-05-15
- Status: Approved

| State | Definition | Deterministic Action |
|---|---|---|
| Missing | Schema identity exists; persisted record absent | Insert |
| Matching | Schema and persisted schema-managed fields aligned | No-op |
| Default-Drift | Persisted schema-managed default differs from approved schema intent | Controlled update |
| Deprecated/Inactive | Persisted record exists but schema lifecycle marks non-active | Lifecycle policy action |
| Unknown/Unmapped | Persisted record has no schema identity mapping | Defer + escalate |

## Deterministic Constraints
1. Exactly one state classification per record per run.
2. Ambiguous classification is invalid and escalated.
3. Action class is fixed by state.

## Gate Result
- B2: Closed
