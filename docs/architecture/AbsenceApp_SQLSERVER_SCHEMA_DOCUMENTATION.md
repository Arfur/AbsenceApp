================================================================================
 File        : AbsenceApp_SQLSERVER_SCHEMA_DOCUMENTATION.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
 Updated     : 2026-04-16
--------------------------------------------------------------------------------
 Purpose     :
   This document provides a complete, domain‑grouped schema reference for the
   SQL Server version of AbsenceApp. Each table and field is documented with
   data type, description, example values, and relational links.

   This file exists to:
     - Prevent schema drift
     - Prevent guessing or inventing table/field meanings
     - Provide a single source of truth for developers and AI assistants
     - Support accurate migration, documentation, and governance
--------------------------------------------------------------------------------
 Notes       :
   - Descriptions, example values, and FK links are invented for documentation.
   - Field names and data types are taken directly from SQL Server schema.
   - Domains follow the AbsenceApp_TABLE_GROUPS.md structure.
================================================================================


Table: __EFMigrationsHistory
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| MigrationId              | nvarchar       | EF migration identifier…                  | "20241012_AddUsers"       | (PK)                         |
| ProductVersion           | nvarchar       | EF Core version used…                     | "7.0.12"                  |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: GlobalConfig
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| ConfigKey                | nvarchar       | Unique config key…                        | "attendance.lockout"      | (PK)                         |
| ConfigValue              | nvarchar       | Stored config value…                      | "15"                      |                              |
| ValueType                | nvarchar       | Type of stored value…                     | "integer"                 |                              |
| IsEnabled                | bit            | Whether config is enabled…                | 1                         |                              |
| UpdatedAtUtc             | datetime2      | Last update timestamp…                    | 2026‑03‑01 10:22:00        |                              |
| Description              | nvarchar       | Human‑readable description…               | "Lockout duration…"       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: SystemEvents
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Unique event id…                          | 1001                      | (PK)                         |
| EventType                | nvarchar       | Type of system event…                     | "USER_LOGIN"              |                              |
| EventTime                | datetime2      | Timestamp of event…                       | 2026‑04‑10 09:15:00        |                              |
| TriggeredBy              | nvarchar       | Actor that triggered event…               | "AuthService"             |                              |
| Description              | nvarchar       | Event description…                        | "User login success…"     |                              |
| Metadata                 | nvarchar       | JSON metadata…                            | {"ip":"10.0.0.5"}         |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: ExternalSystems
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Unique external system id…                | 3                         | (PK)                         |
| Name                     | nvarchar       | External system name…                     | "Google Workspace"        |                              |
| Code                     | nvarchar       | Short system code…                        | "GOOGLE"                  |                              |
| Description              | nvarchar       | Purpose of integration…                   | "Syncs staff acc…"        |                              |
| CreatedAt                | datetime2      | Created timestamp…                        | 2026‑01‑12 14:00:00        |                              |
| UpdatedAt                | datetime2      | Last update timestamp…                    | 2026‑03‑05 09:30:00        |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: MenuItemsGlobalConfig
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| ItemType                 | nvarchar       | Menu item type…                           | "group"                   |                              |
| Id                       | int            | Menu config id…                           | 12                        | (PK)                         |
| ParentId                 | int            | Parent menu id…                           | 3                         | MenuItemsGlobalConfig.Id     |
| Label                    | nvarchar       | Display label…                            | "Attendance"              |                              |
| Icon                     | nvarchar       | Icon name…                                | "calendar"                |                              |
| IsHidden                 | bit            | Hidden flag…                              | 0                         |                              |
| Route                    | nvarchar       | Navigation route…                         | "/attendance"             |                              |
| SortOrder                | int            | Ordering index…                           | 1                         |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026‑02‑01 12:00:00        |                              |
| UpdatedAt                | datetime       | Last update…                              | 2026‑03‑10 09:00:00        |                              |
| Category                 | nvarchar       | Category group…                           | "Core"                    |                              |
| GroupName                | nvarchar       | Group label…                              | "Registers"               |                              |
| GroupIcon                | nvarchar       | Group icon…                               | "folder"                  |                              |
| IsFlat                   | bit            | Flattened flag…                           | 0                         |                              |
| Status                   | nvarchar       | Status (active/disabled)…                 | "active"                  |                              |
| Description              | nvarchar       | Menu item description…                    | "Main attendance…"        |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: AuditLog
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| AuditId                  | int            | Audit entry id…                           | 501                       | (PK)                         |
| UserId                   | int            | Acting user id…                           | 42                        | Users.Id                     |
| Action                   | nvarchar       | Action performed…                         | "Updated profile"         |                              |
| Timestamp                | datetime2      | Action timestamp…                         | 2026‑03‑12 11:45:00        |                              |
| UserId1                  | bigint         | Legacy user id…                           | 42                        | Users.Id                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: LoginAudit
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Login audit id…                           | 9001                      | (PK)                         |
| UserId                   | bigint         | User attempting login…                    | 42                        | Users.Id                     |
| LoginTime                | datetime2      | Login timestamp…                          | 2026‑04‑10 08:00:00        |                              |
| IpAddress                | nvarchar       | IP address used…                          | "192.168.1.10"            |                              |
| UserAgent                | nvarchar       | Browser user agent…                       | "Chrome/123"              |                              |
| Success                  | bit            | Login success flag…                       | 1                         |                              |
| CreatedAt                | datetime2      | Record created…                           | 2026‑04‑10 08:00:01        |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: RoleChangeAudit
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Role change audit id…                     | 3001                      | (PK)                         |
| UserId                   | bigint         | User whose role changed…                  | 42                        | Users.Id                     |
| OldRoleId                | bigint         | Previous role id…                         | 2                         | Roles.RoleId                 |
| NewRoleId                | bigint         | New role id…                              | 3                         | Roles.RoleId                 |
| ChangedBy                | bigint         | Admin who changed role…                   | 1                         | Users.Id                     |
| ChangeTime               | datetime2      | Timestamp of change…                      | 2026‑03‑01 14:00:00        |                              |
| Reason                   | nvarchar       | Reason for change…                        | "Promotion"               |                              |
| Metadata                 | nvarchar       | JSON metadata…                            | {"source":"admin"}        |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: StaffDeviceAudit
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Device audit id…                          | 7001                      | (PK)                         |
| StaffDeviceId            | bigint         | Device audited…                           | 12                        | StaffDevices.Id              |
| StaffId                  | bigint         | Staff member…                             | 42                        | Staff.Id                     |
| Action                   | nvarchar       | Action performed…                         | "Returned device"         |                              |
| ChangedBy                | bigint         | User who changed…                         | 1                         | Users.Id                     |
| ChangeTime               | datetime2      | Timestamp of change…                      | 2026‑02‑20 10:00:00        |                              |
| OldValues                | nvarchar       | JSON before change…                       | {"ReturnedDate":nul…}     |                              |
| NewValues                | nvarchar       | JSON after change…                        | {"ReturnedDate":"202…"}   |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: StaffExternalAccountAudit
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | External account audit id…                | 8101                      | (PK)                         |
| StaffExternalAccountId   | bigint         | External account audited…                 | 5                         | StaffExternalAccount.Id      |
| StaffId                  | bigint         | Staff member…                             | 42                        | Staff.Id                     |
| Action                   | nvarchar       | Action performed…                         | "Linked Google acc…"      |                              |
| ChangedBy                | bigint         | Admin who changed…                        | 1                         | Users.Id                     |
| ChangeTime               | datetime2      | Timestamp of change…                      | 2026‑03‑10 12:00:00        |                              |
| OldValues                | nvarchar       | JSON before change…                       | {}                        |                              |
| NewValues                | nvarchar       | JSON after change…                        | {"Email":"x@schoo…"}      |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: StaffSchoolAudit
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Staff‑school audit id…                    | 9001                      | (PK)                         |
| StaffSchoolId            | bigint         | Staff‑school link audited…                | 22                        | StaffSchools.Id              |
| StaffId                  | bigint         | Staff member…                             | 42                        | Staff.Id                     |
| Action                   | nvarchar       | Action performed…                         | "Assigned to school"      |                              |
| ChangedBy                | bigint         | Admin who changed…                        | 1                         | Users.Id                     |
| ChangeTime               | datetime2      | Timestamp of change…                      | 2026‑01‑15 09:00:00        |                              |
| OldValues                | nvarchar       | JSON before change…                       | {}                        |                              |
| NewValues                | nvarchar       | JSON after change…                        | {"SchoolId":3}            | Schools.Id                   |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: StudentAbsenceAudit
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Absence audit id…                         | 6001                      | (PK)                         |
| StudentAbsenceId         | bigint         | Absence record audited…                   | 101                       | StudentAbsences.Id           |
| StudentId                | bigint         | Student associated…                       | 2001                      | Students.Id                  |
| Action                   | nvarchar       | Action performed…                         | "Updated absence"         |                              |
| ChangedBy                | bigint         | User who changed…                         | 1                         | Users.Id                     |
| ChangeTime               | datetime2      | Timestamp of change…                      | 2026‑02‑10 11:00:00        |                              |
| OldValues                | nvarchar       | JSON before change…                       | {}                        |                              |
| NewValues                | nvarchar       | JSON after change…                        | {"Code":"I"}              |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Domain 2

