using CoreBeliefsSurvey.Server.Services;
using CoreBeliefsSurvey.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBeliefsSurvey.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly PdfService _pdfService;
        private readonly ILogger<PdfController> _logger;

        public PdfController(PdfService pdfService, ILogger<PdfController> logger)
        {
            _pdfService = pdfService;
            _logger = logger;
        }

        [HttpPost]
        /// <summary>
        /// Creates a PDF based on the provided list of CoreBeliefResponse objects.
        /// </summary>
        /// <param name="beliefsList">The list of CoreBeliefResponse objects to base the PDF on.</param>
        /// <returns>A PdfCreationResult object containing the result of the operation.</returns>
        public async Task<IActionResult> CreatePdf([FromBody] List<CoreBeliefResponse> beliefsList)
        {
            var result = new PdfCreationResult();

            try
            {
                if (beliefsList == null || !beliefsList.Any())
                {
                    _logger.LogWarning("No data provided for PDF creation.");
                    result.Success = false;
                    result.Message = "No data provided for PDF creation.";
                    return Ok(result); // Or BadRequest(result) depending on how you want to handle it
                }

                Guid pdfId = Guid.NewGuid();
                var url = await _pdfService.GenerateFileAndUpload(beliefsList, pdfId);
                result.PdfId = pdfId;
                result.Url = url;
                result.Success = true;
                result.Message = "PDF creation successful.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating PDF.");
                result.Success = false;
                result.Message = "Error while creating PDF.";
            }

            return Ok(result); // you can consider different status codes based on the result
        }
    }
}
