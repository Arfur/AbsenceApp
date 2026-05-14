# CSS Output Domain Contract

- Phase: C1
- Mode: EXECUTION MODE
- Date: 2026-05-15
- Status: Approved

## Domains
1. Token-Variable Output Domain
2. Component-Rule Output Domain

## Domain Boundaries
1. Token-Variable Domain defines variable-level semantic outputs derived from schema token structure.
2. Component-Rule Domain defines selector/declaration intent derived from schema component/variant/property relationships.
3. Domain overlap is not permitted.

## Deterministic Constraints
1. Every schema-mapped output belongs to exactly one domain.
2. Output classification must be reproducible under unchanged inputs.

## Gate Result
- C1: Closed
- Blocking ambiguity: None
