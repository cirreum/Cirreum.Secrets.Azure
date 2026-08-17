# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Updated

- Updated NuGet packages.

## [2.0.0] - 2026-07-29

### Breaking

- **Credential configuration moves to the shared nested `Credential` block** that
  `Cirreum.SecretsProvider` 1.1.0 surfaces on the instance settings base. The provider-local
  `CredentialMode` enum and the flat `CredentialMode` / `ManagedIdentityClientId` properties on
  `AzureKeyVaultInstanceSettings` are removed, and the mode value `DevChain` is renamed
  `Developer`. **Unmigrated flat configuration keys bind to nothing** — the instance silently
  falls back to default-chain behavior — so migrate configuration together with the package
  update. See `MIGRATION-v2.md`.

### Added

- `Default` mode now honors `Credential.IdentityId`, pinning the default chain's managed-identity
  leg to a specific user-assigned identity. 1.1.0 only consumed the client id under
  `ManagedIdentity`.

### Changed

- An unrecognized `CredentialMode` value now fails at startup with `InvalidOperationException`
  instead of silently degrading to the default chain, so a future mode configured against a
  version that has not mapped it is loud rather than wrong.

## [1.1.0] - 2026-07-28

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
