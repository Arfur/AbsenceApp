/*
===============================================================================
 File        : seed_fullview_table_settings.sql
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-18
 Updated     : 2026-03-18
-------------------------------------------------------------------------------
 Purpose     : Upserts Table Settings rows for the four Full View projection
               pages (students_full, staff_full, users_full, classes_full).
               Fields are derived exclusively from the matching FullViewDto
               properties — no system fields (id, created_at, updated_at)
               and no invented fields.
-------------------------------------------------------------------------------
 Rules applied:
   - FieldName  = snake_case of the DTO property name
   - DisplayLabel = property name with a space before each capital letter
                    (matching ToDisplayLabel() in TableSettingsService)
   - IsVisible    = 1
   - IsSortable   = 1
   - IsFilterable = 1
   - IsSearchable = 1
   - DisplayOrder = sequential 1-based position matching DTO property order
   - MERGE on (PageName, FieldName) — INSERT if new, UPDATE if exists
-------------------------------------------------------------------------------
 Notes       :
   - Does NOT touch any non-FullView page rows.
   - Safe to re-run: MERGE prevents duplicate rows.
===============================================================================
*/

USE AbsenceApp;
GO

-- ===========================================================================
-- students_full  (15 fields from StudentFullViewDto)
-- Fields: AdmissionNumber, FirstName, MiddleNames, LastName, LegalFirstName,
--         LegalLastName, Gender, DateOfBirth, YearGroupName, ClassName,
--         HouseName, Username, Upn, AdmissionDate, Status
-- ===========================================================================

MERGE dbo.table_page_settings AS target
USING (VALUES
    ('students_full', 'admission_number',  'Admission Number',  1, 1, 1, 1,  1),
    ('students_full', 'first_name',        'First Name',        1, 1, 1, 1,  2),
    ('students_full', 'middle_names',      'Middle Names',      1, 1, 1, 1,  3),
    ('students_full', 'last_name',         'Last Name',         1, 1, 1, 1,  4),
    ('students_full', 'legal_first_name',  'Legal First Name',  1, 1, 1, 1,  5),
    ('students_full', 'legal_last_name',   'Legal Last Name',   1, 1, 1, 1,  6),
    ('students_full', 'gender',            'Gender',            1, 1, 1, 1,  7),
    ('students_full', 'date_of_birth',     'Date Of Birth',     1, 1, 1, 1,  8),
    ('students_full', 'year_group_name',   'Year Group Name',   1, 1, 1, 1,  9),
    ('students_full', 'class_name',        'Class Name',        1, 1, 1, 1, 10),
    ('students_full', 'house_name',        'House Name',        1, 1, 1, 1, 11),
    ('students_full', 'username',          'Username',          1, 1, 1, 1, 12),
    ('students_full', 'upn',               'Upn',               1, 1, 1, 1, 13),
    ('students_full', 'admission_date',    'Admission Date',    1, 1, 1, 1, 14),
    ('students_full', 'status',            'Status',            1, 1, 1, 1, 15)
) AS source (PageName, FieldName, DisplayLabel, IsVisible, IsSortable, IsFilterable, IsSearchable, DisplayOrder)
ON target.PageName = source.PageName AND target.FieldName = source.FieldName
WHEN MATCHED THEN
    UPDATE SET
        DisplayLabel = source.DisplayLabel,
        IsVisible    = source.IsVisible,
        IsSortable   = source.IsSortable,
        IsFilterable = source.IsFilterable,
        IsSearchable = source.IsSearchable,
        DisplayOrder = source.DisplayOrder
WHEN NOT MATCHED THEN
    INSERT (PageName, FieldName, DisplayLabel, IsVisible, IsSortable, IsFilterable, IsSearchable, DisplayOrder)
    VALUES (source.PageName, source.FieldName, source.DisplayLabel, source.IsVisible, source.IsSortable, source.IsFilterable, source.IsSearchable, source.DisplayOrder);

-- ===========================================================================
-- staff_full  (23 fields from StaffFullViewDto)
-- Fields: StaffNumber, FirstName, LastName, PreferredName, Title,
--         DateOfBirth, Gender, WorkEmail, AltEmail, PhoneHome, PhoneMobile,
--         PhoneEmergency, EmploymentType, ContractType, HireDate, EndDate,
--         WorkLocation, ReportingManagerName, JobTitleName, JobGroupName,
--         DepartmentName, ProfilePhotoUrl, AccountStatus
-- ===========================================================================

