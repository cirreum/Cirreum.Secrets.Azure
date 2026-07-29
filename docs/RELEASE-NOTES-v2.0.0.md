# Cirreum.Secrets.Azure 2.0.0 — Credential Configuration Joins the Shared Shape

## Why this release exists

1.1.0 gave this provider credential selection as flat, provider-local properties — one day before
the framework settled a shared, vendor-neutral credential vocabulary for all providers. Rather than
let two shapes coexist (and let five more providers copy the flat one), 2.0.0 moves to the shared
nested `Credential` block immediately, while real-world adoption of the flat surface is near zero.
This is the cheapest this change will ever be.

## What's new

Configuration now uses the `Credential` block every Cirreum provider shares:

```json
"Instances": {
  "default": {
    "Endpoint": "https://your-vault.vault.azure.net/",
    "Identifier": "<tenant-id, optional>",
    "Credential": { "Mode": "ManagedIdentity", "IdentityId": "<user-assigned-client-id>" }
  }
}
```

- `Default` — `DefaultAzureCredential`; now honors `IdentityId` to pin the chain's
  managed-identity leg (new in 2.0.0).
- `ManagedIdentity` — deterministic `ManagedIdentityCredential`; `IdentityId` selects a
  user-assigned identity, omit for system-assigned.
- `Developer` — Visual Studio → Azure CLI → Azure PowerShell, as the signed-in developer.
  This is 1.1.0's `DevChain`, renamed for whose identity it is rather than how it is built.

An unrecognized mode now fails at startup instead of silently using the default chain.

## Compatibility

Breaking for code that referenced the local `CredentialMode` enum or the flat properties, and for
configuration using the flat keys — **the old keys bind to nothing and the instance silently
reverts to default-chain behavior**, so migrate configuration with the package update.
[MIGRATION-v2.md](MIGRATION-v2.md) has the find/replace table. Instances that never configured a
credential mode need no changes.

## See also

- [MIGRATION-v2.md](MIGRATION-v2.md)
- [CHANGELOG](CHANGELOG.md)
- [Cirreum.Providers — Credential Configuration](https://github.com/cirreum/Cirreum.Providers#credential-configuration)
