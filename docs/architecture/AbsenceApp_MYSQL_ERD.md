================================================================================
 File        : AbsenceApp_MYSQL_ERD.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
 Updated     : 2026-04-16
--------------------------------------------------------------------------------
 Purpose     :
   Provides a complete ASCII-based Entity Relationship Diagram (ERD) for the
   MySQL AbsenceApp schema. This diagram visualises the logical relationships
   between tables and domains.

   This file exists to:
     - Provide a high-level architectural view of the database
     - Support onboarding, debugging, and schema comprehension
     - Prevent incorrect assumptions about relational structure
--------------------------------------------------------------------------------
 Notes       :
   - MySQL schema does not define explicit FKs; relationships are inferred.
   - Diagram is domain-grouped for clarity.
================================================================================


# ASCII ERD (Domain Grouped)

LEGEND:
  [TABLE] = entity
  -->     = one-to-many
  ---     = logical link (no enforced FK)


================================================================================
 DOMAIN 1 — SYSTEM & INFRASTRUCTURE
================================================================================

[menuitems] <-- (ParentId) -- [menuitems]
[menuitemsglobalconfig] --> [menuitems]
[feature]
[globalconfig]
[systemevents]
[__efmigrationshistory]


================================================================================
 DOMAIN 2 — STAFF
================================================================================

[staff]
   |--> [staffdepartments]
   |--> [staffjobtitles]
   |--> [staffjobgroups]
   |--> [stafflocations]
   |--> [staffassignments]
   |--> [staffschools]
   |--> [staffdevices]
   |--> [staffexternalaccounts]
   |--> [staffresponsibilities]

[staffschools] --> [schools]
[staffassignments] --> [stafflocations]
[staffexternalaccounts] --> [externalsystems]

[staffdeviceaudit] --> [staffdevices]
[staffexternalaccountaudit] --> [staffexternalaccounts]
[staffschoolaudit] --> [staffschools]


================================================================================
 DOMAIN 3 — STUDENTS
================================================================================

[students]
   |--> [studentcontacts]
   |--> [studentflags]
   |--> [studentmedical]
   |--> [studentabsences]

[students] --> [yeargroups]
[students] --> [houses]

[studentabsences] --> [absencetypes]
[studentabsenceaudit] --> [studentabsences]
[studentabsenceaudit] --> [absencestatuses]


================================================================================
 DOMAIN 4 — ATTENDANCE
================================================================================

[attendance]
   |--> [users]
   |--> [classes]

[attendanceregisters] --> [classes]
[attendancemarks] --> [attendanceregisters]
[attendancemarks] --> [students]

Lookup tables:
  [absencecategories]
  [absencemethods]
  [absencereasons]
  [absencesources]
  [absencestatuses]
  [absencetypes]


================================================================================
 DOMAIN 5 — CLASSES & GROUPS
================================================================================

[classes]
   |--> [classmember]
   |--> [classyeargroupassignments]

[classyeargroupassignments] --> [yeargroups]
[classyeargroupassignments] --> [schools]


================================================================================
 DOMAIN 6 — DEVICES
================================================================================

[devicetypes]
[staffdevices] --> [staff]
[staffdeviceaudit] --> [staffdevices]


================================================================================
 DOMAIN 7 — EXTERNAL ACCOUNTS & INTEGRATIONS
================================================================================

[externalsystems]
[staffexternalaccounts] --> [externalsystems]
[staffexternalaccountaudit] --> [staffexternalaccounts]
[accountverificationevents] --> [users]


================================================================================
 DOMAIN 8 — USERS, ROLES, PERMISSIONS, AUDIT, MESSAGING & MISC
================================================================================

[users]
   |--> [userprofiles]
   |--> [userrole]
   |--> [userfeatureoverride]
   |--> [messages]
   |--> [appnotifications]
   |--> [loginaudit]
   |--> [auditlog]

[userrole] --> [roles]
[roles] --> [roletypes]

[rolefeature] --> [roles]
[rolemenuitem] --> [roles]
[rolemenuitem] --> [menuitems]
