namespace ProductIdentification.Infrastructure
{
    public interface ISecretsFetcher
    {
        string GetStorageConnectionString { get; }
        string GetDatabaseConnectionString { get; }
        string GetCustomVisionPredictionKey { get; }
        string GetCustomVisionPredictionId { get; }
        string GetCustomVisionProjectId { get; }
        string GetCustomVisionTrainingKey { get; }
        string GetCustomVisionEndpoint { get; }
        string GetEmailFrom { get; }
        string GetEmailUsername { get; }
        string GetEmailPassword { get; }
        string GetEmailSmtpHost { get; }
        int GetEmailSmtpPort { get; }
        
    }
}