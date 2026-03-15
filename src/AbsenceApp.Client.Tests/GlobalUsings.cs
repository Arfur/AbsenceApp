/*
===============================================================================
 File        : GlobalUsings.cs
 Namespace   : AbsenceApp.Client.Tests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-14
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : Global using directives for C# test files. Enables Xunit,
               Moq, bUnit TestContext etc. without per-file declarations.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-14  Initial creation.
===============================================================================
*/

global using Xunit;
global using Moq;
global using Bunit;
global using Microsoft.Extensions.DependencyInjection;
global using AbsenceApp.Core.Models;
global using AbsenceApp.Core.DTOs;
global using AbsenceApp.Core.Interfaces;
global using AbsenceApp.Core.ViewModels;
global using AbsenceApp.Client.Tests.Components;
