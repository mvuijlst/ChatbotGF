using Chatbot_GF.MessageBuilder.Factories;
using Chatbot_GF.MessageBuilder.Model;
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
            //tring[] locaties = { /*"BAUDELOHOF", "BEVERHOUTPLEINPLACEMUSETTE", "SINTJACOBS","STADSHAL",*/ "CENTRUM",  "EMILE BRAUNPLEIN", "LUISTERPLEIN", "GROENTENMARKT", "KORENLEI-GRASLEI", "KORENMARKT", "SINTBAAFSPLEIN", "STVEERLEPLEIN", "VLASMARKT", "VRIJDAGMARKT", "WILLEM DE BEERSTEEG" };
            /*{
                List<QuickReply> reply = new List<QuickReply>();
                foreach (string loc in locaties)
                {
                    string l = loc.ToLowerInvariant();
                    reply.Add(new QuickReply("text", l, "DEVELOPER_DEFINED_LOCATION-" + loc));

                }
                GenericMessage message = new GenericMessage(1333062916810232, "Welke locatie wil je bezoeken?", reply);
                return Ok(message);
            }*/
            return Ok(LocationFactory.makeLocationButton(1333062916810232));
            }
        }
    }
