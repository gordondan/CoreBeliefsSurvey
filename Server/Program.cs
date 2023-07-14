using CoreBeliefsSurvey.Server.Services;
using CoreBeliefsSurvey.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;

var builder = WebApplication.CreateBuilder(args);

// Get the connection string from Azure Key Vault
var keyVaultUrl = "https://core-beliefs-vault.vault.azure.net/";
var connectionStringSecretName = "ConnectionString";
var credential = new DefaultAzureCredential();
var client = new SecretClient(new Uri(keyVaultUrl), credential);
Console.WriteLine($"AZURE_TENANT_ID: {Environment.GetEnvironmentVariable("AZURE_TENANT_ID")}");
Console.WriteLine($"AZURE_CLIENT_ID: {Environment.GetEnvironmentVariable("AZURE_CLIENT_ID")}");
// Don't log the secret, this is just for testing. Never log secrets in a real application.
Console.WriteLine($"AZURE_CLIENT_SECRET: {Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET")}");

var secret = await client.GetSecretAsync(connectionStringSecretName);
var connectionString = secret.Value?.Value;

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Failed to retrieve the Azure Storage connection string from Azure Key Vault.");
}

// Create an instance of AppSettings
var appSettings = new AppSettings { DefaultConnectionString = connectionString, TableName = "Beliefs" };

// Add services to the container.
builder.Services.AddSingleton(appSettings); // Inject AppSettings
builder.Services.AddSingleton<BeliefService>();
builder.Services.AddTransient<PdfService>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapDefaultControllerRoute();
app.MapFallbackToFile("index.html");

app.Run();
