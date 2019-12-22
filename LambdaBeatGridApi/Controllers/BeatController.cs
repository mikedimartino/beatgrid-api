using Microsoft.AspNetCore.Mvc;
using BeatGrid.Application.Services;
using System.Threading.Tasks;

namespace BeatGrid.Rest
{
    [Route("v1/beat")]
    [ApiController]
    public class BeatController : ControllerBase
    {
        private readonly IBeatService _beatService;

        public BeatController(IBeatService beatService)
        {
            _beatService = beatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBeats()
        {
            var beats = await _beatService.GetBeats();
            return Ok(beats);
        }
    }
}