MERGE dbo.table_page_settings AS target
USING (VALUES
    ('staff_full', 'staff_number',           'Staff Number',            1, 1, 1, 1,  1),
    ('staff_full', 'first_name',             'First Name',              1, 1, 1, 1,  2),
    ('staff_full', 'last_name',              'Last Name',               1, 1, 1, 1,  3),
    ('staff_full', 'preferred_name',         'Preferred Name',          1, 1, 1, 1,  4),
    ('staff_full', 'title',                  'Title',                   1, 1, 1, 1,  5),
    ('staff_full', 'date_of_birth',          'Date Of Birth',           1, 1, 1, 1,  6),
    ('staff_full', 'gender',                 'Gender',                  1, 1, 1, 1,  7),
    ('staff_full', 'work_email',             'Work Email',              1, 1, 1, 1,  8),
    ('staff_full', 'alt_email',              'Alt Email',               1, 1, 1, 1,  9),
    ('staff_full', 'phone_home',             'Phone Home',              1, 1, 1, 1, 10),
    ('staff_full', 'phone_mobile',           'Phone Mobile',            1, 1, 1, 1, 11),
    ('staff_full', 'phone_emergency',        'Phone Emergency',         1, 1, 1, 1, 12),
    ('staff_full', 'employment_type',        'Employment Type',         1, 1, 1, 1, 13),
    ('staff_full', 'contract_type',          'Contract Type',           1, 1, 1, 1, 14),
    ('staff_full', 'hire_date',              'Hire Date',               1, 1, 1, 1, 15),
    ('staff_full', 'end_date',               'End Date',                1, 1, 1, 1, 16),
    ('staff_full', 'work_location',          'Work Location',           1, 1, 1, 1, 17),
    ('staff_full', 'reporting_manager_name', 'Reporting Manager Name',  1, 1, 1, 1, 18),
    ('staff_full', 'job_title_name',         'Job Title Name',          1, 1, 1, 1, 19),
    ('staff_full', 'job_group_name',         'Job Group Name',          1, 1, 1, 1, 20),
    ('staff_full', 'department_name',        'Department Name',         1, 1, 1, 1, 21),
    ('staff_full', 'profile_photo_url',      'Profile Photo Url',       1, 1, 1, 1, 22),
    ('staff_full', 'account_status',         'Account Status',          1, 1, 1, 1, 23)
) AS source (PageName, FieldName, DisplayLabel, IsVisible, IsSortable, IsFilterable, IsSearchable, DisplayOrder)
ON target.PageName = source.PageName AND target.FieldName = source.FieldName
WHEN MATCHED THEN
    UPDATE SET
        DisplayLabel = source.DisplayLabel,
        IsVisible    = source.IsVisible,
        IsSortable   = source.IsSortable,
        IsFilterable = source.IsFilterable,
        IsSearchable = source.IsSearchable,
        DisplayOrder = source.DisplayOrder
WHEN NOT MATCHED THEN
    INSERT (PageName, FieldName, DisplayLabel, IsVisible, IsSortable, IsFilterable, IsSearchable, DisplayOrder)
    VALUES (source.PageName, source.FieldName, source.DisplayLabel, source.IsVisible, source.IsSortable, source.IsFilterable, source.IsSearchable, source.DisplayOrder);

-- ===========================================================================
-- users_full  (23 fields from UserFullViewDto)
-- Fields: Name, Username, Email, EmailVerifiedAt, Status, IsAdmin,
--         ProfilePhotoPath, PhoneNumber, LastLoginAt, LoginCount,
--         RoleTypeName, DepartmentName, Designation, IsTwoFactorEnabled,
--         Timezone, LanguageCode, Bio, DateOfBirth, Gender,
--         Address, City, Country, PostalCode
-- Excluded: Password, RememberToken, TwoFactorSecret, BackupCodes, LastLoginIp
-- ===========================================================================

