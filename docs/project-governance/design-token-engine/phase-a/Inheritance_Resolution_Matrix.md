# Inheritance Resolution Matrix

- Phase: A4
- Date: 2026-05-14
- Status: Approved

## Resolution Order
1. Local variant explicit definition
2. Direct inherited variant definition
3. Transitive inherited definitions (nearest ancestor first)
4. Component-level default definition
5. Schema-defined terminal default

## Override Precedence
- Local explicit value always overrides inherited value.
- Inherited value is used only when no local explicit value exists.

## Validity Matrix

| Condition | Classification | Outcome |
|---|---|---|
| Valid parent exists, no cycle | Valid | Resolve per order |
| Missing parent reference | Invalid | Fail validation |
| Circular inheritance detected | Invalid | Fail validation |
| Parent exists but property absent | Valid | Continue to next resolution layer |
| Multiple parent candidates | Invalid | Fail validation |

## Deterministic Constraints
1. Exactly zero or one parent per variant in Phase A.
2. Multiple inheritance is not permitted in Phase A.
3. Any ambiguity is invalid and blocks acceptance.

## Gate Result
- A4: Closed
