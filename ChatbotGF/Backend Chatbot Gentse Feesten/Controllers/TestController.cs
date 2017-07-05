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
            List<Element> lijst = new List<Element>();
            DefaultAction def = new DefaultAction("web_url", "https://gentsefeesten.stad.gent", true);
            List<Button> buttons = new List<Button>();
            for (var i = 0; i < 3; i++)
            {
                buttons.Add(new Button("Knop " + i, "web_url", "https://gentsefeesten.stad.gent", null, true));
            }
            for (var i = 0; i < 5; i++) {
                lijst.Add(new Element("Event " + i, "", "Uitleg " + i, buttons, def));
            }
            Payload pay = new Payload("generic", lijst, false, null);
            Attachment at = new Attachment("template", pay);
            GenericMessage mes = new GenericMessage(1333062916810232, at);
            return Ok(mes);
        }
    }
}