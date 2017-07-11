using Microsoft.AspNetCore.Mvc;

namespace Chatbot_GF.Controllers
{
    [Produces("application/json")]
    [Route("api/Test")]
    public class TestController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}

