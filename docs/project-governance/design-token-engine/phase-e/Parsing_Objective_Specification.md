# Parsing Objective Specification

- Phase: E2
- Date: 2026-05-15
- Status: Approved

## Objective
Define deterministic parsing behavior that interprets edited representation back into schema-constrained token update intent.

## Scope
1. Schema-constrained parsing only
2. Explicit accepted/ignored/rejected outcome classes
3. Deterministic mapping from parsed input to update intent

## Outcome Classes
1. Accepted
2. Rejected
3. Accepted with advisory
4. Ignored by policy

## Deterministic Rules
1. Parsing decisions must be reproducible under unchanged inputs and schema.
2. Non-schema shortcuts are disallowed.
3. Missing classification is invalid.

## Gate Result
- E2: Closed
