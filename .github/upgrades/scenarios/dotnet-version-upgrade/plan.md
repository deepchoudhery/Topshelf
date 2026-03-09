# .NET 10 Upgrade Plan — Topshelf Solution

## Overview

**Target**: net10.0-windows (keeping net452 for backward compatibility)
**Scope**: 11 projects; TFM changes, package updates, minor source fixes

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously.
**Rationale**: 11 projects, all on netstandard2.0/net452, clear dependency structure, low-to-medium complexity.

---

## Tasks

### 01-update-project-files: Update all .csproj files (TFMs and packages)

Update all project files (except Topshelf.Elmah which is left unchanged) to replace netstandard2.0/netcoreapp2.1 targets with net10.0-windows, and update package versions per the upgrade specification in scenario-instructions.md. Remove packages that are included in the net10.0-windows framework reference (Microsoft.Win32.Registry, Microsoft.Win32.SystemEvents, System.ServiceProcess.ServiceController, System.Runtime.InteropServices.RuntimeInformation). Update conditional NETCORE defines from netstandard2.0 condition to net10.0-windows condition.

**Done when**: All .csproj files (except Topshelf.Elmah) have net10.0-windows as a target framework, all package versions are updated per the plan.

---

### 02-fix-service-installer: Fix type conflicts in TopShelf.ServiceInstaller

When TopShelf.ServiceInstaller targets net10.0-windows instead of netstandard2.0, the custom ServiceAccount enum definition conflicts with the one provided by the Windows framework. Guard the ServiceAccount.cs file with `#if NETSTANDARD2_0` so it only compiles when targeting netstandard2.0.

**Done when**: TopShelf.ServiceInstaller compiles without duplicate type errors for net10.0-windows.

---

### 03-fix-source-code: Fix source code issues from package API changes

- SampleTopshelfService/Program.cs: Change `.WriteTo.ColoredConsole()` to `.WriteTo.Console()` because Serilog.Sinks.ColoredConsole is replaced by Serilog.Sinks.Console 6.0.0.
- Review any NUnit 4.x breaking changes in test files.

**Done when**: All source files compile without errors related to updated package APIs.

---

### 04-build-and-fix: Build solution and fix all remaining compilation errors

Run `dotnet build D:\a\Topshelf\Topshelf\src\Topshelf.sln` and fix all compilation errors that arise from the framework upgrade. Focus on API changes, deprecated APIs, and breaking changes in the upgraded packages.

**Done when**: Solution builds with 0 errors (warnings acceptable).

---

### 05-run-tests: Run tests to verify correctness

Run the test suite to validate the upgraded code behaves correctly.

**Done when**: All tests pass (or known pre-existing failures are documented).

