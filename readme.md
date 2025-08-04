# Tamro BackOffice solution

## Overview

This is Tamro BackOffice, built with .NET8 Blazor. MudBlazor component library is used for UI components and the CQRS (Command Query Responsibility Segregation) pattern is used for clean and maintainable architecture

## Prerequisites
- .NET 8.0 SDK
- IDE like Visual Studio or code editor like Visual Studio Code + .NET build tools

## Getting started
### Clone the repository
```bash
git clone https://<Your_BitBucket_Name>@bitbucket.org/phoenixgroupicc/madam.git
cd madam
```
### Install the dependencies, build and run the application
Use IDE feature or
```bash
dotnet restore
dotnet build
dotnet run
```

## Migrations
### Tooling
We're generating our migrations using Entity Framework Tools from Developer Console of Visual Studio:
- Set `Tamro.Madam.Ui` as Startup Project (right click -> set as startup)
- To open the Console, use menu Tools -> NuGet Package Manager -> Package Manager Console
- To switch the context to the Repository project, select it in the dropdown on the top of the Console

### Creating
To create a migration after applying the changes to the Entities and DBContexts, use this command to create a new migration, replacing \<MIGRATION_NAME\> and the context class name, if migrating another:
``` bash
Add-Migration <MIGRATION_NAME> -Context MadamDbContext -o Migrations
```
### Applying
``` bash
Update-Database -Context MadamDbContext
```

## Project structure
```plaintext
|-- test
    |-- Tamro.Madam.Application.Tests
    |-- Tamro.Madam.Common.Tests
    |-- Tamro.Madam.Repository.Tests
    |-- Tamro.Madam.Ui.Tests
|-- Tamro.Madam.Application
    |-- Access
    |-- Commands
    |-- Constants
    |-- DependencyInjection
    |-- Extensions
    |-- Handlers
    |-- Infrastructure
    |-- Jobs
    |-- Profiles
    |-- Queries
    |-- Services
    |-- Validation
|-- Tamro.Madam.Common
    |-- Constants
    |-- Regex
    |-- Utils
|-- Tamro.Madam.Models
|-- Tamro.Madam.Repository
    |-- Audit
    |-- Common
    |-- Configuration
    |-- Constants
    |-- Context
    |-- Entities
    |-- Migrations
    |-- Repositories
|-- Tamro.Madam.Ui
    |-- wwwroot
        |-- css
        |-- images
    |-- ComponentExtensions
    |-- Components
    |-- Constants
    |-- DependencyInjection
    |-- Handlers
    |-- Pages
    |-- Services
    |-- Shared
    |-- Store
    |-- Utils
```
## Key Layers

- **Application**: Contains business logic and CQRS handlers (Commands & Queries).
- **Models**: Represents the core domain models
- **Ui**: Contains Blazor components, pages, UI logic and IOC container for application build up
- **Common**: Common utilities
- **Repository**: Data access layer. Contains DB entities, DB migrations, configuration
- **Tests**: Unit and integration tests.

## Directory Details

### Test

- **Tamro.Madam.Application.Tests**: Tests for Tamro.Madam.Application assembly
- **Tamro.Madam.Common.Tests**: Tests for Tamro.Madam.Common assembly
- **Tamro.Madam.Repository.Tests**: Tests for Tamro.Madam.Repository assembly
- **Tamro.Madam.Ui.Tests**: Tests for Tamro.Madam.Ui executable

### Tamro.Madam.Application

- **Access**: Access control logic and policies, permissions as constants
- **Commands**: Write operations
- **Constants**: Application-wide constant values
- **DependencyInjection**: Service registrations for IOC
- **Extensions**: Helper methods for extending functionality
- **Handlers**: Handlers for commands and queries
- **Infrastructure**: Infrastructure-specific logic such as MediatR behaviors, cache, session and attributes
- **Jobs**: Background jobs and scheduled tasks
- **Profiles**: Mapping profiles for AutoMapper
- **Queries**: Read operations
- **Services**: Application business logic services
- **Validation**: Input validation logic

### Tamro.Madam.Common

- **Constants**: Shared constant values
- **Regex**: Regular expressions for validation and matching
- **Utils**: Utility methods and helpers

### Tamro.Madam.Models

### Tamro.Madam.Repository

- **Audit**: Logic and extensions for auditing changes in the database.
- **Common**: Shared repository logic and utilities
- **Configuration**: Database entity configurations, applied within contexts
- **Constants**: Repository-specific constants
- **Context**: Database context classes
- **Entities**: Database entity definitions
- **Migrations**: Database migrations
- **Repositories**: Repository classes for accessing data

### Tamro.Madam.Ui

- **wwwroot**: Static assets like CSS and images
    - **css**: Custom stylesheets
    - **images**: Application images
- **ComponentExtensions**: Extensions for Blazor components
- **Components**: Reusable Blazor components
- **Constants**: UI-specific constants
- **DependencyInjection**: UI-related service registrations
- **Handlers**: Event handlers for UI interactions
- **Pages**: Blazor pages
- **Services**: UI services for API calls and frontend logic
- **Shared**: Shared components and layouts
- **Store**: State management for the application via Fluxor library
- **Utils**: UI utility functions and helpers

---
