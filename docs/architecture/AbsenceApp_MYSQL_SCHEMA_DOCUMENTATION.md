================================================================================
 File        : AbsenceApp_MYSQL_SCHEMA_DOCUMENTATION.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
 Updated     : 2026-04-16
--------------------------------------------------------------------------------
 Purpose     :
   This document provides a complete, domain‑grouped schema reference for the
   MySQL version of AbsenceApp. Each table and field is documented with
   data type, description, example values, and relational links.

   This file exists to:
     - Prevent schema drift
     - Prevent guessing or inventing table/field meanings
     - Provide a single source of truth for developers and AI assistants
     - Support accurate migration, documentation, and governance
--------------------------------------------------------------------------------
 Notes       :
   - Descriptions, example values, and FK links are invented for documentation.
   - Field names and data types are taken directly from the MySQL schema.
   - Domains follow the AbsenceApp_TABLE_GROUPS.md structure; extra tables are
     placed in the most logical domain.
================================================================================

Domain 1
Table: __efmigrationshistory
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| MigrationId              | varchar(150)   | EF migration identifier…                  | "20241012_AddUsers"       | (PK)                         |
| ProductVersion           | varchar(32)    | EF Core version used…                     | "7.0.12"                  |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: globalconfig
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| ConfigKey                | varchar(100)   | Unique config key…                        | "attendance.lockout"      | (PK)                         |
| ConfigValue              | longtext       | Stored config value…                      | "15"                      |                              |
| ValueType                | varchar(20)    | Type of stored value…                     | "integer"                 |                              |
| IsEnabled                | tinyint        | Whether config is enabled…                | 1                         |                              |
| UpdatedAtUtc             | datetime       | Last update timestamp (UTC)…              | 2026-03-01 10:22:00       |                              |
| Description              | varchar(255)   | Human‑readable description…               | "Lockout duration…"       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: feature
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Feature id…                               | 1                         | (PK)                         |
| Code                     | varchar(100)   | Unique feature code…                      | "attendance.mark"         |                              |
| DisplayName              | varchar(150)   | Display name…                             | "Mark Attendance"         |                              |
| Description              | varchar(255)   | Feature description…                      | "Allows marking…"         |                              |
| IsEnabled                | tinyint        | Global enabled flag…                      | 1                         |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: menuitems
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Menu item id…                             | 10                        | (PK)                         |
| Code                     | varchar(100)   | Unique menu code…                         | "menu.attendance"         |                              |
| DisplayName              | varchar(150)   | Label shown in UI…                        | "Attendance"              |                              |
| Description              | varchar(255)   | Menu description…                         | "Access attendance…"      |                              |
| Url                      | varchar(255)   | Route or URL…                             | "/attendance"             |                              |
| Icon                     | varchar(100)   | Icon identifier…                          | "calendar"                |                              |
| ParentId                 | int            | Parent menu id…                           | 1                         | menuitems.Id                 |
| DisplayOrder             | int            | Sort order…                               | 1                         |                              |
| IsVisible                | tinyint        | Visibility flag…                          | 1                         |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-10 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-05 11:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: menuitemsglobalconfig
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Global menu config id…                    | 5                         | (PK)                         |
| MenuItemCode             | varchar(100)   | Menu item code…                           | "menu.attendance"         | menuitems.Code               |
| IsEnabled                | tinyint        | Whether menu item is enabled…             | 1                         |                              |
| VisibilityCondition      | varchar(255)   | Optional condition expression…            | "role=Admin"              |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-10 09:05:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-05 11:05:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: systemevents
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | System event id…                          | 1001                      | (PK)                         |
| EventType                | longtext       | Event type code…                          | "USER_LOGIN"              |                              |
| EventTime                | datetime       | When event occurred…                      | 2026-04-10 09:15:00       |                              |
| TriggeredBy              | longtext       | Source component…                         | "AuthService"             |                              |
| Description              | longtext       | Event description…                        | "User login success…"     |                              |
| Metadata                 | longtext       | JSON metadata…                            | "{\"ip\":\"10.0.0.5\"}"   |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+


