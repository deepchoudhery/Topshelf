# Scenario Instructions

## Upgrade Parameters

- **Solution**: D:\a\Topshelf\Topshelf\src\Topshelf.sln
- **Target Framework**: net10.0-windows (plus keeping net452 for backward compatibility)
- **Source Branch**: copilot/upgrade-to-dotnet-10
- **Working Branch**: copilot/upgrade-to-dotnet-10

## Strategy

**Selected**: All-at-Once
**Rationale**: 11 projects, all low-to-medium complexity, straightforward TFM/package bumps. Assessment shows 337 API incidents in Topshelf.csproj but these are mostly from the NETCORE conditional compilation path being analyzed as if fully compiled.

### Execution Constraints

- Single atomic upgrade — all projects updated together
- Validate full solution build after upgrade
- Topshelf.Elmah is left as-is (Framework-only; Elmah doesn't support .NET 5+)
- net452 targets are preserved for backward compatibility
- Replace netstandard2.0 → net10.0-windows, netcoreapp2.1 → net10.0-windows

## Upgrade Strategy Details

### TFM Changes

| Project | From | To |
|---------|------|----|
| Topshelf | net452;netstandard2.0 | net452;net10.0-windows |
| TopShelf.ServiceInstaller | netstandard2.0 | net10.0-windows |
| Topshelf.Tests | net452;netcoreapp2.1 | net452;net10.0-windows |
| Topshelf.Extensions.Logging | net452;netstandard2.0 | net452;net10.0-windows |
| Topshelf.Extensions.Configuration | net452;netstandard2.0 | net452;net10.0-windows |
| Topshelf.Extensions.Configuration.Tests | net452 | net452;net10.0-windows |
| Topshelf.Serilog | net452;netstandard2.0 | net452;net10.0-windows |
| SampleTopshelfService | net452;netcoreapp2.1 | net452;net10.0-windows |
| Topshelf.Log4Net | net452;netstandard2.0 | net452;net10.0-windows |
| Topshelf.NLog | net452;netstandard2.0 | net452;net10.0-windows |
| Topshelf.Elmah | net452 | net452 (unchanged) |

### Package Version Updates

| Package | From | To |
|---------|------|-----|
| Microsoft.SourceLink.GitHub | 1.0.0 | 8.0.0 |
| Microsoft.NET.Test.Sdk | 16.8.3 | 17.12.0 |
| NUnit | 3.12.0 | 4.3.2 |
| NUnit3TestAdapter | 3.17.0 | 4.6.0 |
| Microsoft.Extensions.Logging | 2.1.1 (netstandard2.0) | 9.0.0 (net10.0-windows) |
| Microsoft.Extensions.Configuration.Abstractions | 2.1.1 | 9.0.0 |
| Microsoft.Extensions.Configuration.Binder | 2.1.1 | 9.0.0 |
| Serilog | 2.10.0 | 4.2.0 |
| Serilog.Sinks.ColoredConsole | 3.0.1 (deprecated) | replaced with Serilog.Sinks.Console 6.0.0 |
| log4net | 2.0.12 | 2.0.17 |
| NLog | 4.7.5 | 5.3.4 |

### Packages to Remove (included in net10.0-windows framework)

- Microsoft.Win32.Registry
- Microsoft.Win32.SystemEvents
- System.ServiceProcess.ServiceController
- System.Runtime.InteropServices.RuntimeInformation

### Source Code Changes

- `TopShelf.ServiceInstaller/System.ServiceProcess/ServiceAccount.cs`: Guard with `#if NETSTANDARD2_0` since ServiceAccount is part of the framework on net10.0-windows
- `SampleTopshelfService/Program.cs`: Change `.WriteTo.ColoredConsole()` to `.WriteTo.Console()`
- All NETCORE conditional compilation conditions updated from `netstandard2.0` to `net10.0-windows`

## Preferences

- **Flow Mode**: Automatic
- **Commit Strategy**: Single Commit at End

## Key Decisions Log

- 2025-07-12: All-at-Once strategy selected; user provided full upgrade spec upfront
- 2025-07-12: Topshelf.Elmah left as-is (Framework-only)
- 2025-07-12: Serilog.Sinks.ColoredConsole replaced with Serilog.Sinks.Console (deprecated package)
