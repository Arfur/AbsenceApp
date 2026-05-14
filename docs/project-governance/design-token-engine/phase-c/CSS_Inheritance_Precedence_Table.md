# CSS Inheritance and Precedence Table

- Phase: C3
- Date: 2026-05-15
- Status: Approved

| Resolution Layer | Precedence Rank | Rule |
|---|---:|---|
| Local explicit variant/property intent | 1 | Highest precedence |
| Direct inherited variant/property intent | 2 | Used when no local explicit intent |
| Transitive inherited intent | 3 | Nearest ancestor first |
| Component-level default intent | 4 | Used when variant-level intent unresolved |
| Terminal schema fallback intent | 5 | Final deterministic fallback |

## Precedence Rules
1. Higher-rank layer always wins over lower-rank layer.
2. Equal-rank conflict is invalid and escalated.
3. Missing inheritance references fail validation.

## Gate Result
- C3: Closed
