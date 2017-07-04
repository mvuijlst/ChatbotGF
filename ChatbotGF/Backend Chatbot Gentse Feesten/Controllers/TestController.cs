using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chatbot_GF.Data;

namespace Chatbot_GF.Controllers
{
    [Produces("application/json")]
    [Route("api/Test")]
    public class TestController : Controller
    {

        [HttpGet]
        public ActionResult Get()
        {
            RemoteDataManager rdm = new RemoteDataManager();
            DateTime time = DateTime.Now.AddDays(14).AddHours(-3);
            rdm.GetEventsHereNow("https://gentsefeesten.stad.gent/api/v1/location/f2e7a735-7632-486c-b70d-7e7340bfd340", time);
            Console.WriteLine(time.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            return Ok();
        }
    }
}