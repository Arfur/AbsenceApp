/*******************************************************
 SECTION 1 — MENU SYSTEM
*******************************************************/

Overview:
The Menu System controls what appears in the left-hand sidebar. It is role-driven and data-driven, using `menuitems` plus `rolemenuitems` and the user’s role assignments to decide which items are visible.

Core Tables:
- menuitems
- rolemenuitems
- menuitemsglobalconfig (for Global Settings / Design System only)
- roles
- userrole

Table: menuitems
- Purpose: Defines every navigation item (categories, menus, submenus).
- Key columns:
  - Id: Primary key.
  - ParentId: Builds hierarchy (null for root categories).
  - ItemType: category | menu | submenu.
  - Label: Text shown in the sidebar.
  - Icon: Icon key/name.
  - Route: Route for navigable items (null for pure group headers).
  - SortOrder: Ordering within parent.
  - IsHidden: If true, item is not shown even if role allows it.
  - Category, GroupName, GroupIcon: Additional grouping metadata.
- Usage:
  - Seeded from CSV and SQL scripts (e.g., aaa_menuitems.csv, E37_StudentNavigationItems.sql).
  - Read by NavigationApiServiceV2 and MenuResolver to build the sidebar tree.

Table: rolemenuitems
- Purpose: Controls which menu items each role can see.
- Key columns:
  - Id: Primary key (NOT auto-increment; must be explicitly set).
  - RoleId: FK → roles.Id.
  - MenuItemId: FK → menuitems.Id.
  - IsEnabled: 1 = visible, 0 = hidden for that role.
  - AssignedAt, AssignedBy: Audit metadata.
- Critical behaviour:
  - Because Id is not auto-increment, any INSERT must supply Id.
  - Using `INSERT IGNORE` without Id will silently insert 0 rows.
- Usage:
  - Acts as the bridge between roles and menuitems.
  - If no row exists for (RoleId, MenuItemId) with IsEnabled=1, the menu item is not shown.

Table: menuitemsglobalconfig
- Purpose: Separate navigation tree for the Global Settings / Design System sidebar.
- Key columns: Similar to menuitems (Id, ParentId, ItemType, Label, Icon, Route, SortOrder, IsHidden, etc.).
- Usage:
  - Used only for the Global Settings / Design System area.
  - Must never be used for People/Attendance/Academics navigation.
  - Queried by NavigationApiServiceV2 for the settings sidebar.

Relationships:
- roles → rolemenuitems → menuitems
- users → userrole → roles → rolemenuitems → menuitems

Data Flow (Menu Resolution):
1. Determine the user’s role(s) via userrole.
2. Use rolemenuitems to find which menuitems are enabled for those roles.
3. Load the matching menuitems.
4. Build a hierarchical tree using ParentId and ItemType.
5. Filter out IsHidden=1 items.
6. Render the sidebar from the resulting tree.

/*******************************************************
 SECTION 2 — NAVIGATION SYSTEM
*******************************************************/

Overview:
The Navigation System is responsible for resolving the sidebar structure dynamically for the current user. It uses raw SQL to join `menuitems`, `rolemenuitems`, and `userrole` to determine which items are visible.

Core Tables:
- menuitems
- rolemenuitems
- userrole
- roles

High-Level Behaviour:
- The system does not hard-code the menu in code.
- Instead, it queries the database to determine which menu items are visible for the current user’s role(s).
- The result is a flat list of menuitems, which is then transformed into a hierarchical structure.

EXAMPLE ONLY — Navigation Role Resolution:
(This shows how the system might resolve a user’s role. It is documentation, not an instruction.)

/*******************************************************
 EXAMPLE ONLY — NOT FOR VS CODE AI — DO NOT EXECUTE
*******************************************************/
-- Resolve the user’s role(s)
SELECT ur.RoleId
FROM userrole ur
WHERE ur.UserId = @UserId;

EXAMPLE ONLY — Navigation Menu Resolution:
(This shows the typical pattern used to resolve visible menu items.)

