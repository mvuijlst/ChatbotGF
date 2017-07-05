using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chatbot_GF.Data;
using Chatbot_GF.MessageBuilder.Model;
using static Chatbot_GF.MessageBuilder.Model.GenericMessage;

namespace Chatbot_GF.Controllers
{
    [Produces("application/json")]
    [Route("api/Test")]
    public class TestController : Controller
    {

        [HttpGet]
        public ActionResult Get()
        {
            GenericMessage welcomeImage = new GenericMessage(1333062916810232, new Attachment("image", new PayloadImage("https://cdn.pastemagazine.com/www/system/images/photo_albums/cuberdons/large/cuberdons-1.jpg?1384968217")));

            return Ok(welcomeImage);
        }
    }
}