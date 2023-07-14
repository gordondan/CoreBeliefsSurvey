using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CoreBeliefsSurvey.Server;
using CoreBeliefsSurvey.Server.Controllers;
using CoreBeliefsSurvey.Server.Services;
using CoreBeliefsSurvey.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace PdfGenerationTest
{
    class NullLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => false;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
                // Get the connection string from Azure Key Vault
            var keyVaultUrl = "https://core-beliefs-vault.vault.azure.net/";
            var tableConnectionStringName = "TableConnectionString";
            var credential = new DefaultAzureCredential();
            var secretClient = new SecretClient(new Uri(keyVaultUrl), credential);

            var secret = await secretClient.GetSecretAsync(tableConnectionStringName);
            var connectionString = secret.Value?.Value;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Failed to retrieve the Azure Storage connection string from Azure Key Vault.");
            }

            // Create an instance of AppSettings
            var appSettings = new AppSettings { ConnectionString = connectionString, TableName = "Beliefs", BlobName = "BlobStorage" };
            var logger = new NullLogger<PdfController>();
            var pdfServiceLogger = new NullLogger<PdfService>();
            var pdfService = new PdfService(appSettings, pdfServiceLogger);
            var pdfController = new PdfController(pdfService,logger);

            Console.Write("Enter the number of beliefs to generate: ");
            var numberOfBeliefs = int.Parse(Console.ReadLine());

            var mockBeliefs = CreateMockBeliefs(numberOfBeliefs);

            var result = await pdfController.CreatePdf(mockBeliefs);

            if (result is FileContentResult fileResult)
            {
                SavePdfToFile(fileResult.FileContents, "BeliefsPdfProcessing-Generated.pdf");
                Console.WriteLine($"Generated and saved a PDF file with {fileResult.FileContents.Length} bytes.");
            }
            else
            {
                Console.WriteLine("Failed to generate a PDF.");
            }
        }

        static List<CoreBeliefResponse> CreateMockBeliefs(int numberOfBeliefs)
        {
            var mockBeliefs = new List<CoreBeliefResponse>();

            for (var beliefIndex = 0; beliefIndex < numberOfBeliefs; beliefIndex++)
            {
                var mockBelief = new CoreBeliefResponse
                {
                    Belief = new CoreBelief
                    {
                        BeliefName = $"Mock Belief {beliefIndex + 1} Mock Belief {beliefIndex + 1} Mock Belief {beliefIndex + 1}",
                        BeliefDescription = $"This is a description for Mock Belief {beliefIndex + 1}."
                    },
                    SelectedValue = beliefIndex % 5 + 1
                };

                mockBeliefs.Add(mockBelief);
            }

            return mockBeliefs;
        }

        static void SavePdfToFile(byte[] pdfData, string fileName)
        {
            var fullPath = Path.Combine(Environment.CurrentDirectory, fileName);
            File.WriteAllBytes(fullPath, pdfData);
            Console.WriteLine($"PDF saved to: {fullPath}");
        }
    }
}
