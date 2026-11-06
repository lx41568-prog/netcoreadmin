# NetCoreAdmin

A starter admin application built with ASP.NET Core Razor Pages (.NET 10), MySQL, and SqlSugar, using a Microsoft Fluent-inspired visual style.

## Included in this foundation

- Cookie-based sign-in and sign-out, with ASP.NET Core password hashing
- SqlSugar MySQL connection and CodeFirst creation of system tables
- User creation/listing
- Role creation/listing
- Dictionary type and item creation/listing (one-to-many relationship)
- System parameter create/upsert and listing
- Fluent-inspired responsive shell, navigation, login page, and overview
- Core entities for users, roles, menus, role assignments, dictionaries, settings, and audit records

## Run locally

1. Install the .NET 10 SDK and MySQL 8 (or a compatible MySQL server).
2. Create the database, for example: `CREATE DATABASE netcoreadmin CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;`
3. Set `ConnectionStrings:MySql` in `appsettings.json` or environment variable `ConnectionStrings__MySql`. Do not commit production credentials.
4. Optionally set `AdminSeed:UserName` and `AdminSeed:Password` / environment variables `AdminSeed__UserName` and `AdminSeed__Password` before first run. The seed account is created only when the configured username is absent.
5. From `NetCoreAdmin/`, run:

   ```bash
   dotnet restore
   dotnet run
   ```

6. Sign in with the configured seed credentials. Change the sample password before exposing the app to a network.

The database must already exist and the MySQL account needs permission to create/alter the application tables. The seed user is not associated with the seeded role yet; role-user and role-menu assignment screens and fine-grained authorization are follow-up work. HTTPS should be enabled in deployed environments.
