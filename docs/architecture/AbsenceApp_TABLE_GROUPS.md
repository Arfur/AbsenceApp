================================================================================
 File        : AbsenceApp_TABLE_GROUPS.md
 Author      : Michael
 Version     : 1.0.1
 Created     : 2026-04-16
--------------------------------------------------------------------------------
 Purpose     :
   Groups all SQL Server and MySQL tables into functional domains using
   ASCII-styled tables for maximum clarity and Notepad compatibility.
================================================================================


# 1. System & Infrastructure Tables

+----------------------------------------------+
| SYSTEM & INFRASTRUCTURE TABLES               |
+----------------------------------------------+
| __EFMigrationsHistory / __efmigrationshistory|
| GlobalConfig / globalconfig                  |
| SystemEvents / systemevents                  |
| ExternalSystems / externalsystems            |
| MenuItemsGlobalConfig / menuitemsglobalconfig|
| AuditLog / auditlog                          |
| LoginAudit / loginaudit                      |
| RoleChangeAudit / rolechangeaudit            |
| StaffDeviceAudit / staffdeviceaudit          |
| StaffExternalAccountAudit / staffexternalaccountaudit |
| StaffSchoolAudit / staffschoolaudit          |
| StudentAbsenceAudit / studentabsenceaudit    |
| StaffAbsenceAudit / staffabsenceaudit        |
| AccountVerificationEvents / accountverificationevents |
+----------------------------------------------+


# 2. Security, Roles & Permissions (RBAC)

+----------------------------------------------+
| SECURITY / ROLES / PERMISSIONS (RBAC)        |
+----------------------------------------------+
| Users / users                                |
| UserProfiles / userprofiles                  |
| Roles / roles                                |
| RoleTypes / roletypes                        |
| RoleFeature / rolefeature                    |
| UserRole / userrole                          |
| UserFeatureOverride / userfeatureoverride    |
| Feature / feature                            |
| RoleMenuItem (MySQL only) / rolemenuitem     |
+----------------------------------------------+


# 3. Menu System (Navigation)

+----------------------------------------------+
| MENU SYSTEM (NAVIGATION)                     |
+----------------------------------------------+
| MenuItems / menuitems                        |
| MenuItemsGlobalConfig / menuitemsglobalconfig|
| RoleMenuItem (MySQL only) / rolemenuitem     |
+----------------------------------------------+


# 4. Staff Domain

+-----------------------------------------------------------+
| STAFF DOMAIN                                              |
+-----------------------------------------------------------+
| Staff / staff                                             |
| StaffAbsences / staffabsences                             |
| StaffAbsenceAudit / staffabsenceaudit                     |
| StaffDevices / staffdevices                               |
| StaffDeviceAudit / staffdeviceaudit                       |
| StaffExternalAccounts / staffexternalaccounts             |
| StaffExternalAccountAudit / staffexternalaccountaudit     |
| StaffSchools / staffschools                               |
| StaffSchoolAudit / staffschoolaudit                       |
| StaffAssignments / staffassignments                       |
| StaffDepartments / staffdepartments                       |
| StaffJobGroups / staffjobgroups                           |
| StaffJobTitles / staffjobtitles                           |
| StaffResponsibilities / staffresponsibilities             |
| StaffLocations / stafflocations                           |
| Departments (SQL Server only)                             |
| JobGroups (SQL Server only)                               |
| JobTitles (SQL Server only)                               |
+-----------------------------------------------------------+


# 5. Student Domain

+----------------------------------------------+
| STUDENT DOMAIN                               |
+----------------------------------------------+
| Students / students                           |
| StudentAbsences / studentabsences             |
| StudentAbsenceAudit / studentabsenceaudit     |
| StudentContacts / studentcontacts             |
| StudentFlags / studentflags                   |
| StudentMedical / studentmedical               |
+----------------------------------------------+


# 6. Attendance Domain

+-----------------------------------------------------------+
| ATTENDANCE DOMAIN                                         |
+-----------------------------------------------------------+
| Attendance / attendance                                   |
| AttendanceMarks / attendancemarks                         |
| AttendanceRegisters / attendanceregisters                 |
| AbsenceTypes / absencetypes                               |
| AbsenceCategories (MySQL only) / absencecategories        |
| AbsenceMethods (MySQL only) / absencemethods              |
| AbsenceReasons (MySQL only) / absencereasons              |
| AbsenceSources (MySQL only) / absencesources              |
| AbsenceStatuses (MySQL only) / absencestatuses            |
+-----------------------------------------------------------+


# 7. Academic Structure

+-----------------------------------------------------------+
| ACADEMIC STRUCTURE                                        |
+-----------------------------------------------------------+
| Schools / schools                                         |
| Classes / classes                                         |
| ClassMember / classmember                                 |
| ClassYearGroupAssignments / classyeargroupassignments     |
| YearGroups / yeargroups                                   |
| Phases / phases                                           |
| Houses / houses                                           |
| Responsibilities / responsibilities                       |
+-----------------------------------------------------------+


# 8. Messaging & Notifications

+----------------------------------------------+
| MESSAGING & NOTIFICATIONS                    |
+----------------------------------------------+
| Messages / messages                           |
| AppNotifications / appnotifications           |
+----------------------------------------------+


================================================================================
 Summary:
   These ASCII tables provide a deterministic, domain‑grouped reference for all
   SQL Server and MySQL tables. They ensure clarity, prevent schema drift, and
   support correct data‑population ordering.
================================================================================
