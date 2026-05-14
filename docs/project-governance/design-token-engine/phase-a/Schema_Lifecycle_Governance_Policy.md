# Schema Lifecycle Governance Policy

- Phase: A6
- Date: 2026-05-14
- Status: Approved

## Versioning Policy
1. Every schema change requires a schema version update.
2. Compatibility classification is mandatory: compatible, conditionally compatible, breaking.
3. Version transitions must include rationale and decision reference.

## Approval Policy
1. Every schema revision requires named approver and approval timestamp.
2. Revisions without governance metadata are invalid for progression.
3. Superseded decisions remain immutable historical records.

## Change Control Requirements
1. Required metadata for each revision:
   - schemaVersion
   - compatibilityClass
   - approver
   - approvedAt
2. Optional but recommended metadata:
   - rationale
   - related issue/reference

## Compliance Gates
1. No phase progression without complete governance metadata.
2. Failed governance checks block downstream execution.
3. Historical records are append-only.

## Gate Result
- A6: Closed
