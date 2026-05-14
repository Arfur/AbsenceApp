# Field Authority Map

- Phase: B1
- Date: 2026-05-15
- Status: Approved

| Field Class | Authority | Update Eligibility via Reconciliation | Notes |
|---|---|---|---|
| Identity fields | Schema | Controlled | Must remain deterministic and unique |
| Structural metadata | Schema | Controlled | Managed through governance-approved schema revisions |
| Default intent fields | Schema | Controlled | Updated only under reconciliation policy |
| Runtime override value | Persistence/User | Preserved | Never destructively overwritten by structural sync |
| Lifecycle status flags | Policy-governed | Controlled | Must follow lifecycle governance addendum |
| Run evidence metadata | Reconciliation process | Append-only | Used for audit and traceability |

## Authority Rules
1. Schema authority cannot erase runtime override intent.
2. Runtime override authority cannot redefine schema structure.
3. Conflicts resolve by authority class and policy gate.

## Gate Result
- B1 dependency closure: Confirmed
