using CoreBeliefsSurvey.Server.Models;
using CoreBeliefsSurvey.Server.Services;
using CoreBeliefsSurvey.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBeliefsSurvey.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeliefsController : ControllerBase
    {
        private readonly BeliefService _beliefService;

        public BeliefsController(BeliefService beliefService)
        {
            _beliefService = beliefService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoreBelief>>> GetBeliefs()
        {
            var beliefs = await _beliefService.DownloadBeliefs();
            return Ok(beliefs);
        }

        [HttpPost]
        public async Task<ActionResult> UploadBeliefs([FromBody] List<CoreBeliefEntity> beliefs, [FromQuery] bool preventDuplicates = true)
        {
            await _beliefService.UploadBeliefs(beliefs, preventDuplicates);
            return Ok();
        }
    }
}
