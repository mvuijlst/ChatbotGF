using Chatbot_GF.MessageBuilder.Factories;
using Chatbot_GF.MessageBuilder.Model;
using Chatbot_GF.MessengerManager;
using Chatbot_GF.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chatbot_GF.Controllers
{
    [Produces("application/json")]
    [Route("api/Test")]
    public class TestController : Controller
    {

        [HttpGet]
        public ActionResult Get()
        {
            FreeTextHandler.GetResponse("waar is da feestje","nl");
            return Ok();
            }
        
    }
}

