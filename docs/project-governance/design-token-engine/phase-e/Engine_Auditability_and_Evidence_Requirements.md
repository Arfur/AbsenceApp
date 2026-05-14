# Engine Auditability & Evidence Requirements

- Phase: E5
- Date: 2026-05-15
- Status: Approved

## Mandatory Trace Fields
1. Operation identifier
2. Schema version linkage
3. Input scope summary
4. Resolution decisions summary
5. Outcome class totals
6. Rejected/ignored item listing
7. Severity classifications
8. Timestamp envelope

## Evidence Rules
1. Every synthesis and parsing operation must emit complete evidence metadata.
2. Missing mandatory evidence fields invalidate operation acceptance.
3. Evidence schema must be stable and deterministic.

## Severity Classes
1. Critical
2. High
3. Medium
4. Low

## Compliance
1. Silent failures are prohibited.
2. Unclassified outcomes are invalid.

## Gate Result
- E5: Closed
