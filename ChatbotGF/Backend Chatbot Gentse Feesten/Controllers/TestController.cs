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
            rdm.ReadFromOnlineStore();
            return Ok();
        }
    }
}