Domain 2#
Table: staff
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Staff id…                                 | 42                        | (PK)                         |
| StaffNumber              | varchar(20)    | Organisation staff number…                | "S-00042"                 |                              |
| FirstName                | varchar(50)    | Given name…                               | "John"                    |                              |
| LastName                 | varchar(50)    | Family name…                              | "Smith"                   |                              |
| PreferredName            | varchar(50)    | Preferred name…                           | "Johnny"                  |                              |
| Title                    | varchar(20)    | Title (Mr/Ms/Dr)…                         | "Mr"                      |                              |
| DateOfBirth              | date           | Birth date…                               | 1980-05-12                |                              |
| Gender                   | varchar(20)    | Gender…                                   | "Male"                    |                              |
| WorkEmail                | varchar(100)   | Primary work email…                       | "john.s@school.org"       |                              |
| AltEmail                 | varchar(100)   | Alternate email…                          | "john@gmail.com"          |                              |
| PhoneHome                | varchar(20)    | Home phone…                               | "01332 000000"            |                              |
| PhoneMobile              | varchar(20)    | Mobile phone…                             | "+44 7700 900000"         |                              |
| PhoneEmergency           | varchar(20)    | Emergency contact phone…                  | "+44 7700 900111"         |                              |
| EmploymentType           | varchar(50)    | Employment type…                          | "Full-time"               |                              |
| ContractType             | varchar(50)    | Contract type…                            | "Permanent"               |                              |
| HireDate                 | date           | Start date…                               | 2015-09-01                |                              |
| EndDate                  | date           | End date (if left)…                       | NULL                      |                              |
| WorkLocation             | varchar(100)   | Primary work location…                    | "Main Site"               |                              |
| ReportingManagerId       | int            | Manager staff id…                         | 1                         | staff.Id                     |
| JobTitleId               | int            | Job title id…                             | 3                         | staffjobtitles.Id            |
| JobGroupId               | int            | Job group id…                             | 2                         | staffjobgroups.Id            |
| DepartmentId             | int            | Department id…                            | 5                         | staffdepartments.Id          |
| ProfilePhotoUrl          | varchar(255)   | Profile photo URL…                        | "/img/staff/42.jpg"       |                              |
| AccountStatus            | varchar(20)    | Account status…                           | "Active"                  |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-04-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffdepartments
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Department id…                            | 5                         | (PK)                         |
| Name                     | varchar(100)   | Department name…                          | "Mathematics"             |                              |
| Code                     | varchar(20)    | Department code…                          | "MATH"                    |                              |
| Description              | varchar(255)   | Department description…                   | "Maths department"        |                              |
| HeadUserId               | int            | Head of department user id…               | 10                        | users.Id                     |
| Status                   | varchar(20)    | Status (Active/Closed)…                   | "Active"                  |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffjobtitles
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Job title id…                             | 3                         | (PK)                         |
| Name                     | varchar(100)   | Job title name…                           | "Headteacher"             |                              |
| Description              | varchar(255)   | Job title description…                    | "School leader"           |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffjobgroups
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Job group id…                             | 2                         | (PK)                         |
| Name                     | varchar(100)   | Job group name…                           | "Teaching Staff"          |                              |
| Description              | varchar(255)   | Job group description…                    | "All teaching roles"      |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: stafflocations
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Staff location id…                        | 1                         | (PK)                         |
| Name                     | varchar(100)   | Location name…                            | "Main Campus"             |                              |
| Code                     | varchar(20)    | Location code…                            | "MAIN"                    |                              |
| Description              | varchar(255)   | Location description…                     | "Primary site"            |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffassignments
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Staff assignment id…                      | 1001                      | (PK)                         |
| StaffId                  | int            | Staff member id…                          | 42                        | staff.Id                     |
| LocationId               | int            | Assigned location id…                     | 1                         | stafflocations.Id            |
| StartDate                | date           | Assignment start date…                    | 2026-01-01                |                              |
| EndDate                  | date           | Assignment end date…                      | NULL                      |                              |
| Notes                    | varchar(255)   | Notes about assignment…                   | "Part-time at site B"     |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffschools
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Staff‑school link id…                     | 22                        | (PK)                         |
| StaffId                  | int            | Staff member id…                          | 42                        | staff.Id                     |
| SchoolId                 | int            | School id…                                | 3                         | schools.Id                   |
| StartDate                | date           | Start date at school…                     | 2020-09-01                |                              |
| EndDate                  | date           | End date at school…                       | NULL                      |                              |
| Notes                    | varchar(255)   | Notes…                                    | "Secondment"              |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2020-09-01 08:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffdevices
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Staff device id…                          | 12                        | (PK)                         |
| StaffId                  | int            | Staff owner id…                           | 42                        | staff.Id                     |
| DeviceType               | varchar(100)   | Device type…                              | "Laptop"                  |                              |
| DeviceIdentifier         | varchar(200)   | Serial/asset id…                          | "SN123456"                |                              |
| AssignedDate             | date           | Date issued…                              | 2021-01-15                |                              |
| ReturnedDate             | date           | Date returned…                            | NULL                      |                              |
| Notes                    | varchar(255)   | Notes…                                    | "Damaged screen"          |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2021-01-15 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-02-20 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffdeviceaudit
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Device audit id…                          | 7001                      | (PK)                         |
| StaffDeviceId            | int            | Device id audited…                        | 12                        | staffdevices.Id              |
| StaffId                  | int            | Staff member…                             | 42                        | staff.Id                     |
| Action                   | varchar(100)   | Action performed…                         | "Returned device"         |                              |
| Details                  | varchar(500)   | Additional details…                       | "Returned with charger"   |                              |
| CreatedAt                | datetime       | Audit timestamp…                          | 2026-02-20 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffexternalaccounts
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | External account id…                      | 5                         | (PK)                         |
| StaffId                  | int            | Staff owner id…                           | 42                        | staff.Id                     |
| ExternalSystemId         | int            | External system id…                       | 3                         | externalsystems.Id           |
| ExternalUsername         | varchar(150)   | Username in external system…              | "john.s@google.com"       |                              |
| ExternalUserId           | varchar(150)   | External user id…                         | "1234567890"              |                              |
| Status                   | varchar(50)    | Link status…                              | "Active"                  |                              |
| LastSyncAt               | datetime       | Last sync timestamp…                      | 2026-03-10 12:00:00       |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-03-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-10 12:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffexternalaccountaudit
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | External account audit id…                | 8101                      | (PK)                         |
| StaffExternalAccountId   | int            | External account audited…                 | 5                         | staffexternalaccounts.Id     |
| StaffId                  | int            | Staff member…                             | 42                        | staff.Id                     |
| Action                   | varchar(100)   | Action performed…                         | "Linked Google account"   |                              |
| Details                  | varchar(500)   | Additional details…                       | "Initial sync"            |                              |
| CreatedAt                | datetime       | Audit timestamp…                          | 2026-03-10 12:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffschoolsaudit
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Staff‑school audit id…                    | 9001                      | (PK)                         |
| StaffSchoolId            | int            | Staff‑school link audited…                | 22                        | staffschools.Id              |
| StaffId                  | int            | Staff member…                             | 42                        | staff.Id                     |
| Action                   | varchar(100)   | Action performed…                         | "Assigned to school"      |                              |
| Details                  | varchar(500)   | Additional details…                       | "{\"SchoolId\":3}"        |                              |
| CreatedAt                | datetime       | Audit timestamp…                          | 2026-01-15 09:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffresponsibilities
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Staff responsibility id…                  | 100                       | (PK)                         |
| StaffId                  | int            | Staff member id…                          | 42                        | staff.Id                     |
| ResponsibilityId         | int            | Responsibility id…                        | 3                         | responsibilities.Id          |
| AssignedAt               | datetime       | When assigned…                            | 2026-02-01 09:00:00       |                              |
| AssignedBy               | int            | User who assigned…                        | 1                         | users.Id                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: responsibilities
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Responsibility id…                        | 3                         | (PK)                         |
| Name                     | varchar(100)   | Responsibility name…                      | "Safeguarding Lead"       |                              |
| Code                     | varchar(20)    | Responsibility code…                      | "SAFE_LEAD"               |                              |
| Description              | varchar(255)   | Description…                              | "Leads safeguarding"      |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

