# Validation & Feedback Standards

- Phase: D5
- Date: 2026-05-15
- Status: Approved

## Validation Layers
1. Structural validation
2. Semantic validation
3. Transition validation
4. Policy validation

## Validation Outcomes
1. Accepted
2. Rejected
3. Accepted with advisory

## Feedback Standards
1. Every rejected action must return explicit reason class.
2. Every accepted-with-advisory action must return advisory class.
3. Feedback messages must be deterministic for same validation condition.

## Severity Classes
1. Critical (blocking)
2. High (blocking)
3. Medium (non-blocking with remediation)
4. Low (advisory)

## Compliance Rules
1. Silent failure is prohibited.
2. Missing feedback classification is invalid.

## Gate Result
- D5: Closed