/*******************************************************
 EXAMPLE ONLY — NOT FOR VS CODE AI — DO NOT EXECUTE
*******************************************************/
-- Resolve visible menu items for a user
SELECT m.*
FROM menuitems m
INNER JOIN rolemenuitems rm ON rm.MenuItemId = m.Id
INNER JOIN userrole ur ON ur.RoleId = rm.RoleId
WHERE ur.UserId = @UserId
  AND rm.IsEnabled = 1
  AND m.IsHidden = 0
ORDER BY m.SortOrder;

Data Flow:
1. Identify the user’s roles via userrole.
2. Join rolemenuitems to find which menuitems are enabled for those roles.
3. Filter out hidden items (IsHidden=0).
4. Order by SortOrder.
5. Build a hierarchical tree in memory (categories → menus → submenus).
6. Return the tree to the client for rendering.

Global Settings Navigation:
- Uses menuitemsglobalconfig instead of menuitems.
- No role-based filtering; access is controlled by higher-level permissions (e.g., super_admin).

/*******************************************************
 SECTION 3 — PAGE REGISTRY (AppPages)
*******************************************************/

Overview:
The Page Registry defines every logical page/route in the system that can be permission-controlled. It is independent of the menu system, but often aligned with it.

Core Table:
- apppages

Table: apppages
- Purpose: Canonical registry of all pages.
- Key columns:
  - Id: Primary key.
  - Name: Human-readable name.
  - Slug: Unique identifier (e.g., "student-profile").
  - Route: The route/path used by the UI (e.g., "/students/profile").
  - CategoryKey: Logical grouping (e.g., "students", "staff").
  - MenuKey: Optional link to menu grouping.
  - IconKey: Optional icon reference.
  - IsActive: Whether the page is active.
  - SortOrder: Ordering in lists.
  - SupportsRead, SupportsWrite, SupportsCreate, SupportsDelete, SupportsImport, SupportsExport: Boolean flags indicating which operations the page conceptually supports.
  - CreatedAt, UpdatedAt: Audit timestamps.

Usage:
- PermissionServiceV2 loads all active pages to build the permission matrix.
- UserManagementService uses apppages to show which pages a user can access.
- The permission UI (PermissionMatrixV2) displays one row per apppage.

Relationships:
- apppages → roledefaultpagepermissions (default role-based CRUD)
- apppages → userpagepermissions (per-user CRUD overrides)
- apppages → userpageoverrides (additional grant/deny overrides, currently not fully used)

Data Flow:
1. Load all active apppages.
2. For each page, load roledefaultpagepermissions for the user’s role.
3. Overlay userpagepermissions if present.
4. Optionally consider userpageoverrides (if/when implemented).
5. Use the resulting permission set to control access to routes and UI actions.

/*******************************************************
 SECTION 4 — PERMISSIONS SYSTEM (CRUD Access Control)
*******************************************************/

Overview:
The Permissions System controls what a user can do on each page: read, write, create, delete, import, export. It is layered on top of the Page Registry and role/user assignments.

Core Tables:
- roledefaultpagepermissions
- userpagepermissions
- userpageoverrides
- roles
- userrole
- apppages

Table: roledefaultpagepermissions
- Purpose: Default CRUD permissions for a given role type and page.
- Key columns:
  - Id: Primary key.
  - RoleTypeName: String identifier of the role (e.g., "super_admin", "admin", "user").
  - PageId: FK → apppages.Id.
  - CanRead, CanWrite, CanCreate, CanDelete, CanImport, CanExport: Boolean flags.
  - CreatedAt: Timestamp.
- Behaviour:
  - Acts as the baseline permission set for users with that role.
  - If no user-specific override exists, these values are used.

Table: userpagepermissions
- Purpose: Per-user explicit CRUD overrides for specific pages.
- Key columns:
  - Id: Primary key.
  - UserId: FK → users.Id.
  - PageId: FK → apppages.Id.
  - CanRead, CanWrite, CanCreate, CanDelete, CanImport, CanExport: Boolean flags.
  - CreatedAt: Timestamp.
