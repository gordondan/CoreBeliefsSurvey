using CoreBeliefsSurvey.Shared.Models;
using System.Collections.Generic;
using System.IO;
using Telerik.Documents.Primitives;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace CoreBeliefsSurvey.Server.Services
{
    public class PdfService
    {
        private static readonly double defaultLeftIndent = 50;
        private static readonly double defaultTopOffset = 50;
        private static readonly double beliefHeight = 20;
        private static readonly double beliefSpacing = 5;
        private static readonly double beliefTextPadding = 5;
        private static readonly int beliefsPerPage = 15;

        public byte[] GenerateFile(List<CoreBeliefResponse> beliefsList)
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
            int beliefsCount = beliefsList.Count;

            int pageNumber = 0;

            while (pageNumber * beliefsPerPage < beliefsCount)
            {
                RadFixedPage page = document.Pages.AddPage();
                page.Size = new Size(600, 750);

                for (int beliefIndex = pageNumber * beliefsPerPage; beliefIndex < (pageNumber + 1) * beliefsPerPage && beliefIndex < beliefsCount; beliefIndex++)
                {
                    double currentTopOffset = defaultTopOffset + (beliefIndex % beliefsPerPage) * (beliefHeight + beliefSpacing);
                    FixedContentEditor editor = new FixedContentEditor(page);
                    editor.TextProperties.FontSize = 14;
                    editor.Position.Translate(defaultLeftIndent, currentTopOffset);

                    CoreBeliefResponse currentBelief = beliefsList[beliefIndex];
                    DrawBelief(editor, currentBelief);
                }

                pageNumber++;
            }

            return document;
        }

        private static void DrawBelief(FixedContentEditor editor, CoreBeliefResponse belief)
        {
            // Adjust selected value to ensure it falls within 1-5 range
            int selectedValue = belief.SelectedValue % 6;
            selectedValue = selectedValue == 0 ? 1 : selectedValue;

            // Calculate positivity value
            int positivityValue = belief.Belief.IsPositive ? selectedValue : 6 - selectedValue;

            // Determine rectangle color based on positivity value
            RgbColor fillColor = GetFillColor(positivityValue);

            // Insert belief name
            Block block = new Block();
            block.TextProperties.Font = FontsRepository.Helvetica;
            block.TextProperties.FontSize = 12;
            block.TextProperties.HighlightColor = fillColor;
            block.InsertText(belief.Belief.BeliefName);

            // Position the text block within the rectangle
           // editor.Position.Translate(beliefTextPadding, beliefTextPadding); // padding from the top-left of the rectangle
            editor.DrawBlock(block, new Size(490, double.PositiveInfinity)); // width to account for padding on either side
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
