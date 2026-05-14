# Schema Authority Statement

- Phase: A
- Date: 2026-05-14
- Status: Approved

## Authority Declaration
The token schema is the single source of truth for:
1. Component taxonomy
2. Variant taxonomy
3. Property definitions
4. Default value definitions
5. Inheritance declarations and precedence intent
6. Special-value semantics intent
7. Editor grouping/order metadata

## Non-Authority Boundaries
The schema does not directly authorize:
1. Runtime override values currently persisted by users
2. Emergency operational overrides outside schema-governed structural metadata

## Policy Outcomes
1. Any non-schema structural source is non-authoritative.
2. Conflicts are resolved in favor of schema structure/intent.
3. Runtime/user override preservation is governed in reconciliation policy (Phase B), not by structural authority changes.

## Gate Result
- A1 dependency closure: Confirmed
- A2 enablement: Confirmed