- Behaviour:
  - When a row exists for (UserId, PageId), it overrides the roledefaultpagepermissions for that page.

Table: userpageoverrides
- Purpose: Additional override mechanism (Grant/Deny) for specific pages.
- Key columns:
  - Id: Primary key.
  - UserId: FK → users.Id.
  - PageId: FK → apppages.Id.
  - OverrideType: e.g., "Grant" or "Deny".
  - CreatedAt: Timestamp.
- Current status:
  - Loaded in PermissionServiceV2 but not fully applied in the main permission resolution logic.
  - Effectively placeholder / future extension.

Table: roles
- Purpose: Defines roles (e.g., super_admin, admin, user).
- Used by: userrole, rolemenuitems, roledefaultpagepermissions (via RoleTypeName).

Table: userrole
- Purpose: Assigns roles to users.
- Key columns:
  - Id: Primary key.
  - UserId: FK → users.Id.
  - RoleId: FK → roles.Id.
  - AssignedAt, AssignedBy: Audit metadata.
- Behaviour:
  - A user can have one or more roles.
  - Navigation and permissions both rely on this table.

Permission Resolution Order:
1. Check userpagepermissions for (UserId, PageId).
   - If present, use those flags.
2. Otherwise, use roledefaultpagepermissions for (RoleTypeName, PageId).
3. If neither exists, fall back to the page’s SupportsX flags (conceptual capability).
4. userpageoverrides may later be used to force Grant/Deny on top of the above.

Data Flow:
- PermissionServiceV2:
  - Loads apppages.
  - Loads roledefaultpagepermissions for the user’s role.
  - Loads userpagepermissions for the user.
  - Optionally loads userpageoverrides.
  - Produces an effective permission matrix for the user.
- UI:
  - PermissionMatrixV2 displays this matrix for editing.
  - UserFormPageV2 uses it to show and edit user-specific permissions.

/*******************************************************
 SECTION 5 — FEATURE FLAGS (Entitlements)
*******************************************************/

Overview:
Feature flags control whether certain capabilities are enabled globally, per role, or per user. This is separate from page-level CRUD permissions and is used for higher-level toggles (e.g., enabling the sidebar, enabling a module).

Core Tables:
- features
- rolefeature
- userfeatureoverrides

Table: features
- Purpose: Registry of all feature flags.
- Key columns:
  - Id: Primary key.
  - Code: Unique feature code (e.g., "ui.sidebar", "student.view").
  - DisplayName: Human-readable name.
  - Description: Explanation of what the feature does.
  - IsEnabled: Global on/off flag.
  - CreatedAt, UpdatedAt: Audit timestamps.
- Behaviour:
  - If IsEnabled = 0, the feature is globally disabled regardless of role/user overrides.

Table: rolefeature
- Purpose: Assigns features to roles.
- Key columns:
  - Id: Primary key.
  - RoleId: FK → roles.Id.
  - FeatureId: FK → features.Id.
  - IsEnabled: Whether the feature is enabled for that role.
  - CreatedAt: Timestamp.
- Behaviour:
  - If a feature is globally enabled, rolefeature can still disable it for specific roles (depending on resolution logic).

Table: userfeatureoverrides
- Purpose: Per-user feature overrides.
- Key columns:
  - Id: Primary key.
  - UserId: FK → users.Id.
  - FeatureId: FK → features.Id.
  - IsEnabled: Whether the feature is enabled for that user.
  - CreatedAt: Timestamp.
- Behaviour:
  - Typically highest priority: user-specific override can enable/disable a feature regardless of role defaults.

Resolution Logic (Conceptual):
1. Check global feature IsEnabled.
2. Check rolefeature for the user’s role(s).
3. Check userfeatureoverrides for the user.
4. Combine according to defined precedence (usually: user override > role > global).

