using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AbsenceApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMySQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

                /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbsenceTypes");

            migrationBuilder.DropTable(
                name: "AccountVerificationEvents");

            migrationBuilder.DropTable(
                name: "AppNotifications");

            migrationBuilder.DropTable(
                name: "Attendance");

            migrationBuilder.DropTable(
                name: "AttendanceMarks");

            migrationBuilder.DropTable(
                name: "AttendanceRegisters");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "ClassMember");

            migrationBuilder.DropTable(
                name: "ClassYearGroupAssignments");

            migrationBuilder.DropTable(
                name: "DeviceTypes");

            migrationBuilder.DropTable(
                name: "ExternalSystems");

            migrationBuilder.DropTable(
                name: "Houses");

            migrationBuilder.DropTable(
                name: "LoginAudit");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Phases");

            migrationBuilder.DropTable(
                name: "Responsibilities");

            migrationBuilder.DropTable(
                name: "role_default_page_permissions");

            migrationBuilder.DropTable(
                name: "role_features");

            migrationBuilder.DropTable(
                name: "RoleChangeAudit");

            migrationBuilder.DropTable(
                name: "RoleTypes");

            migrationBuilder.DropTable(
                name: "StaffAbsenceAudit");

            migrationBuilder.DropTable(
                name: "StaffAbsences");

            migrationBuilder.DropTable(
                name: "StaffAssignmentAudit");

            migrationBuilder.DropTable(
                name: "StaffAssignments");

            migrationBuilder.DropTable(
                name: "StaffDeviceAudit");

            migrationBuilder.DropTable(
                name: "StaffDevices");

            migrationBuilder.DropTable(
                name: "StaffExternalAccountAudit");

            migrationBuilder.DropTable(
                name: "StaffExternalAccounts");

            migrationBuilder.DropTable(
                name: "StudentAbsenceAudit");

            migrationBuilder.DropTable(
                name: "StudentAbsences");

            migrationBuilder.DropTable(
                name: "StudentContacts");

            migrationBuilder.DropTable(
                name: "StudentFlags");

            migrationBuilder.DropTable(
                name: "StudentMedical");

            migrationBuilder.DropTable(
                name: "SystemEvents");

            migrationBuilder.DropTable(
                name: "table_page_settings");

            migrationBuilder.DropTable(
                name: "user_feature_overrides");

            migrationBuilder.DropTable(
                name: "user_page_overrides");

            migrationBuilder.DropTable(
                name: "user_page_permissions");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "YearGroups");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "JobGroups");

            migrationBuilder.DropTable(
                name: "JobTitles");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "features");

            migrationBuilder.DropTable(
                name: "app_pages");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Schools");
        }
    }
}