Table: Staff
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Primary staff identifier…                  | 42                        | (PK)                         |
| StaffNumber              | nvarchar       | Organisation staff number…                 | "S-00042"                 |                              |
| FirstName                | nvarchar       | Given name…                                | "John"                    |                              |
| LastName                 | nvarchar       | Family name…                               | "Smith"                   |                              |
| Email                    | nvarchar       | Primary email address…                     | "john.s@school.org"       |                              |
| Phone                    | nvarchar       | Contact phone number…                      | "+44 7700 900000"         |                              |
| DateOfBirth              | date           | Birth date…                                | 1980-05-12                |                              |
| EmploymentStartDate      | date           | Employment start date…                     | 2015-09-01                |                              |
| EmploymentEndDate        | date           | Employment end date (nullable)…            |                           |                              |
| Status                   | nvarchar       | Employment status…                         | "active"                  |                              |
| CreatedAt                | datetime2      | Record creation timestamp…                 | 2026‑01‑01 09:00:00       |                              |
| UpdatedAt                | datetime2      | Last update timestamp…                     | 2026‑04‑01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: StaffSchools
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Staff‑school link id…                      | 101                       | (PK)                         |
| StaffId                  | bigint         | Staff member id…                           | 42                        | Staff.Id                     |
| SchoolId                 | bigint         | Linked school id…                          | 3                         | Schools.Id                   |
| Role                     | nvarchar       | Role at school…                            | "Teacher"                 |                              |
| IsPrimary                | bit            | Primary school flag…                       | 1                         |                              |
| AssignedAt               | datetime2      | Assignment timestamp…                      | 2020‑09‑01 08:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: StaffDevices
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Device record id…                          | 12                        | (PK)                         |
| StaffId                  | bigint         | Owner staff id…                            | 42                        | Staff.Id                     |
| DeviceType               | nvarchar       | Type of device…                            | "Laptop"                  |                              |
| SerialNumber             | nvarchar       | Device serial number…                      | "SN123456"                |                              |
| IssuedAt                 | datetime2      | When device was issued…                    | 2021‑01‑15 09:00:00       |                              |
| ReturnedAt               | datetime2      | When device was returned (nullable)…       |                           |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: StaffExternalAccounts
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | External account id…                       | 5                         | (PK)                         |
| StaffId                  | bigint         | Staff owner id…                            | 42                        | Staff.Id                     |
| Provider                 | nvarchar       | External provider name…                    | "Google"                  |                              |
| ExternalId               | nvarchar       | Provider account id…                       | "john.s@google.com"       |                              |
| LinkedAt                 | datetime2      | When account was linked…                   | 2022‑06‑01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: StaffRoles
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Role id…                                   | 2                         | (PK)                         |
| Name                     | nvarchar       | Role name…                                 | "Headteacher"             |                              |
| Description              | nvarchar       | Role description…                          | "School lead role"        |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Domain 3
Table: Students
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Primary student id…                        | 2001                      | (PK)                         |
| UPN                      | nvarchar       | Unique pupil number…                       | "UPN123456"               |                              |
| FirstName                | nvarchar       | Given name…                                | "Alice"                   |                              |
| LastName                 | nvarchar       | Family name…                               | "Brown"                   |                              |
| DateOfBirth              | date           | Birth date…                                | 2010-07-20                |                              |
| YearGroup                | nvarchar       | Year group…                                | "Year 6"                  |                              |
| EnrolledAt               | datetime2      | Enrollment date…                            | 2016‑09‑01 08:30:00       |                              |
| Status                   | nvarchar       | Student status…                             | "active"                  |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: StudentAbsences
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Absence record id…                         | 101                       | (PK)                         |
| StudentId                | bigint         | Student id…                                | 2001                      | Students.Id                  |
| Date                     | date           | Absence date…                              | 2026-04-01                |                              |
| Reason                   | nvarchar       | Absence reason…                            | "Illness"                 |                              |
| RecordedBy               | bigint         | User who recorded absence…                 | 42                        | Users.Id                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: StudentContacts
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Contact id…                                | 301                       | (PK)                         |
| StudentId                | bigint         | Student id…                                | 2001                      | Students.Id                  |
| Name                     | nvarchar       | Contact name…                              | "Mary Brown"              |                              |
| Relationship             | nvarchar       | Relationship to student…                   | "Mother"                  |                              |
| Phone                    | nvarchar       | Contact phone…                             | "+44 7700 900111"         |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Domain 4
Table: AttendanceSessions
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Attendance session id…                     | 5001                      | (PK)                         |
| SchoolId                 | bigint         | School id…                                 | 3                         | Schools.Id                   |
| SessionDate              | date           | Date of session…                           | 2026-04-10                |                              |
| StartTime                | time           | Session start time…                        | 08:45:00                  |                              |
| EndTime                  | time           | Session end time…                          | 15:15:00                  |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: AttendanceRecords
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Attendance record id…                      | 7001                      | (PK)                         |
| SessionId                | bigint         | Attendance session id…                     | 5001                      | AttendanceSessions.Id        |
| StudentId                | bigint         | Student id…                                | 2001                      | Students.Id                  |
| Status                   | nvarchar       | Attendance status (present/absent)…        | "present"                 |                              |
| RecordedAt               | datetime2      | When record was taken…                     | 2026‑04‑10 08:50:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Domain 5
Table: Classes
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Class id…                                  | 4001                      | (PK)                         |
| Name                     | nvarchar       | Class name…                                | "6A"                      |                              |
| YearGroup                | nvarchar       | Year group…                                | "Year 6"                  |                              |
| TeacherId                | bigint         | Class teacher id…                           | 42                        | Staff.Id                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: Groups
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Group id…                                  | 6001                      | (PK)                         |
| Name                     | nvarchar       | Group name…                                | "Math Club"               |                              |
| LeaderId                 | bigint         | Group leader staff id…                      | 42                        | Staff.Id                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Domain 6
Table: Devices
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Device id…                                 | 9001                      | (PK)                         |
| Tag                      | nvarchar       | Asset tag…                                 | "DEV-9001"                |                              |
| Type                     | nvarchar       | Device type…                               | "Tablet"                  |                              |
| Model                    | nvarchar       | Device model…                              | "Tab X"                   |                              |
| AssignedTo               | bigint         | Assigned staff id…                          | 42                        | Staff.Id                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: DeviceMaintenance
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Maintenance record id…                     | 10001                     | (PK)                         |
| DeviceId                 | bigint         | Device id…                                 | 9001                      | Devices.Id                   |
| PerformedAt              | datetime2      | Maintenance timestamp…                     | 2026‑02‑10 10:00:00       |                              |
| Notes                    | nvarchar       | Maintenance notes…                          | "Battery replaced"        |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Domain 7
Table: ExternalAccountProviders
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Provider id…                               | 1                         | (PK)                         |
| Name                     | nvarchar       | Provider name…                             | "Google"                  |                              |
| Code                     | nvarchar       | Provider code…                             | "GOOGLE"                  |                              |
| Config                   | nvarchar       | JSON config…                               | {"sync":true}             |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: ExternalSyncJobs
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Sync job id…                               | 20001                     | (PK)                         |
| ProviderId               | bigint         | Provider id…                               | 1                         | ExternalAccountProviders.Id  |
| StartedAt                | datetime2      | Job start time…                            | 2026‑03‑01 02:00:00       |                              |
| Status                   | nvarchar       | Job status…                                | "Completed"               |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Domain 8
Table: Lookups
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Lookup id…                                 | 1                         | (PK)                         |
| Category                 | nvarchar       | Lookup category…                           | "AbsenceReason"           |                              |
| Key                      | nvarchar       | Lookup key…                                | "ILL"                     |                              |
| Value                    | nvarchar       | Lookup display value…                      | "Illness"                 |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: Notifications
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Notification id…                           | 30001                     | (PK)                         |
| UserId                   | bigint         | Recipient user id…                         | 42                        | Users.Id                     |
| Message                  | nvarchar       | Notification message…                      | "Absence recorded"        |                              |
| SentAt                   | datetime2      | When notification sent…                    | 2026‑04‑10 09:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
