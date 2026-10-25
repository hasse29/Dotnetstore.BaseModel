# Dotnetstore.BaseModel

[![CI](https://github.com/hasse29/Dotnetstore.BaseModel/actions/workflows/ci.yml/badge.svg)](https://github.com/hasse29/Dotnetstore.BaseModel/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Dotnetstore.BaseModel.svg)](https://www.nuget.org/packages/Dotnetstore.BaseModel)
[![NuGet downloads](https://img.shields.io/nuget/dt/Dotnetstore.BaseModel.svg)](https://www.nuget.org/packages/Dotnetstore.BaseModel)
[![License: MIT](https://img.shields.io/github/license/hasse29/Dotnetstore.BaseModel.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4.svg)](https://dotnet.microsoft.com/)
[![Last commit](https://img.shields.io/github/last-commit/hasse29/Dotnetstore.BaseModel.svg)](https://github.com/hasse29/Dotnetstore.BaseModel/commits/main)
[![Issues](https://img.shields.io/github/issues/hasse29/Dotnetstore.BaseModel.svg)](https://github.com/hasse29/Dotnetstore.BaseModel/issues)

Common base models for .NET applications, with matching Entity Framework Core configurations.

The library provides abstract entity classes for auditing, companies, people and user identities, plus abstract
`IEntityTypeConfiguration<T>` classes you inherit from, so the shared mapping is written once.

## Contents

- [Installation](#installation)
- [Entities](#entities)
- [Configurations](#configurations)
- [Usage](#usage)
- [Design notes](#design-notes)
- [Building and testing](#building-and-testing)
- [Releasing](#releasing)
- [License](#license)

## Installation

```shell
dotnet add package Dotnetstore.BaseModel
```

Requires .NET 10. The package depends on `Microsoft.EntityFrameworkCore`.

## Entities

All entities are `abstract`. You derive your own entities from them. Properties use `init` setters.

```
Auditable
├── Company
└── Person
    └── PersonIdentity
```

Namespace: `Dotnetstore.BaseModel.Entities`

### Auditable

Base class for everything that needs audit and soft-delete data.

| Property          | Type        | Description                         |
|-------------------|-------------|-------------------------------------|
| `CreatedDate`     | `DateTime`  | When the record was created.        |
| `CreatedBy`       | `Guid?`     | Who created it.                     |
| `LastUpdatedDate` | `DateTime?` | When it was last updated.           |
| `LastUpdatedBy`   | `Guid?`     | Who last updated it.                |
| `DeletedDate`     | `DateTime?` | When it was soft-deleted.           |
| `DeletedBy`       | `Guid?`     | Who deleted it.                     |
| `IsDeleted`       | `bool`      | Soft-delete flag.                   |
| `IsSystem`        | `bool`      | Marks system-owned records.         |
| `IsGdpr`          | `bool`      | Marks records containing GDPR data. |

### Company

Inherits `Auditable`.

| Property          | Type      | Required | Description                 |
|-------------------|-----------|----------|-----------------------------|
| `Id`              | `Guid`    |          | Primary key.                |
| `Name`            | `string`  | yes      | Company name.               |
| `Description`     | `string?` |          | Free-text description.      |
| `LogoUrl`         | `Uri?`    |          | Logo location.              |
| `WebsiteUrl`      | `Uri?`    |          | Website.                    |
| `CorporateNumber` | `string?` |          | Corporate registration no.  |
| `TaxId`           | `string?` |          | Tax / VAT identifier.       |

### Person

Inherits `Auditable`. It has no `Id`, so your derived class defines its own key.

| Property               | Type        | Required | Description              |
|------------------------|-------------|----------|--------------------------|
| `FirstName`            | `string`    | yes      |                          |
| `MiddleName`           | `string?`   |          |                          |
| `LastName`             | `string`    | yes      |                          |
| `DateOfBirth`          | `DateTime?` |          |                          |
| `SocialSecurityNumber` | `string?`   |          | Personal identity number |
| `IsMale`               | `bool`      |          |                          |

### PersonIdentity

Inherits `Person`. Adds login and account-state data. `Username` is expected to be the user's email address.

| Property                        | Type        | Required | Description                                   |
|---------------------------------|-------------|----------|-----------------------------------------------|
| `Username`                      | `string`    | yes      | Login name, the user's email address.         |
| `PasswordHash`                  | `string`    | yes      | Hashed password. Never store plain text.      |
| `SecurityStamp`                 | `string`    | yes      | Rotate to invalidate existing tokens/sessions.|
| `EmailAddressConfirmed`         | `bool`      |          | Whether the email address is confirmed.       |
| `EmailAddressConfirmationToken` | `string?`   |          | Token used to confirm the email address.      |
| `AccountIsApproved`             | `bool`      |          | Whether the account has been approved.        |
| `IsLockedOut`                   | `bool`      |          | Whether the account is locked.                |
| `LockoutEnd`                    | `DateTime?` |          | When the lockout ends.                        |
| `AccessFailedCount`             | `int`       |          | Consecutive failed sign-in attempts.          |
| `LastLoginAt`                   | `DateTime?` |          | Last successful sign-in.                      |

## Configurations

Namespace: `Dotnetstore.BaseModel.Configurations`

Each configuration is an abstract generic class implementing `IEntityTypeConfiguration<T>`. They form the same chain as
the entities. Each `Configure` is a `virtual` method that overrides the one above it, so a derived configuration calls
`base.Configure(builder)` once and gets the whole chain.

| Configuration                      | Applies to       | What it configures                                                          |
|------------------------------------|------------------|-----------------------------------------------------------------------------|
| `AuditableConfiguration<T>`        | `Auditable`      | Audit columns as required/optional, and an index on `IsDeleted`.            |
| `CompanyConfiguration<T>`          | `Company`        | Primary key `Id`, required `Name`, max lengths for the text and Uri columns.|
| `PersonConfiguration<T>`           | `Person`         | Required names, max lengths. No key is set.                                 |
| `PersonIdentityConfiguration<T>`   | `PersonIdentity` | Credential and account-state columns, and a unique index on `Username`.     |

### Column lengths

| Property                                              | Max length |
|-------------------------------------------------------|-----------:|
| `Company.Name`                                        |        200 |
| `Company.Description`                                 |       2000 |
| `Company.LogoUrl`, `Company.WebsiteUrl`               |       2048 |
| `Company.CorporateNumber`, `Company.TaxId`            |         50 |
| `Person.FirstName`, `MiddleName`, `LastName`          |         25 |
| `Person.SocialSecurityNumber`                         |         50 |
| `PersonIdentity.Username`, `SecurityStamp`            |        256 |
| `PersonIdentity.EmailAddressConfirmationToken`        |        256 |
| `PersonIdentity.PasswordHash`                         |        512 |

## Usage

### Define your entities

```csharp
using Dotnetstore.BaseModel.Entities;

public sealed class Customer : Company
{
    public string? Segment { get; init; }
}

public sealed class User : PersonIdentity
{
    public Guid Id { get; init; }
}
```

### Define your configurations

Inherit the matching configuration, call `base.Configure(builder)`, then add your own mapping.

```csharp
using Dotnetstore.BaseModel.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class CustomerConfiguration : CompanyConfiguration<Customer>
{
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Segment).HasMaxLength(50);
        builder.ToTable("Customers");
    }
}

public sealed class UserConfiguration : PersonIdentityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.HasKey(x => x.Id);
        builder.ToTable("Users");
    }
}
```

`Person` and `PersonIdentity` have no key, so you must call `HasKey` yourself.

### Register them in your DbContext

```csharp
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
```

### Creating entities

Required members must be set in the initializer:

```csharp
var user = new User
{
    Id = Guid.NewGuid(),
    Username = "ada@example.com",
    PasswordHash = hasher.Hash(password),
    SecurityStamp = Guid.NewGuid().ToString("N"),
    FirstName = "Ada",
    LastName = "Lovelace",
    CreatedDate = DateTime.UtcNow
};
```

## Design notes

- **`DateTime`, not `DateTimeOffset`.** PostgreSQL's `timestamp with time zone` maps awkwardly to offsets, so all
  timestamps are `DateTime`. Store them as UTC.
- **`init` setters.** Entities are immutable after construction by default. If your workflow mutates entities, add
  your own methods or setters in the derived class.
- **Soft delete is not filtered automatically.** The library stores `IsDeleted` and indexes it, but adds no global
  query filter. Add one in your own configuration if you want it.
- **`Username` uniqueness.** The unique index also covers soft-deleted rows. On PostgreSQL, a filtered index
  (`WHERE "IsDeleted" = false`) lets a deleted user's address be reused.
- **Passwords.** `PasswordHash` holds a hash only. Use a proper password hasher such as ASP.NET Core's
  `PasswordHasher<T>`.

## Building and testing

```shell
dotnet build Dotnetstore.BaseModel.slnx
dotnet test --solution Dotnetstore.BaseModel.slnx
```

Repository layout:

```
src/Dotnetstore.BaseModel/          Library (Entities, Configurations)
tests/Dotnetstore.BaseModel.Tests/  xUnit v3 + Shouldly tests, EF InMemory for configuration tests
Directory.Build.props               Shared build settings (nullable, warnings as errors)
Directory.Packages.props            Central package management
.github/workflows/ci.yml            Build, test, and publish workflow
```

## Releasing

Pushing a tag that starts with `v` builds, tests, packs, and publishes to NuGet using
[trusted publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing) (no API key stored). The package
version is the tag without the `v`.

```shell
git tag v0.2.0
git push origin v0.2.0
```

## License

Released under the [MIT License](LICENSE).
