using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatGrid.Rest.Controllers
{
    [Route("v1/sound-category")]
    [ApiController]
    public class SoundCategoryController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCategories(string parent)
        {
            throw new NotImplementedException();
        }
    }
}
