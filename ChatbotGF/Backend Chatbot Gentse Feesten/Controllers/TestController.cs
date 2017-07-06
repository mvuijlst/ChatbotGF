using Chatbot_GF.MessageBuilder.Factories;
using Chatbot_GF.MessageBuilder.Model;
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
            Name name = new Name();
            name.nl = "Level Six";
            Description description = new Description();
            description.nl = "Vanaf de eerste noten ben je gegarandeerd van een volle dansvloer!\r\nJo Hens (Niko uit Familie) \u0026 Katerine Avgoustakis (VTM, Winnares Star Acedemy, Here Come All The Boys,\u2026) zorgen samen met 4 professionele muzikanten voor interactie, top-entertainment, Non-stop hits.";
            Event event1 = new Event
            {
                name = name,
                description = description
            };
            
            List<Event> events = new List<Event>();
            events.Add(event1);
            return Ok(CarouselFactory.makeCarousel(1333062916810232, events));
            }
        
    }
}

