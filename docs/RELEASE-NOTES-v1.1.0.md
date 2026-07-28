# Cirreum.Secrets.Azure 1.1.0

## Summary

Adds `CredentialMode` to `AzureKeyVaultInstanceSettings`, letting an instance choose a
deterministic Azure credential instead of always trying the full `DefaultAzureCredential`
chain. Also fixes three latent bugs in credential and vault-URI handling. Additive — no
consumer code changes required to upgrade.

## Why

`DefaultAzureCredential` is convenient but non-deterministic: on a host where more than one
credential source is available (managed identity, plus a developer who ran `az login` on the
same box, for example), which credential wins can change silently between runs — the exact
failure mode Microsoft's own authentication best-practices guidance warns against for
production apps. A recent MSAL.NET regression (4.80.0's IMDS source-detection rework, fixed in
4.83.0) that could cause `DefaultAzureCredential` to skip past managed identity entirely on
some hosts prompted this review of how the package constructs its credential.

## What's new

### `CredentialMode`

```csharp
settings.Instances.Add(new AzureKeyVaultInstanceSettings
{
    Endpoint = "https://your-vault.vault.azure.net/",
    CredentialMode = CredentialMode.ManagedIdentity,
    ManagedIdentityClientId = "<user-assigned-client-id>" // optional; omit for system-assigned
});
```

| Mode | Credential | When to use |
| --- | --- | --- |
| `Default` (unchanged) | `DefaultAzureCredential` | General-purpose; tries the full credential chain |
| `ManagedIdentity` | `ManagedIdentityCredential` | Production; deterministic, no chain fallback, and supports the resilient managed-identity retry mode `DefaultAzureCredential` does not |
| `DevChain` | Visual Studio → Azure CLI → Azure PowerShell | Local development only |

`Identifier` (tenant ID) now applies to both `Default` and `DevChain`. It has no effect under
`ManagedIdentity`, since a managed identity's tenant is implicit to the resource it's attached to.

## Fixed

- A blank `Identifier` produced a `null` credential, which threw `ArgumentNullException` from
  `SecretClient`'s constructor at startup. A credential is now always constructed.
- `Identifier` (tenant ID) was previously only forwarded to `DefaultAzureCredential`; any other
  credential path silently ignored it.
- The "vault URI missing" startup exception named the internal `VaultUri` property instead of
  `Endpoint`, the setting consumers actually configure.

## Compatibility

- **Additive.** `CredentialMode` defaults to `Default`, matching prior behavior exactly.
  `ManagedIdentityClientId` is optional.
- **Internal-detail cleanup, not a meaningful public API change:**
  `AzureKeyVaultInstanceSettings.VaultUri` is now `protected internal` instead of `public`. It's
  a value parsed from `Endpoint` via `ParseEndPoint()`, not independent configuration — no
  supported usage sets or reads it directly. Shipped as part of this minor release rather than a
  major, per project convention for reshaping derived/computed surface.

## Upgrade

Update the package reference to `1.1.0`. No code changes required unless you want to opt into
`CredentialMode.ManagedIdentity` or `CredentialMode.DevChain`.
