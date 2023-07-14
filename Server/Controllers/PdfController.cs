using CoreBeliefsSurvey.Server.Services;
using CoreBeliefsSurvey.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CoreBeliefsSurvey.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly PdfService _pdfService;

        public PdfController(PdfService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpPost]
        public IActionResult CreatePdf([FromBody] List<CoreBeliefResponse> beliefsList)
        {
            byte[] fileData = _pdfService.GenerateFile(beliefsList);
            return File(fileData, "application/pdf", "BeliefsPdfProcessing-Generated.pdf");
        }
    }

}
