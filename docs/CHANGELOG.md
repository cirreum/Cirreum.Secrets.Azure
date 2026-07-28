# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- `CredentialMode` on `AzureKeyVaultInstanceSettings` (`Default`, `ManagedIdentity`, `DevChain`) to select a
  deterministic credential instead of the full `DefaultAzureCredential` chain.
- `ManagedIdentityClientId` on `AzureKeyVaultInstanceSettings` for user-assigned managed identity, used when
  `CredentialMode` is `ManagedIdentity`. Omit it to use the system-assigned managed identity.

### Fixed

- `GetSecretClient` could pass a `null` credential into `SecretClient` when no `Identifier` (tenant ID) was
  configured, throwing `ArgumentNullException` at startup. A credential is now always constructed.
- `Identifier` (tenant ID) was only applied to `DefaultAzureCredential`. It's now forwarded to every
  tenant-aware credential, including the `VisualStudioCredential`/`AzureCliCredential`/`AzurePowerShellCredential`
  chain used by `CredentialMode.DevChain`.
- The "vault URI missing" startup exception referenced the internal `VaultUri` property instead of `Endpoint`,
  the setting consumers actually configure. `VaultUri` is also now `protected internal` rather than public,
  since it's a value parsed from `Endpoint`, not independent configuration.

## [1.0.21] - 2026-07-20

### Updated

- Updated NuGet packages.

## [1.0.20] - 2026-07-19

### Updated

- Updated NuGet packages.

## [1.0.19] - 2026-07-04

### Updated

- Updated NuGet packages.

## [1.0.18] - 2026-07-04

### Updated

- Updated NuGet packages.

## [1.0.17] - 2026-05-07

### Updated

- Updated NuGet packages.

## [1.0.16] - 2026-05-01

### Updated

- Updated NuGet packages.
