================================================================================
 File        : AbsenceApp_MYSQL_FK_MAP.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
 Updated     : 2026-04-16
--------------------------------------------------------------------------------
 Purpose     :
   Provides a complete, authoritative map of all *inferred* foreign key
   relationships in the MySQL AbsenceApp schema. MySQL schema does not define
   explicit FKs, so this file documents the logical relational model.

   This file exists to:
     - Prevent incorrect joins
     - Provide a single source of truth for relational structure
     - Support ORM modelling, migrations, and documentation
--------------------------------------------------------------------------------
 Notes       :
   - All relationships are inferred from naming conventions and schema usage.
   - Only relationships with clear, deterministic meaning are included.
================================================================================


# Inferred Foreign Key Map (MySQL)

## System & Infrastructure
- menuitems.ParentId → menuitems.Id
- menuitemsglobalconfig.MenuItemCode → menuitems.Code

## Staff Domain
- staff.ReportingManagerId → staff.Id
- staff.JobTitleId → staffjobtitles.Id
- staff.JobGroupId → staffjobgroups.Id
- staff.DepartmentId → staffdepartments.Id

- staffassignments.StaffId → staff.Id
- staffassignments.LocationId → stafflocations.Id

- staffschools.StaffId → staff.Id
- staffschools.SchoolId → schools.Id

- staffdevices.StaffId → staff.Id
- staffdeviceaudit.StaffDeviceId → staffdevices.Id
- staffdeviceaudit.StaffId → staff.Id

- staffexternalaccounts.StaffId → staff.Id
- staffexternalaccounts.ExternalSystemId → externalsystems.Id
- staffexternalaccountaudit.StaffExternalAccountId → staffexternalaccounts.Id
- staffexternalaccountaudit.StaffId → staff.Id

- staffresponsibilities.StaffId → staff.Id
- staffresponsibilities.ResponsibilityId → responsibilities.Id

- staffschoolaudit.StaffSchoolId → staffschools.Id
- staffschoolaudit.StaffId → staff.Id

## Students Domain
- students.YearGroupId → yeargroups.Id
- students.HouseId → houses.Id

- studentcontacts.StudentId → students.Id
- studentflags.StudentId → students.Id
- studentflags.AssignedBy → users.Id
- studentmedical.StudentId → students.Id
- studentmedical.RecordedBy → users.Id

- studentabsences.StudentId → students.Id
- studentabsences.AbsenceTypeId → absencetypes.Id
- studentabsences.RecordedBy → users.Id

- studentabsenceaudit.StudentId → students.Id
- studentabsenceaudit.AbsenceId → studentabsences.Id
- studentabsenceaudit.ChangedBy → users.Id
- studentabsenceaudit.OldStatusId → absencestatuses.Id
- studentabsenceaudit.NewStatusId → absencestatuses.Id

## Attendance Domain
- attendance.UserId → users.Id
- attendance.ClassId → classes.Id
- attendance.RecordedBy → users.Id

- attendanceregisters.ClassId → classes.Id
- attendanceregisters.OpenedBy → users.Id
- attendanceregisters.ClosedBy → users.Id

- attendancemarks.AttendanceRegisterId → attendanceregisters.Id
- attendancemarks.StudentId → students.Id
- attendancemarks.RecordedBy → users.Id

## Classes & Groups
- classmember.ClassId → classes.Id
- classmember.StaffId → staff.Id

- classyeargroupassignments.ClassId → classes.Id
- classyeargroupassignments.YearGroupId → yeargroups.Id
- classyeargroupassignments.SchoolId → schools.Id

## Users, Roles, Permissions
- users.StaffId → staff.Id

- userprofiles.UserId → users.Id

- userrole.UserId → users.Id
- userrole.RoleId → roles.Id

- roles.RoleTypeId → roletypes.Id

- rolefeature.RoleId → roles.Id
- rolefeature.FeatureCode → feature.Code

- rolemenuitem.RoleId → roles.Id
- rolemenuitem.MenuItemId → menuitems.Id

- userfeatureoverride.UserId → users.Id
- userfeatureoverride.FeatureCode → feature.Code

## Messaging, Audit, Notifications
- messages.UserId → users.Id
- appnotifications.UserId → users.Id

- loginaudit.UserId → users.Id
- auditlog.UserId → users.Id
- auditlog.UserId1 → users.Id

## External Integrations
- accountverificationevents.UserId → users.Id
