using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using Azure.Data.Tables;
using CoreBeliefsSurvey.Shared.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Azure;
using System.Linq;
using CoreBeliefsSurvey.Server.Models;
using Microsoft.Extensions.Options;

namespace CoreBeliefsSurvey.Server.Services
{
    public class BeliefService
    {
        private readonly string connectionString;
        private readonly string tableName;
        
        private readonly AppSettings _appSettings;

        public BeliefService(AppSettings appSettings)
        {
            _appSettings = appSettings;
            connectionString = _appSettings.ConnectionString;
            tableName = _appSettings.TableName;
        }

        public async Task UploadBeliefs(List<CoreBeliefEntity> beliefs, bool preventDuplicates = true)
        {
            // Instantiate a TableClient which will be used to create and manipulate the table
            var tableClient = new TableClient(connectionString, tableName);

            // Create the table if it doesn't exist
            await tableClient.CreateIfNotExistsAsync();

            // Loop through each record
            foreach (var record in beliefs)
            {
                // Create a new TableEntity for each record
                TableEntity beliefEntity = new TableEntity(record.PartitionKey, record.RowKey)
                {
                    { "Belief", record.BeliefName },
                    { "Description", record.BeliefDescription },
                    {"IsPositive",record.IsPositive }
                };

                if (preventDuplicates)
                {
                    try
                    {
                        // Add the entity to the table
                        await tableClient.AddEntityAsync(beliefEntity);
                    }
                    catch (RequestFailedException ex) when (ex.Status == 409) // 409 status code means the entity already exists
                    {
                        // Continue to the next record if this one already exists
                        continue;
                    }
                }
                else
                {
                    // Upsert the entity to the table (insert or update)
                    await tableClient.UpsertEntityAsync(beliefEntity);
                }
            }
        }

        public async Task<List<CoreBelief>> DownloadBeliefs()
        {
            var tableClient = new TableClient(connectionString, "Beliefs");

            List<CoreBelief> beliefs = new List<CoreBelief>();

            await foreach (var response in tableClient.QueryAsync<TableEntity>())
            {
                CoreBelief belief = new CoreBelief()
                {
                    BeliefName = response.GetString("Belief"),
                    BeliefDescription = response.GetString("Description"),
                    IsPositive = response.GetBoolean("IsPositive") ?? false
            };

                beliefs.Add(belief);
            }

            return beliefs;
        }


        public List<CoreBeliefEntity> ReadBeliefsFromCSV(string path)
        {
            List<CoreBeliefEntity> beliefs = new List<CoreBeliefEntity>();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<CoreBeliefEntityMap>();
                beliefs = csv.GetRecords<CoreBeliefEntity>().ToList();
            }

            return beliefs;
        }

        public async Task DeleteAllBeliefs()
        {
            var tableClient = new TableClient(connectionString, tableName);

            await foreach (var entity in tableClient.QueryAsync<TableEntity>())
            {
                await tableClient.DeleteEntityAsync(entity.PartitionKey, entity.RowKey);
            }
        }

        public List<CoreBeliefEntity> ReformatBeliefs(List<CoreBeliefEntity> beliefs)
        {
            List<CoreBeliefEntity> reformattedBeliefs = new List<CoreBeliefEntity>();

            foreach (var belief in beliefs)
            {
                // Split the BeliefName into belief and description
                string[] splitBelief = belief.BeliefName.Split(". ", 2);

                // If the split resulted in two parts, update the BeliefName and BeliefDescription accordingly
                if (splitBelief.Length == 2)
                {
                    belief.BeliefName = splitBelief[0].Trim();
                    belief.BeliefDescription = splitBelief[1].Trim();
                }

                reformattedBeliefs.Add(belief);
            }

            return reformattedBeliefs;
        }

    }
}