Usage:
- FeaturePermissionResolver and EntitlementsResolver use these tables to decide:
  - Whether to show certain UI elements.
  - Whether to enable certain modules or flows.

/*******************************************************
 SECTION 6 — USER MANAGEMENT SYSTEM
*******************************************************/

Overview:
The User Management System handles user accounts, roles, profiles, login tracking, and account verification.

Core Tables:
- users
- userprofiles
- roles
- userrole
- loginaudits
- accountverificationevents
- rolechangeaudits

Table: users
- Purpose: Core authentication and account record.
- Key columns (simplified):
  - Id: Primary key.
  - Username, Email, Password: Login credentials.
  - StaffId: Optional FK → staff.Id (links user to staff record).
  - Status: Account status (e.g., active, disabled).
  - IsAdmin: Legacy/admin flag.
  - LastLoginAt, LastLoginIp, LoginCount: Login tracking.
  - Two-factor fields, timezone, language, etc.
  - CreatedAt, UpdatedAt: Audit timestamps.
- Usage:
  - Central to authentication, permissions, and profile.

Table: userprofiles
- Purpose: Extended profile data for users.
- Key columns:
  - Id: Primary key.
  - UserId: FK → users.Id.
  - ProfilePictureUrl, Bio, Timezone, LanguageCode, DisplayName, ThemePreference, AccentColor.
  - CreatedAt, UpdatedAt.
- Usage:
  - Used by UserManagementService to populate profile UI.
  - Allows separation of auth data (users) from profile data (userprofiles).

Table: roles
- Purpose: Defines roles (e.g., super_admin, admin, user).
- Key columns:
  - Id: Primary key.
  - Name, DisplayName, Description.
  - IsSystemRole, IsDefault, Priority.
  - CreatedAt, UpdatedAt.

Table: userrole
- Purpose: Assigns roles to users.
- Key columns:
  - Id: Primary key.
  - UserId: FK → users.Id.
  - RoleId: FK → roles.Id.
  - AssignedAt, AssignedBy.
- Usage:
  - Used by navigation, permissions, and feature resolution.

Table: loginaudits
- Purpose: Records login attempts.
- Key columns:
  - Id: Primary key.
  - UserId: FK → users.Id.
  - LoginTime: Timestamp.
  - IpAddress, UserAgent.
  - Success: Boolean.
- Usage:
  - Displayed in UserFormPageV2 (Login Audit tab).
  - Useful for security and troubleshooting.

Table: accountverificationevents
- Purpose: Tracks account/email verification events.
- Key columns:
  - Id: Primary key.
  - UserId: FK → users.Id.
  - EventType: e.g., "TokenSent", "Verified", "Expired".
  - EventTime: Timestamp.
  - IpAddress, Metadata.
- Usage:
  - Supports verification flows and audit.

Table: rolechangeaudits
- Purpose: Tracks changes to user roles.
- Key columns:
  - Id: Primary key.
  - UserId: FK → users.Id.
  - OldRoleId, NewRoleId: FK → roles.Id.
  - ChangedBy: FK → users.Id (admin who changed it).
  - ChangedAt: Timestamp.
- Usage:
  - Provides an audit trail for role changes.

Relationships:
- users → userprofiles
- users → userrole → roles
- users → loginaudits
- users → accountverificationevents
- users → rolechangeaudits
- users → staff (via StaffId, if linked)

/*******************************************************
 SECTION 7 — STUDENT ABSENCE MANAGEMENT
*******************************************************/

Overview:
Student Absence Management handles recording, updating, and viewing absences for both students and staff, using a unified absences table with a PersonType discriminator.

Core Tables:
- absences
- absencetypes
- absencestatuses
- absenceaudits
- students
- staff

