---
description: Technologies and processes used in the application
---

# 🛠 Methodology

<details>

<summary>Project Template</summary>

For this project with the goal of exploring the Telerik Component Library I used the Blazor template from the Blazor library, which seemed pretty much like the default Blazor template with references added for the Telerik components. Steps for using the template are as follows:

1. **Install Telerik UI for Blazor:** First, you need to make sure that Telerik UI for Blazor is installed in your system. You can download it from the Telerik website and follow the installation steps provided.
2. **Open Visual Studio:** Launch Visual Studio on your machine.
3. **Create a New Project:** Click on "Create a new project" on the Visual Studio welcome screen.
4. **Search for the Template:** In the 'Create a new project' window, use the search bar to find "Telerik UI for Blazor Template". If the template is correctly installed, it should appear in the search results.
5. **Select the Template:** Click on the "Telerik UI for Blazor Template" and then click the 'Next' button.
6. **Configure Your New Project:** In the next window, name your project, select the location where the project files will be stored, and optionally change the solution name. Click 'Create' when done.
7. **Choose the Type of Application:** Choose whether you want a 'WebAssembly App' or a 'Server App'. Click the 'Create' button when you're ready.
8. **Wait for Project Setup:** Visual Studio will then set up the new project with the Telerik Blazor template. Wait for it to finish.
9. **Run Your Application:** Once the project setup is completed, you can run your new Telerik Blazor application by clicking the green 'Start Debugging' button, or by pressing F5.
10. **Add Telerik Components:** Now you can add Telerik components to your application as needed.



</details>

<details>

<summary>Repository</summary>

I chose to host the code in a public GitHub repository. I was curious about GitHub deployment and didn’t think there was anything particularly proprietary about the application that I was writing.

