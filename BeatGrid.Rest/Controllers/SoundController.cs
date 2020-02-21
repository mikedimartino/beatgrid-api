using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatGrid.Application.Services;
using BeatGrid.Contracts.Common;
using BeatGrid.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeatGrid.Rest.Controllers
{
    [Route("v1/sound")]
    [ApiController]
    [Authorize]
    public class SoundController : ControllerBase
    {
        private readonly ISoundService _soundService;

        public SoundController(ISoundService soundService) => _soundService = soundService;

        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SoundEntity>>> GetSounds()
        {
            var sounds = await _soundService.GetSounds();
            return Ok(sounds);
        }

        [HttpPost]
        public async Task<ActionResult<Beat>> CreateSound(SoundEntity sound)
        {
            var id = await _soundService.CreateSound(sound);
            return Created($"/sound/{id}", sound);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSound(SoundEntity sound)
        {
            await _soundService.UpdateSound(sound);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSound(string id)
        {
            try
            {
                await _soundService.DeleteSound(id);
                return Ok();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}