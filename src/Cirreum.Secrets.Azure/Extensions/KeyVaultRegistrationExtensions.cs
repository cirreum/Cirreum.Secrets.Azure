namespace Cirreum.Secrets.Extensions;

using Azure.Core;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Cirreum.Secrets.Configuration;
using Microsoft.Extensions.Configuration;

internal static class KeyVaultRegistrationExtensions {

	public static void AddAzureKeyVaultClient(
		this IConfigurationBuilder builder,
		AzureKeyVaultInstanceSettings settings) {

		ArgumentNullException.ThrowIfNull(builder);

		var client = settings.GetSecretClient();

		builder.AddAzureKeyVault(client, settings.ClientOptions ?? new AzureKeyVaultConfigurationOptions());

	}

	/// <summary>
	/// Creates a SecretClient instance using the provided settings.
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when Endpoint is missing or cannot be parsed as a valid Uri.</exception>
	private static SecretClient GetSecretClient(
		this AzureKeyVaultInstanceSettings settings) {

		if (settings.VaultUri is null || string.IsNullOrWhiteSpace(settings.VaultUri.ToString())) {
			var err = $"Endpoint is missing or could not be parsed as a valid Uri. Value: '{settings.Endpoint}'";
			throw new InvalidOperationException(err);
		}

		return new SecretClient(settings.VaultUri, settings.GetCredential());

	}

	private static TokenCredential GetCredential(
		this AzureKeyVaultInstanceSettings settings) {

		var tenantId = string.IsNullOrWhiteSpace(settings.Identifier) ? null : settings.Identifier;

		return settings.CredentialMode switch {

			CredentialMode.ManagedIdentity => new ManagedIdentityCredential(
				string.IsNullOrWhiteSpace(settings.ManagedIdentityClientId)
					? ManagedIdentityId.SystemAssigned
					: ManagedIdentityId.FromUserAssignedClientId(settings.ManagedIdentityClientId)),

			CredentialMode.DevChain => new ChainedTokenCredential(
				new VisualStudioCredential(new VisualStudioCredentialOptions { TenantId = tenantId }),
				new AzureCliCredential(new AzureCliCredentialOptions { TenantId = tenantId }),
				new AzurePowerShellCredential(new AzurePowerShellCredentialOptions { TenantId = tenantId })),

			_ => new DefaultAzureCredential(new DefaultAzureCredentialOptions { TenantId = tenantId })

		};

	}

}