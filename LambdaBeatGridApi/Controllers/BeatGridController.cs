using Microsoft.AspNetCore.Mvc;

namespace LambdaBeatGridApi.Controllers
{
    [Route("v1/beatgrid")]
    [ApiController]
    public class BeatGridController : ControllerBase
    {
        [HttpGet]
        public IActionResult HelloWorld()
        {
            return Ok("Hello World!");
        }
    }
}