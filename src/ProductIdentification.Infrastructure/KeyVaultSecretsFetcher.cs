using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace ProductIdentification.Infrastructure
{
    public class KeyVaultSecretsFetcher : ISecretsFetcher
    {
        private readonly KeyVaultClient _keyVaultClient;
        private readonly string _keyVaultUri;

        public KeyVaultSecretsFetcher(AppSettings appSettings)
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            _keyVaultClient =
                new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            _keyVaultUri = appSettings.KeyVaultUri;
        }

        public string GetStorageConnectionString  => GetSecret("StorageConnectionString");
        public string GetDatabaseConnectionString => GetSecret("DatabaseConnectionString");
        public string GetCustomVisionPredictionKey => GetSecret("CustomVisionPredictionKey");
        public string GetCustomVisionPredictionId => GetSecret("CustomVisionPredictionId");
        public string GetCustomVisionProjectId => GetSecret("CustomVisionProjectId");
        public string GetCustomVisionTrainingKey => GetSecret("CustomVisionTrainingKey");
        public string GetCustomVisionEndpoint => GetSecret("CustomVisionEndpoint");
        public string GetEmailFrom => GetSecret("EmailFrom");
        public string GetEmailUsername => GetSecret("EmailUsername");
        public string GetEmailPassword => GetSecret("EmailPassword");
        public string GetEmailSmtpHost => GetSecret("EmailSmtpHost");
        public int GetEmailSmtpPort => int.Parse(GetSecret("EmailSmtpPort"));

        private string GetSecret(string secretName)
        {
            var sec = _keyVaultClient.GetSecretAsync(_keyVaultUri, secretName);
            return sec.Result.Value;
        }
    }
}