Table: absences
- Purpose: Unified absence record for both students and staff.
- Key columns:
  - Id: Primary key.
  - PersonType: "Student" or "Staff".
  - PersonId: Id of the student or staff member.
  - AbsenceTypeId: FK → absencetypes.Id.
  - StatusId: FK → absencestatuses.Id.
  - StartDate, EndDate: Date range.
  - DurationDays: Calculated duration.
  - ReportedVia: How the absence was reported (Manual/Email/Phone/MIS).
  - Notes: Free text.
  - RecordedBy: FK → users.Id (who recorded it).
  - ApprovedBy: FK → users.Id (who approved it).
  - ApprovedAt: Timestamp.
  - CreatedAt, UpdatedAt.
- Usage:
  - AbsenceService performs CRUD operations.
  - StudentProfileApiServiceV2 exposes absences for the Student Profile UI.
  - Staff absences are shown on staff-related pages.

Table: absencetypes
- Purpose: Lookup of absence categories (Illness, Holiday, etc.).
- Key columns:
  - Id, Code, Name, Category, IsAuthorised, CreatedAt.
- Usage:
  - Used to populate dropdowns in absence forms.
  - Used by AbsenceService to validate absence type.

Table: absencestatuses
- Purpose: Workflow statuses (PENDING, APPROVED, REJECTED, etc.).
- Key columns:
  - Id, Code, Name, IsFinal, CreatedAt.
- Usage:
  - AbsenceService sets initial status (e.g., PENDING).
  - Status changes should be tracked in absenceaudits.

Table: absenceaudits
- Purpose: Audit trail for absence changes.
- Key columns:
  - Id: Primary key.
  - AbsenceId: FK → absences.Id.
  - ChangedBy: FK → users.Id.
  - ChangeType: e.g., Created, Updated, StatusChanged, Deleted.
  - OldStatusId, NewStatusId: FK → absencestatuses.Id (nullable).
  - Notes: Optional.
  - ChangedAt: Timestamp.
- Current status:
  - Entity and mapping exist.
  - AbsenceService does not yet write audit records (implementation gap).

Relationships:
- students → absences (PersonType="Student", PersonId=students.Id).
- staff → absences (PersonType="Staff", PersonId=staff.Id).
- absences → absencetypes.
- absences → absencestatuses.
- absences → absenceaudits.
- users → absenceaudits (ChangedBy).

/*******************************************************
 SECTION 8 — STUDENT PROFILE SYSTEM
*******************************************************/

Overview:
The Student Profile System aggregates multiple related tables to present a complete view of a student: core details, contacts, medical information, flags, classes, and absences.

Core Tables:
- students
- studentcontacts
- studentmedicals
- studentflags
- classmembers
- classyeargroups
- absences (for student absences)
- yeargroups
- houses
- schools

Table: students
- Purpose: Core student record.
- Key columns:
  - Id: Primary key.
  - AdmissionNumber, FirstName, MiddleName, LastName, LegalFirstName, LegalLastName, PreferredName.
  - Gender, DateOfBirth.
  - YearGroupId: FK → yeargroups.Id.
  - ClassId: FK → classes.Id (if present).
  - HouseId: FK → houses.Id.
  - Username, Upn.
  - SchoolId: FK → schools.Id.
  - AdmissionDate, Status.
  - CreatedAt, UpdatedAt.
- Usage:
  - Central to all student-related features.
  - Used by StudentProfilePageV2, StudentCalendarPageV2, and student lists.

Table: studentcontacts
- Purpose: Emergency and guardian contacts.
- Key columns:
  - Id: Primary key.
  - StudentId: FK → students.Id.
  - ContactName, Relationship, Phone, Email.
  - IsPrimary: Boolean.
  - CreatedAt, UpdatedAt.
- Usage:
  - Displayed on StudentProfilePageV2 (Contacts tab).

Table: studentmedicals
- Purpose: Medical records for students.
- Key columns:
  - Id: Primary key.
  - StudentId: FK → students.Id.
  - MedicalCondition, IsAllergic, AllergyDetails, Medication, EmergencyActionPlan.
  - RecordedBy: FK → users.Id (optional).
  - CreatedAt, UpdatedAt.
- Usage:
  - Displayed on StudentProfilePageV2 (Medical tab).

