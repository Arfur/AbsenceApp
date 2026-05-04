---
doc_type: changelog
project_name: "AbsenceApp"
module: "V2"
version: "Unreleased"
file_name: "AbsenceApp_CHANGE_LOG.md"
created_date: "2026-04-16"
updated_date: "2026-04-30"
authority: "Michael"
active_phase: "Phase 1 — Foundations & Documentation"
phase_changelogs:
  "Phase 1": "AbsenceApp_CHANGELOG_Phase1.md"
  "Phase 2": "AbsenceApp_CHANGELOG_Phase2.md"
  "Phase 3": "AbsenceApp_CHANGELOG_Phase3.md"
ai_contract:
  read_rules: "Agents may read the master changelog and any phase changelogs for context."
  write_rules: "Changelogs are append-only. Agents may append entries only to the current phase file listed in active_phase; update the master changelog only with short summary lines. Agents MUST NOT edit or delete existing changelog entries or change historical dates/versions."
---
================================================================================
 File        : Absence_CHANGE_LOG.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
 Updated     : 2026-04-30
--------------------------------------------------------------------------------
 Purpose     :
   Master changelog that links to phase-specific changelogs and records
   high-level, append-only release notes. Use per-phase files for detailed
   task-level entries (e.g., CHANGELOG_PHASE1.md).
--------------------------------------------------------------------------------
 Active Phase : Phase 1 — Foundations & Documentation
--------------------------------------------------------------------------------
 Status values : Unreleased | Draft | Released
--------------------------------------------------------------------------------
 Changes     :
   - Unreleased (2026-04-30) Master changelog created; points to per-phase files.
   - 1.2.0 (2026-04-30) Tracker: normalized tables and machine rules; added AI rules.
   - 1.1.0 (2026-04-17) Tracker rewritten as pure task tracker; MySQL population tasks added.
   - 1.0.0 (2026-04-16) Initial creation.
--------------------------------------------------------------------------------
 Notes       :
   - This master file is append-only. Do not edit or remove existing entries.
   - For detailed, append-only entries about ongoing work, update the current
     phase file named `CHANGELOG_PHASE{N}.md` (e.g., CHANGELOG_PHASE1.md).
   - When a phase becomes complete, add a short summary line under its released
     version in this master changelog and update the Active Phase line.
   - Keep the boxed header for human readability and ensure the boxed header
     Updated line matches any YAML updated_date if present in the file.
================================================================================

## Phase changelogs

| Phase                                     | File                                | Status      | Last updated | Notes                                                      |
|-------------------------------------------|-------------------------------------|-------------|--------------|------------------------------------------------------------|
| Phase 1 — Foundations & Documentation     | AbsenceApp_CHANGELOG_Phase1.md      | **Active**  | 2026-04-30   | Phase 1 MySQL schema & seed changelog; append-only.        |
| Phase 2 — Design System & Core Components | AbsenceApp_CHANGELOG_Phase1.md      | **Active**  | 2026-04-30   | User Management module changelog; append-only for Phase 2. |
| Phase 3 — Layout & Templates              | CHANGELOG_PHASE3.md                 | Not started | -            | Create when Phase 3 work begins.                           |

