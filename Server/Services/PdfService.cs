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
        private static readonly double beliefHeight = 23;
        private static readonly double beliefSpacing = 10;
        private static readonly double beliefTextPadding = 5;
        private static readonly int beliefsPerPage = 20;
        private static readonly int fontSize = 16;

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

            RadFixedPage page = null;
            FixedContentEditor editor = null; // Declare the editor here

            for (int i = 0; i < beliefsList.Count; i++)
            {
                if (i % beliefsPerPage == 0)
                {
                    page = document.Pages.AddPage();
                    page.Size = new Size(600, 750);
                }

                double currentTopOffset = defaultTopOffset + (i % beliefsPerPage) * (beliefHeight + beliefSpacing);

                editor = new FixedContentEditor(page);
                editor.TextProperties.FontSize = 14;
                editor.Position.Translate(defaultLeftIndent, currentTopOffset);

                DrawBelief(editor, beliefsList[i], defaultLeftIndent, currentTopOffset);
            }

            return document;
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
