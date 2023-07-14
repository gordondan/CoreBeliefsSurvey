using CoreBeliefsSurvey.Server.Services;
using CoreBeliefsSurvey.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBeliefsSurvey.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GeneratePdf([FromBody] List<CoreBeliefResponse> beliefResponses)
        {
            try
            {
                byte[] pdfBytes = await _pdfService.GeneratePdf(beliefResponses);
                var stream = new MemoryStream(pdfBytes);

                // Provide a file name for the PDF file to be downloaded/previewed in browser.
                return File(stream, "application/pdf", "BeliefsSummary.pdf");
            }
            catch (Exception ex)
            {
                // Log exception details here and return a proper error response.
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}