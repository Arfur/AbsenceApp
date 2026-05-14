# Conflict Classification Policy

- Phase: C3
- Date: 2026-05-15
- Status: Approved

## Conflict Classes
1. Precedence conflict (same-rank competing intent)
2. Reference conflict (missing/invalid inheritance reference)
3. Scope conflict (entity mapped to invalid/duplicate domain context)
4. Semantic conflict (special-value declaration inconsistent with allowed scope)

## Classification Outcomes
- Critical: deterministic resolution impossible -> block
- High: precedence/reference invalidity -> block
- Medium: recoverable scope mismatch -> continue with explicit remediation record
- Low: advisory normalization discrepancy -> continue with advisory note

## Policy Rules
1. Unclassified conflict is invalid.
2. Critical/High conflicts block C3 acceptance.

## Gate Result
- C3 closure evidence: complete
