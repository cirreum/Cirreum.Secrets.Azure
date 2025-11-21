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
	/// <exception cref="InvalidOperationException">Thrown when VaultUri is missing.</exception>
	private static SecretClient GetSecretClient(
		this AzureKeyVaultInstanceSettings settings) {

		TokenCredential? credentials = null;
		if (!string.IsNullOrWhiteSpace(settings.Identifier)) {
			credentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions {
				TenantId = settings.Identifier
			});
		}

		if (settings.VaultUri is null || string.IsNullOrWhiteSpace(settings.VaultUri.ToString())) {
			var err = $"VaultUri is missing. It should be provided from the Endpoint. Value: '{settings.Endpoint}'";
			throw new InvalidOperationException(err);
		}

		return new SecretClient(settings.VaultUri, credentials);

	}

}