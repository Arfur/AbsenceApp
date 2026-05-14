# State Classification Rules

- Phase: B2
- Date: 2026-05-15
- Status: Approved

## Classification Order
1. Identity match evaluation
2. Lifecycle status evaluation
3. Schema-managed field parity evaluation
4. Runtime override preservation check
5. Final state assignment

## Rule Set
1. Identity mismatch with no schema mapping => Unknown/Unmapped.
2. Identity match + schema-managed parity => Matching.
3. Identity match + schema-managed default mismatch => Default-Drift.
4. Identity match + schema lifecycle non-active => Deprecated/Inactive.
5. Identity exists in schema but absent in persistence => Missing.

## Invalid Conditions
1. Multiple state matches in same run.
2. Missing identity classification evidence.
3. Action assignment not defined in state matrix.

## Gate Result
- B2 closure evidence: complete
