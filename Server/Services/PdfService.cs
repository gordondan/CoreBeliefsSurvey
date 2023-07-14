using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using CoreBeliefsSurvey.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Telerik.Documents.Primitives;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Media;

namespace CoreBeliefsSurvey.Server.Services
{
    public class PdfService
    {
        private static readonly double defaultLeftIndent = 50;
        private static readonly double defaultTopOffset = 100; // Double the top margin
        private static readonly double beliefHeight = 23;
        private static readonly double beliefSpacing = 10;
        private static readonly double beliefTextPadding = 5;
        private static readonly int beliefsPerPage = 18; // Adjusted beliefs per page
        private static readonly int fontSize = 16;

        private readonly BlobServiceClient _blobServiceClient;

        private readonly AppSettings _appSettings;
        private ILogger<PdfService> _logger;
        public PdfService(AppSettings appSettings,ILogger<PdfService> logger)
        {
            _appSettings = appSettings;
            _blobServiceClient = new BlobServiceClient(_appSettings.ConnectionString);
            _logger = logger;
        }

        public async Task<string> GenerateFileAndUpload(List<CoreBeliefResponse> beliefsList,Guid blobName)
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

            // Save PDF to Blob storage and return the blob's URI
            return await SavePdfToBlob(renderedBytes,$"{blobName}.pdf");
        }

        /// <summary>
        /// Saves a PDF to a blob container.
        /// </summary>
        /// <param name="pdfData">The byte array representing the PDF data.</param>
        /// <param name="blobName">The unique identifier for the blob.</param>
        /// <returns>The URI to the uploaded PDF if successful, otherwise null.</returns>
        public async Task<string> SavePdfToBlob(byte[] pdfData, string blobName)
        {
            try
            {
                // Get a reference to a container
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_appSettings.BlobName);

                // Check if containerClient is null
                if (containerClient == null)
                {
                    _logger.LogError($"Container client for {_appSettings.BlobName} could not be created");
                    return null;
                }

                // Create the container if it does not exist
                await containerClient.CreateIfNotExistsAsync();

                // Get a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(blobName.ToString());

                // Check if blobClient is null
                if (blobClient == null)
                {
                    _logger.LogError($"Blob client for {blobName} could not be created");
                    return null;
                }

                // Upload blob
                using (var ms = new MemoryStream(pdfData))
                {
                    await blobClient.UploadAsync(ms, true);
                }

                // Check if blob upload was successful by verifying if the blob exists
                var blobExists = await blobClient.ExistsAsync();

                if (!blobExists)
                {
                    _logger.LogError($"Blob {blobName} could not be uploaded");
                    return null;
                }

                // Return the URI to the uploaded PDF
                _logger.LogInformation($"Blob {blobName} uploaded successfully");

                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = containerClient.Name,
                    BlobName = blobClient.Name,
                    Resource = "b", // b for blob
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(24) // SAS token will be valid for 24 hours
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read); // Grant read permissions

                var sasUrl = GetSasUrl(containerClient,blobClient,$"{blobClient.Uri}");

                return sasUrl;
                //return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to save the PDF to the blob.");
                return null;
            }
        }
        private string GetSasUrl(BlobContainerClient containerClient, BlobClient blobClient,string url) {
            var storageAccountName = _appSettings.StorageAccountName;
            var storageAccountKey = _appSettings.StorageAccountKey;

            var storageSharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerClient.Name,
                BlobName = blobClient.Name,
                Resource = "b", // b for blob
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1) // SAS token will be valid for 1 hour
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read); // Grant read permissions

            var sasToken = sasBuilder.ToSasQueryParameters(storageSharedKeyCredential).ToString();

            var sasUrl = $"{blobClient.Uri}?{sasToken}"; // Append the SAS token to the blob URL
            return sasUrl;
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

        private Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource LoadImage(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                return new Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource(stream);
            }
        }

        private static void AddFooter(RadFixedPage page, int pageNumber, int totalPages)
        {
            // Add footer text
            FixedContentEditor editor = new FixedContentEditor(page);
            Block block = new Block();
            block.GraphicProperties.IsStroked = false;
            block.GraphicProperties.StrokeThickness = 0;
            block.InsertText($"Page {pageNumber} of {totalPages}");

            Size blockSize = block.ActualSize;
            double textLeftOffset = (page.Size.Width - blockSize.Width) / 2;
            double textTopOffset = page.Size.Height - defaultTopOffset / 2;
            editor.Position.Translate(textLeftOffset-50, textTopOffset);

            editor.DrawBlock(block);
        }


        private static void DrawBelief(FixedContentEditor editor, CoreBeliefResponse belief, double left, double top)
        {
            // Adjust selected value to ensure it falls within 1-5 range
            int selectedValue = belief.SelectedValue % 6;
            selectedValue = selectedValue == 0 ? 1 : selectedValue;

            // Calculate positivity value
            int positivityValue = belief.Belief.IsPositive ? selectedValue : 6 - selectedValue;

            // Determine rectangle color based on positivity value
            RgbColor fillColor = GetFillColor(positivityValue);

            // Define rectangle dimensions
            double rectWidth = 550 - defaultLeftIndent; // full width of page minus left indent
            double rectHeight = beliefHeight; // use beliefHeight only, no spacing is included in the rectangle

            // Set graphic properties for the rectangle
            editor.GraphicProperties.FillColor = fillColor;
            editor.GraphicProperties.StrokeColor = fillColor; // set the StrokeColor to match the FillColor
            editor.GraphicProperties.StrokeThickness = 0; // set the StrokeThickness to 0 to remove the border

            // Draw the colored rectangle
            editor.DrawRectangle(new Rect(0, -beliefSpacing / 2, rectWidth, rectHeight)); // position the rectangle 5 units from the left

            // Insert belief name
            Block block = new Block();
            block.TextProperties.Font = FontsRepository.Helvetica;
            block.TextProperties.FontSize = fontSize;
            string selectedValueText = GetSelectedValueText(selectedValue);
            block.InsertText($"{belief.Belief.BeliefName} - {selectedValueText}");

            editor.Position.Translate(left + 5, top);
            editor.DrawBlock(block, new Size(rectWidth - 2 * beliefTextPadding - 5, double.PositiveInfinity)); // Subtract 5 to account for the new padding
        }

        private static string GetSelectedValueText(int selectedValue)
        {
            switch (selectedValue)
            {
                case 1:
                    return "Disagree";
                case 2:
                    return "Somewhat Disagree";
                case 3:
                    return "Neutral";
                case 4:
                    return "Somewhat Agree";
                case 5:
                    return "Agree";
                default:
                    return "No Opinion";
            }
        }


        private static RgbColor GetFillColor(int positivityValue)
        {
            switch (positivityValue)
            {
                case 1:
                    return new RgbColor(255, 0, 0); // Red
                case 2:
                    return new RgbColor(255, 165, 0); // Orange
                case 3:
                    return new RgbColor(255, 255, 255); // White
                case 4:
                    return new RgbColor(255, 255, 0); // Yellow
                case 5:
                    return new RgbColor(0, 128, 0); // Green
                default:
                    return new RgbColor(0, 0, 0); // Default to black
            }
        }

    }
}