# DOMAIN 3 — STUDENTS

Table: yeargroups
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Year group id…                            | 7                         | (PK)                         |
| Name                     | varchar(50)    | Year group name…                          | "Year 7"                  |                              |
| Code                     | varchar(10)    | Short code…                               | "Y7"                      |                              |
| Description              | varchar(255)   | Description…                              | "First KS3 year"          |                              |
| DisplayOrder             | int            | Sort order…                               | 7                         |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: houses
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | House id…                                 | 3                         | (PK)                         |
| Code                     | varchar(50)    | House code…                               | "RED"                     |                              |
| Name                     | varchar(150)   | House name…                               | "Red House"               |                              |
| Description              | varchar(255)   | Description…                              | "Red team"                |                              |
| Colour                   | varchar(50)    | Display colour…                           | "#ff0000"                 |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: phases
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Phase id…                                 | 1                         | (PK)                         |
| Name                     | varchar(100)   | Phase name…                               | "Key Stage 1"             |                              |
| Code                     | varchar(20)    | Phase code…                               | "KS1"                     |                              |
| Description              | varchar(255)   | Description…                              | "Years 1–2"               |                              |
| DisplayOrder             | int            | Sort order…                               | 1                         |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: students
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Student id…                               | 2001                      | (PK)                         |
| AdmissionNumber          | varchar(50)    | Admission number…                         | "ADM12345"                |                              |
| UPN                      | varchar(50)    | Unique pupil number…                      | "UPN123456789"            |                              |
| FirstName                | varchar(100)   | Given name…                               | "Alice"                   |                              |
| LastName                 | varchar(100)   | Family name…                              | "Brown"                   |                              |
| MiddleName               | varchar(100)   | Middle name…                              | "Jane"                    |                              |
| PreferredName            | varchar(100)   | Preferred name…                           | "AJ"                      |                              |
| DateOfBirth              | date           | Birth date…                               | 2012-07-20                |                              |
| Gender                   | varchar(20)    | Gender…                                   | "Female"                  |                              |
| YearGroupId              | int            | Year group id…                            | 7                         | yeargroups.Id                |
| HouseId                  | int            | House id…                                 | 3                         | houses.Id                    |
| IsActive                 | tinyint        | Active enrolment flag…                    | 1                         |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2020-09-01 08:30:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: studentcontacts
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Contact id…                               | 301                       | (PK)                         |
| StudentId                | int            | Student id…                               | 2001                      | students.Id                  |
| ContactName              | varchar(150)   | Contact full name…                        | "Mary Brown"              |                              |
| Relationship             | varchar(100)   | Relationship to student…                  | "Mother"                  |                              |
| Phone                    | varchar(50)    | Contact phone…                            | "+44 7700 900111"         |                              |
| Email                    | varchar(150)   | Contact email…                            | "mary@example.com"        |                              |
| IsPrimary                | tinyint        | Primary contact flag…                     | 1                         |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2020-09-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: studentflags
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Student flag id…                          | 10                        | (PK)                         |
| StudentId                | int            | Student id…                               | 2001                      | students.Id                  |
| FlagCode                 | varchar(100)   | Flag code…                                | "SEN"                     |                              |
| IsActive                 | tinyint        | Whether flag is active…                   | 1                         |                              |
| Notes                    | varchar(255)   | Notes about flag…                         | "SEN support required"    |                              |
| AssignedAt               | datetime       | When flag assigned…                       | 2024-01-10 09:00:00       |                              |
| AssignedBy               | int            | User who assigned…                        | 42                        | users.Id                     |
| CreatedAt                | datetime       | Created timestamp…                        | 2024-01-10 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: studentmedical
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Medical record id…                        | 55                        | (PK)                         |
| StudentId                | int            | Student id…                               | 2001                      | students.Id                  |
| MedicalCondition         | varchar(255)   | Condition name…                           | "Asthma"                  |                              |
| IsAllergic               | tinyint        | Allergy flag…                             | 1                         |                              |
| AllergyDetails           | varchar(255)   | Allergy details…                          | "Peanuts"                 |                              |
| Medication               | varchar(255)   | Medication details…                       | "Inhaler"                 |                              |
| EmergencyActionPlan      | varchar(255)   | Emergency plan summary…                   | "Use inhaler, call 999"   |                              |
| RecordedBy               | int            | User who recorded…                        | 10                        | users.Id                     |
| CreatedAt                | datetime       | Created timestamp…                        | 2023-09-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: studentabsences
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Student absence id…                       | 5001                      | (PK)                         |
| StudentId                | int            | Student id…                               | 2001                      | students.Id                  |
| AbsenceTypeId            | int            | Absence type id…                          | 2                         | absencetypes.Id             |
| Date                     | date           | Absence date…                             | 2026-04-01                |                              |
| StartTime                | time           | Start time (optional)…                    | 09:00:00                  |                              |
| EndTime                  | time           | End time (optional)…                      | 15:15:00                  |                              |
| IsAuthorised             | tinyint        | Authorised flag…                          | 1                         |                              |
| Notes                    | varchar(255)   | Notes…                                    | "Medical appointment"     |                              |
| RecordedBy               | int            | User who recorded…                        | 42                        | users.Id                     |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-04-01 08:30:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-04-01 09:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: studentabsenceaudit
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Absence audit id…                         | 9001                      | (PK)                         |
| StudentId                | int            | Student id…                               | 2001                      | students.Id                  |
| AbsenceId                | int            | Absence id…                               | 5001                      | studentabsences.Id          |
| ChangedBy                | int            | User who changed…                         | 10                        | users.Id                     |
| OldStatusId              | int            | Previous status id…                       | 1                         | absencestatuses.Id          |
| NewStatusId              | int            | New status id…                            | 2                         | absencestatuses.Id          |
| ChangeReason             | varchar(255)   | Reason for change…                        | "Parent provided note"    |                              |
| ChangedAt                | datetime       | When change occurred…                     | 2026-04-02 09:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

