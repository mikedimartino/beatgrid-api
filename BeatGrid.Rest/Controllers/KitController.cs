using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeatGrid.Rest.Controllers
{
    [Route("v1/kit")]
    [ApiController]
    public class KitController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetKits()
        {
            throw new NotImplementedException();
        }

        [HttpGet("name")]
        public async Task<ActionResult<object>> GetKit(string name)
        {
            throw new NotImplementedException();
        }
    }
}