using AbsenceApp.Core.Interfaces;
using AbsenceApp.Core.Navigation;

namespace AbsenceApp.Client.Services;

public class NavigationMetadataService : INavigationMetadataService
{
    private static readonly List<NavigationItem> _items = new()
    {
        // Dashboard
        new() { Group = "Dashboard", Title = "Operational Overview",      Href = "/dashboard/overview",         Icon = "bi-kanban",        Status = PageStatus.Live,    Description = "KPI cards, absence counts, and today's register summary." },
        new() { Group = "Dashboard", Title = "Attendance & Trends",       Href = "/dashboard/student-activity", Icon = "bi-graph-up",      Status = PageStatus.Live,    Description = "Weekly trend charts and persistent absence tracking." },
        new() { Group = "Dashboard", Title = "Safeguarding & Wellbeing",  Href = "/dashboard/staff-activity",   Icon = "bi-shield-check",  Status = PageStatus.Live,    Description = "Safeguarding flags, medical alerts, and SEND register." },

        // Students
        new() { Group = "Students",  Title = "All Students",    Href = "/students",         Icon = "bi-list-ul",           Status = PageStatus.Live,    Description = "Browse and search all enrolled students." },
        new() { Group = "Students",  Title = "Student Details", Href = "/students/details", Icon = "bi-person-lines-fill", Status = PageStatus.Live,    Description = "View full profile for a selected student." },
        new() { Group = "Students",  Title = "Add Student",     Href = "/students/add",     Icon = "bi-person-plus",       Status = PageStatus.Live,    Description = "Enrol a new student into the system." },

        // Staff
        new() { Group = "Staff",     Title = "All Staff",    Href = "/staff",         Icon = "bi-list-ul",      Status = PageStatus.Live,    Description = "Browse all staff members and their details." },
        new() { Group = "Staff",     Title = "Staff Details", Href = "/staff/details", Icon = "bi-person-vcard", Status = PageStatus.Live,    Description = "View full profile for a selected staff member." },
        new() { Group = "Staff",     Title = "Add Staff",    Href = "/staff/add",     Icon = "bi-person-plus",  Status = PageStatus.Live,    Description = "Add a new staff member to the system." },

        // Parents
        new() { Group = "Parents",   Title = "Parent Contacts", Href = "/parents", Icon = "bi-house-heart", Status = PageStatus.Live,    Description = "View and manage parent and guardian contact records." },

        // Classes
        new() { Group = "Classes",   Title = "All Classes",   Href = "/classes",         Icon = "bi-list-ul",      Status = PageStatus.Live,    Description = "Browse all classes and year groups." },
        new() { Group = "Classes",   Title = "Class Details", Href = "/classes/details", Icon = "bi-journal-text", Status = PageStatus.Live,    Description = "View details and students in a class." },
        new() { Group = "Classes",   Title = "Add Class",     Href = "/classes/add",     Icon = "bi-plus-circle",  Status = PageStatus.Live,    Description = "Create a new class." },

        // Subjects
        new() { Group = "Subjects",  Title = "All Subjects",    Href = "/subjects",     Icon = "bi-book",         Status = PageStatus.Live,    Description = "Browse all subjects taught in the school." },
        new() { Group = "Subjects",  Title = "Subject Details", Href = "/subjects/1",   Icon = "bi-book-half",    Status = PageStatus.Live,    Description = "View details for a specific subject." },
        new() { Group = "Subjects",  Title = "Add Subject",     Href = "/subjects/add", Icon = "bi-plus-circle",  Status = PageStatus.Live,    Description = "Create a new subject record." },

        // Attendance
        new() { Group = "Attendance", Title = "Staff Attendance",   Href = "/attendance/staff",   Icon = "bi-person-check",      Status = PageStatus.Live,    Description = "View class registers and mark staff attendance." },
        new() { Group = "Attendance", Title = "Student Attendance", Href = "/attendance/student", Icon = "bi-person-check-fill", Status = PageStatus.Live,    Description = "Record and review student attendance marks." },
        new() { Group = "Attendance", Title = "Absence Log",        Href = "/attendance/log",     Icon = "bi-pencil-square",     Status = PageStatus.Live,    Description = "Log and review individual student absences." },

        // Audit Log
        new() { Group = "Audit",     Title = "Audit Log", Href = "/auditlog", Icon = "bi-journal-check", Status = PageStatus.Beta, Description = "Review system audit trail and change history." },

        // Settings
        new() { Group = "Settings",  Title = "Table Settings", Href = "/settings/table-settings", Icon = "bi-table",        Status = PageStatus.Live,    Description = "Configure column visibility and row limits per table." },
        new() { Group = "Settings",  Title = "Diagnostics",    Href = "/settings/diagnostics",    Icon = "bi-shield-check", Status = PageStatus.Live,    Description = "Run system health checks and view diagnostic output." },
        new() { Group = "Settings",  Title = "Site Map",       Href = "/settings/site-map",       Icon = "bi-map",          Status = PageStatus.Live,    Description = "Browse all available pages grouped by area." },

        // System Management — Data Management (Super Admin only)
        new() { Group = "System – Data",   Title = "Classes",       Href = "/classes",                  Icon = "bi-door-open",   Status = PageStatus.Live,        Description = "Manage all classes and year group assignments." },
        new() { Group = "System – Data",   Title = "Departments",   Href = "/system/departments",       Icon = "bi-building",    Status = PageStatus.Planned, Description = "Configure school departments." },
        new() { Group = "System – Data",   Title = "Job Titles",    Href = "/system/job-titles",        Icon = "bi-briefcase",   Status = PageStatus.Planned, Description = "Manage staff job title reference data." },
        new() { Group = "System – Data",   Title = "Houses",        Href = "/system/houses",            Icon = "bi-house",       Status = PageStatus.Planned, Description = "Configure student house groups." },
        new() { Group = "System – Data",   Title = "Year Groups",   Href = "/system/year-groups",       Icon = "bi-calendar3",   Status = PageStatus.Planned, Description = "Configure year group reference data." },
        new() { Group = "System – Data",   Title = "Absence Types", Href = "/system/absence-types",     Icon = "bi-x-circle",    Status = PageStatus.Planned, Description = "Manage absence type codes and labels." },
        new() { Group = "System – Data",   Title = "Subjects",      Href = "/subjects",                 Icon = "bi-book",        Status = PageStatus.Live,        Description = "Browse and manage all subjects." },

        // System Management — Security (Super Admin only)
        new() { Group = "System – Security", Title = "Users", Href = "/system/users", Icon = "bi-people", Status = PageStatus.Planned, Description = "Manage system user accounts." },
        new() { Group = "System – Security", Title = "Roles", Href = "/system/roles", Icon = "bi-key",    Status = PageStatus.Planned, Description = "Manage user roles and permissions." },

        // System Management — Global Configuration (Super Admin only)
        new() { Group = "System – Config", Title = "Logo",                  Href = "/system/config/logo",           Icon = "bi-image",               Status = PageStatus.Planned, Description = "Configure the application logo." },
        new() { Group = "System – Config", Title = "Button Styles",         Href = "/system/config/buttons",        Icon = "bi-ui-checks",           Status = PageStatus.Planned, Description = "Customise global button appearance." },
        new() { Group = "System – Config", Title = "Fonts",                 Href = "/system/config/fonts",          Icon = "bi-fonts",               Status = PageStatus.Planned, Description = "Configure application fonts." },
        new() { Group = "System – Config", Title = "Theme Colours",         Href = "/system/config/theme",          Icon = "bi-palette",             Status = PageStatus.Planned, Description = "Customise the colour theme." },
        new() { Group = "System – Config", Title = "Table Layout Defaults", Href = "/system/config/table-defaults", Icon = "bi-table",               Status = PageStatus.Planned, Description = "Set default table display options." },
        new() { Group = "System – Config", Title = "Spacing / Panels / Cards", Href = "/system/config/spacing",     Icon = "bi-distribute-vertical", Status = PageStatus.Planned, Description = "Configure layout spacing and panel styles." },
    };

    public IEnumerable<NavigationItem> GetAll() => _items;

    public IEnumerable<string> GetGroups()
        => _items.Select(i => i.Group).Distinct();

    public IEnumerable<NavigationItem> GetByGroup(string group)
        => _items.Where(i => i.Group == group);
}