MERGE dbo.table_page_settings AS target
USING (VALUES
    ('users_full', 'name',                   'Name',                    1, 1, 1, 1,  1),
    ('users_full', 'username',               'Username',                1, 1, 1, 1,  2),
    ('users_full', 'email',                  'Email',                   1, 1, 1, 1,  3),
    ('users_full', 'email_verified_at',      'Email Verified At',       1, 1, 1, 1,  4),
    ('users_full', 'status',                 'Status',                  1, 1, 1, 1,  5),
    ('users_full', 'is_admin',               'Is Admin',                1, 1, 1, 1,  6),
    ('users_full', 'profile_photo_path',     'Profile Photo Path',      1, 1, 1, 1,  7),
    ('users_full', 'phone_number',           'Phone Number',            1, 1, 1, 1,  8),
    ('users_full', 'last_login_at',          'Last Login At',           1, 1, 1, 1,  9),
    ('users_full', 'login_count',            'Login Count',             1, 1, 1, 1, 10),
    ('users_full', 'role_type_name',         'Role Type Name',          1, 1, 1, 1, 11),
    ('users_full', 'department_name',        'Department Name',         1, 1, 1, 1, 12),
    ('users_full', 'designation',            'Designation',             1, 1, 1, 1, 13),
    ('users_full', 'is_two_factor_enabled',  'Is Two Factor Enabled',   1, 1, 1, 1, 14),
    ('users_full', 'timezone',               'Timezone',                1, 1, 1, 1, 15),
    ('users_full', 'language_code',          'Language Code',           1, 1, 1, 1, 16),
    ('users_full', 'bio',                    'Bio',                     1, 1, 1, 1, 17),
    ('users_full', 'date_of_birth',          'Date Of Birth',           1, 1, 1, 1, 18),
    ('users_full', 'gender',                 'Gender',                  1, 1, 1, 1, 19),
    ('users_full', 'address',               'Address',                  1, 1, 1, 1, 20),
    ('users_full', 'city',                   'City',                    1, 1, 1, 1, 21),
    ('users_full', 'country',               'Country',                  1, 1, 1, 1, 22),
    ('users_full', 'postal_code',            'Postal Code',             1, 1, 1, 1, 23)
) AS source (PageName, FieldName, DisplayLabel, IsVisible, IsSortable, IsFilterable, IsSearchable, DisplayOrder)
ON target.PageName = source.PageName AND target.FieldName = source.FieldName
WHEN MATCHED THEN
    UPDATE SET
        DisplayLabel = source.DisplayLabel,
        IsVisible    = source.IsVisible,
        IsSortable   = source.IsSortable,
        IsFilterable = source.IsFilterable,
        IsSearchable = source.IsSearchable,
        DisplayOrder = source.DisplayOrder
WHEN NOT MATCHED THEN
    INSERT (PageName, FieldName, DisplayLabel, IsVisible, IsSortable, IsFilterable, IsSearchable, DisplayOrder)
    VALUES (source.PageName, source.FieldName, source.DisplayLabel, source.IsVisible, source.IsSortable, source.IsFilterable, source.IsSearchable, source.DisplayOrder);

-- ===========================================================================
-- classes_full  (3 fields from ClassFullViewDto)
-- Fields: Name, Code, Description
-- ===========================================================================

MERGE dbo.table_page_settings AS target
USING (VALUES
    ('classes_full', 'name',        'Name',        1, 1, 1, 1, 1),
    ('classes_full', 'code',        'Code',        1, 1, 1, 1, 2),
    ('classes_full', 'description', 'Description', 1, 1, 1, 1, 3)
) AS source (PageName, FieldName, DisplayLabel, IsVisible, IsSortable, IsFilterable, IsSearchable, DisplayOrder)
ON target.PageName = source.PageName AND target.FieldName = source.FieldName
WHEN MATCHED THEN
    UPDATE SET
        DisplayLabel = source.DisplayLabel,
        IsVisible    = source.IsVisible,
        IsSortable   = source.IsSortable,
        IsFilterable = source.IsFilterable,
        IsSearchable = source.IsSearchable,
        DisplayOrder = source.DisplayOrder
WHEN NOT MATCHED THEN
    INSERT (PageName, FieldName, DisplayLabel, IsVisible, IsSortable, IsFilterable, IsSearchable, DisplayOrder)
    VALUES (source.PageName, source.FieldName, source.DisplayLabel, source.IsVisible, source.IsSortable, source.IsFilterable, source.IsSearchable, source.DisplayOrder);

-- ===========================================================================
-- Verify — show the inserted rows grouped by page
-- ===========================================================================

SELECT PageName, COUNT(*) AS FieldCount
FROM   dbo.table_page_settings
WHERE  PageName IN ('students_full', 'staff_full', 'users_full', 'classes_full')
GROUP  BY PageName
ORDER  BY PageName;

SELECT PageName, DisplayOrder, FieldName, DisplayLabel,
       IsVisible, IsSortable, IsFilterable, IsSearchable
FROM   dbo.table_page_settings
WHERE  PageName IN ('students_full', 'staff_full', 'users_full', 'classes_full')
ORDER  BY PageName, DisplayOrder;
GO
