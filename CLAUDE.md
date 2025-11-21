# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is **Cirreum.Secrets.Azure**, a .NET library that provides Azure Key Vault integration for the Cirreum secrets provider framework. It implements the `SecretsProviderRegistrar` pattern to enable seamless configuration of Azure Key Vault as a secrets source in .NET applications.

## Architecture

- **Core Implementation**: `AzureKeyVaultRegistrar` class implements `SecretsProviderRegistrar<AzureKeyVaultSettings, AzureKeyVaultInstanceSettings>`
- **Configuration**: Two-tier settings model with `AzureKeyVaultSettings` (provider-level) and `AzureKeyVaultInstanceSettings` (instance-level)
- **Extensions**: `KeyVaultRegistrationExtensions` provides internal implementation details for registering Azure Key Vault clients
- **Authentication**: Uses Azure Identity with `DefaultAzureCredential` and optional tenant-specific credentials

## Common Development Commands

### Build and Development
```bash
dotnet build                          # Build the solution
dotnet build -c Release              # Release build
dotnet pack                           # Create NuGet package
```

### Project Structure
- Uses .NET 10.0 target framework
- Follows Microsoft.Extensions.* patterns
- Integrates with `Cirreum.SecretsProvider` base package
- Dependencies: Azure.Identity, Azure.Extensions.AspNetCore.Configuration.Secrets

## Key Components

- `AzureKeyVaultRegistrar:26` - Main registrar implementing the provider pattern
- `KeyVaultRegistrationExtensions:13` - Extension methods for configuration builder integration
- `GetSecretClient:29` - Private method handling SecretClient instantiation with proper credential management

## Build Configuration

- Uses MSBuild with shared props files in `/build/` directory
- Versioning handled through `Versioning.props`
- CI/CD detection for Azure DevOps and GitHub Actions
- Local development supports version suffixes for pre-release builds