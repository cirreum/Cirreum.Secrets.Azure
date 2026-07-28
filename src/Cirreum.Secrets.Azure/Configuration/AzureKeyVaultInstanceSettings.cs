namespace Cirreum.Secrets.Configuration;

using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;
using Cirreum.SecretsProvider.Configuration;


public class AzureKeyVaultInstanceSettings
	: SecretsProviderInstanceSettings {

	/// <summary>
	/// Optional <see cref="AzureKeyVaultConfigurationOptions"/>.
	/// </summary>
	public AzureKeyVaultConfigurationOptions? ClientOptions { get; set; }

	/// <summary>
	/// A <see cref="Uri"/> to the vault on which the client operates. Appears as "DNS Name" in the Azure portal.
	/// If you have a secret <see cref="Uri"/>, use <see cref="KeyVaultSecretIdentifier"/> to parse the <see cref="KeyVaultSecretIdentifier.VaultUri"/> and other information.
	/// You should validate that this URI references a valid Key Vault resource. See <see href="https://aka.ms/azsdk/blog/vault-uri"/> for details.
	/// </summary>
	protected internal Uri? VaultUri { get; set; }

	/// <summary>
	/// Determines which credential is used to authenticate to the vault.
	/// </summary>
	public CredentialMode CredentialMode { get; set; } = CredentialMode.Default;

	/// <summary>
	/// The client ID of a user-assigned managed identity. Only used when <see cref="CredentialMode"/> is
	/// <see cref="CredentialMode.ManagedIdentity"/>. If not supplied, the system-assigned managed identity is used.
	/// </summary>
	public string? ManagedIdentityClientId { get; set; }

	public override void ParseEndPoint() {
		if (!string.IsNullOrEmpty(this.Endpoint) &&
			Uri.TryCreate(this.Endpoint, UriKind.Absolute, out var uri)) {
			this.VaultUri = uri;
		}
	}

}