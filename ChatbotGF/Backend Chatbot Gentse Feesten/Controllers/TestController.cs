

using Chatbot_GF.MessageBuilder.Factories;

using Chatbot_GF.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Chatbot_GF.Controllers
{
    [Produces("application/json")]
    [Route("api/Test")]
    public class TestController : Controller
    {

        [HttpGet]
        public ActionResult Get()
        {
            var name = new Name();
            var des = new Description();
            name.nl = "Zuid";
            des.nl = "Wow";
            Event event1 = new Event
            {
                name = name,
                description = des
            };
            event1.organizer = "iemand";
            List<Event> events = new List<Event>();
            events.Add(event1);
            return Ok(CarouselFactory.makeCarousel(1333062916810232, events));
        }
    }
}