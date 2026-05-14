# CSS Generation Audit Specification

- Phase: C5
- Date: 2026-05-15
- Status: Approved

## Mandatory Audit Fields
1. Generation run identifier
2. Schema version linkage
3. Domain scope
4. Mapping totals
5. Conflict totals by class/severity
6. Output decision status
7. Timestamp envelope

## Audit Rules
1. All generation runs must emit complete audit metadata.
2. Missing mandatory fields invalidate run acceptance.
3. Audit format is stable and deterministic.

## Gate Result
- C5: Closed
