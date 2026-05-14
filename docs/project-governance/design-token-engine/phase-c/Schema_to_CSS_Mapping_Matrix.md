# Schema-to-CSS Mapping Matrix

- Phase: C2
- Date: 2026-05-15
- Status: Approved

| Schema Entity/Relation | CSS Domain | Mapping Intent | Determinism Rule |
|---|---|---|---|
| Component | Component-Rule | Defines top-level rule grouping context | Stable component ordering required |
| Variant | Component-Rule | Defines variant-scoped rule intent | Stable variant ordering within component |
| Property | Token-Variable + Component-Rule | Defines variable intent and declaration intent | Stable property ordering within scope |
| Default Value | Token-Variable | Defines fallback/default semantic intent | Canonical value normalization required |
| Inheritance Reference | Component-Rule | Defines inherited declaration resolution intent | Precedence table must resolve conflicts deterministically |
| Special-Value Semantic | Both domains | Declares non-standard interpretation behavior | Must be schema-declared and validated |

## Mapping Rules
1. Mapping is schema-driven only.
2. Unmapped schema entities are invalid for generation scope.
3. Non-schema mapping shortcuts are disallowed.

## Gate Result
- C2: Closed
