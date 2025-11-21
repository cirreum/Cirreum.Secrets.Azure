namespace Cirreum.Secrets;

using Azure.Security.KeyVault.Secrets;
using Cirreum.Secrets.Configuration;
using Cirreum.Secrets.Extensions;
using Cirreum.SecretsProvider;
using Microsoft.Extensions.Configuration;

public sealed class AzureKeyVaultRegistrar
	: SecretsProviderRegistrar<
		AzureKeyVaultSettings,
		AzureKeyVaultInstanceSettings> {

	public override ProviderType ProviderType => ProviderType.Secrets;

	public override string ProviderName => "Azure";

	public override string[] ActivitySourceNames { get; } = [$"{typeof(SecretClient).Namespace}.*"];

	public override void AddSecretsProviderInstance(
		IConfigurationBuilder configuration,
		AzureKeyVaultInstanceSettings settings) {
		configuration.AddAzureKeyVaultClient(settings);
	}

}