# DOMAIN 4 — ATTENDANCE

Table: attendance
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| AttendanceId             | int            | Attendance record id…                     | 10001                     | (PK)                         |
| UserId                   | int            | User associated with attendance…          | 42                        | users.Id                     |
| ClassId                  | int            | Class id (optional)…                      | 4001                      | classes.Id                   |
| Status                   | longtext       | Attendance status…                        | "Present"                 |                              |
| Timestamp                | datetime       | When attendance was recorded…             | 2026-04-10 08:45:00       |                              |
| RecordedBy               | int            | User who recorded…                        | 42                        | users.Id                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: attendanceregisters
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Attendance register id…                   | 5001                      | (PK)                         |
| ClassId                  | bigint         | Class id…                                 | 4001                      | classes.Id                   |
| Date                     | date           | Register date…                            | 2026-04-10                |                              |
| Session                  | longtext       | Session name…                             | "AM"                      |                              |
| OpenedBy                 | bigint         | User who opened register…                 | 42                        | users.Id                     |
| ClosedBy                 | bigint         | User who closed register…                 | 10                        | users.Id                     |
| Status                   | longtext       | Register status…                          | "Closed"                  |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-04-10 08:40:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-04-10 09:10:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: attendancemarks
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Attendance mark id…                       | 7001                      | (PK)                         |
| AttendanceRegisterId     | bigint         | Register id…                              | 5001                      | attendanceregisters.Id       |
| StudentId                | bigint         | Student id…                               | 2001                      | students.Id                  |
| MarkCode                 | text           | Attendance mark code…                     | "L"                       |                              |
| IsLate                   | tinyint        | Late flag…                                | 1                         |                              |
| MinutesLate              | int            | Minutes late…                             | 5                         |                              |
| Notes                    | text           | Notes…                                    | "Traffic delay"           |                              |
| RecordedBy               | bigint         | User who recorded…                        | 42                        | users.Id                     |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-04-10 08:50:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-04-10 09:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: absencecategories
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Absence category id…                      | 1                         | (PK)                         |
| Code                     | varchar(20)    | Category code…                            | "AUTH"                    |                              |
| Name                     | varchar(100)   | Category name…                            | "Authorised"              |                              |
| Description              | varchar(255)   | Description…                              | "Authorised absences"     |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: absencemethods
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Absence method id…                        | 1                         | (PK)                         |
| Code                     | varchar(20)    | Method code…                              | "PHONE"                   |                              |
| Name                     | varchar(100)   | Method name…                              | "Phone Call"              |                              |
| Description              | varchar(255)   | Description…                              | "Reported via phone"      |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: absencereasons
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Absence reason id…                        | 10                        | (PK)                         |
| Code                     | varchar(20)    | Reason code…                              | "ILL"                     |                              |
| Name                     | varchar(100)   | Reason name…                              | "Illness"                 |                              |
| Description              | varchar(255)   | Description…                              | "Student unwell"          |                              |
| Category                 | varchar(50)    | Category grouping…                        | "Medical"                 |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: absencesources
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Absence source id…                        | 3                         | (PK)                         |
| Code                     | varchar(20)    | Source code…                              | "PARENT"                  |                              |
| Name                     | varchar(100)   | Source name…                              | "Parent Reported"         |                              |
| Description              | varchar(255)   | Description…                              | "Reported by parent"      |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: absencestatuses
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Absence status id…                        | 2                         | (PK)                         |
| Code                     | varchar(20)    | Status code…                              | "AUTH"                    |                              |
| Name                     | varchar(100)   | Status name…                              | "Authorised"              |                              |
| Description              | varchar(255)   | Description…                              | "Authorised absence"      |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: absencetypes
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Absence type id…                          | 2                         | (PK)                         |
| Code                     | varchar(20)    | Type code…                                | "MED"                     |                              |
| Name                     | varchar(100)   | Type name…                                | "Medical"                 |                              |
| Description              | varchar(255)   | Description…                              | "Medical absence"         |                              |
| Category                 | varchar(50)    | Category grouping…                        | "Authorised"              |                              |
| IsAuthorised             | tinyint        | Authorised flag…                          | 1                         |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

