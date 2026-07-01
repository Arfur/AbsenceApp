# AbsenceApp_V2_TreeView.md
This document provides the **complete folder and file structure for AbsenceApp V2**.  
It is used by Claude AI to understand:

- where V2 components, layouts, services, and modules live  
- how V2 is separated from V1  
- where new V2 code must be placed  
- how the design‑system, layout, menu, and token subsystems map to real files  
- how the project is physically organised on disk  

## Important Notes for Claude AI
- **This tree only represents V2**, not V1.  
- **V1 files must never be modified** (they are frozen).  
- **All new development must occur inside the V2 folders shown here.**  
- The structure reflects the real, current state of the application.  
- Use this tree to correctly locate files when generating code, components, services, or documentation.  
- Do not invent new paths — always follow this structure.  
- When adding new V2 components, place them in the correct folder based on this tree.

## Relationship to Other Documents
- The functional and architectural rules are defined in `AbsenceApp_PRD.md`.  
- The token architecture is defined in `AbsenceApp_TokenSystem.md`.  
- The layout system is defined in `AbsenceApp_LayoutV2.md`.  
- The menu system is defined in `AbsenceApp_MenuSystemV2.md`.  
- The design‑system loader is defined in `AbsenceApp_DesignSystemConfigService.md`.  

This tree view shows **where those systems live in the actual codebase**.

---

