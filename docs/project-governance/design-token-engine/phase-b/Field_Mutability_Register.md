# Field Mutability Register

- Phase: B3
- Date: 2026-05-15
- Status: Approved

| Field Class | Mutability | Allowed Change Driver |
|---|---|---|
| Identity fields | Immutable | Governance-managed migration only |
| Structural metadata | Controlled mutable | Approved schema revision |
| Default intent fields | Controlled mutable | Reconciliation under schema authority |
| Runtime override value | Protected mutable | User/runtime operations, not structural sync |
| Lifecycle status | Controlled mutable | Lifecycle governance policy |
| Evidence metadata | Append-only mutable | Reconciliation process |

## Policy Notes
1. Mutability is governed by authority class.
2. Any update outside allowed change driver is invalid.

## Gate Result
- B3 dependency closure: Confirmed