# DOMAIN 5 — CLASSES & GROUPS

Table: classes
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Class id…                                 | 4001                      | (PK)                         |
| Name                     | varchar(255)   | Class name…                               | "6A"                      |                              |
| Code                     | varchar(100)   | Class code…                               | "CLS6A"                   |                              |
| Description              | varchar(255)   | Description…                              | "Year 6 class A"          |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: classmember
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| ClassId                  | int            | Class id…                                 | 4001                      | classes.Id                   |
| StaffId                  | int            | Staff id…                                 | 42                        | staff.Id                     |
| RoleInClass              | varchar(50)    | Role in class…                            | "Teacher"                 |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: classyeargroupassignments
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Assignment id…                            | 9001                      | (PK)                         |
| ClassId                  | bigint         | Class id…                                 | 4001                      | classes.Id                   |
| YearGroupId              | bigint         | Year group id…                            | 7                         | yeargroups.Id                |
| SchoolId                 | int            | School id…                                | 3                         | schools.Id                   |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

# DOMAIN 6 — DEVICES

Table: devicetypes
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Device type id…                           | 1                         | (PK)                         |
| Name                     | text           | Device type name…                         | "Laptop"                  |                              |
| Code                     | text           | Device type code…                         | "LAPTOP"                  |                              |
| Description              | text           | Description…                              | "Portable computer"       |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffdevices
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Staff device id…                          | 12                        | (PK)                         |
| StaffId                  | int            | Staff owner id…                           | 42                        | staff.Id                     |
| DeviceType               | varchar(100)   | Device type…                              | "Laptop"                  |                              |
| DeviceIdentifier         | varchar(200)   | Serial/asset id…                          | "SN123456"                |                              |
| AssignedDate             | date           | Date issued…                              | 2021-01-15                |                              |
| ReturnedDate             | date           | Date returned…                            | NULL                      |                              |
| Notes                    | varchar(255)   | Notes…                                    | "Damaged screen"          |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2021-01-15 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-02-20 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffdeviceaudit
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Device audit id…                          | 7001                      | (PK)                         |
| StaffDeviceId            | int            | Device id audited…                        | 12                        | staffdevices.Id              |
| StaffId                  | int            | Staff member…                             | 42                        | staff.Id                     |
| Action                   | varchar(100)   | Action performed…                         | "Returned device"         |                              |
| Details                  | varchar(500)   | Additional details…                       | "Returned with charger"   |                              |
| CreatedAt                | datetime       | Audit timestamp…                          | 2026-02-20 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

