================================================================================
 File        : AbsenceApp_MYSQL_SCHEMA_OVERVIEW.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
 Updated     : 2026-04-16
--------------------------------------------------------------------------------
 Purpose     :
   Provides a high-level architectural overview of the MySQL AbsenceApp schema.
   This file is designed for onboarding, architectural review, and quick
   reference without needing to read the full schema documentation.

   This file exists to:
     - Summarise the database structure at a domain level
     - Provide a fast orientation for new developers and AI assistants
     - Support governance and architectural decision-making
--------------------------------------------------------------------------------
 Notes       :
   - This is a summary, not a replacement for the full schema documentation.
   - Domains follow AbsenceApp_TABLE_GROUPS.md.
================================================================================


# Schema Overview (Domain-Level Summary)

## DOMAIN 1 — SYSTEM & INFRASTRUCTURE
Core system configuration, feature toggles, menu structure, and system events.
Tables:
- __efmigrationshistory
- globalconfig
- feature
- menuitems
- menuitemsglobalconfig
- systemevents

## DOMAIN 2 — STAFF
All staff records, job metadata, assignments, devices, external accounts, and
audit trails.
Tables:
- staff, staffdepartments, staffjobtitles, staffjobgroups
- stafflocations, staffassignments, staffschools
- staffdevices, staffdeviceaudit
- staffexternalaccounts, staffexternalaccountaudit
- staffresponsibilities, responsibilities
- staffschoolaudit

## DOMAIN 3 — STUDENTS
Student core data, contacts, flags, medical info, absences, and year/house data.
Tables:
- students, studentcontacts, studentflags, studentmedical
- studentabsences, studentabsenceaudit
- yeargroups, houses, phases

## DOMAIN 4 — ATTENDANCE
Attendance registers, marks, and all absence lookup tables.
Tables:
- attendance, attendanceregisters, attendancemarks
- absencecategories, absencemethods, absencereasons
- absencesources, absencestatuses, absencetypes

## DOMAIN 5 — CLASSES & GROUPS
Class definitions, membership, and year group assignments.
Tables:
- classes, classmember, classyeargroupassignments

## DOMAIN 6 — DEVICES
Device types and staff device assignments.
Tables:
- devicetypes, staffdevices, staffdeviceaudit

## DOMAIN 7 — EXTERNAL ACCOUNTS & INTEGRATIONS
External system definitions and staff account links.
Tables:
- externalsystems, staffexternalaccounts
- staffexternalaccountaudit, accountverificationevents

## DOMAIN 8 — USERS, ROLES, PERMISSIONS, AUDIT, MESSAGING & MISC
User accounts, roles, permissions, audit logs, and messaging.
Tables:
- users, userprofiles
- roles, roletypes, userrole
- rolefeature, rolemenuitem, userfeatureoverride
- auditlog, loginaudit
- messages, appnotifications
