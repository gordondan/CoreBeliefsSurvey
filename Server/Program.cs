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
var tableConnectionStringName = "ConnectionString";
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
