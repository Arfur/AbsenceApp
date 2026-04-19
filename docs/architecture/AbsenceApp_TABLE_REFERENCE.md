================================================================================
 File        : AbsenceApp_TABLE_REFERENCE.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
--------------------------------------------------------------------------------
 Purpose     :
   This file provides a definitive, authoritative reference list of all table
   names in both SQL Server and MySQL for the AbsenceApp V2 project.

   It exists to prevent:
     - Guessing table names
     - Renaming tables incorrectly
     - Schema drift
     - AI hallucination of table names
     - Repeated re-checking of schema

   This file must be loaded into every AI session to ensure strict adherence to
   the real schema and to maintain operational hygiene.
--------------------------------------------------------------------------------
 Notes       :
   - Table names MUST be used exactly as listed.
   - No assumptions, no inferred names, no pluralisation changes.
   - If a new table is added, this file MUST be updated immediately.
================================================================================


# 1. SQL Server vs MySQL Table Reference

The following table lists all tables from SQL Server and MySQL side-by-side.
This is the authoritative mapping for the project.

| SQL Server Table               | MySQL Table                     |
|-------------------------------|----------------------------------|
| __EFMigrationsHistory         | __efmigrationshistory            |
| AbsenceTypes                  | absencetypes                     |
| AccountVerificationEvents     | accountverificationevents        |
| AppNotifications              | appnotifications                 |
| Attendance                    | attendance                       |
| AttendanceMarks               | attendancemarks                  |
| AttendanceRegisters           | attendanceregisters              |
| AuditLog                      | auditlog                         |
| Classes                       | classes                          |
| ClassMember                   | classmember                      |
| ClassYearGroupAssignments     | classyeargroupassignments        |
| Departments                   | staffdepartments                 |
| DeviceTypes                   | devicetypes                      |
| ExternalSystems               | externalsystems                  |
| Feature                       | feature                          |
| GlobalConfig                  | globalconfig                     |
| Houses                        | houses                           |
| JobGroups                     | staffjobgroups                   |
| JobTitles                     | staffjobtitles                   |
| LoginAudit                    | loginaudit                       |
| MenuItems                     | menuitems                        |
| MenuItemsGlobalConfig         | menuitemsglobalconfig            |
| Messages                      | messages                         |
| Phases                        | phases                           |
| Responsibilities              | responsibilities                 |
| RoleChangeAudit               | rolechangeaudit                  |
| RoleFeature                   | rolefeature                      |
| Roles                         | roles                            |
| RoleTypes                     | roletypes                        |
| Schools                       | schools                          |
| Staff                         | staff                            |
| StaffAbsenceAudit             | staffabsenceaudit                |
| StaffAbsences                 | staffabsences                    |
| StaffDeviceAudit              | staffdeviceaudit                 |
| StaffDevices                  | staffdevices                     |
| StaffExternalAccountAudit     | staffexternalaccountaudit        |
| StaffExternalAccounts         | staffexternalaccounts            |
| StaffAssignments              | staffassignments                 |
| StaffLocations                | stafflocations                   |
| StaffResponsibilities         | staffresponsibilities            |
| StaffSchoolAudit              | staffschoolaudit                 |
| StaffSchools                  | staffschools                     |
| StudentAbsenceAudit           | studentabsenceaudit              |
| StudentAbsences               | studentabsences                  |
| StudentContacts               | studentcontacts                  |
| StudentFlags                  | studentflags                     |
| StudentMedical                | studentmedical                   |
| Students                      | students                         |
| SystemEvents                  | systemevents                     |
| UserFeatureOverride           | userfeatureoverride              |
| UserProfiles                  | userprofiles                     |
| UserRole                      | userrole                         |
| Users                         | users                            |
| YearGroups                    | yeargroups                       |
| **N/A (SQL Server did not have)** | rolemenuitem                 |
| **N/A (SQL Server did not have)** | absencecategories            |
| **N/A (SQL Server did not have)** | absencemethods               |
| **N/A (SQL Server did not have)** | absencereasons               |
| **N/A (SQL Server did not have)** | absencesources               |
| **N/A (SQL Server did not have)** | absencestatuses              |

================================================================================

# 2. Rules for Using This File

1. **These names are authoritative.**  
   No renaming, no pluralisation changes, no assumptions.

2. **AI must use these names exactly as written.**

3. **If a table is added or removed, this file must be updated immediately.**

4. **This file must be referenced before generating any SQL.**

5. **If the AI is unsure about a table name, it must stop and ask.**

================================================================================
