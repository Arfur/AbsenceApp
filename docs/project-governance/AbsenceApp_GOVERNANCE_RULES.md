================================================================================
 File        : AbsenceApp_GOVERNANCE_RULES.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
--------------------------------------------------------------------------------
 Purpose     :
   This file defines the three governance documents that MUST be continually 
   updated throughout the AbsenceApp V2 project. It explains WHAT must be kept 
   updated, WHY these files are critical, and HOW the AI must update them.
--------------------------------------------------------------------------------
 Notes       :
   - This file must be loaded into every new AI session before any work begins.
   - The AI must follow these rules with zero deviation, zero assumptions, and 
     zero invention of data.
================================================================================

# 1. Overview

AbsenceApp V2 uses three authoritative governance documents:

1. **AbsenceApp_PRD.md**  
2. **AbsenceApp_DECISIONS.md**  
3. **AbsenceApp_PROGRESS.md**

These files define:
- What the system must do  
- Why the system behaves the way it does  
- Where the project is right now  

They must remain accurate at all times.

---

# 2. File Responsibilities

## 2.1 AbsenceApp_PRD.md (Product Requirements Document)

**Purpose:**  
Defines the required behaviour of the system, including UI, UX, architecture, 
permissions, data flow, and non-functional requirements.

**When to update:**  
- When system behaviour changes  
- When new tables affect behaviour  
- When permission logic changes  
- When architecture changes  
- When new features are added  
- When existing features change  
- When the menu system or RBAC model changes  

**Rules:**  
- No assumptions  
- No invented behaviour  
- Only update when behaviour actually changes  
- Must remain the single source of truth for “how the app works”

---

## 2.2 AbsenceApp_DECISIONS.md (Decision Log)

**Purpose:**  
Records every architectural, schema, naming, UI/UX, coding, and process decision.

**When to update:**  
- ONLY when a new decision is made  
- When a decision is superseded  
- When a new table changes system behaviour  
- When a new rule is introduced  
- When a defect becomes a formal decision  

**Rules:**  
- Never modify past decisions  
- Add new decisions at the bottom  
- Follow the exact DEC-XXX format  
- No assumptions or inferred decisions  
- Every decision must have a rationale and impacted docs

---

## 2.3 AbsenceApp_PROGRESS.md (Project Progress Tracker)

**Purpose:**  
Tracks the real-time status of the project, including phases, tasks, migration 
steps, defects, and current work.

**When to update:**  
- When a task starts  
- When a task completes  
- When a migration step completes  
- When new tables are created  
- When real data population begins  
- When defects are found or resolved  
- When a phase gate is passed  
- When the active track changes  

**Rules:**  
- Must always reflect the real project state  
- Must never fall behind actual progress  
- Must use the existing table formats  
- Must remain strictly factual

---

# 3. AI Behaviour Rules

The AI must:

1. **Always keep these three files accurate.**
2. **Never update DECISIONS.md unless a real decision has been made.**
3. **Always update PROGRESS.md when work changes.**
4. **Update PRD.md only when behaviour changes.**
5. **Ask for clarification if unsure.**
6. **Never invent data, behaviour, or decisions.**
7. **Never modify historical entries.**
8. **Always append new entries in the correct format.**
9. **Always maintain consistency across all three files.**

---

# 4. Why These Files Matter

These three documents form the governance backbone of AbsenceApp V2:

- **PRD** ensures the system behaves correctly.  
- **DECISIONS** ensures architectural integrity and traceability.  
- **PROGRESS** ensures the project stays aligned with reality.  

If any of these files become inaccurate, the project risks:
- Drift  
- Conflicting behaviour  
- Incorrect AI-generated code  
- Incorrect assumptions  
- Broken architecture  
- Lost decisions  
- Regression in V2  

Therefore, these files must remain **continuously updated**, **accurate**, and 
**synchronised**.

---

# 5. Summary

The AI must always maintain:

- **PRD = What the system must do**  
- **DECISIONS = Why the system works this way**  
- **PROGRESS = Where the project is right now**

These three files are mandatory, authoritative, and must never fall out of sync 
with the real project.

================================================================================
