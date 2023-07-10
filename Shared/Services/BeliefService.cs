using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using Azure.Data.Tables;

namespace CoreBeliefsSurvey.Shared.Services
{
    public class BeliefService
    {

    // Instantiate a TableClient which will be used to create and manipulate the table
    string connectionString = "<Your Azure Storage Connection String>";
    string tableName = "<Your Table Name>";
    var tableClient = new TableClient(connectionString, tableName);

    // Create the table if it doesn't exist
    await tableClient.CreateIfNotExistsAsync();

    // Path to the CSV file
    string path = "coreBeliefs.csv";

using (var reader = new StreamReader(path))
using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    // Get records from CSV file
    var records = csv.GetRecords<BeliefRecord>();

    // Loop through each record
    foreach (var record in records)
    {
        // Create a new TableEntity for each record
        TableEntity beliefEntity = new TableEntity(record.PartitionKey, record.RowKey)
        {
            { "Belief", record.Belief }
        };

    // Add the entity to the table
    await tableClient.AddEntityAsync(beliefEntity);
}
}

    }
}