Table: studentflags
- Purpose: Behavioural, welfare, or SEN flags.
- Key columns:
  - Id: Primary key.
  - StudentId: FK → students.Id.
  - FlagCode: Code representing the flag.
  - IsActive: Boolean.
  - Notes, AssignedAt, AssignedBy.
  - CreatedAt, UpdatedAt.
- Usage:
  - Displayed on StudentProfilePageV2 (Flags tab).

Table: classmembers
- Purpose: Links students to classes.
- Key columns:
  - Id: Primary key.
  - ClassId: FK → classes.Id.
  - StudentId: FK → students.Id.
- Usage:
  - Used to determine which students are in which classes.

Table: classyeargroups
- Purpose: Links classes to year groups.
- Key columns:
  - Id: Primary key.
  - ClassId: FK → classes.Id.
  - YearGroupId: FK → yeargroups.Id.
- Usage:
  - Helps derive academic structure.

Relationships:
- students → studentcontacts.
- students → studentmedicals.
- students → studentflags.
- students → absences (PersonType="Student").
- students → yeargroups, houses, schools.
- students → classmembers → classes → classyeargroups → yeargroups.

/*******************************************************
 SECTION 9 — STAFF MANAGEMENT SYSTEM
*******************************************************/

Overview:
The Staff Management System manages staff records, contacts, devices, external accounts, and related audit trails. It also has future tables for richer staff profiles.

Core Tables:
- staff
- staffcontacts
- staffdevices
- staffdeviceaudits
- staffexternalaccounts
- staffexternalaccountsaudits
- staffassignments
- Future profile tables:
  - staffphases
  - staffqualifications
  - staffresponsibilities
  - staffworkingpatterns
  - staffattributes
  - staffattributetypes

Table: staff
- Purpose: Core staff record.
- Key columns:
  - Id: Primary key.
  - StaffNumber, FirstName, LastName, PreferredName, Title.
  - DateOfBirth, Gender.
  - WorkEmail, AltEmail.
  - PhoneHome, PhoneMobile, PhoneEmergency.
  - EmploymentType, ContractType.
  - HireDate, EndDate.
  - WorkLocation.
  - ReportingManagerId: FK → staff.Id (self-reference).
  - JobTitleId, JobGroupId, DepartmentId.
  - ProfilePhotoUrl, AccountStatus.
  - CreatedAt, UpdatedAt.
- Usage:
  - Linked to users via users.StaffId.
  - Used by staff-related pages and services.

Table: staffcontacts
- Purpose: Staff emergency and alternative contacts.
- Key columns:
  - Id: Primary key.
  - StaffId: FK → staff.Id.
  - ContactName, Relationship, Phone, Email, etc.
  - CreatedAt, UpdatedAt.
- Usage:
  - Displayed on staff-related UI (e.g., UserFormPageV2 Contacts tab).

Table: staffdevices
- Purpose: Devices assigned to staff.
- Key columns:
  - Id: Primary key.
  - StaffId: FK → staff.Id.
  - DeviceType, SerialNumber, AssignedDate, ReturnedDate, Status.
- Usage:
  - Displayed on staff device tab.

Table: staffdeviceaudits
- Purpose: Audit trail for staff device assignments/returns.
- Key columns:
  - Id: Primary key.
  - StaffDeviceId: FK → staffdevices.Id (or equivalent).
  - ChangedBy: FK → users.Id.
  - ChangeType, ChangedAt, Notes.
- Usage:
  - Tracks history of device assignments.

Table: staffexternalaccounts
- Purpose: External system accounts linked to staff (e.g., Google, MIS).
- Key columns:
  - Id: Primary key.
  - StaffId: FK → staff.Id.
  - SystemName, SystemCode.
  - AccountUsername, AccountEmail.
  - Status.
  - CreatedAt, UpdatedAt.
- Usage:
  - Displayed on staff external systems tab.