The code is at: [https://github.com/gordondan/CoreBeliefsSurvey](https://github.com/gordondan/CoreBeliefsSurvey).

To get a copy of the code you can use the following steps:

1. **Install Git**, if you haven't already done so. Download it from the official Git website: [https://git-scm.com/downloads](https://git-scm.com/downloads)
2. **Open Terminal (macOS/Linux) or Command Prompt (Windows).**
3. **Navigate to the directory** where you want to clone the repository by using the 'cd' (change directory) command. For example, if you want to navigate to a folder named "Projects" on your desktop, you would type: `cd Desktop/Projects` Replace "Desktop/Projects" with the correct path for your case.
4. Clone the repository by using the 'git clone' command followed by the URL of the GitHub repository. In your case, the command would be: `git clone https://github.com/gordondan/CoreBeliefsSurvey.git`
5. Once the repository is cloned, navigate into the new directory that was created with the 'cd' command: `cd CoreBeliefsSurvey`
6. Now you have a local copy of the repository and you can view or edit the code as needed. If the repository is updated in the future, you can get those updates by pulling the changes. Use the 'git pull' command while inside the repository directory: `git pull`





</details>

<details>

<summary>Hosting</summary>

I hosted the survey as an Azure App Service. The following are the steps to create an Azure App Service.

1. **Sign in to the Azure portal**: Go to the Azure portal at [https://portal.azure.com](https://portal.azure.com) and sign in with your Azure account.
2. **Create a resource**: In the Azure portal, select "Create a resource" in the upper left-hand corner.
3. **Select "Web App"**: In the "New" window, search for "Web App" and select it.
4. **Create Web App**: Click the "Create" button to start creating your new Web App.
5. **Fill in the basics**: You will need to fill in the following details:
   * **Subscription**: Choose the subscription under which you want to group your resources.
   * **Resource Group**: Create a new one or use an existing resource group. A resource group is a logical container for resources deployed on Azure.
   * **Name**: Choose a unique name for your Web App. This will be part of the URL users will use to access your application (name.azurewebsites.net).
   * **Publish**: Choose "Code" if you are deploying code or "Docker Container" if you are deploying a Dockerized application.
   * **Runtime Stack**: Choose the technology stack that you are using for your app, for example, .NET, Node.js, PHP, etc.
   * **Operating System**: Choose between Windows and Linux based on your preference and app requirements.
   * **Region**: Select the geographic location where you want to host your app.
   * **App Service Plan**: Here, you can either create a new plan or use an existing one. The plan determines the cost, capacity, and features of the App Service.
6. **Review + create**: After filling in the details, click the "Review + create" button to review your settings. Once you've verified everything, click the "Create" button to create the app service.
7. **Deployment**: After your app service is created, you can deploy your application. You have several options for deployment like local Git, GitHub, Bitbucket, or direct deployment with Visual Studio, among others.

In this case I used my MSDN account subscription and created a new resource group for the application. I named it corebeliefs selected code. I chose to run the application in .NET on Windows in the region West3. As previously mentioned I decided to use GitHub as the deployment method. When configuring the project the Azure tools created a yaml file and stored it in my repository which is used as a GitHub action for deployments.



</details>

<details>

<summary>Blazor Overview</summary>

## Blazor Overview

**Blazor** is a free and open-source web framework developed by Microsoft. It allows developers to build interactive web UIs using C# instead of JavaScript. Blazor apps are composed of reusable web UI components implemented using C#, HTML, and CSS.

### Types of Blazor Apps

There are two hosting models for Blazor:

* **Blazor Server**: In this model, the app is executed on the server from within an ASP.NET Core app. UI updates, event handling, and JavaScript calls are handled over a SignalR connection.
* **Blazor WebAssembly** (used in this project): This is a single-page app framework for building interactive client-side web apps with .NET. Blazor WebAssembly uses open web standards without plugins or code transpilation and works in all modern web browsers, including mobile browsers.

### How to create a new Blazor application:

To create a new Blazor app, you can use the following command:

```bash
dotnet new blazorserver -o BlazorApp
```

* `blazorserver` specifies the Blazor template to use.
* `-o BlazorApp` creates the project in a new directory named BlazorApp.

Once the application is created, navigate into the new directory:

```bash
cd BlazorApp
```

Then, run the app with:

```bash
dotnet run
```

When the app is running, navigate to `https://localhost:5001` in your web browser to view the app.

### Example of a simple component in Blazor:

Here's an example of a simple counter component in Blazor:

```csharp
@page "/counter"

<h1>Counter</h1>

<p>Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }
}
```

In this example, `@page "/counter"` specifies the route for this component, `currentCount` is a field that holds the current count, and `IncrementCount` is a method that increments the current count. The `@onclick="IncrementCount"` attribute in the button element specifies that `IncrementCount` should be called when the button is clicked.

</details>

<details>

<summary>Azure Key Vault</summary>

**Azure Key Vault** is a cloud service for securely storing and accessing secrets, keys, and certificates. A "secret" could be an API key, connection string, or any other piece of confidential data that your application needs to run, but that you don't want to expose in your code. Here's a basic guide to setting it up and using it:

### Step 1: Create an Azure Key Vault

1. Sign in to the [Azure portal](https://portal.azure.com).
2. In the left-hand menu, click on "Create a resource".
3. In the "Search the Marketplace" box, type "Key Vault" and select it from the dropdown.
4. Click "Create".
5. Fill in the fields:
   * "Subscription": Choose the subscription that should be charged for the usage of this service.
   * "Resource group": Create a new one or use an existing one.
   * "Key vault name": Choose a unique name for your Key Vault.
   * "Region": Choose the geographic region where you want your Key Vault to reside.
   * "Pricing tier": Choose a pricing tier. The default should be fine for most uses.
6. Click "Review + create", then "Create" after checking your settings.

### Step 2: Add a secret to the Key Vault

1. In the Azure portal, go to the Key Vault you just created.
2. In the Key Vault's left-hand menu, click on "Secrets".
3. Click on "+ Generate/Import".
4. Fill in the fields:
   * "Upload options": Choose "Manual" to manually enter the secret.
   * "Name": Choose a name for your secret. You'll use this name to retrieve the secret in your code.
   * "Value": The actual secret value that you want to store.
   * "Set activation date" and "Set expiration date" are optional and allow you to set a time frame in which the secret is valid.
5. Click "Create".

### Step 3: Access the secret in your code

You'll need to use the Azure.Identity and Azure.Security.KeyVault.Secrets libraries in your .NET project. If they're not already installed, you can add them with the following commands in your terminal:

```bash
dotnet add package Azure.Identity
dotnet add package Azure.Security.KeyVault.Secrets
```

Then you can use this code to retrieve the secret:

```csharp
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var kvUri = "https://<YourKeyVaultName>.vault.azure.net";
var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

KeyVaultSecret secret = client.GetSecret("<YourSecretName>");

string secretValue = secret.Value;
```

Replace `<YourKeyVaultName>` with the name of your Key Vault, and `<YourSecretName>` with the name of your secret.

The `DefaultAzureCredential()` is a part of the Azure Identity library. It provides a simple way to authenticate the user running the application and can be used with minimal configuration.

For local development, you can authenticate by logging into Azure with the Azure CLI by running `az login`. For deploying into an Azure-hosted service, like Azure Functions or Azure App Service, you can authenticate with Managed Identities.

Remember to handle secrets retrieved from Key Vault securely in your application to prevent potential security risks.

### Step 4: Granting access

By default, the account used to create the Key Vault is the only account that has permission to access it. To grant access to other accounts:

1. Go to the Key Vault in the Azure portal.
2. In the Key Vault's left-hand menu, click on "Access policies".
3. Click on "+ Add Access Policy".
4. Fill out the "Add access policy" form and click "Add".
5. Don't forget to click "Save" on the "Access policies" page.

Now the account specified in the access policy you just created can access the Key Vault.

**Note:**

Please remember to keep the Key Vault name and secret name confidential. Anyone who has these, along with the appropriate access, could potentially read your secrets.

</details>

<details>

<summary>Azure Storage</summary>

## Using Azure Blob Storage

**Azure Blob Storage** is Microsoft's object storage solution for the cloud. It is optimized for storing massive amounts of unstructured data. Here's a simple guide on how to use it:

### Step 1: Create a Storage Account

1. Sign in to the [Azure portal](https://portal.azure.com).
2. In the left-hand menu, click on "Create a resource".
3. In the "Search the Marketplace" box, type "Storage Account" and select it from the dropdown.
4. Click "Create".
5. Fill in the fields:
   * "Subscription": Choose the subscription that should be charged for the usage of this service.
   * "Resource group": Create a new one or use an existing one.
   * "Storage account name": Choose a unique name for your Storage Account.
   * "Location": Choose the geographic region where you want your Storage Account to reside.
   * "Performance": Choose between Standard and Premium. Standard should be fine for most uses.
   * "Account kind": Choose "StorageV2" as it supports all the latest features.
6. Click "Review + create", then "Create" after checking your settings.

### Step 2: Create a Blob Container

1. In the Azure portal, go to the Storage Account you just created.
2. In the Storage Account's left-hand menu, click on "Blob service", then "Containers".
3. Click on "+ Container".
4. Name your new container and set the "Public access level". For most uses, "Private" is the best choice unless you specifically want the blobs to be publicly accessible.
5. Click "Create".

### Step 3: Upload a Blob

1. In the Azure portal, go to the container you just created.
2. Click on "+ Upload".
3. Select the file you want to upload and click "Upload".

### Step 4: Access a Blob

1. In the Azure portal, go to the blob you just uploaded.
2. To download the blob, click "Download". To get the blob's URL, click "Properties" and copy the URL.

In a .NET application, you would use the Azure.Storage.Blobs library to interact with the Blob Storage. You can add it to your project with the following command:

```bash
dotnet add package Azure.Storage.Blobs
```

Then you can use this code to upload a blob:

```csharp
using Azure.Storage.Blobs;
BlobServiceClient blobServiceClient = new BlobServiceClient("Your Connection String");
BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("Your Container Name");
BlobClient blobClient = containerClient.GetBlobClient("Your Blob Name");
await blobClient.UploadAsync("Your File Path");
```

Replace "Your Connection String" with the connection string of your Storage Account, "Your Container Name" with the name of your blob container, "Your Blob Name" with the name you want to give to your blob, and "Your File Path" with the path of the file you want to upload.

To download a blob:

```csharp
BlobDownloadInfo download = await blobClient.DownloadAsync();
using (FileStream downloadFileStream = File.OpenWrite("Download Path"))
{
    await download.Content.CopyToAsync(downloadFileStream);
    downloadFileStream.Close();
}
```

Replace "Download Path" with the path where you want to download the blob to.

## Using Azure Table Storage

**Azure Table Storage** is a service that stores structured NoSQL data in the cloud. Tables are ideal for storing structured, non-relational data.

### Step 1: Create a Storage Account

Follow the steps from the Blob Storage guide to create a Storage Account if you haven't already.

### Step 2: Create a Table

1. In the Azure portal, go to the Storage Account you created.
2. In the Storage Account's left-hand menu, click on "Table service", then "Tables".
3. Click on "+ Table".
4. Name your new table.
5. Click "OK".

### Step 3: Add an Entity

1. In the Azure portal, go to the table you just created.
2. Click on "+ Add".
3. Fill in the "Partition Key", "Row Key", and any other properties you want your entity to have.
4. Click "Insert".

In a .NET application, you would use the Azure.Data.Tables library to interact with Table Storage. You can add it to your project with the following command:

```bash
dotnet add package Azure.Data.Tables
```

Then you can use this code to create a table:

```csharp
using Azure.Data.Tables;
var serviceClient = new TableServiceClient("Your Connection String");
TableClient tableClient = serviceClient.GetTableClient("Your Table Name");
await tableClient.CreateIfNotExistsAsync();
```

Replace "Your Connection String" with the connection string of your Storage Account, and "Your Table Name" with the name of your table.

To add an entity:

```csharp
var entity = new TableEntity("PartitionKey", "RowKey")
{
    {"Property1", "Value1"},
    {"Property2", "Value2"}
};
await tableClient.AddEntityAsync(entity);
```

Replace "PartitionKey", "RowKey", "Property1", "Value1", "Property2", "Value2" with the keys, properties, and values for your entity.

To get an entity:

```csharp
TableEntity entity = await tableClient.GetEntityAsync<TableEntity>("PartitionKey", "RowKey");
```

Replace "PartitionKey", "RowKey" with the keys of the entity you want to get.

</details>

<details>

<summary>Secret Management</summary>

I used two methods of hiding sensitive information in this project. The first was the use of environment variables to hold the values of AZURE\_TENANT\_ID, AZURE\_CLIENT\_ID, AZURE\_CLIENT\_SECRET, and a key needed by Telerik for accessing licensed content. I also used Azure Key Vault to hold the connection string to the Azure storage that I configured for the project.

**Environment Variables**

To get the values of AZURE\_TENANT\_ID, AZURE\_CLIENT\_ID and AZURE\_CLIENT\_SECRET you can use the following steps.

1. **AZURE\_TENANT\_ID**: This is your Azure Active Directory (AAD) Tenant ID.
   * Go to the Azure portal at [https://portal.azure.com](https://portal.azure.com).
   * Click on "Azure Active Directory" from the side menu.
   * Click on "Properties". Here, you can see your "Directory ID". This is your Azure Tenant ID.
2. **AZURE\_CLIENT\_ID and AZURE\_CLIENT\_SECRET**: These are the Application (Client) ID and the Client Secret of your App Registration in AAD.
   * Go to the Azure portal.
   * Click on "Azure Active Directory" from the side menu.
   * Click on "App Registrations".
   * Click "New registration" to create a new App Registration, or select an existing one if you have already created it.
   * Once you are in your App Registration, you can see the "Application (Client) ID". This is your Azure Client ID.
   * To create a new Client Secret, click on "Certificates & secrets" in the left-hand menu.
   * Click on "New client secret", add a description, select the duration for the secret, and then click "Add".
   * Copy the value of the client secret immediately and store it securely. You won't be able to see it again after you leave this page. This is your Azure Client Secret.

Remember to secure these values as they are sensitive information. Anyone with these values can potentially access your Azure resources.

In the visual studio environment these variables can be set under the debug tab. In GitHub things are a little more complicated, but still reasonably straightforward. You use a secret in GitHub and then set the secret to an environment variable by editing the YAML file that is used for your deployment. These are the steps for setting a secret in GitHub:



1.  **Adding a secret to your GitHub repository**

    * Go to your GitHub repository and click on "Settings".
    * Click on "Secrets" in the left sidebar.
    * Click on the "New repository secret" button.
    * Add a name for your secret in the "Name" field. For example, `AZURE_TENANT_ID`.
    * Enter the corresponding value in the "Value" field.
    * Click on "Add secret" to save it.

    Repeat the steps for `AZURE_CLIENT_ID` and `AZURE_CLIENT_SECRET`.
2.  **Using the secret in your GitHub Actions workflow**

    In your GitHub Actions YAML workflow file, you can now reference these secrets as environment variables. For example:

```yaml
jobs:
  build:
    runs-on: ubuntu-latest
    env:
      AZURE_TENANT_ID: ${{ secrets.AZURE_TENANT_ID }}
      AZURE_CLIENT_ID: ${{ secrets.AZURE_CLIENT_ID }}
      AZURE_CLIENT_SECRET: ${{ secrets.AZURE_CLIENT_SECRET }}
```

**Nuget CI, CD Build Server Setup**

Notes for making Telerik part of the CICD pipeline can be found at: https://docs.telerik.com/blazor-ui/deployment/ci-cd-build-server

Use the instructions from Telerik to obtain a Telerik key.

After you have a Telerik key it can be used as an Environment Variable in your deployment workflow with settings made to the Nuget.config file in your project. For example:

```xml
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="MyTelerikFeed" value="https://nuget.telerik.com/v3/index.json" protocolVersion="3" />
  </packageSources>
  <packageSourceCredentials>
    <MyTelerikFeed>
      <add key="Username" value="api-key" />
      <add key="ClearTextPassword" value="%TELERIK%" />
    </MyTelerikFeed>
  </packageSourceCredentials>
</configuration>
```

</details>

<details>

<summary>CORS</summary>

In as much as I removed the data retrieval stories from this project, the configuration of CORS became unnecessary, but I am going to include notes in this report because it will be needed if data retrieval functions are later added to the scope of the project.

CORS is a security feature implemented by browsers to prevent resources being loaded from domains different from the current one, unless the server explicitly allows it.

In Azure, you can configure CORS rules for your storage account via the Azure Portal. Here's how you can do that:

1. **Sign in to the Azure portal.**
2. **Navigate to your storage account.**
3. **In the left menu for the storage account, scroll to the CORS section under the Settings group.**
   * You will see a list of services (Blob service, File service, etc.). Select the Blob Service.
   * Click on the "+ Add" button to add a new rule.
4. **Set the following fields:**
   * Allowed origins: This should be the domain from where you're making the request, e.g. `https://localhost:44380`. If you want to allow any domain, you can use `*`, but keep in mind that this is less secure.
   * Allowed methods: Choose at least `GET`, since you want to download a file.
   * Allowed headers: Enter `*` to allow all headers.
   * Exposed headers: Enter `*` to expose all headers.
   * Max age (seconds): This is the time that the results of a preflight request can be cached. You can set it to `600` (10 minutes) for example.
5. **Click on the "Add" button to save the rule.**

After setting up these rules, your Blazor application should be able to download files from your storage account without running into CORS issues. Please, remember to replace https://localhost:44380 with the actual URL of your production environment when deploying your app.

</details>

<details>

<summary>Reading and Writing Data to Local Storage</summary>

This project uses the Telerik.Windows.Documents.Fixed libraries to create PDF documents. The pdf document information is transferred between pages though local storage. In this project I used JavaScript interop to communicate with local storage because that was the first method that I came across, but in future projects or a rewtite of this one, I think I would use a third party library such a Blazored.LocalStorage.

To read from and write to local storage in a Blazor application, follow these steps:

1.  **Inject the IJSRuntime service into your Blazor component using the following code:**

    ```csharp
    @inject IJSRuntime JSRuntime;
    ```
2.  **Create methods for setting and getting local storage items. Use the following code:**

    ```csharp
    public async Task SetLocalStorage(string key, string value)
    {
        await JSRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }

    public async Task<string> GetLocalStorage(string key)
    {
        return await JSRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }
    ```
3.  **Use these methods in your code like this:**

    ```csharp
    @code {
        protected override async Task OnInitializedAsync()
        {
            // Writing to local storage
            await SetLocalStorage("ExampleKey", "ExampleValue");

            // Reading from local storage
            var value = await GetLocalStorage("ExampleKey");

            // Now value contains "ExampleValue"
        }
    }
    ```

"localStorage.setItem" and "localStorage.getItem" are JavaScript functions for interacting with local storage. "IJSRuntime.InvokeVoidAsync" and "IJSRuntime.InvokeAsync" are used to call these functions from Blazor.

You can also use the third-party library Blazored.LocalStorage, which provides .NET APIs for accessing local storage.

Here's how to use it:

1. **Install the Blazored.LocalStorage NuGet package.**
2. **Register the service. Add this line to your Startup.cs file or Program.cs in .NET 5+.**
3.  **Inject the service into your component:**

    ```csharp
    @inject Blazored.LocalStorage.ILocalStorageService LocalStorage
    ```
4.  **Use it in your code:**

    ```csharp
    @code {
        protected override async Task OnInitializedAsync()
        {
            // Writing to local storage
            await LocalStorage.SetItemAsync("ExampleKey", "ExampleValue");

            // Reading from local storage
            var value = await LocalStorage.GetItemAsync<string>("ExampleKey");

            // Now value contains "ExampleValue"
        }
    }
    ```

</details>

<details>

<summary>PDF Write</summary>

I used the Telerik.Windows.Documents.Fixed libraries to write the document. A method written with the help of ChatGPT accepts a list of CoreBeliefResponses and writes a byte array that can be used by the Telerik PDF display component. Some sample code is listed below to give a flavor of the library.

```csharp
    public async Task<byte[]> GeneratePdf(List<CoreBeliefResponse> beliefsList)
    {
        PdfFormatProvider formatProvider = new PdfFormatProvider();
        formatProvider.ExportSettings.ImageQuality = ImageQuality.High;

        byte[] renderedBytes = null;
        using (MemoryStream ms = new MemoryStream())
        {
            RadFixedDocument document = CreateDocument(beliefsList);
            formatProvider.Export(document, ms);
            renderedBytes = ms.ToArray();
        }

        return renderedBytes;
    }
    private RadFixedDocument CreateDocument(List<CoreBeliefResponse> beliefsList)
        {
            RadFixedDocument document = new RadFixedDocument();

            RadFixedPage page = null;
            FixedContentEditor editor = null;

            for (int i = 0; i < beliefsList.Count; i++)
            {
                if (i % beliefsPerPage == 0)
                {
                    page = document.Pages.AddPage();
                    page.Size = new Size(600, 750);

                    editor = new FixedContentEditor(page);
                    editor.TextProperties.FontSize = 14;

                    DrawHeader(editor, 600);
                    AddFooter(page, i / beliefsPerPage + 1, (beliefsList.Count - 1) / beliefsPerPage + 1);
                }

                double currentTopOffset = defaultTopOffset + (i % beliefsPerPage) * (beliefHeight + beliefSpacing);
                editor.Position.Translate(defaultLeftIndent, currentTopOffset);

                DrawBelief(editor, beliefsList[i], defaultLeftIndent, currentTopOffset);

                if ((i + 1) % beliefsPerPage == 0 || i == beliefsList.Count - 1)
                {
                    DrawBeliefsBorder(editor);
                }
            }

            return document;
        }
        private static void DrawBeliefsBorder(FixedContentEditor editor)
        {
            editor.GraphicProperties.StrokeColor = new RgbColor(0, 0, 0); // Black
            editor.GraphicProperties.StrokeThickness = 1; // Thickness for the border
            editor.GraphicProperties.IsStroked = true; // Enable stroking (drawing the outline)
            editor.GraphicProperties.IsFilled = false; // Disable filling
            editor.Position.Translate(0, 0);
            editor.DrawRectangle(new Rect(40,80, 525, 650));
        }

        private void DrawHeader(FixedContentEditor editor, double pageWidth)
        {
            double rectWidth = pageWidth - 2 * defaultLeftIndent;
            double rectHeight = 50;
            double leftOffset = defaultLeftIndent;
            double topOffset = defaultTopOffset / 2 - rectHeight / 2;

            editor.GraphicProperties.FillColor = new RgbColor(192, 192, 192);
            editor.GraphicProperties.StrokeThickness = 0;
            editor.GraphicProperties.IsStroked = false;

            editor.Position.Translate(leftOffset, topOffset);
            editor.DrawRectangle(new Rect(0, 0, rectWidth, rectHeight));

            Block block = new Block();
            block.TextProperties.Font = FontsRepository.Helvetica;
            block.TextProperties.FontSize = fontSize*3;
            block.InsertText("Core Beliefs Survey");

            Size blockSize = block.ActualSize;
            double textLeftOffset = 80;
            double textTopOffset = (rectHeight - blockSize.Height) / 2;

            editor.Position.Translate(textLeftOffset, textTopOffset);
            editor.DrawBlock(block);
        }

```

</details>

<details>

<summary>PDF Read</summary>

To display the PDF file I used the TelerikPdfViewer. The component reads the byte array from local storage and displays the PDF. The viewer presents the user with options for saving or printing the document. [https://demos.telerik.com/aspnet-ajax/pdfviewer/overview/defaultcs.aspx](https://demos.telerik.com/aspnet-ajax/pdfviewer/overview/defaultcs.aspx)

```csharp
@page "/pdfviewer"
@using Telerik.Blazor.Components.PdfViewer
@using Telerik.Blazor.Components

@if (isLoading)
{
    <TelerikLoader Type="LoaderType.ConvergingSpinner" />
}
else if (PdfData != null)
{
    <TelerikPdfViewer Width="100%" Height="100%" Data="@PdfData"></TelerikPdfViewer>
}

@code {
    [Inject] IJSRuntime JSRuntime { get; set; }
    public byte[] PdfData { get; set; }
    bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        var pdfBase64 = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "pdfData");
        if (!string.IsNullOrEmpty(pdfBase64))
        {
            PdfData = Convert.FromBase64String(pdfBase64);
        }
        isLoading = false;
    }

}
```



</details>

<details>

<summary>Data Grid</summary>

I used the Telerik data grid for this project. I want to do some further analysis, but out of the box I was very impressed with it for large displays and disappointed with it for mobile displays. I need to explore the documentation more to understand all of the switches available, but the defaults seemed to be very powerful. The filter didn't seem to work for the unicode emojies, but was great with numbers. Enabling excel export was as easy as adding the code

```xml
<GridToolBarTemplate>
     <GridCommandButton Command="ExcelExport" 
             Icon="@SvgIcon.FileExcel">Export to Excel</GridCommandButton>
</GridToolBarTemplate>
```

This is an example of the whole table instance in Blazor:

```xml
                 FilterMode="GridFilterMode.FilterMenu" Resizable="true" Reorderable="true" PageSize="25">
        <GridToolBarTemplate>
            <GridCommandButton Command="ExcelExport" Icon="@SvgIcon.FileExcel">Export to Excel</GridCommandButton>
        </GridToolBarTemplate>
        <GridExport>
            <GridExcelExport FileName="summary-grid-export" AllPages="true" />
        </GridExport>
        <GridColumns>
            <GridColumn Field="@nameof(GridDataRow.Positivity)" Title="Positivity" Width="5%" OnCellRender="@OnCellRenderHandler" />
            <GridColumn Field="@nameof(GridDataRow.Icon)" Title="" Width="5%" />
            <GridColumn Field="@nameof(GridDataRow.Belief)" Title="Belief" Width="60%" />
            <GridColumn Field="@nameof(GridDataRow.SelectedValueText)" Title="Response" Width="30%" />
        </GridColumns>
    </TelerikGrid>
```

I intend to experiment more with the width modifiers to see if there are better answers for mobile phone views.

</details>
