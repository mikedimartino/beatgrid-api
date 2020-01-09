using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatGrid.Application.Services;
using BeatGrid.Contracts.Common;
using BeatGrid.Contracts.Response;
using FluentValidation;

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
        public async Task<ActionResult<IEnumerable<Beat>>> GetBeats()
        {
            try
            {
                var beats = await _beatService.GetBeats();
                return Ok(beats);
            }
            catch (Exception ex)
            {
                throw;
            }
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

        private string GetUserId() => HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? null;
    }
}