Table: staffexternalaccountsaudits
- Purpose: Audit trail for external account changes.
- Key columns:
  - Id: Primary key.
  - StaffExternalAccountId: FK → staffexternalaccounts.Id.
  - ChangedBy, ChangeType, ChangedAt, Notes.
- Usage:
  - Tracks history of external account changes.

Future Profile Tables (not yet actively used):
- staffphases: Links staff to academic phases.
- staffqualifications: Staff qualifications.
- staffresponsibilities: Responsibilities assigned to staff.
- staffworkingpatterns: Working patterns/schedules.
- staffattributes: Custom attributes for staff.
- staffattributetypes: Types/definitions for staffattributes.

Relationships:
- staff → staffcontacts.
- staff → staffdevices → staffdeviceaudits.
- staff → staffexternalaccounts → staffexternalaccountsaudits.
- staff → users (via users.StaffId).
- staff → future profile tables (for extended profile features).

/*******************************************************
 SECTION 10 — MESSAGING & NOTIFICATIONS
*******************************************************/

Overview:
The Messaging & Notifications subsystem provides internal messages and in-app notifications. The tables exist and are mapped, and header icons reference them, but full UI flows may not yet be implemented.

Core Tables:
- messages
- appnotifications

Table: messages
- Purpose: Internal user-to-user messages.
- Key columns:
  - Id: Primary key.
  - UserId: FK → users.Id (typically sender or recipient depending on design).
  - Title, Body.
  - IsRead: Boolean.
  - CreatedAt: Timestamp.
- Usage:
  - Intended to back the Messages icon in the header.
  - Can be used to show a list of messages and unread counts.

Table: appnotifications
- Purpose: In-app notifications for users.
- Key columns:
  - Id: Primary key.
  - UserId: FK → users.Id.
  - Title, Body.
  - IsRead: Boolean.
  - CreatedAt: Timestamp.
- Usage:
  - Intended to back the Notifications bell in the header.
  - Can be used to show notification lists and unread counts.

Relationships:
- users → messages.
- users → appnotifications.

Status:
- Entities and tables exist.
- Header icons are wired to the concept.
- Full UI pages and flows may still be pending implementation.

/*******************************************************
 SECTION 11 — AUDIT LOGGING
*******************************************************/

Overview:
Audit Logging tracks important events and changes in the system, including absences, logins, and role changes. Some generic audit infrastructure also exists.

Core Tables:
- absenceaudits
- loginaudits
- rolechangeaudits
- auditlog (generic)

Table: absenceaudits
- Purpose: Tracks changes to absences.
- Key columns:
  - Id: Primary key.
  - AbsenceId: FK → absences.Id.
  - ChangedBy: FK → users.Id.
  - ChangeType: e.g., Created, Updated, StatusChanged, Deleted.
  - OldStatusId, NewStatusId: FK → absencestatuses.Id (nullable).
  - Notes.
  - ChangedAt: Timestamp.
- Status:
  - Entity and mapping exist.
  - AbsenceService does not yet write audit records (implementation gap).

Table: loginaudits
- Purpose: Tracks login attempts.
- Key columns:
  - Id: Primary key.
  - UserId: FK → users.Id.
  - LoginTime: Timestamp.
  - IpAddress, UserAgent.
  - Success: Boolean.
- Usage:
  - Displayed in UserFormPageV2 (Login Audit tab).
  - Useful for security monitoring.

Table: rolechangeaudits
- Purpose: Tracks changes to user roles.
- Key columns:
  - Id: Primary key.
  - UserId: FK → users.Id.
  - OldRoleId, NewRoleId: FK → roles.Id.
  - ChangedBy: FK → users.Id.
  - ChangedAt: Timestamp.
- Usage:
  - Provides an audit trail for permission changes.

Table: auditlog
- Purpose: Generic application audit log (entity-agnostic).
- Key columns (inferred):
  - Id: Primary key.
  - Action, EntityType, EntityId.
  - UserId: FK → users.Id.
  - Timestamp.
  - OldValue, NewValue.
- Status:
  - Entity and mapping exist.
  - No active write code found in current implementation.

