using Microsoft.AspNetCore.Mvc;
using BeatGrid.Application.Services;
using System.Threading.Tasks;
using System.Linq;
using BeatGrid.Contracts.Response;
using Microsoft.AspNetCore.Authorization;
using BeatGrid.Contracts.Common;
using FluentValidation;
using System.Collections.Generic;

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

        [HttpPost]
        public async Task<ActionResult<Beat>> CreateBeat(Beat beat)
        {
            try
            {
                var id = await _beatService.CreateBeat(beat, GetUserId());
                return Created($"/beat/{id}", beat);
            }
            catch(ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        // TODO: Endpoint for getting single beat

        [HttpPut]
        public async Task<IActionResult> UpdateBeat(Beat beat)
        {
            try
            {
                await _beatService.UpdateBeat(beat, GetUserId());
                return NoContent();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(beat);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeat(string id)
        {
            try
            {
                await _beatService.DeleteBeat(id);
                return Ok();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(id);
            }
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? null;
        }
    }
}