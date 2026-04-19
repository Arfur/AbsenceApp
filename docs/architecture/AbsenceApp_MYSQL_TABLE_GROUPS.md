================================================================================
 File        : AbsenceApp_MYSQL_TABLE_GROUPS.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
 Updated     : 2026-04-16
--------------------------------------------------------------------------------
 Purpose     :
   Provides the authoritative domain grouping for all MySQL AbsenceApp tables.
   This ensures consistent documentation, onboarding, and architectural clarity.

   This file exists to:
     - Prevent domain drift
     - Provide a single source of truth for table categorisation
     - Support deterministic documentation and governance
--------------------------------------------------------------------------------
 Notes       :
   - Groups follow the SQL Server domain structure.
   - MySQL‑only tables are placed in the most logical domain.
================================================================================


# Domain Grouping (Final)

## DOMAIN 1 — SYSTEM & INFRASTRUCTURE
- __efmigrationshistory
- globalconfig
- feature
- menuitems
- menuitemsglobalconfig
- systemevents

## DOMAIN 2 — STAFF
- staff
- staffdepartments
- staffjobtitles
- staffjobgroups
- stafflocations
- staffassignments
- staffschools
- staffschoolaudit
- staffdevices
- staffdeviceaudit
- staffexternalaccounts
- staffexternalaccountaudit
- staffresponsibilities
- responsibilities

## DOMAIN 3 — STUDENTS
- students
- studentcontacts
- studentflags
- studentmedical
- studentabsences
- studentabsenceaudit
- yeargroups
- houses
- phases

## DOMAIN 4 — ATTENDANCE
- attendance
- attendanceregisters
- attendancemarks
- absencecategories
- absencemethods
- absencereasons
- absencesources
- absencestatuses
- absencetypes

## DOMAIN 5 — CLASSES & GROUPS
- classes
- classmember
- classyeargroupassignments

## DOMAIN 6 — DEVICES
- devicetypes
- staffdevices
- staffdeviceaudit

## DOMAIN 7 — EXTERNAL ACCOUNTS & INTEGRATIONS
- externalsystems
- staffexternalaccounts
- staffexternalaccountaudit
- accountverificationevents

## DOMAIN 8 — USERS, ROLES, PERMISSIONS, AUDIT, MESSAGING & MISC
- users
- userprofiles
- roles
- roletypes
- userrole
- rolefeature
- rolemenuitem
- userfeatureoverride
- auditlog
- loginaudit
- messages
- appnotifications