Relationships:
- users → loginaudits.
- users → rolechangeaudits.
- users → absenceaudits (ChangedBy).
- users → auditlog.
- absences → absenceaudits.

/*******************************************************
 SECTION 12 — LOOKUP TABLES (Reference Data)
*******************************************************/

Overview:
Lookup tables provide static or semi-static reference data used across the system. They are typically small, read-mostly tables that define codes, categories, and structures.

Core Tables:
- absencetypes
- absencestatuses
- devicetypes
- externalsystems
- phases
- roletypes
- houses
- yeargroups
- schools
- (plus other small reference tables as needed)

Table: absencetypes
- Purpose: Defines absence categories.
- Usage:
  - Used by absences.AbsenceTypeId.
  - Used in absence forms and filters.

Table: absencestatuses
- Purpose: Defines absence workflow statuses.
- Usage:
  - Used by absences.StatusId.
  - Used in status dropdowns and workflow logic.

Table: devicetypes
- Purpose: Defines device categories (Laptop, Tablet, etc.).
- Usage:
  - Used by staffdevices to classify devices.

Table: externalsystems
- Purpose: Registry of external systems (MIS, LDAP, Google, etc.).
- Usage:
  - Used by staffexternalaccounts and integration logic.

Table: phases
- Purpose: Academic phases/terms.
- Usage:
  - Linked to staffphases and possibly academic structures.

Table: roletypes
- Purpose: Master role-type lookup (super_admin, admin, user).
- Usage:
  - Used by user management and permission logic.

Table: houses
- Purpose: School houses.
- Usage:
  - Linked to students.HouseId.

Table: yeargroups
- Purpose: Year/grade groups.
- Usage:
  - Linked to students.YearGroupId.
  - Linked via classyeargroups.

Table: schools
- Purpose: School registry.
- Usage:
  - Linked to students.SchoolId.
  - Forms the top-level scope for many entities.

Role of Lookup Tables:
- Provide consistent codes and names.
- Avoid magic strings in code.
- Support filtering, grouping, and validation.

/*******************************************************
 SECTION 13 — FUTURE STAFF PROFILE TABLES
*******************************************************/

Overview:
These tables are intentionally preserved for future expansion of the Staff Profile and User Profile features. They are mapped and present in the schema but not yet actively used in the current services/UI.

Core Tables:
- staffattributes
- staffattributetypes
- staffphases
- staffqualifications
- staffresponsibilities
- staffworkingpatterns

Table: staffattributes
- Purpose: Custom key-value attributes for staff.
- Likely columns:
  - Id: Primary key.
  - StaffId: FK → staff.Id.
  - AttributeTypeId: FK → staffattributetypes.Id.
  - Value: Attribute value.
  - CreatedAt, UpdatedAt.
- Usage:
  - Future: store arbitrary staff metadata (e.g., skills, preferences).

Table: staffattributetypes
- Purpose: Defines types of staff attributes.
- Likely columns:
  - Id: Primary key.
  - Name, Code, Description.
  - CreatedAt, UpdatedAt.
- Usage:
  - Future: drives the schema for staffattributes.

Table: staffphases
- Purpose: Links staff to academic phases.
- Usage:
  - Future: track which phases/terms a staff member is active in.

Table: staffqualifications
- Purpose: Staff qualifications.
- Usage:
  - Future: show qualifications on staff profile.

Table: staffresponsibilities
- Purpose: Responsibilities assigned to staff (e.g., Head of Year).
- Usage:
  - Future: show responsibilities on staff profile and use them in workflows.

Table: staffworkingpatterns
- Purpose: Working patterns/schedules.
- Usage:
  - Future: drive availability, timetabling, and absence calculations.

Planned Role:
- These tables will support a richer Staff Profile DTO including:
  - Department
  - Job Group
  - Job Title
  - Phase
  - Location
  - Responsibilities
  - Custom attributes
- They are intentionally kept in the schema and must not be deleted or renamed.
