# Cirreum.Secrets.Azure

[![NuGet Version](https://img.shields.io/nuget/v/Cirreum.Secrets.Azure.svg?style=flat-square&labelColor=1F1F1F&color=003D8F)](https://www.nuget.org/packages/Cirreum.Secrets.Azure/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Cirreum.Secrets.Azure.svg?style=flat-square&labelColor=1F1F1F&color=003D8F)](https://www.nuget.org/packages/Cirreum.Secrets.Azure/)
[![GitHub Release](https://img.shields.io/github/v/release/cirreum/Cirreum.Secrets.Azure?style=flat-square&labelColor=1F1F1F&color=FF3B2E)](https://github.com/cirreum/Cirreum.Secrets.Azure/releases)
[![License](https://img.shields.io/github/license/cirreum/Cirreum.Secrets.Azure?style=flat-square&labelColor=1F1F1F&color=F2F2F2)](https://github.com/cirreum/Cirreum.Secrets.Azure/blob/main/LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-003D8F?style=flat-square&labelColor=1F1F1F)](https://dotnet.microsoft.com/)

**Azure Key Vault secrets provider for the Cirreum Foundation Framework**

## Overview

**Cirreum.Secrets.Azure** provides seamless integration between Azure Key Vault and the Cirreum secrets provider framework, enabling secure configuration management for .NET applications.

This package implements the `SecretsProviderRegistrar` pattern to automatically configure Azure Key Vault as a secrets source, with built-in support for Azure Identity authentication and flexible configuration options.

## Key Features

- **Seamless Integration**: Drop-in Azure Key Vault support for Cirreum-based applications
- **Azure Identity**: Uses `DefaultAzureCredential` with optional tenant-specific authentication
- **Flexible Configuration**: Two-tier settings model for provider and instance-level configuration
- **Activity Tracing**: Built-in support for distributed tracing with Azure SDK telemetry
- **Production Ready**: Follows Microsoft.Extensions.* patterns and conventions

## Usage

```csharp
// Register Azure Key Vault as a secrets provider
services.AddSecretsProvider<AzureKeyVaultRegistrar>(configuration =>
{
    configuration.Configure<AzureKeyVaultSettings>(settings =>
    {
        settings.Instances.Add(new AzureKeyVaultInstanceSettings
        {
            Endpoint = "https://your-vault.vault.azure.net/",
            Identifier = "your-tenant-id" // Optional
        });
    });
});
```

## Configuration

The provider supports hierarchical configuration through:

- **`AzureKeyVaultSettings`**: Provider-level settings
- **`AzureKeyVaultInstanceSettings`**: Individual Key Vault instance settings including vault URI and tenant configuration

## Contribution Guidelines

1. **Be conservative with new abstractions**  
   The API surface must remain stable and meaningful.

2. **Limit dependency expansion**  
   Only add foundational, version-stable dependencies.

3. **Favor additive, non-breaking changes**  
   Breaking changes ripple through the entire ecosystem.

4. **Include thorough unit tests**  
   All primitives and patterns should be independently testable.

5. **Document architectural decisions**  
   Context and reasoning should be clear for future maintainers.

6. **Follow .NET conventions**  
   Use established patterns from Microsoft.Extensions.* libraries.

## Versioning

Cirreum.Secrets.Azure follows [Semantic Versioning](https://semver.org/):

- **Major** - Breaking API changes
- **Minor** - New features, backward compatible
- **Patch** - Bug fixes, backward compatible

Given its foundational role, major version bumps are rare and carefully considered.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Cirreum Foundation Framework**  
*Layered simplicity for modern .NET*