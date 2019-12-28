using Microsoft.AspNetCore.Mvc;
using BeatGrid.Application.Services;
using System.Threading.Tasks;
using System.Linq;
using BeatGrid.Contracts.Response;
using Microsoft.AspNetCore.Authorization;

namespace BeatGrid.Rest
{
    [Route("v1/beat")]
    [ApiController]
    [Authorize]
    public class BeatController : ControllerBase
    {
        private readonly IBeatService _beatService;
        private readonly ISoundService _soundService;

        public BeatController(IBeatService beatService, ISoundService soundService)
        {
            _beatService = beatService;
            _soundService = soundService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<GetBeatsResponse>> GetBeats()
        {
            var beats = await _beatService.GetBeats();

            var distinctSoundIds = beats.SelectMany(b => b.GetSoundIds()).Distinct();
            var sounds = await _soundService.GetSounds(distinctSoundIds);

            var response = new GetBeatsResponse { Beats = beats, Sounds = sounds };

            return Ok(response);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<GetBeatResponse>> GetBeat(string id)
        {
            var beat = await _beatService.GetBeat(id);

            if (beat == null)
            {
                return NotFound();
            }

            var distinctSoundIds = beat.GetSoundIds().Distinct();
            var sounds = await _soundService.GetSounds(distinctSoundIds);

            var response = new GetBeatResponse { Beat = beat, Sounds = sounds };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateBeat(object beat)
        {
            // TODO: Perform validation
            // NOTE: Can get creator id by looking at 
                // HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value
            var id = await _beatService.CreateBeat(beat);
            return Created($"/beat/{id}", id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBeat(object beat)
        {
            await _beatService.UpdateBeat(beat);
            // Return 201 if didn't exist already?
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeat(string id)
        {
            await _beatService.DeleteBeat(id);
            return Ok();
        }
    }
}