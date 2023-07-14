using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CoreBeliefsSurvey.Server.Services;
using System.Collections.Generic;
using CoreBeliefsSurvey.Server.Models;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CoreBeliefsSurvey.Server;

class Program
{
    /// <summary>
    /// Main entry point for the application. It retrieves Azure Storage connection string from Azure Key Vault, 
    /// reads beliefs from a CSV file, and uploads them to an Azure Table Storage.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    static async Task Main(string[] args)
    {
        try
        {
            // Build the configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .Build();

            // Retrieve the Azure Storage connection string from Azure Key Vault
            var keyVaultUrl = "https://core-beliefs-vault.vault.azure.net/";
            var connectionStringSecretName = "ConnectionString";

            var connectionString = await GetConnectionStringFromKeyVault(keyVaultUrl, connectionStringSecretName);

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Failed to retrieve the Azure Storage connection string from Azure Key Vault.");
                return;
            }

            // Create an instance of AppSettings
            var appSettings = new AppSettings { ConnectionString = connectionString, TableName = "Beliefs" };

            // Set up the dependency injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton(appSettings)  // Inject AppSettings
                .AddSingleton<BeliefService>()
                .BuildServiceProvider();

            // Get the belief service from the service provider
            var beliefService = serviceProvider.GetService<BeliefService>();
            if (beliefService == null)
            {
                Console.WriteLine("Failed to retrieve BeliefService from the service provider.");
                return;
            }

            // Path to the CSV file
            var path = "C:\\Users\\gordon\\source\\repos\\CoreBeliefsSurvey\\CoreBeliefsSurvey.Console\\CoreBeliefs.csv";

            // Check if the file exists
            if (!File.Exists(path))
            {
                Console.WriteLine($"The file at path '{path}' does not exist.");
                return;
            }

            // Read the beliefs from the CSV file
            var beliefs = beliefService.ReadBeliefsFromCSV(path);
            if (beliefs == null)
            {
                Console.WriteLine("Failed to read beliefs from the CSV file.");
                return;
            }

            beliefs = beliefService.ReformatBeliefs(beliefs);
            if (beliefs == null)
            {
                Console.WriteLine("Failed to reformat beliefs.");
                return;
            }

            beliefs = beliefs.Where(x => !string.IsNullOrEmpty(x.BeliefName)).ToList();

            //Clear the table
            await beliefService.DeleteAllBeliefs();
            // Upload the beliefs to the table
            await beliefService.UploadBeliefs(beliefs);

            Console.WriteLine("Operation completed successfully.");
        }
        catch (Exception ex)
        {
            // Handle any exception that might occur
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves the connection string from Azure Key Vault.
    /// </summary>
    /// <param name="keyVaultUrl">The URL of the Azure Key Vault.</param>
    /// <param name="secretName">The name of the secret stored in the Azure Key Vault.</param>
    /// <returns>The connection string if successful, null otherwise.</returns>
    static async Task<string?> GetConnectionStringFromKeyVault(string keyVaultUrl, string secretName)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(keyVaultUrl))
        {
            Console.WriteLine("Invalid Key Vault URL provided.");
            return null;
        }

        if (string.IsNullOrWhiteSpace(secretName))
        {
            Console.WriteLine("Invalid secret name provided.");
            return null;
        }

        try
        {
            var credential = new DefaultAzureCredential();
            var client = new SecretClient(new Uri(keyVaultUrl), credential);

            var secret = await client.GetSecretAsync(secretName);

            // Log the retrieval
            Console.WriteLine($"Retrieved secret '{secretName}' from Key Vault.");

            return secret.Value?.Value;
        }
        catch (Exception ex)
        {
            // Handle any exception that might occur
            Console.WriteLine($"An error occurred while retrieving the secret from Key Vault: {ex.Message}");
            return null;
        }
    }

}

