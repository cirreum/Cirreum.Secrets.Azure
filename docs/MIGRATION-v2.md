# Migration to Cirreum.Secrets.Azure v2.0

**From:** `Cirreum.Secrets.Azure 1.x`
**To:** `Cirreum.Secrets.Azure 2.0.0`

## Why v2

1.1.0 introduced credential selection (`CredentialMode`, `ManagedIdentityClientId`) as flat,
provider-local surface — one day before the framework settled a shared credential vocabulary for
*all* providers (`Cirreum.Providers` `CredentialSettings`, surfaced by `Cirreum.SecretsProvider`
1.1.0 as a nested `Credential` block). 2.0.0 replaces the local surface with the shared one while
adoption is near zero, so every Cirreum provider — secrets, persistence, storage, messaging —
reads the same configuration shape from here on.

## Breaking changes — find/replace

| 1.x | 2.0 |
| --- | --- |
| `using Cirreum.Secrets.Configuration;` (for the enum) | `using Cirreum.Providers.Configuration;` |
| `settings.CredentialMode = CredentialMode.X` | `settings.Credential = new() { Mode = CredentialMode.X }` |
| `settings.ManagedIdentityClientId = "..."` | `settings.Credential = new() { ..., IdentityId = "..." }` |
| `CredentialMode.DevChain` | `CredentialMode.Developer` |
| Config key `"CredentialMode": "..."` | `"Credential": { "Mode": "..." }` |
| Config key `"ManagedIdentityClientId": "..."` | `"Credential": { ..., "IdentityId": "..." }` |
| Config value `"DevChain"` | `"Developer"` |

## ⚠️ The configuration change is silent if you skip it

The old flat keys are not detected or rejected — configuration binding simply no longer has
properties for them, so **an unmigrated instance quietly reverts to default-chain behavior**
(`DefaultAzureCredential`, tenant still honored via `Identifier`). If you set
`CredentialMode: "ManagedIdentity"` in 1.1.0 for deterministic production auth, migrate the keys
in the same change as the package update; nothing will warn you otherwise.

## Migration walkthrough

1. Update the package to `2.0.0` (pulls `Cirreum.SecretsProvider` ≥ 1.1.0).
2. In `appsettings.json` (or wherever instances are configured), replace the flat keys:

   ```json
   // 1.x
   "Instances": {
     "default": {
       "Endpoint": "https://your-vault.vault.azure.net/",
       "CredentialMode": "ManagedIdentity",
       "ManagedIdentityClientId": "<client-id>"
     }
   }

   // 2.0
   "Instances": {
     "default": {
       "Endpoint": "https://your-vault.vault.azure.net/",
       "Credential": { "Mode": "ManagedIdentity", "IdentityId": "<client-id>" }
     }
   }
   ```

3. If you used `"DevChain"`, it is now `"Developer"` — same Visual Studio → Azure CLI →
   Azure PowerShell chain, named for whose identity it is rather than how it is built.
4. Programmatic configuration follows the same shape via the `Credential` property.
5. If you never set `CredentialMode` in 1.x: **no action** — absent block means the default
   chain, exactly as before.

## New capabilities

- `Default` mode now honors `IdentityId`, pinning the default chain's managed-identity leg to a
  specific user-assigned identity — useful on hosts with several identities without giving up
  chain fallback for local runs.
- An unrecognized mode value fails at startup instead of silently using the default chain.

## What didn't change

- `Endpoint`, `Identifier` (tenant), `ClientOptions` — names, meaning, and behavior
- The credentials constructed per mode, and the tenant forwarding to every tenant-aware credential
- Registration (`AzureKeyVaultRegistrar`) and the configuration-builder integration
- Activity tracing

## Downstream package impact

None — no Cirreum package depends on the removed surface. Applications binding the flat 1.1.0
keys are the only affected consumers.
