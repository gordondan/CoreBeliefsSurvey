using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CoreBeliefsSurvey.Server.Services;
using System.Collections.Generic;
using CoreBeliefsSurvey.Server.Models;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

class Program
{
    static async Task Main(string[] args)
    {
        // Build the configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //.AddJsonFile("appsettings.json")
            .Build();

        // Set up the dependency injection
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton<BeliefService>()
            .BuildServiceProvider();

        // Get the belief service from the service provider
        var beliefService = serviceProvider.GetService<BeliefService>();

        // Retrieve the Azure Storage connection string from Azure Key Vault
        var keyVaultUrl = "https://core-beliefs-vault.vault.azure.net/";
        var connectionStringSecretName = "ConnectionString";

        var connectionString = await GetConnectionStringFromKeyVault(keyVaultUrl, connectionStringSecretName);

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Failed to retrieve the Azure Storage connection string from Azure Key Vault.");
            return;
        }

        // Update the configuration with the retrieved connection string
        configuration["ConnectionStrings:Default"] = connectionString;

        // Path to the CSV file
        var path = "C:\\Users\\gordon\\source\\repos\\CoreBeliefsSurvey\\CoreBeliefsSurvey.Console\\CoreBeliefs.csv";

        // Read the beliefs from the CSV file
        var beliefs = beliefService.ReadBeliefsFromCSV(path);

        // Upload the beliefs to the table
        await beliefService.UploadBeliefs(beliefs);
    }

    static async Task<string> GetConnectionStringFromKeyVault(string keyVaultUrl, string secretName)
    {
        var credential = new DefaultAzureCredential();
        var client = new SecretClient(new Uri(keyVaultUrl), credential);

        var secret = await client.GetSecretAsync(secretName);

        return secret.Value?.Value;
    }
}
