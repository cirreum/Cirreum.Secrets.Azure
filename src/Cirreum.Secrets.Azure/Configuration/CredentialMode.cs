namespace Cirreum.Secrets.Configuration;

/// <summary>
/// The credential strategy used to authenticate to Azure Key Vault.
/// </summary>
public enum CredentialMode {

	/// <summary>
	/// Uses <see cref="Azure.Identity.DefaultAzureCredential"/>, trying the full chain of available credentials.
	/// </summary>
	Default,

	/// <summary>
	/// Uses <see cref="Azure.Identity.ManagedIdentityCredential"/> directly. Deterministic, and enables the
	/// resilient managed-identity retry mode that <see cref="Azure.Identity.DefaultAzureCredential"/> does not.
	/// </summary>
	ManagedIdentity,

	/// <summary>
	/// Uses a <see cref="Azure.Identity.ChainedTokenCredential"/> of local development tool credentials
	/// (Visual Studio, Azure CLI, Azure PowerShell).
	/// </summary>
	DevChain,

}