# DOMAIN 7 — EXTERNAL ACCOUNTS & INTEGRATIONS

Table: externalsystems
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | External system id…                       | 3                         | (PK)                         |
| Name                     | varchar(100)   | System name…                              | "Google Workspace"        |                              |
| Code                     | varchar(20)    | System code…                              | "GOOGLE"                  |                              |
| Description              | varchar(255)   | Description…                              | "Google account sync"     |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffexternalaccounts
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | External account id…                      | 5                         | (PK)                         |
| StaffId                  | int            | Staff owner id…                           | 42                        | staff.Id                     |
| ExternalSystemId         | int            | External system id…                       | 3                         | externalsystems.Id           |
| ExternalUsername         | varchar(150)   | Username in external system…              | "john.s@school.org"       |                              |
| ExternalUserId           | varchar(150)   | External system user id…                  | "1234567890"              |                              |
| Status                   | varchar(50)    | Link status…                              | "Active"                  |                              |
| LastSyncAt               | datetime       | Last sync timestamp…                      | 2026-03-10 12:00:00       |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-03-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-10 12:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: staffexternalaccountaudit
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | External account audit id…                | 8101                      | (PK)                         |
| StaffExternalAccountId   | int            | External account audited…                 | 5                         | staffexternalaccounts.Id     |
| StaffId                  | int            | Staff member…                             | 42                        | staff.Id                     |
| Action                   | varchar(100)   | Action performed…                         | "Linked Google account"   |                              |
| Details                  | varchar(500)   | Additional details…                       | "Initial sync"            |                              |
| CreatedAt                | datetime       | Audit timestamp…                          | 2026-03-10 12:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: accountverificationevents
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Verification event id…                    | 90001                     | (PK)                         |
| UserId                   | bigint         | User id…                                  | 42                        | users.Id                     |
| EventType                | longtext       | Event type…                               | "EMAIL_VERIFIED"          |                              |
| EventTime                | datetime       | When event occurred…                      | 2026-04-01 10:00:00       |                              |
| IpAddress                | longtext       | IP address…                               | "192.168.1.10"            |                              |
| Metadata                 | longtext       | JSON metadata…                            | "{\"browser\":\"Chrome\"}"|                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-04-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

