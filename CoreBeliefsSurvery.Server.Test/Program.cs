using CoreBeliefsSurvey.Server.Controllers;
using CoreBeliefsSurvey.Server.Services;
using CoreBeliefsSurvey.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace PdfGenerationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var pdfService = new PdfService();
            var pdfController = new PdfController(pdfService);

            Console.Write("Enter the number of beliefs to generate: ");
            var numberOfBeliefs = int.Parse(Console.ReadLine());

            var mockBeliefs = CreateMockBeliefs(numberOfBeliefs);

            var result = pdfController.CreatePdf(mockBeliefs);

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
