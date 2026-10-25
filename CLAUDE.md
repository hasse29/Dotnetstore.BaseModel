# Dotnetstore.BaseModel

A .NET 10 class library (NuGet-style shared base model) with a Vertical Slice organization.

## Architecture

- **Style**: Vertical Slice Architecture — group code by feature/capability, not by technical layer.
- **Project type**: Class library (`src/Dotnetstore.BaseModel`), no host, no DI container of its own.
- Keep the public API surface small and intentional; everything is `internal` unless it must be public.
- No dependencies on web, database, or messaging frameworks.

## Tech Stack

- .NET 10 / C# 14, nullable enabled, warnings as errors (`Directory.Build.props`)
- Solution format: `.slnx`
- Testing: xUnit v3 (`tests/Dotnetstore.BaseModel.Tests`)
- Database / Auth / Caching / Messaging: none (not applicable yet)

## Structure

```
src/Dotnetstore.BaseModel/        # the library
tests/Dotnetstore.BaseModel.Tests/ # xUnit v3 tests
Directory.Build.props             # shared build settings
```

## Conventions

- Prefer records, primary constructors, collection expressions, and `sealed` classes.
- Use file-scoped namespaces matching folder structure.
- Public types need XML docs; avoid breaking public API changes.
- Propagate `CancellationToken` on any async public method.
- Tests: one test class per type under test, mirror the source folder layout.

## Commands

- Build: `dotnet build Dotnetstore.BaseModel.slnx`
- Test: `dotnet test --solution Dotnetstore.BaseModel.slnx`

## Tooling

- Roslyn MCP server configured in `.mcp.json` — prefer it over file scanning for symbol lookup.
- `/plan` before non-trivial work, `/health-check` for a baseline, `/scaffold` to add features.
