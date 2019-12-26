using Microsoft.AspNetCore.Mvc;
using BeatGrid.Application.Services;
using System.Threading.Tasks;
using System.Linq;
using BeatGrid.Contracts.Response;

namespace BeatGrid.Rest
{
    [Route("v1/beat")]
    [ApiController]
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
        public async Task<ActionResult<GetBeatsResponse>> GetBeats()
        {
            var beats = await _beatService.GetBeats();

            var distinctSoundIds = beats.SelectMany(b => b.GetSoundIds()).Distinct();
            var sounds = await _soundService.GetSounds(distinctSoundIds);

            var response = new GetBeatsResponse { Beats = beats, Sounds = sounds };

            return Ok(response);
        }

        [HttpGet("{id}")]
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
        public async Task<IActionResult> CreateBeat(object beat)
        {
            // TODO: Perform validation
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