# DOMAIN 8 — USERS, ROLES, PERMISSIONS, AUDIT, MESSAGING & MISC

Table: users
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | User id…                                  | 42                        | (PK)                         |
| Username                 | varchar(100)   | Login username…                           | "jsmith"                  |                              |
| Email                    | varchar(255)   | User email…                               | "jsmith@example.com"      |                              |
| EmailVerifiedAt          | datetime       | Email verification timestamp…             | 2026-03-01 10:00:00       |                              |
| PasswordHash             | varchar(255)   | Password hash…                            | "hashedvalue"             |                              |
| StaffId                  | int            | Linked staff id…                          | 42                        | staff.Id                     |
| Status                   | varchar(50)    | Account status…                           | "Active"                  |                              |
| IsAdmin                  | tinyint        | Admin flag…                               | 0                         |                              |
| LastLoginAt              | datetime       | Last login timestamp…                     | 2026-04-10 08:00:00       |                              |
| LastLoginIp              | varchar(50)    | Last login IP…                            | "192.168.1.10"            |                              |
| LoginCount               | int            | Number of logins…                         | 152                       |                              |
| IsTwoFactorEnabled       | tinyint        | 2FA enabled flag…                         | 1                         |                              |
| TwoFactorSecret          | varchar(255)   | 2FA secret…                               | "ABC123"                  |                              |
| BackupCodes              | varchar(500)   | Backup codes…                             | "code1,code2"             |                              |
| RememberToken            | varchar(255)   | Remember‑me token…                        | "tokenvalue"              |                              |
| Timezone                 | varchar(50)    | User timezone…                            | "Europe/London"           |                              |
| LanguageCode             | varchar(10)    | Preferred language…                       | "en"                      |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: userprofiles
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Profile id…                               | 1001                      | (PK)                         |
| UserId                   | int            | User id…                                  | 42                        | users.Id                     |
| DisplayName              | varchar(150)   | Display name…                             | "John Smith"              |                              |
| ProfilePhotoUrl          | varchar(255)   | Profile photo URL…                        | "/img/users/42.jpg"       |                              |
| Bio                      | varchar(500)   | User bio…                                  | "Teacher and mentor"      |                              |
| Timezone                 | varchar(50)    | Preferred timezone…                       | "Europe/London"           |                              |
| LanguageCode             | varchar(10)    | Preferred language…                       | "en"                      |                              |
| ThemePreference          | varchar(20)    | UI theme…                                  | "dark"                    |                              |
| AccentColor              | varchar(20)    | UI accent colour…                         | "blue"                    |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: roles
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Role id…                                  | 3                         | (PK)                         |
| Name                     | varchar(100)   | Role name…                                | "Teacher"                 |                              |
| Code                     | varchar(50)    | Role code…                                | "TEACHER"                 |                              |
| RoleTypeId               | int            | Role type id…                             | 1                         | roletypes.Id                 |
| Description              | varchar(255)   | Description…                              | "Standard teaching role"  |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: roletypes
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Role type id…                             | 1                         | (PK)                         |
| Name                     | varchar(100)   | Type name…                                | "System Role"             |                              |
| DisplayName              | varchar(150)   | Display name…                             | "System Role"             |                              |
| Description              | varchar(255)   | Description…                              | "Internal system role"    |                              |
| IsSystemRole             | tinyint        | System role flag…                         | 1                         |                              |
| IsDefault                | tinyint        | Default role flag…                        | 0                         |                              |
| Priority                 | int            | Priority order…                           | 1                         |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-01-01 09:00:00       |                              |
| UpdatedAt                | datetime       | Last update timestamp…                    | 2026-03-01 10:00:00       |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: userrole
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | User‑role link id…                        | 2001                      | (PK)                         |
| UserId                   | int            | User id…                                  | 42                        | users.Id                     |
| RoleId                   | int            | Role id…                                  | 3                         | roles.Id                     |
| AssignedAt               | datetime       | When assigned…                            | 2026-01-01 09:00:00       |                              |
| AssignedBy               | int            | User who assigned…                        | 1                         | users.Id                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: rolefeature
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Role‑feature link id…                     | 501                       | (PK)                         |
| RoleId                   | int            | Role id…                                  | 3                         | roles.Id                     |
| FeatureCode              | varchar(100)   | Feature code…                             | "attendance.mark"         | feature.Code                 |
| IsEnabled                | tinyint        | Enabled flag…                             | 1                         |                              |
| AssignedAt               | datetime       | When assigned…                            | 2026-01-01 09:00:00       |                              |
| AssignedBy               | int            | User who assigned…                        | 1                         | users.Id                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: rolemenuitem
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Role‑menu link id…                        | 601                       | (PK)                         |
| RoleId                   | int            | Role id…                                  | 3                         | roles.Id                     |
| MenuItemId               | int            | Menu item id…                             | 10                        | menuitems.Id                 |
| IsEnabled                | tinyint        | Enabled flag…                             | 1                         |                              |
| AssignedAt               | datetime       | When assigned…                            | 2026-01-01 09:00:00       |                              |
| AssignedBy               | int            | User who assigned…                        | 1                         | users.Id                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: userfeatureoverride
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Override id…                              | 701                       | (PK)                         |
| UserId                   | int            | User id…                                  | 42                        | users.Id                     |
| FeatureCode              | varchar(100)   | Feature code…                             | "attendance.mark"         | feature.Code                 |
| IsEnabled                | tinyint        | Override enabled flag…                    | 0                         |                              |
| OverriddenAt             | datetime       | When overridden…                          | 2026-03-01 10:00:00       |                              |
| OverriddenBy             | int            | User who changed…                         | 1                         | users.Id                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: auditlog
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| AuditId                  | int            | Audit id…                                 | 9001                      | (PK)                         |
| UserId                   | int            | User id…                                  | 42                        | users.Id                     |
| Action                   | longtext       | Action description…                       | "Updated profile"         |                              |
| Timestamp                | datetime       | When action occurred…                     | 2026-04-01 10:00:00       |                              |
| UserId1                  | int            | Secondary user id…                        | 10                        | users.Id                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: loginaudit
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | int            | Login audit id…                           | 3001                      | (PK)                         |
| UserId                   | int            | User id…                                  | 42                        | users.Id                     |
| LoginAt                  | datetime       | Login timestamp…                          | 2026-04-10 08:00:00       |                              |
| LoginIp                  | varchar(50)    | Login IP…                                 | "192.168.1.10"            |                              |
| UserAgent                | varchar(255)   | Browser user agent…                       | "Chrome on Windows"       |                              |
| WasSuccessful            | tinyint        | Success flag…                             | 1                         |                              |
| FailureReason            | varchar(255)   | Failure reason…                           | NULL                      |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: messages
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Message id…                               | 8001                      | (PK)                         |
| UserId                   | bigint         | Recipient user id…                        | 42                        | users.Id                     |
| SenderName               | longtext       | Sender name…                              | "Admin"                   |                              |
| Subject                  | longtext       | Message subject…                          | "Welcome"                 |                              |
| Preview                  | longtext       | Preview text…                             | "Your account is ready…"  |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-04-01 09:00:00       |                              |
| IsRead                   | tinyint        | Read flag…                                | 0                         |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+

Table: appnotifications
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Field Name               | Data Type      | Description                               | Example Value             | Links To                     |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
| Id                       | bigint         | Notification id…                          | 6001                      | (PK)                         |
| UserId                   | bigint         | User id…                                  | 42                        | users.Id                     |
| Title                    | longtext       | Notification title…                       | "New Message"             |                              |
| Body                     | longtext       | Notification body…                        | "You have a new message"  |                              |
| CreatedAt                | datetime       | Created timestamp…                        | 2026-04-01 09:00:00       |                              |
| IsRead                   | tinyint        | Read flag…                                | 0                         |                              |
+--------------------------+----------------+-------------------------------------------+---------------------------+------------------------------+