# V2 Folder Structure
Volume serial number is 0000007B E029:B294
C:.
�   AbsenceAppV2.sln
�   
+---scripts
�       E19_NavigationAuditFixes.sql
�       E32_SystemManagementRoleFeature.sql
�       E33_E15PermissionTables.sql
�       E36_UserProfilesMissingColumns.sql
�       E37_StudentNavigationItems.sql
�       E38_AttendanceStudentsMenu.sql
�       Phase8_Execute.py
�       Phase8_GlobalSettingsMenuSeed.sql
�       
+---src
    +---AbsenceApp.Analyzers
    �   �   AbsenceApp.Analyzers.csproj
    �   �   HeaderCommentAnalyzer.cs
    �   �   HeaderCommentCodeFixProvider.cs
    �   �   
    �   +---bin
    �   �   +---Debug
    �   �   �   +---net8.0-windows10.0.19041.0
    �   �   �   +---netstandard2.0
    �   �   �           AbsenceApp.Analyzers.deps.json
    �   �   �           AbsenceApp.Analyzers.dll
    �   �   �           AbsenceApp.Analyzers.pdb
    �   �   �           
    �   �   +---Release
    �   �       +---netstandard2.0
    �   +---obj
    �       �   AbsenceApp.Analyzers.csproj.nuget.dgspec.json
    �       �   AbsenceApp.Analyzers.csproj.nuget.g.props
    �       �   AbsenceApp.Analyzers.csproj.nuget.g.targets
    �       �   project.assets.json
    �       �   project.nuget.cache
    �       �   
    �       +---Debug
    �       �   +---net8.0-windows10.0.19041.0
    �       �   �   �   AbsenceApp.Analyzers.csproj.FileListAbsolute.txt
    �       �   �   �   
    �       �   �   +---ref
    �       �   �   +---refint
    �       �   +---netstandard2.0
    �       �           .NETStandard,Version=v2.0.AssemblyAttributes.cs
    �       �           AbsenceApp.Analyzers.AssemblyInfo.cs
    �       �           AbsenceApp.Analyzers.AssemblyInfoInputs.cache
    �       �           AbsenceApp.Analyzers.assets.cache
    �       �           AbsenceApp.Analyzers.csproj.AssemblyReference.cache
    �       �           AbsenceApp.Analyzers.csproj.CoreCompileInputs.cache
    �       �           AbsenceApp.Analyzers.csproj.FileListAbsolute.txt
    �       �           AbsenceApp.Analyzers.dll
    �       �           AbsenceApp.Analyzers.GeneratedMSBuildEditorConfig.editorconfig
    �       �           AbsenceApp.Analyzers.pdb
    �       �           AbsenceApp.Analyzers.sourcelink.json
    �       �           
    �       +---Release
    �           +---netstandard2.0
    �                   .NETStandard,Version=v2.0.AssemblyAttributes.cs
    �                   AbsenceApp.Analyzers.AssemblyInfo.cs
    �                   AbsenceApp.Analyzers.AssemblyInfoInputs.cache
    �                   AbsenceApp.Analyzers.assets.cache
    �                   AbsenceApp.Analyzers.csproj.AssemblyReference.cache
    �                   AbsenceApp.Analyzers.GeneratedMSBuildEditorConfig.editorconfig
    �                   
    +---AbsenceApp.Api
    �   �   AbsenceApp.Api.csproj
    �   �   appsettings.json
    �   �   Program.cs
    �   �   
    �   +---bin
    �   �   +---Debug
    �   �   �   +---net8.0
    �   �   �   �   �   AbsenceApp.Api.deps.json
    �   �   �   �   �   AbsenceApp.Api.dll
    �   �   �   �   �   AbsenceApp.Api.exe
    �   �   �   �   �   AbsenceApp.Api.pdb
    �   �   �   �   �   AbsenceApp.Api.runtimeconfig.json
    �   �   �   �   �   AbsenceApp.Core.dll
    �   �   �   �   �   AbsenceApp.Core.pdb
    �   �   �   �   �   AbsenceApp.Data.dll
    �   �   �   �   �   AbsenceApp.Data.pdb
    �   �   �   �   �   appsettings.json
    �   �   �   �   �   Microsoft.AspNetCore.OpenApi.dll
    �   �   �   �   �   Microsoft.EntityFrameworkCore.Abstractions.dll
    �   �   �   �   �   Microsoft.EntityFrameworkCore.dll
    �   �   �   �   �   Microsoft.EntityFrameworkCore.Relational.dll
    �   �   �   �   �   Microsoft.OpenApi.dll
    �   �   �   �   �   MySqlConnector.dll
    �   �   �   �   �   Pomelo.EntityFrameworkCore.MySql.dll
    �   �   �   �   �   Swashbuckle.AspNetCore.Swagger.dll
    �   �   �   �   �   Swashbuckle.AspNetCore.SwaggerGen.dll
    �   �   �   �   �   Swashbuckle.AspNetCore.SwaggerUI.dll
    �   �   �   �   �   
    �   �   �   �   +---runtimes
    �   �   �   �       +---unix
    �   �   �   �       �   +---lib
    �   �   �   �       �       +---net6.0
    �   �   �   �       +---win
    �   �   �   �       �   +---lib
    �   �   �   �       �       +---net6.0
    �   �   �   �       +---win-arm
    �   �   �   �       �   +---native
    �   �   �   �       +---win-arm64
    �   �   �   �       �   +---native
    �   �   �   �       +---win-x64
    �   �   �   �       �   +---native
    �   �   �   �       +---win-x86
    �   �   �   �           +---native
    �   �   �   +---net8.0-windows10.0.19041.0
    �   �   +---Release
    �   �       +---net8.0
    �   +---Configuration
    �   �       FeatureFlagsOptions.cs
    �   �       
    �   +---Data
    �   �   +---Entities
    �   �           Feature.cs
    �   �           RoleFeature.cs
    �   �           UserFeatureOverride.cs
    �   �           
    �   +---obj
    �   �   �   AbsenceApp.Api.csproj.nuget.dgspec.json
    �   �   �   AbsenceApp.Api.csproj.nuget.g.props
    �   �   �   AbsenceApp.Api.csproj.nuget.g.targets
    �   �   �   project.assets.json
    �   �   �   project.nuget.cache
    �   �   �   
    �   �   +---Debug
    �   �   �   +---net8.0
    �   �   �   �   �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    �   �   �   �   �   AbsenceA.4C6F7E25.Up2Date
    �   �   �   �   �   AbsenceApp.Api.AssemblyInfo.cs
    �   �   �   �   �   AbsenceApp.Api.AssemblyInfoInputs.cache
    �   �   �   �   �   AbsenceApp.Api.assets.cache
    �   �   �   �   �   AbsenceApp.Api.csproj.AssemblyReference.cache
    �   �   �   �   �   AbsenceApp.Api.csproj.CoreCompileInputs.cache
    �   �   �   �   �   AbsenceApp.Api.csproj.FileListAbsolute.txt
    �   �   �   �   �   AbsenceApp.Api.dll
    �   �   �   �   �   AbsenceApp.Api.GeneratedMSBuildEditorConfig.editorconfig
    �   �   �   �   �   AbsenceApp.Api.genruntimeconfig.cache
    �   �   �   �   �   AbsenceApp.Api.GlobalUsings.g.cs
    �   �   �   �   �   AbsenceApp.Api.MvcApplicationPartsAssemblyInfo.cache
    �   �   �   �   �   AbsenceApp.Api.MvcApplicationPartsAssemblyInfo.cs
    �   �   �   �   �   AbsenceApp.Api.pdb
    �   �   �   �   �   AbsenceApp.Api.sourcelink.json
    �   �   �   �   �   apphost.exe
    �   �   �   �   �   staticwebassets.build.json
    �   �   �   �   �   
    �   �   �   �   +---ref
    �   �   �   �   �       AbsenceApp.Api.dll
    �   �   �   �   �       
    �   �   �   �   +---refint
    �   �   �   �   �       AbsenceApp.Api.dll
    �   �   �   �   �       
    �   �   �   �   +---staticwebassets
    �   �   �   �           msbuild.build.AbsenceApp.Api.props
    �   �   �   �           msbuild.buildMultiTargeting.AbsenceApp.Api.props
    �   �   �   �           msbuild.buildTransitive.AbsenceApp.Api.props
    �   �   �   �           
    �   �   �   +---net8.0-windows10.0.19041.0
    �   �   �       �   AbsenceApp.Api.csproj.FileListAbsolute.txt
    �   �   �       �   AbsenceApp.Api.GlobalUsings.g.cs
    �   �   �       �   
    �   �   �       +---ref
    �   �   �       +---refint
    �   �   +---Release
    �   �       +---net8.0
    �   �           �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    �   �           �   AbsenceApp.Api.AssemblyInfo.cs
    �   �           �   AbsenceApp.Api.AssemblyInfoInputs.cache
    �   �           �   AbsenceApp.Api.assets.cache
    �   �           �   AbsenceApp.Api.csproj.AssemblyReference.cache
    �   �           �   AbsenceApp.Api.GeneratedMSBuildEditorConfig.editorconfig
    �   �           �   AbsenceApp.Api.GlobalUsings.g.cs
    �   �           �   
    �   �           +---ref
    �   �           +---refint
    �   +---Services
    �       +---Entitlements
    �       �       EntitlementsResolver.cs
    �       �       
    �       +---Navigation
    �               FeaturePermissionResolver.cs
    �               MenuDtos.cs
    �               MenuItemRow.cs
    �               MenuResolver.cs
    �               
    +---AbsenceApp.Client
    �   �   AbsenceApp.Client.csproj
    �   �   App.xaml
    �   �   App.xaml.cs
    �   �   appsettings.Development.json
    �   �   appsettings.json
    �   �   MainPage.xaml
    �   �   MainPage.xaml.cs
    �   �   MauiProgram.cs
    �   �   
    �   +---Components
    �   �   �   AppHost.razor
    �   �   �   Routes.razor
    �   �   �   _Imports.razor
    �   �   �   
    �   �   +---Alerts
    �   �   �       AlertV2.razor
    �   �   �       AlertV2.razor.css
    �   �   �       
    �   �   +---DesignSystem
    �   �   �       ActionButtonV2.razor
    �   �   �       Card.razor
    �   �   �       Card.razor.css
    �   �   �       DropdownV2.razor
    �   �   �       Icon.razor
    �   �   �       Icon.razor.css
    �   �   �       IconButton.razor
    �   �   �       IconButton.razor.css
    �   �   �       SectionHeader.razor
    �   �   �       SectionHeader.razor.css
    �   �   �       
    �   �   +---Pages
    �   �   �   �   Index.razor
    �   �   �   �   Login.razor
    �   �   �   �   Login.razor.css
    �   �   �   �   NotFoundPageV2.razor
    �   �   �   �   NotFoundPageV2.razor.css
    �   �   �   �   
    �   �   �   +---GlobalSettings
    �   �   �       +---AdvanceUI
    �   �   �       �   +---Animation
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---BlockUI
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---BootstrapSlider
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---CountDown
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---CountUp
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Draggable
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Modals
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---OffcanvasToggle
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Prismjs
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Rating
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Scrollbar
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Scrolly
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Slider
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Spinners
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---SweatAlert
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---TooltipPopovers
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Tour
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---TreeView
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---VideoEmbed
    �   �   �       �           Index.razor
    �   �   �       �           
    �   �   �       +---Apps
    �   �   �       �       Index.razor
    �   �   �       �       
    �   �   �       +---Colors
    �   �   �       �       Index.razor
    �   �   �       �       
    �   �   �       +---Dashboard
    �   �   �       �       Index.razor
    �   �   �       �       
    �   �   �       +---Dashboards
    �   �   �       �       Index.razor
    �   �   �       �       
    �   �   �       +---Forms
    �   �   �       �       Index.razor
    �   �   �       �       
    �   �   �       +---Icons
    �   �   �       �   �   Index.razor
    �   �   �       �   �   
    �   �   �       �   +---Fontawesome
    �   �   �       �           Index.razor
    �   �   �       �           
    �   �   �       +---Layouts
    �   �   �       �       Index.razor
    �   �   �       �       
    �   �   �       +---MapCharts
    �   �   �       �   +---Charts
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Map
    �   �   �       �           Index.razor
    �   �   �       �           
    �   �   �       +---Pages
    �   �   �       �       Index.razor
    �   �   �       �       
    �   �   �       +---Spacing
    �   �   �       �       Index.razor
    �   �   �       �       
    �   �   �       +---TableForms
    �   �   �       �   +---BasicTable
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---BorderedTable
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---DarkTable
    �   �   �       �           Index.razor
    �   �   �       �           
    �   �   �       +---Tables
    �   �   �       �       Index.razor
    �   �   �       �       
    �   �   �       +---Typography
    �   �   �       �       Index.razor
    �   �   �       �       
    �   �   �       +---UIKits
    �   �   �       �   +---Accordions
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Alert
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Avatar
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Background
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Badges
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Buttons
    �   �   �       �   �       Index.razor
    �   �   �       �   �       Index.razor.cs
    �   �   �       �   �       Index.razor.css
    �   �   �       �   �       
    �   �   �       �   +---Cards
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Cheatsheet
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Divider
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Dropdown
    �   �   �       �   �       Index.razor
    �   �   �       �   �       Index.razor.cs
    �   �   �       �   �       Index.razor.css
    �   �   �       �   �       
    �   �   �       �   +---Editor
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Grid
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---HelperClasses
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Lists
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Notifications
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Progress
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Ribbons
    �   �   �       �   �       Index.razor
    �   �   �       �   �       
    �   �   �       �   +---Tabs
    �   �   �       �           Index.razor
    �   �   �       �           
    �   �   �       +---Widgets
    �   �   �               Index.razor
    �   �   �               
    �   �   +---Templates
    �   �           BlankPageTemplate.razor
    �   �           DashboardPageTemplate.razor
    �   �           DashboardPageTemplate.razor.css
    �   �           FormPageTemplate.razor
    �   �           FormPageTemplate.razor.css
    �   �           UnderConstructionTemplate.razor
    �   �           
    �   +---Configuration
    �   �       FeatureFlagsOptions.cs
    �   �       
    �   +---docs
    �   �       migration-plan-v2.txt
    �   �       
    �   +---Extensions
    �   �       V2ServiceCollectionExtensions.cs
    �   �       
    �   +---Framework
    �   �   �   _Imports.razor
    �   �   �   
    �   �   +---Components
    �   �   �   +---DesignSystem
    �   �   �           Card.razor
    �   �   �           Card.razor.css
    �   �   �           Icon.razor
    �   �   �           Icon.razor.css
    �   �   �           IconButton.razor
    �   �   �           IconButton.razor.css
    �   �   �           SectionHeader.razor
    �   �   �           SectionHeader.razor.css
    �   �   �           
    �   �   +---Config
    �   �   �   �   branding.json
    �   �   �   �   components.json
    �   �   �   �   icons.json
    �   �   �   �   table-schema.json
    �   �   �   �   theme.json
    �   �   �   �   
    �   �   �   +---Tokens
    �   �   �       �   colors.css
    �   �   �       �   layout.css
    �   �   �       �   radius.css
    �   �   �       �   spacing.css
    �   �   �       �   typography.css
    �   �   �       �   
    �   �   �       +---Buttons
    �   �   �               buttons.action.json
    �   �   �               buttons.active.json
    �   �   �               buttons.basic.json
    �   �   �               buttons.block.json
    �   �   �               buttons.disabled.json
    �   �   �               buttons.group.json
    �   �   �               buttons.icon.json
    �   �   �               buttons.loading.json
    �   �   �               buttons.outline.json
    �   �   �               buttons.radius.json
    �   �   �               buttons.size.json
    �   �   �               buttons.social.json
    �   �   �               buttons.soft.json
    �   �   �               
    �   �   +---Docs
    �   �   �       architecture.md
    �   �   �       design-system.md
    �   �   �       integration-guide.md
    �   �   �       layout-templates.md
    �   �   �       tables.md
    �   �   �       theming-branding.md
    �   �   �       
    �   �   +---Layout
    �   �   �       BaseLayoutV2.razor
    �   �   �       BaseLayoutV2.razor.css
    �   �   �       GlobalErrorBoundaryV2.razor
    �   �   �       GlobalErrorBoundaryV2.razor.css
    �   �   �       GlobalSettingsLayout.razor
    �   �   �       GlobalSettingsLayout.razor.css
    �   �   �       HeaderV2.razor
    �   �   �       HeaderV2.razor.css
    �   �   �       LoginLayout.razor
    �   �   �       MainLayoutV2.razor
    �   �   �       MainLayoutV2.razor.css
    �   �   �       PageHeaderV2.razor
    �   �   �       PageHeaderV2.razor.css
    �   �   �       ScrollSpyV2.razor
    �   �   �       ScrollSpyV2.razor.css
    �   �   �       SidebarV2.razor
    �   �   �       SidebarV2.razor.css
    �   �   �       SplashScreenV2.razor
    �   �   �       SplashScreenV2.razor.css
    �   �   �       
    �   �   +---Models
    �   �   �       BrandingConfigModel.cs
    �   �   �       DetailSectionModel.cs
    �   �   �       FormSectionModel.cs
    �   �   �       SettingsGroupModel.cs
    �   �   �       SettingsItemModel.cs
    �   �   �       TableColumnModel.cs
    �   �   �       ThemeConfigModel.cs
    �   �   �       ThemeMode.cs
    �   �   �       
    �   �   +---PageTemplates
    �   �   �       PageTemplateV2.razor
    �   �   �       PageTemplateV2.razor.css
    �   �   �       
    �   �   +---Services
    �   �   �       BrandingServiceV2.cs
    �   �   �       DesignSystemConfigService.cs
    �   �   �       TableConfigService.cs
    �   �   �       ThemeServiceV2.cs
    �   �   �       
    �   �   +---Tables
    �   �           EmptyStateV2.razor
    �   �           EmptyStateV2.razor.css
    �   �           FilterRowV2.razor
    �   �           FilterRowV2.razor.css
    �   �           InlineActionsV2.razor
    �   �           InlineActionsV2.razor.css
    �   �           PaginationV2.razor
    �   �           PaginationV2.razor.css
    �   �           SearchBarV2.razor
    �   �           SearchBarV2.razor.css
    �   �           SkeletonRowV2.razor
    �   �           SkeletonRowV2.razor.css
    �   �           TableSettingsPageV2.razor
    �   �           TableSettingsPageV2.razor.css
    �   �           TableV2.razor
    �   �           TableV2.razor.css
    �   �           
    �   +---Models
    �   �   +---DataV2
    �   �   �       ApiErrorV2.cs
    �   �   �       ApiResponseV2.cs
    �   �   �       PagedResultV2.cs
    �   �   �       
    �   �   +---TableV2
    �   �   �       TableColumnModel.cs
    �   �   �       
    �   �   +---Theming
    �   �   �       BrandingConfigModel.cs
    �   �   �       ThemeConfigModel.cs
    �   �   �       ThemeMode.cs
    �   �   �       
    �   �   +---V2
    �   �           DetailSectionModel.cs
    �   �           FormSectionModel.cs
    �   �           MenuCategoryModel.cs
    �   �           MenuGroupModel.cs
    �   �           MenuItemModel.cs
    �   �           SettingsGroupModel.cs
    �   �           SettingsItemModel.cs
    �   �           
    �   +---Modules
    �   �   �   PlaceholderPageV2.razor
    �   �   �   _Imports.razor
    �   �   �   
    �   �   +---Attendance
    �   �   �       AttendanceDetailPageV2.razor
    �   �   �       AttendanceDetailPageV2.razor.css
    �   �   �       AttendanceFormPageV2.razor
    �   �   �       AttendanceFormPageV2.razor.css
    �   �   �       AttendanceListPageV2.razor
    �   �   �       AttendanceListPageV2.razor.css
    �   �   �       AttendanceStaffPageV2.razor
    �   �   �       AttendanceStudentPageV2.razor
    �   �   �       
    �   �   +---AuditLog
    �   �   �       AuditLogDetailPageV2.razor
    �   �   �       AuditLogDetailPageV2.razor.css
    �   �   �       AuditLogFormPageV2.razor
    �   �   �       AuditLogFormPageV2.razor.css
    �   �   �       AuditLogListPageV2.razor
    �   �   �       AuditLogListPageV2.razor.css
    �   �   �       
    �   �   +---Classes
    �   �   �       ClassDetailPageV2.razor
    �   �   �       ClassDetailPageV2.razor.css
    �   �   �       ClassesListPageV2.razor
    �   �   �       ClassesListPageV2.razor.css
    �   �   �       ClassFormPageV2.razor
    �   �   �       ClassFormPageV2.razor.css
    �   �   �       
    �   �   +---Dashboard
    �   �   �       DashboardPageV2.razor
    �   �   �       DashboardPageV2.razor.css
    �   �   �       
    �   �   +---Pages
    �   �   �       PageFormPageV2.razor
    �   �   �       PageFormPageV2.razor.css
    �   �   �       PagesListPageV2.razor
    �   �   �       PagesListPageV2.razor.css
    �   �   �       
    �   �   +---Settings
    �   �   �       SettingsDetailPageV2.razor
    �   �   �       SettingsDetailPageV2.razor.css
    �   �   �       SettingsFormPageV2.razor
    �   �   �       SettingsFormPageV2.razor.css
    �   �   �       SettingsListPageV2.razor
    �   �   �       SettingsListPageV2.razor.css
    �   �   �       ThemeSettingPageV2.razor
    �   �   �       
    �   �   +---Staff
    �   �   �       StaffFormPageV2.razor
    �   �   �       StaffFormPageV2.razor.css
    �   �   �       StaffListPageV2.razor
    �   �   �       StaffListPageV2.razor.css
    �   �   �       StaffProfilePageV2.razor
    �   �   �       StaffProfilePageV2.razor.css
    �   �   �       
    �   �   +---Students
    �   �   �       StudentAbsenceFormPageV2.razor
    �   �   �       StudentAbsenceFormPageV2.razor.css
    �   �   �       StudentCalendarPageV2.razor
    �   �   �       StudentCalendarPageV2.razor.css
    �   �   �       StudentProfilePageV2.razor
    �   �   �       StudentProfilePageV2.razor.css
    �   �   �       StudentsListPageV2.razor
    �   �   �       StudentsListPageV2.razor.css
    �   �   �       
    �   �   +---SystemManagement
    �   �   �       AllPagesPage.razor
    �   �   �       PageAccessPage.razor
    �   �   �       PageLayoutsPage.razor
    �   �   �       PageMetadataPage.razor
    �   �   �       PermissionsPage.razor
    �   �   �       RolesPage.razor
    �   �   �       UsersPage.razor
    �   �   �       
    �   �   +---Users
    �   �           UserFormPageV2.razor
    �   �           UserFormPageV2.razor.css
    �   �           UserProfilePageV2.razor
    �   �           UsersListPageV2.razor
    �   �           UsersListPageV2.razor.css
    �   �           
    �   +---Pages
    �   �   +---DesignSystem
    �   +---Platforms
    �   �   +---Android
    �   �   �   �   AndroidManifest.xml
    �   �   �   �   MainActivity.cs
    �   �   �   �   MainApplication.cs
    �   �   �   �   
    �   �   �   +---Resources
    �   �   �       +---values
    �   �   �               colors.xml
    �   �   �               
    �   �   +---iOS
    �   �   �   �   AppDelegate.cs
    �   �   �   �   Info.plist
    �   �   �   �   Program.cs
    �   �   �   �   
    �   �   �   +---Resources
    �   �   �           PrivacyInfo.xcprivacy
    �   �   �           
    �   �   +---MacCatalyst
    �   �   �       AppDelegate.cs
    �   �   �       Entitlements.plist
    �   �   �       Info.plist
    �   �   �       Program.cs
    �   �   �       
    �   �   +---Tizen
    �   �   �       Main.cs
    �   �   �       tizen-manifest.xml
    �   �   �       
    �   �   +---Windows
    �   �           app.manifest
    �   �           App.xaml
    �   �           App.xaml.cs
    �   �           Package.appxmanifest
    �   �           WindowsTrayIcon.cs
    �   �           
    �   +---Properties
    �   �   �   launchSettings.json
    �   �   �   
    �   �   +---PublishProfiles
    �   �           WindowsSelfContained.pubxml
    �   �           
    �   +---Resources
    �   �   +---AppIcon
    �   �   �       appicon.svg
    �   �   �       appiconfg.svg
    �   �   �       
    �   �   +---Fonts
    �   �   �       OpenSans-Regular.ttf
    �   �   �       
    �   �   +---Images
    �   �   �       dotnet_bot.svg
    �   �   �       
    �   �   +---Raw
    �   �   �       AboutAssets.txt
    �   �   �       
    �   �   +---Splash
    �   �           splash.svg
    �   �           
    �   +---Services
    �   �   �   AlertServiceV2.cs
    �   �   �   AppLog.cs
    �   �   �   AppStateService.cs
    �   �   �   EntitlementsService.cs
    �   �   �   ErrorHandlerV2.cs
    �   �   �   IconService.cs
    �   �   �   NavigationServiceV2.cs
    �   �   �   PermissionServiceV2.cs
    �   �   �   
    �   �   +---ApiV2
    �   �   �   �   ApiClientV2.cs
    �   �   �   �   ApiServiceBaseV2.cs
    �   �   �   �   
    �   �   �   +---Modules
    �   �   �           AttendanceApiServiceV2.cs
    �   �   �           AuditLogApiServiceV2.cs
    �   �   �           ClassesApiServiceV2.cs
    �   �   �           DesignTokenApiServiceV2.cs
    �   �   �           FeaturePermissionApiServiceV2.cs
    �   �   �           NavigationApiServiceV2.cs
    �   �   �           PagesApiServiceV2.cs
    �   �   �           ParentsApiServiceV2.cs
    �   �   �           SettingsApiServiceV2.cs
    �   �   �           StaffApiServiceV2.cs
    �   �   �           StaffProfileApiServiceV2.cs
    �   �   �           StudentProfileApiServiceV2.cs
    �   �   �           StudentsApiServiceV2.cs
    �   �   �           UserManagementApiServiceV2.cs
    �   �   �           
    �   �   +---DesignSystem
    �   �   �       DesignSystemService.cs
    �   �   �       IDesignSystemService.cs
    �   �   �       
    �   �   +---TableV2
    �   �   �       TableConfigService.cs
    �   �   �       TableSettingsFileService.cs
    �   �   �       
    �   �   +---Theming
    �   �           BrandingServiceV2.cs
    �   �           ThemeServiceV2.cs
    �   �           
    �   +---Shared
    �   �   �   FilterChip.cs
    �   �   �   TablePageTemplateV2.razor
    �   �   �   TablePageTemplateV2.razor.css
    �   �   �   
    �   �   +---Components
    �   �   �       DesignTokenStyleInjectorV2.razor
    �   �   �       PermissionMatrixV2.razor
    �   �   �       PermissionMatrixV2.razor.css
    �   �   �       ProfileBannerV2.razor
    �   �   �       ProfileBannerV2.razor.css
    �   �   �       ProfileNameSelector.razor
    �   �   �       ProfileNameSelector.razor.css
    �   �   �       ProfileTabsV2.razor
    �   �   �       ProfileTabsV2.razor.css
    �   �   �       
    �   �   +---Templates
    �   �       +---UIKits
    �   �               UiKitsGroup.razor
    �   �               UiKitsGroup.razor.css
    �   �               UiKitsPage.razor
    �   �               UiKitsPage.razor.css
    �   �               
    �   +---State
    �   �   +---V2
    �   �           AppStateStoreV2.cs
    �   �           UiStateV2.cs
    �   �           UserStateV2.cs
    �   �           
    �   +---ViewModels
    �   �   +---V2
    �   �           AttendanceDetailViewModelV2.cs
    �   �           AttendanceFormViewModelV2.cs
    �   �           AttendanceListViewModelV2.cs
    �   �           AuditLogDetailViewModelV2.cs
    �   �           AuditLogListViewModelV2.cs
    �   �           ButtonsEditorViewModelV2.cs
    �   �           ClassDetailViewModelV2.cs
    �   �           ClassesListViewModelV2.cs
    �   �           ClassFormViewModelV2.cs
    �   �           PageFormViewModelV2.cs
    �   �           PagesListViewModelV2.cs
    �   �           SettingsModuleViewModelV2.cs
    �   �           StaffDetailViewModelV2.cs
    �   �           StaffFormViewModelV2.cs
    �   �           StaffListViewModelV2.cs
    �   �           StaffProfileViewModelV2.cs
    �   �           StudentAbsenceFormViewModelV2.cs
    �   �           StudentCalendarViewModelV2.cs
    �   �           StudentDetailViewModelV2.cs
    �   �           StudentFormViewModelV2.cs
    �   �           StudentProfileViewModelV2.cs
    �   �           StudentsListViewModelV2.cs
    �   �           TableSettingsViewModelV2.cs
    �   �           UserFormViewModelV2.cs
    �   �           UserListViewModelV2.cs
    �   �           UserProfileViewModelV2.cs
    �   �           
    �   +---wwwroot
    �       �   favicon.png
    �       �   index.html
    �       �   
    �       +---config
    �       �   +---designsystem
    �       �   �       branding.json
    �       �   �       components.json
    �       �   �       icons.json
    �       �   �       menu.globalsettings.json
    �       �   �       menu.json
    �       �   �       static-components.json
    �       �   �       table-schema.json
    �       �   �       theme.json
    �       �   �       
    �       �   +---global
    �       �   �       buttons.json
    �       �   �       colors.json
    �       �   �       dashboards.json
    �       �   �       forms.json
    �       �   �       tables.json
    �       �   �       typography.json
    �       �   �       
    �       �   +---tablesettings
    �       �           attendance.json
    �       �           auditlog.json
    �       �           classes.json
    �       �           staff.json
    �       �           students.json
    �       �           users.json
    �       �           
    �       +---css
    �           �   app.css
    �           �   GlobalSettings.css
    �           �   
    �           +---bootstrap
    �           �       bootstrap-icons.min.css
    �           �       bootstrap.min.css
    �           �       bootstrap.min.css.map
    �           �       
    �           +---components
    �           �       action-buttons.css
    �           �       buttons.css
    �           �       cards.css
    �           �       dropdown.css
    �           �       forms.css
    �           �       stats.css
    �           �       
    �           +---fonts
    �           �       bootstrap-icons.woff
    �           �       bootstrap-icons.woff2
    �           �       
    �           +---tokens
    �                   colors.css
    �                   components.css
    �                   layout.css
    �                   radius.css
    �                   spacing.css
    �                   typography.css
    �                   
    +---AbsenceApp.Client.Tests
    �   �   AbsenceApp.Client.Tests.csproj
    �   �   GlobalUsings.cs
    �   �   _Imports.razor
    �   �   
    �   +---bin
    �   �   +---Debug
    �   �   �   +---net8.0
    �   �   �   �   �   AbsenceApp.Client.Tests.deps.json
    �   �   �   �   �   AbsenceApp.Client.Tests.dll
    �   �   �   �   �   AbsenceApp.Client.Tests.pdb
    �   �   �   �   �   AbsenceApp.Client.Tests.runtimeconfig.json
    �   �   �   �   �   AbsenceApp.Client.Tests.staticwebassets.runtime.json
    �   �   �   �   �   AbsenceApp.Core.dll
    �   �   �   �   �   AbsenceApp.Core.pdb
    �   �   �   �   �   AngleSharp.Css.dll
    �   �   �   �   �   AngleSharp.Diffing.dll
    �   �   �   �   �   AngleSharp.dll
    �   �   �   �   �   Bunit.Core.dll
    �   �   �   �   �   Bunit.Web.dll
    �   �   �   �   �   Castle.Core.dll
    �   �   �   �   �   CoverletSourceRootsMapping_AbsenceApp.Client.Tests
    �   �   �   �   �   Microsoft.AspNetCore.Authorization.dll
    �   �   �   �   �   Microsoft.AspNetCore.Components.Authorization.dll
    �   �   �   �   �   Microsoft.AspNetCore.Components.dll
    �   �   �   �   �   Microsoft.AspNetCore.Components.Forms.dll
    �   �   �   �   �   Microsoft.AspNetCore.Components.Web.dll
    �   �   �   �   �   Microsoft.AspNetCore.Components.WebAssembly.Authentication.dll
    �   �   �   �   �   Microsoft.AspNetCore.Components.WebAssembly.dll
    �   �   �   �   �   Microsoft.AspNetCore.Metadata.dll
    �   �   �   �   �   Microsoft.Extensions.Caching.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.Caching.Memory.dll
    �   �   �   �   �   Microsoft.Extensions.Configuration.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.Configuration.Binder.dll
    �   �   �   �   �   Microsoft.Extensions.Configuration.dll
    �   �   �   �   �   Microsoft.Extensions.Configuration.FileExtensions.dll
    �   �   �   �   �   Microsoft.Extensions.Configuration.Json.dll
    �   �   �   �   �   Microsoft.Extensions.DependencyInjection.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.DependencyInjection.dll
    �   �   �   �   �   Microsoft.Extensions.FileProviders.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.FileProviders.Physical.dll
    �   �   �   �   �   Microsoft.Extensions.FileSystemGlobbing.dll
    �   �   �   �   �   Microsoft.Extensions.Localization.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.Logging.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.Logging.dll
    �   �   �   �   �   Microsoft.Extensions.Options.dll
    �   �   �   �   �   Microsoft.Extensions.Primitives.dll
    �   �   �   �   �   Microsoft.JSInterop.dll
    �   �   �   �   �   Microsoft.JSInterop.WebAssembly.dll
    �   �   �   �   �   Microsoft.TestPlatform.CommunicationUtilities.dll
    �   �   �   �   �   Microsoft.TestPlatform.CoreUtilities.dll
    �   �   �   �   �   Microsoft.TestPlatform.CrossPlatEngine.dll
    �   �   �   �   �   Microsoft.TestPlatform.PlatformAbstractions.dll
    �   �   �   �   �   Microsoft.TestPlatform.Utilities.dll
    �   �   �   �   �   Microsoft.VisualStudio.CodeCoverage.Shim.dll
    �   �   �   �   �   Microsoft.VisualStudio.TestPlatform.Common.dll
    �   �   �   �   �   Microsoft.VisualStudio.TestPlatform.ObjectModel.dll
    �   �   �   �   �   Moq.dll
    �   �   �   �   �   Newtonsoft.Json.dll
    �   �   �   �   �   NuGet.Frameworks.dll
    �   �   �   �   �   System.Diagnostics.EventLog.dll
    �   �   �   �   �   System.IO.Pipelines.dll
    �   �   �   �   �   testhost.dll
    �   �   �   �   �   testhost.exe
    �   �   �   �   �   xunit.abstractions.dll
    �   �   �   �   �   xunit.assert.dll
    �   �   �   �   �   xunit.core.dll
    �   �   �   �   �   xunit.execution.dotnet.dll
    �   �   �   �   �   xunit.runner.reporters.netcoreapp10.dll
    �   �   �   �   �   xunit.runner.utility.netcoreapp10.dll
    �   �   �   �   �   xunit.runner.visualstudio.testadapter.dll
    �   �   �   �   �   
    �   �   �   �   +---cs
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---de
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---es
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---fr
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---it
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---ja
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---ko
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---pl
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---pt-BR
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---ru
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---runtimes
    �   �   �   �   �   +---win
    �   �   �   �   �       +---lib
    �   �   �   �   �           +---net6.0
    �   �   �   �   �                   System.Diagnostics.EventLog.dll
    �   �   �   �   �                   System.Diagnostics.EventLog.Messages.dll
    �   �   �   �   �                   
    �   �   �   �   +---tr
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---zh-Hans
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---zh-Hant
    �   �   �   �           Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �           Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �           Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �           Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �           Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �           
    �   �   �   +---net8.0-windows10.0.19041.0
    �   �   +---Release
    �   �       +---net8.0
    �   �               CoverletSourceRootsMapping_AbsenceApp.Client.Tests
    �   �               
    �   +---Components
    �   �       ClassDetailsComponent.razor
    �   �       ClassListComponent.razor
    �   �       StudentDetailsComponent.razor
    �   �       StudentsComponent.razor
    �   �       
    �   +---ComponentTests
    �   �       ClassDetailsPageTests.cs
    �   �       ClassListPageTests.cs
    �   �       StudentDetailsPageTests.cs
    �   �       StudentsPageTests.cs
    �   �       
    �   +---obj
    �   �   �   AbsenceApp.Client.Tests.csproj.nuget.dgspec.json
    �   �   �   AbsenceApp.Client.Tests.csproj.nuget.g.props
    �   �   �   AbsenceApp.Client.Tests.csproj.nuget.g.targets
    �   �   �   project.assets.json
    �   �   �   project.nuget.cache
    �   �   �   
    �   �   +---Debug
    �   �   �   +---net8.0
    �   �   �   �   �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    �   �   �   �   �   AbsenceA.1D73B16E.Up2Date
    �   �   �   �   �   AbsenceApp.Client.Tests.AssemblyInfo.cs
    �   �   �   �   �   AbsenceApp.Client.Tests.AssemblyInfoInputs.cache
    �   �   �   �   �   AbsenceApp.Client.Tests.assets.cache
    �   �   �   �   �   AbsenceApp.Client.Tests.csproj.AssemblyReference.cache
    �   �   �   �   �   AbsenceApp.Client.Tests.csproj.CoreCompileInputs.cache
    �   �   �   �   �   AbsenceApp.Client.Tests.csproj.FileListAbsolute.txt
    �   �   �   �   �   AbsenceApp.Client.Tests.dll
    �   �   �   �   �   AbsenceApp.Client.Tests.GeneratedMSBuildEditorConfig.editorconfig
    �   �   �   �   �   AbsenceApp.Client.Tests.genruntimeconfig.cache
    �   �   �   �   �   AbsenceApp.Client.Tests.GlobalUsings.g.cs
    �   �   �   �   �   AbsenceApp.Client.Tests.MvcApplicationPartsAssemblyInfo.cache
    �   �   �   �   �   AbsenceApp.Client.Tests.pdb
    �   �   �   �   �   AbsenceApp.Client.Tests.sourcelink.json
    �   �   �   �   �   staticwebassets.build.json
    �   �   �   �   �   staticwebassets.development.json
    �   �   �   �   �   
    �   �   �   �   +---ref
    �   �   �   �   �       AbsenceApp.Client.Tests.dll
    �   �   �   �   �       
    �   �   �   �   +---refint
    �   �   �   �   �       AbsenceApp.Client.Tests.dll
    �   �   �   �   �       
    �   �   �   �   +---staticwebassets
    �   �   �   �           msbuild.build.AbsenceApp.Client.Tests.props
    �   �   �   �           msbuild.buildMultiTargeting.AbsenceApp.Client.Tests.props
    �   �   �   �           msbuild.buildTransitive.AbsenceApp.Client.Tests.props
    �   �   �   �           
    �   �   �   +---net8.0-windows10.0.19041.0
    �   �   �       �   AbsenceApp.Client.Tests.csproj.FileListAbsolute.txt
    �   �   �       �   AbsenceApp.Client.Tests.GlobalUsings.g.cs
    �   �   �       �   
    �   �   �       +---ref
    �   �   �       +---refint
    �   �   +---Release
    �   �       +---net8.0
    �   �           �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    �   �           �   AbsenceApp.Client.Tests.AssemblyInfo.cs
    �   �           �   AbsenceApp.Client.Tests.AssemblyInfoInputs.cache
    �   �           �   AbsenceApp.Client.Tests.assets.cache
    �   �           �   AbsenceApp.Client.Tests.csproj.AssemblyReference.cache
    �   �           �   AbsenceApp.Client.Tests.GeneratedMSBuildEditorConfig.editorconfig
    �   �           �   AbsenceApp.Client.Tests.GlobalUsings.g.cs
    �   �           �   
    �   �           +---ref
    �   �           +---refint
    �   +---ViewModelTests
    �           AuditLogViewModelTests.cs
    �           DashboardViewModelTests.cs
    �           ParentSubjectViewModelTests.cs
    �           
    +---AbsenceApp.Core
    �   �   AbsenceApp.Core.csproj
    �   �   
    �   +---bin
    �   �   +---Debug
    �   �   �   +---net8.0
    �   �   �           AbsenceApp.Core.deps.json
    �   �   �           AbsenceApp.Core.dll
    �   �   �           AbsenceApp.Core.pdb
    �   �   �           
    �   �   +---Release
    �   �       +---net8.0
    �   +---DTOs
    �   �       AbsenceDto.cs
    �   �       AbsenceStatusDto.cs
    �   �       AbsenceTypeDto.cs
    �   �       AppNotificationDto.cs
    �   �       AttendanceDto.cs
    �   �       AttendanceMarkDto.cs
    �   �       AttendanceRegisterDto.cs
    �   �       AuditLogDto.cs
    �   �       AuthResultDto.cs
    �   �       ClassDto.cs
    �   �       ClassFullViewDto.cs
    �   �       DashboardDto.cs
    �   �       DepartmentDto.cs
    �   �       HouseDto.cs
    �   �       JobTitleDto.cs
    �   �       MessageDto.cs
    �   �       PagesDtos.cs
    �   �       ParentDto.cs
    �   �       ProfileSelectorDtos.cs
    �   �       ProfileSupplementDtos.cs
    �   �       RoleDto.cs
    �   �       StaffDto.cs
    �   �       StaffFullViewDto.cs
    �   �       StudentContactDto.cs
    �   �       StudentDto.cs
    �   �       StudentFullViewDto.cs
    �   �       StudentProfileDtos.cs
    �   �       SubjectDto.cs
    �   �       TablePageSettingDto.cs
    �   �       TableSettingsDiagnosticDto.cs
    �   �       UserDto.cs
    �   �       UserFullViewDto.cs
    �   �       UserManagementDtos.cs
    �   �       YearGroupDto.cs
    �   �       
    �   +---Interfaces
    �   �       IAbsenceService.cs
    �   �       IAbsenceStatusService.cs
    �   �       IAbsenceTypeService.cs
    �   �       IAttendanceRegisterService.cs
    �   �       IAttendanceService.cs
    �   �       IAuditLogService.cs
    �   �       IAuthService.cs
    �   �       IClassFullViewService.cs
    �   �       IClassService.cs
    �   �       IDashboardService.cs
    �   �       IDepartmentService.cs
    �   �       IHouseService.cs
    �   �       IJobTitleService.cs
    �   �       IMessageService.cs
    �   �       INavigationMetadataService.cs
    �   �       INotificationService.cs
    �   �       IPagesService.cs
    �   �       IRoleService.cs
    �   �       IStaffFullViewService.cs
    �   �       IStaffService.cs
    �   �       IStudentContactService.cs
    �   �       IStudentFullViewService.cs
    �   �       IStudentService.cs
    �   �       ISubjectService.cs
    �   �       ITableSettingsService.cs
    �   �       IUserFullViewService.cs
    �   �       IUserManagementService.cs
    �   �       IUserService.cs
    �   �       IYearGroupService.cs
    �   �       
    �   +---Navigation
    �   �       NavigationItem.cs
    �   �       PageStatus.cs
    �   �       
    �   +---obj
    �   �   �   AbsenceApp.Core.csproj.nuget.dgspec.json
    �   �   �   AbsenceApp.Core.csproj.nuget.g.props
    �   �   �   AbsenceApp.Core.csproj.nuget.g.targets
    �   �   �   project.assets.json
    �   �   �   project.nuget.cache
    �   �   �   
    �   �   +---Debug
    �   �   �   +---net8.0
    �   �   �       �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    �   �   �       �   AbsenceApp.Core.AssemblyInfo.cs
    �   �   �       �   AbsenceApp.Core.AssemblyInfoInputs.cache
    �   �   �       �   AbsenceApp.Core.assets.cache
    �   �   �       �   AbsenceApp.Core.csproj.BuildWithSkipAnalyzers
    �   �   �       �   AbsenceApp.Core.csproj.CoreCompileInputs.cache
    �   �   �       �   AbsenceApp.Core.csproj.FileListAbsolute.txt
    �   �   �       �   AbsenceApp.Core.dll
    �   �   �       �   AbsenceApp.Core.GeneratedMSBuildEditorConfig.editorconfig
    �   �   �       �   AbsenceApp.Core.GlobalUsings.g.cs
    �   �   �       �   AbsenceApp.Core.pdb
    �   �   �       �   AbsenceApp.Core.sourcelink.json
    �   �   �       �   
    �   �   �       +---ref
    �   �   �       �       AbsenceApp.Core.dll
    �   �   �       �       
    �   �   �       +---refint
    �   �   �               AbsenceApp.Core.dll
    �   �   �               
    �   �   +---Release
    �   �       +---net8.0
    �   �           �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    �   �           �   AbsenceApp.Core.AssemblyInfo.cs
    �   �           �   AbsenceApp.Core.AssemblyInfoInputs.cache
    �   �           �   AbsenceApp.Core.assets.cache
    �   �           �   AbsenceApp.Core.GeneratedMSBuildEditorConfig.editorconfig
    �   �           �   AbsenceApp.Core.GlobalUsings.g.cs
    �   �           �   
    �   �           +---ref
    �   �           +---refint
    �   +---ViewModels
    �           AttendanceLogViewModel.cs
    �           AttendanceStaffViewModel.cs
    �           AttendanceStudentViewModel.cs
    �           AuditLogViewModel.cs
    �           ClassAddViewModel.cs
    �           ClassDetailsViewModel.cs
    �           ClassesViewModel.cs
    �           DashboardOverviewViewModel.cs
    �           DashboardSafeguardingViewModel.cs
    �           DashboardStudentActivityViewModel.cs
    �           ParentDetailsViewModel.cs
    �           ParentListViewModel.cs
    �           StaffAddViewModel.cs
    �           StaffDetailsViewModel.cs
    �           StaffViewModel.cs
    �           StudentAddViewModel.cs
    �           StudentDetailsViewModel.cs
    �           StudentsViewModel.cs
    �           SubjectAddViewModel.cs
    �           SubjectDetailsViewModel.cs
    �           SubjectListViewModel.cs
    �           TableSettingsViewModel.cs
    �           
    +---AbsenceApp.Data
    �   �   AbsenceApp.Data.csproj
    �   �   DataServiceRegistration.cs
    �   �   rename_tokens.sql
    �   �   
    �   +---bin
    �   �   +---Debug
    �   �   �   +---net8.0
    �   �   �           AbsenceApp.Core.dll
    �   �   �           AbsenceApp.Core.pdb
    �   �   �           AbsenceApp.Data.deps.json
    �   �   �           AbsenceApp.Data.dll
    �   �   �           AbsenceApp.Data.pdb
    �   �   �           
    �   �   +---Release
    �   �       +---net8.0
    �   +---Configurations
    �   �       AttendanceConfiguration.cs
    �   �       AuditLogConfiguration.cs
    �   �       ClassConfiguration.cs
    �   �       ClassMemberConfiguration.cs
    �   �       DesignTokenConfiguration.cs
    �   �       DesignTokenModelBuilderExtensions_Legacy.cs
    �   �       EntitlementsModelBuilderExtensions.cs
    �   �       RoleConfiguration.cs
    �   �       UserConfiguration.cs
    �   �       UserManagementModelBuilderExtensions.cs
    �   �       UserProfileConfiguration.cs
    �   �       UserRoleConfiguration.cs
    �   �       
    �   +---Context
    �   �       AppDbContext.cs
    �   �       
    �   +---Mappers
    �   �       AbsenceMapper.cs
    �   �       AbsenceStatusMapper.cs
    �   �       AbsenceTypeMapper.cs
    �   �       AttendanceMapper.cs
    �   �       AuditLogMapper.cs
    �   �       ClassFullViewMapper.cs
    �   �       ClassMapper.cs
    �   �       DepartmentMapper.cs
    �   �       HouseMapper.cs
    �   �       JobTitleMapper.cs
    �   �       RoleMapper.cs
    �   �       StaffFullViewMapper.cs
    �   �       StaffMapper.cs
    �   �       StudentContactMapper.cs
    �   �       StudentFullViewMapper.cs
    �   �       StudentMapper.cs
    �   �       SubjectMapper.cs
    �   �       UserFullViewMapper.cs
    �   �       UserMapper.cs
    �   �       YearGroupMapper.cs
    �   �       
    �   +---Migrations
    �   �       20260424130330_Baseline.cs
    �   �       20260424130330_Baseline.Designer.cs
    �   �       20260424130406_AddNewAppPagesAndRoleDefaults.cs
    �   �       20260424130406_AddNewAppPagesAndRoleDefaults.Designer.cs
    �   �       20260509002508_UnifiedProfileSchemaV1.cs
    �   �       20260509002508_UnifiedProfileSchemaV1.Designer.cs
    �   �       20260512000624_AddCardDesignTokens.cs
    �   �       20260512000624_AddCardDesignTokens.Designer.cs
    �   �       20260514102105_RenameMenuItemsGlobalConfigToGlobalSettings.cs
    �   �       20260514102105_RenameMenuItemsGlobalConfigToGlobalSettings.Designer.cs
    �   �       20260519000000_AddActionButtonTokens.cs
    �   �       20260519000000_AddActionButtonTokens.Designer.cs
    �   �       20260519112611_AddDropdownTokens.cs
    �   �       20260519112611_AddDropdownTokens.Designer.cs
    �   �       20260601000000_RenameDesignTokensToNewNamingStandard.cs
    �   �       20260601000000_RenameDesignTokensToNewNamingStandard.Designer.cs
    �   �       AppDbContextModelSnapshot.cs
    �   �       
    �   +---Models
    �   �       AbsenceAudit.cs
    �   �       Absences.cs
    �   �       AbsenceStatuses.cs
    �   �       AbsenceTypes.cs
    �   �       AccountVerificationEvent.cs
    �   �       AppNotification.cs
    �   �       AppPage.cs
    �   �       Attendance.cs
    �   �       AuditLog.cs
    �   �       ClassMembers.cs
    �   �       ClassYearGroup.cs
    �   �       DesignToken.cs
    �   �       DeviceType.cs
    �   �       ExternalSystem.cs
    �   �       Feature.cs
    �   �       GlobalConfig.cs
    �   �       House.cs
    �   �       JobGroup.cs
    �   �       JobTitle.cs
    �   �       LoginAudit.cs
    �   �       MenuItem.cs
    �   �       MenuItemsGlobalSettings.cs
    �   �       Message.cs
    �   �       Phase.cs
    �   �       ResponsibilityType.cs
    �   �       Role.cs
    �   �       RoleChangeAudit.cs
    �   �       RoleDefaultPagePermission.cs
    �   �       RoleFeature.cs
    �   �       RoleMenuItem.cs
    �   �       RoleType.cs
    �   �       School.cs
    �   �       Staff.cs
    �   �       StaffAssignment.cs
    �   �       StaffAssignmentAudit.cs
    �   �       StaffAttendance.cs
    �   �       StaffAttribute.cs
    �   �       StaffAttributeType.cs
    �   �       StaffContact.cs
    �   �       StaffDepartment.cs
    �   �       StaffDevice.cs
    �   �       StaffDeviceAudit.cs
    �   �       StaffDuty.cs
    �   �       StaffExternalAccount.cs
    �   �       StaffExternalAccountAudit.cs
    �   �       StaffNote.cs
    �   �       StaffPhase.cs
    �   �       StaffQualification.cs
    �   �       StaffResponsibility.cs
    �   �       StaffWorkingPattern.cs
    �   �       StaffWorkLocation.cs
    �   �       Student.cs
    �   �       StudentContact.cs
    �   �       StudentFlag.cs
    �   �       StudentMedical.cs
    �   �       StudentNote.cs
    �   �       SystemEvent.cs
    �   �       TeachingGroup.cs
    �   �       User.cs
    �   �       UserContact.cs
    �   �       UserDevice.cs
    �   �       UserExternalAccount.cs
    �   �       UserFeatureOverride.cs
    �   �       UserNote.cs
    �   �       UserPageOverride.cs
    �   �       UserPagePermission.cs
    �   �       UserPermissionOverride.cs
    �   �       UserProfile.cs
    �   �       UserRole.cs
    �   �       YearGroup.cs
    �   �       
    �   +---obj
    �   �   �   AbsenceApp.Data.csproj.nuget.dgspec.json
    �   �   �   AbsenceApp.Data.csproj.nuget.g.props
    �   �   �   AbsenceApp.Data.csproj.nuget.g.targets
    �   �   �   project.assets.json
    �   �   �   project.nuget.cache
    �   �   �   
    �   �   +---Debug
    �   �   �   +---net8.0
    �   �   �       �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    �   �   �       �   AbsenceA.5210A7AE.Up2Date
    �   �   �       �   AbsenceApp.Data.AssemblyInfo.cs
    �   �   �       �   AbsenceApp.Data.AssemblyInfoInputs.cache
    �   �   �       �   AbsenceApp.Data.assets.cache
    �   �   �       �   AbsenceApp.Data.csproj.AssemblyReference.cache
    �   �   �       �   AbsenceApp.Data.csproj.BuildWithSkipAnalyzers
    �   �   �       �   AbsenceApp.Data.csproj.CoreCompileInputs.cache
    �   �   �       �   AbsenceApp.Data.csproj.FileListAbsolute.txt
    �   �   �       �   AbsenceApp.Data.dll
    �   �   �       �   AbsenceApp.Data.GeneratedMSBuildEditorConfig.editorconfig
    �   �   �       �   AbsenceApp.Data.GlobalUsings.g.cs
    �   �   �       �   AbsenceApp.Data.pdb
    �   �   �       �   AbsenceApp.Data.sourcelink.json
    �   �   �       �   
    �   �   �       +---ref
    �   �   �       �       AbsenceApp.Data.dll
    �   �   �       �       
    �   �   �       +---refint
    �   �   �               AbsenceApp.Data.dll
    �   �   �               
    �   �   +---Release
    �   �       +---net8.0
    �   �           �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    �   �           �   AbsenceApp.Data.AssemblyInfo.cs
    �   �           �   AbsenceApp.Data.AssemblyInfoInputs.cache
    �   �           �   AbsenceApp.Data.assets.cache
    �   �           �   AbsenceApp.Data.csproj.AssemblyReference.cache
    �   �           �   AbsenceApp.Data.GeneratedMSBuildEditorConfig.editorconfig
    �   �           �   AbsenceApp.Data.GlobalUsings.g.cs
    �   �           �   
    �   �           +---ref
    �   �           +---refint
    �   +---Repositories
    �   �       AbsenceRepository.cs
    �   �       AbsenceStatusRepository.cs
    �   �       AbsenceTypeRepository.cs
    �   �       AttendanceRepository.cs
    �   �       AuditLogRepository.cs
    �   �       ClassRepository.cs
    �   �       DepartmentRepository.cs
    �   �       HouseRepository.cs
    �   �       IAttendanceRepository.cs
    �   �       IAuditLogRepository.cs
    �   �       IClassRepository.cs
    �   �       IRoleRepository.cs
    �   �       IStudentRepository.cs
    �   �       IUserRepository.cs
    �   �       JobTitleRepository.cs
    �   �       RoleRepository.cs
    �   �       StaffRepository.cs
    �   �       StudentContactRepository.cs
    �   �       StudentFlagRepository.cs
    �   �       StudentMedicalRepository.cs
    �   �       StudentRepository.cs
    �   �       UserRepository.cs
    �   �       YearGroupRepository.cs
    �   �       
    �   +---Seeding
    �   �       CsvImportPipeline.cs
    �   �       DatabaseSeeder.cs
    �   �       
    �   +---Services
    �           AbsenceService.cs
    �           AbsenceStatusService.cs
    �           AbsenceTypeService.cs
    �           AttendanceService.cs
    �           AuditLogService.cs
    �           AuthService.cs
    �           ClassFullViewService.cs
    �           ClassService.cs
    �           DashboardService.cs
    �           DepartmentService.cs
    �           HouseService.cs
    �           JobTitleService.cs
    �           MessageService.cs
    �           NotificationService.cs
    �           PagesService.cs
    �           RoleService.cs
    �           StaffFullViewService.cs
    �           StaffService.cs
    �           StudentContactService.cs
    �           StudentFullViewService.cs
    �           StudentService.cs
    �           SubjectService.cs
    �           TableSettingsService.cs
    �           UserFullViewService.cs
    �           UserManagementService.cs
    �           UserService.cs
    �           YearGroupService.cs
    �           
    +---AbsenceApp.EfHost
    �   �   AbsenceApp.EfHost.csproj
    �   �   AppDbContextFactory.cs
    �   �   Program.cs
    �   �   
    �   +---bin
    �   �   +---Debug
    �   �   �   +---net8.0
    �   �   �   �   �   AbsenceApp.Core.dll
    �   �   �   �   �   AbsenceApp.Core.pdb
    �   �   �   �   �   AbsenceApp.Data.dll
    �   �   �   �   �   AbsenceApp.Data.pdb
    �   �   �   �   �   AbsenceApp.EfHost.deps.json
    �   �   �   �   �   AbsenceApp.EfHost.dll
    �   �   �   �   �   AbsenceApp.EfHost.exe
    �   �   �   �   �   AbsenceApp.EfHost.pdb
    �   �   �   �   �   AbsenceApp.EfHost.runtimeconfig.json
    �   �   �   �   �   BCrypt.Net-Next.dll
    �   �   �   �   �   Humanizer.dll
    �   �   �   �   �   Microsoft.Bcl.AsyncInterfaces.dll
    �   �   �   �   �   Microsoft.CodeAnalysis.CSharp.dll
    �   �   �   �   �   Microsoft.CodeAnalysis.CSharp.Workspaces.dll
    �   �   �   �   �   Microsoft.CodeAnalysis.dll
    �   �   �   �   �   Microsoft.CodeAnalysis.Workspaces.dll
    �   �   �   �   �   Microsoft.EntityFrameworkCore.Abstractions.dll
    �   �   �   �   �   Microsoft.EntityFrameworkCore.Design.dll
    �   �   �   �   �   Microsoft.EntityFrameworkCore.dll
    �   �   �   �   �   Microsoft.EntityFrameworkCore.Relational.dll
    �   �   �   �   �   Microsoft.Extensions.Caching.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.Caching.Memory.dll
    �   �   �   �   �   Microsoft.Extensions.Configuration.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.DependencyInjection.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.DependencyInjection.dll
    �   �   �   �   �   Microsoft.Extensions.DependencyModel.dll
    �   �   �   �   �   Microsoft.Extensions.Logging.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.Logging.dll
    �   �   �   �   �   Microsoft.Extensions.Options.dll
    �   �   �   �   �   Microsoft.Extensions.Primitives.dll
    �   �   �   �   �   Mono.TextTemplating.dll
    �   �   �   �   �   MySqlConnector.dll
    �   �   �   �   �   Pomelo.EntityFrameworkCore.MySql.dll
    �   �   �   �   �   System.CodeDom.dll
    �   �   �   �   �   System.Composition.AttributedModel.dll
    �   �   �   �   �   System.Composition.Convention.dll
    �   �   �   �   �   System.Composition.Hosting.dll
    �   �   �   �   �   System.Composition.Runtime.dll
    �   �   �   �   �   System.Composition.TypedParts.dll
    �   �   �   �   �   System.IO.Pipelines.dll
    �   �   �   �   �   
    �   �   �   �   +---cs
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.Workspaces.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---de
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.Workspaces.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---es
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.Workspaces.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---fr
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.Workspaces.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---it
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.Workspaces.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---ja
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.Workspaces.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---ko
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.Workspaces.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---pl
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.Workspaces.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---pt-BR
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.Workspaces.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---ru
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.Workspaces.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---runtimes
    �   �   �   �   �   +---unix
    �   �   �   �   �   �   +---lib
    �   �   �   �   �   �       +---net6.0
    �   �   �   �   �   +---win
    �   �   �   �   �   �   +---lib
    �   �   �   �   �   �       +---net6.0
    �   �   �   �   �   +---win-arm
    �   �   �   �   �   �   +---native
    �   �   �   �   �   +---win-arm64
    �   �   �   �   �   �   +---native
    �   �   �   �   �   +---win-x64
    �   �   �   �   �   �   +---native
    �   �   �   �   �   +---win-x86
    �   �   �   �   �       +---native
    �   �   �   �   +---tr
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.Workspaces.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---zh-Hans
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.resources.dll
    �   �   �   �   �       Microsoft.CodeAnalysis.Workspaces.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---zh-Hant
    �   �   �   �           Microsoft.CodeAnalysis.CSharp.resources.dll
    �   �   �   �           Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    �   �   �   �           Microsoft.CodeAnalysis.resources.dll
    �   �   �   �           Microsoft.CodeAnalysis.Workspaces.resources.dll
    �   �   �   �           
    �   �   �   +---net8.0-windows10.0.19041.0
    �   �   +---Release
    �   �       +---net8.0
    �   +---obj
    �       �   AbsenceApp.EfHost.csproj.nuget.dgspec.json
    �       �   AbsenceApp.EfHost.csproj.nuget.g.props
    �       �   AbsenceApp.EfHost.csproj.nuget.g.targets
    �       �   project.assets.json
    �       �   project.nuget.cache
    �       �   
    �       +---Debug
    �       �   +---net8.0
    �       �   �   �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    �       �   �   �   AbsenceA.F287B7D3.Up2Date
    �       �   �   �   AbsenceApp.EfHost.AssemblyInfo.cs
    �       �   �   �   AbsenceApp.EfHost.AssemblyInfoInputs.cache
    �       �   �   �   AbsenceApp.EfHost.assets.cache
    �       �   �   �   AbsenceApp.EfHost.csproj.AssemblyReference.cache
    �       �   �   �   AbsenceApp.EfHost.csproj.CoreCompileInputs.cache
    �       �   �   �   AbsenceApp.EfHost.csproj.FileListAbsolute.txt
    �       �   �   �   AbsenceApp.EfHost.dll
    �       �   �   �   AbsenceApp.EfHost.GeneratedMSBuildEditorConfig.editorconfig
    �       �   �   �   AbsenceApp.EfHost.genruntimeconfig.cache
    �       �   �   �   AbsenceApp.EfHost.GlobalUsings.g.cs
    �       �   �   �   AbsenceApp.EfHost.pdb
    �       �   �   �   AbsenceApp.EfHost.sourcelink.json
    �       �   �   �   apphost.exe
    �       �   �   �   
    �       �   �   +---ref
    �       �   �   �       AbsenceApp.EfHost.dll
    �       �   �   �       
    �       �   �   +---refint
    �       �   �           AbsenceApp.EfHost.dll
    �       �   �           
    �       �   +---net8.0-windows10.0.19041.0
    �       �       �   AbsenceApp.EfHost.csproj.FileListAbsolute.txt
    �       �       �   AbsenceApp.EfHost.GlobalUsings.g.cs
    �       �       �   
    �       �       +---ref
    �       �       +---refint
    �       +---Release
    �           +---net8.0
    �               �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    �               �   AbsenceApp.EfHost.AssemblyInfo.cs
    �               �   AbsenceApp.EfHost.AssemblyInfoInputs.cache
    �               �   AbsenceApp.EfHost.assets.cache
    �               �   AbsenceApp.EfHost.csproj.AssemblyReference.cache
    �               �   AbsenceApp.EfHost.GeneratedMSBuildEditorConfig.editorconfig
    �               �   AbsenceApp.EfHost.GlobalUsings.g.cs
    �               �   
    �               +---ref
    �               +---refint
    +---AbsenceApp.Tests
    �   �   AbsenceApp.Tests.csproj
    �   �   GlobalUsings.cs
    �   �   NavigationCoreTests.cs
    �   �   
    �   +---bin
    �   �   +---Debug
    �   �   �   +---net8.0
    �   �   �   �   �   AbsenceApp.Core.dll
    �   �   �   �   �   AbsenceApp.Core.pdb
    �   �   �   �   �   AbsenceApp.Data.dll
    �   �   �   �   �   AbsenceApp.Data.pdb
    �   �   �   �   �   AbsenceApp.Tests.deps.json
    �   �   �   �   �   AbsenceApp.Tests.dll
    �   �   �   �   �   AbsenceApp.Tests.pdb
    �   �   �   �   �   AbsenceApp.Tests.runtimeconfig.json
    �   �   �   �   �   Azure.Core.dll
    �   �   �   �   �   Azure.Identity.dll
    �   �   �   �   �   Castle.Core.dll
    �   �   �   �   �   CoverletSourceRootsMapping_AbsenceApp.Tests
    �   �   �   �   �   Microsoft.Bcl.AsyncInterfaces.dll
    �   �   �   �   �   Microsoft.Data.SqlClient.dll
    �   �   �   �   �   Microsoft.EntityFrameworkCore.Abstractions.dll
    �   �   �   �   �   Microsoft.EntityFrameworkCore.dll
    �   �   �   �   �   Microsoft.EntityFrameworkCore.InMemory.dll
    �   �   �   �   �   Microsoft.EntityFrameworkCore.Relational.dll
    �   �   �   �   �   Microsoft.EntityFrameworkCore.SqlServer.dll
    �   �   �   �   �   Microsoft.Extensions.Caching.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.Caching.Memory.dll
    �   �   �   �   �   Microsoft.Extensions.Configuration.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.DependencyInjection.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.DependencyInjection.dll
    �   �   �   �   �   Microsoft.Extensions.Logging.Abstractions.dll
    �   �   �   �   �   Microsoft.Extensions.Logging.dll
    �   �   �   �   �   Microsoft.Extensions.Options.dll
    �   �   �   �   �   Microsoft.Extensions.Primitives.dll
    �   �   �   �   �   Microsoft.Identity.Client.dll
    �   �   �   �   �   Microsoft.Identity.Client.Extensions.Msal.dll
    �   �   �   �   �   Microsoft.IdentityModel.Abstractions.dll
    �   �   �   �   �   Microsoft.IdentityModel.JsonWebTokens.dll
    �   �   �   �   �   Microsoft.IdentityModel.Logging.dll
    �   �   �   �   �   Microsoft.IdentityModel.Protocols.dll
    �   �   �   �   �   Microsoft.IdentityModel.Protocols.OpenIdConnect.dll
    �   �   �   �   �   Microsoft.IdentityModel.Tokens.dll
    �   �   �   �   �   Microsoft.SqlServer.Server.dll
    �   �   �   �   �   Microsoft.TestPlatform.CommunicationUtilities.dll
    �   �   �   �   �   Microsoft.TestPlatform.CoreUtilities.dll
    �   �   �   �   �   Microsoft.TestPlatform.CrossPlatEngine.dll
    �   �   �   �   �   Microsoft.TestPlatform.PlatformAbstractions.dll
    �   �   �   �   �   Microsoft.TestPlatform.Utilities.dll
    �   �   �   �   �   Microsoft.VisualStudio.CodeCoverage.Shim.dll
    �   �   �   �   �   Microsoft.VisualStudio.TestPlatform.Common.dll
    �   �   �   �   �   Microsoft.VisualStudio.TestPlatform.ObjectModel.dll
    �   �   �   �   �   Microsoft.Win32.SystemEvents.dll
    �   �   �   �   �   Moq.dll
    �   �   �   �   �   Newtonsoft.Json.dll
    �   �   �   �   �   NuGet.Frameworks.dll
    �   �   �   �   �   System.Configuration.ConfigurationManager.dll
    �   �   �   �   �   System.Diagnostics.EventLog.dll
    �   �   �   �   �   System.Drawing.Common.dll
    �   �   �   �   �   System.IdentityModel.Tokens.Jwt.dll
    �   �   �   �   �   System.Memory.Data.dll
    �   �   �   �   �   System.Runtime.Caching.dll
    �   �   �   �   �   System.Security.Cryptography.ProtectedData.dll
    �   �   �   �   �   System.Security.Permissions.dll
    �   �   �   �   �   System.Windows.Extensions.dll
    �   �   �   �   �   testhost.dll
    �   �   �   �   �   testhost.exe
    �   �   �   �   �   xunit.abstractions.dll
    �   �   �   �   �   xunit.assert.dll
    �   �   �   �   �   xunit.core.dll
    �   �   �   �   �   xunit.execution.dotnet.dll
    �   �   �   �   �   xunit.runner.reporters.netcoreapp10.dll
    �   �   �   �   �   xunit.runner.utility.netcoreapp10.dll
    �   �   �   �   �   xunit.runner.visualstudio.testadapter.dll
    �   �   �   �   �   
    �   �   �   �   +---cs
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---de
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---es
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---fr
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---it
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---ja
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---ko
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---pl
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---pt-BR
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---ru
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---runtimes
    �   �   �   �   �   +---unix
    �   �   �   �   �   �   +---lib
    �   �   �   �   �   �       +---net6.0
    �   �   �   �   �   �               Microsoft.Data.SqlClient.dll
    �   �   �   �   �   �               System.Drawing.Common.dll
    �   �   �   �   �   �               
    �   �   �   �   �   +---win
    �   �   �   �   �   �   +---lib
    �   �   �   �   �   �       +---net6.0
    �   �   �   �   �   �               Microsoft.Data.SqlClient.dll
    �   �   �   �   �   �               Microsoft.Win32.SystemEvents.dll
    �   �   �   �   �   �               System.Diagnostics.EventLog.dll
    �   �   �   �   �   �               System.Diagnostics.EventLog.Messages.dll
    �   �   �   �   �   �               System.Drawing.Common.dll
    �   �   �   �   �   �               System.Runtime.Caching.dll
    �   �   �   �   �   �               System.Security.Cryptography.ProtectedData.dll
    �   �   �   �   �   �               System.Windows.Extensions.dll
    �   �   �   �   �   �               
    �   �   �   �   �   +---win-arm
    �   �   �   �   �   �   +---native
    �   �   �   �   �   �           Microsoft.Data.SqlClient.SNI.dll
    �   �   �   �   �   �           
    �   �   �   �   �   +---win-arm64
    �   �   �   �   �   �   +---native
    �   �   �   �   �   �           Microsoft.Data.SqlClient.SNI.dll
    �   �   �   �   �   �           
    �   �   �   �   �   +---win-x64
    �   �   �   �   �   �   +---native
    �   �   �   �   �   �           Microsoft.Data.SqlClient.SNI.dll
    �   �   �   �   �   �           
    �   �   �   �   �   +---win-x86
    �   �   �   �   �       +---native
    �   �   �   �   �               Microsoft.Data.SqlClient.SNI.dll
    �   �   �   �   �               
    �   �   �   �   +---tr
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---zh-Hans
    �   �   �   �   �       Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �   �       Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �   �       Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �   �       
    �   �   �   �   +---zh-Hant
    �   �   �   �           Microsoft.TestPlatform.CommunicationUtilities.resources.dll
    �   �   �   �           Microsoft.TestPlatform.CoreUtilities.resources.dll
    �   �   �   �           Microsoft.TestPlatform.CrossPlatEngine.resources.dll
    �   �   �   �           Microsoft.VisualStudio.TestPlatform.Common.resources.dll
    �   �   �   �           Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll
    �   �   �   �           
    �   �   �   +---net8.0-windows10.0.19041.0
    �   �   +---Release
    �   �       +---net8.0
    �   �               CoverletSourceRootsMapping_AbsenceApp.Tests
    �   �               
    �   +---Helpers
    �   �       AsyncQueryHelper.cs
    �   �       
    �   +---MapperTests
    �   �       AttendanceMapperTests.cs
    �   �       AuditLogMapperTests.cs
    �   �       ClassMapperTests.cs
    �   �       RoleMapperTests.cs
    �   �       StudentMapperTests.cs
    �   �       UserMapperTests.cs
    �   �       
    �   +---obj
    �   �   �   AbsenceApp.Tests.csproj.nuget.dgspec.json
    �   �   �   AbsenceApp.Tests.csproj.nuget.g.props
    �   �   �   AbsenceApp.Tests.csproj.nuget.g.targets
    �   �   �   project.assets.json
    �   �   �   project.nuget.cache
    �   �   �   
    �   �   +---Debug
    �   �   �   +---net8.0
    �   �   �   �   �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    �   �   �   �   �   AbsenceA.709E1EA1.Up2Date
    �   �   �   �   �   AbsenceApp.Tests.AssemblyInfo.cs
    �   �   �   �   �   AbsenceApp.Tests.AssemblyInfoInputs.cache
    �   �   �   �   �   AbsenceApp.Tests.assets.cache
    �   �   �   �   �   AbsenceApp.Tests.csproj.AssemblyReference.cache
    �   �   �   �   �   AbsenceApp.Tests.csproj.CoreCompileInputs.cache
    �   �   �   �   �   AbsenceApp.Tests.csproj.FileListAbsolute.txt
    �   �   �   �   �   AbsenceApp.Tests.GeneratedMSBuildEditorConfig.editorconfig
    �   �   �   �   �   AbsenceApp.Tests.genruntimeconfig.cache
    �   �   �   �   �   AbsenceApp.Tests.GlobalUsings.g.cs
    �   �   �   �   �   AbsenceApp.Tests.sourcelink.json
    �   �   �   �   �   
    �   �   �   �   +---ref
    �   �   �   �   �       AbsenceApp.Tests.dll
    �   �   �   �   �       
    �   �   �   �   +---refint
    �   �   �   +---net8.0-windows10.0.19041.0
    �   �   �       �   AbsenceApp.Tests.csproj.FileListAbsolute.txt
    �   �   �       �   AbsenceApp.Tests.GlobalUsings.g.cs
    �   �   �       �   
    �   �   �       +---ref
    �   �   �       +---refint
    �   �   +---Release
    �   �       +---net8.0
    �   �           �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    �   �           �   AbsenceApp.Tests.AssemblyInfo.cs
    �   �           �   AbsenceApp.Tests.AssemblyInfoInputs.cache
    �   �           �   AbsenceApp.Tests.assets.cache
    �   �           �   AbsenceApp.Tests.csproj.AssemblyReference.cache
    �   �           �   AbsenceApp.Tests.GeneratedMSBuildEditorConfig.editorconfig
    �   �           �   AbsenceApp.Tests.GlobalUsings.g.cs
    �   �           �   
    �   �           +---ref
    �   �           +---refint
    �   +---RepositoryTests
    �   �       AttendanceRepositoryTests.cs
    �   �       AuditLogRepositoryTests.cs
    �   �       ClassRepositoryTests.cs
    �   �       RoleRepositoryTests.cs
    �   �       UserRepositoryTests.cs
    �   �       
    �   +---ServiceTests
    �           AuditLogServiceTests.cs
    �           AuthServiceTests.cs
    �           ClassServiceTests.cs
    �           RoleServiceTests.cs
    �           StudentServiceTests.cs
    �           TableSettingsServiceTests.cs
    �           TableSettingsViewModelTests.cs
    �           
    +---AbsenceApp.Updater
        �   AbsenceApp.Updater.csproj
        �   Class1.cs
        �   
        +---bin
        �   +---Debug
        �   �   +---net8.0
        �   �   �       AbsenceApp.Updater.deps.json
        �   �   �       AbsenceApp.Updater.dll
        �   �   �       AbsenceApp.Updater.pdb
        �   �   �       
        �   �   +---net8.0-windows10.0.19041.0
        �   +---Release
        �       +---net8.0
        +---obj
        �   �   AbsenceApp.Updater.csproj.nuget.dgspec.json
        �   �   AbsenceApp.Updater.csproj.nuget.g.props
        �   �   AbsenceApp.Updater.csproj.nuget.g.targets
        �   �   project.assets.json
        �   �   project.nuget.cache
        �   �   
        �   +---Debug
        �   �   +---net8.0
        �   �   �   �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
        �   �   �   �   AbsenceApp.Updater.AssemblyInfo.cs
        �   �   �   �   AbsenceApp.Updater.AssemblyInfoInputs.cache
        �   �   �   �   AbsenceApp.Updater.assets.cache
        �   �   �   �   AbsenceApp.Updater.csproj.CoreCompileInputs.cache
        �   �   �   �   AbsenceApp.Updater.csproj.FileListAbsolute.txt
        �   �   �   �   AbsenceApp.Updater.dll
        �   �   �   �   AbsenceApp.Updater.GeneratedMSBuildEditorConfig.editorconfig
        �   �   �   �   AbsenceApp.Updater.GlobalUsings.g.cs
        �   �   �   �   AbsenceApp.Updater.pdb
        �   �   �   �   AbsenceApp.Updater.sourcelink.json
        �   �   �   �   
        �   �   �   +---ref
        �   �   �   �       AbsenceApp.Updater.dll
        �   �   �   �       
        �   �   �   +---refint
        �   �   �           AbsenceApp.Updater.dll
        �   �   �           
        �   �   +---net8.0-windows10.0.19041.0
        �   �       �   AbsenceApp.Updater.csproj.FileListAbsolute.txt
        �   �       �   AbsenceApp.Updater.GlobalUsings.g.cs
        �   �       �   
        �   �       +---ref
        �   �       +---refint
        �   +---Release
        �       +---net8.0
        �           �   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
        �           �   AbsenceApp.Updater.AssemblyInfo.cs
        �           �   AbsenceApp.Updater.AssemblyInfoInputs.cache
        �           �   AbsenceApp.Updater.assets.cache
        �           �   AbsenceApp.Updater.GeneratedMSBuildEditorConfig.editorconfig
        �           �   AbsenceApp.Updater.GlobalUsings.g.cs
        �           �   
        �           +---ref
        �           +---refint
        +---UpdateLogic
        �       UpdateChecker.cs
        �       UpdateDownloader.cs
        �       
        +---Versioning
                VersionInfo.cs
                
