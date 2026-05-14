# Round-Trip Consistency Contract

- Phase: E3
- Date: 2026-05-15
- Status: Approved

## Contract Objective
Define consistency expectations for synthesis -> parse round-trips under schema-governed behavior.

## Consistency Conditions
1. Round-trip preserves intended semantic meaning.
2. Deterministic ordering/normalization differences that do not alter meaning are permitted when declared.
3. Non-reversible conditions must be explicitly declared and classified.

## Tolerance Policy
1. Allowed tolerance: representation-level normalization differences only.
2. Disallowed tolerance: semantic drift in token intent.

## Failure Conditions
1. Semantic mismatch between source intent and round-trip output intent.
2. Undeclared non-reversible behavior.

## Gate Result
- E3: Closed
