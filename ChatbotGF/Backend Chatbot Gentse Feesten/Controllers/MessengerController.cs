using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using static Chatbot_GF.BotData.MessengerData;
using System.Net;
using Chatbot_GF.BotData;
using System.Diagnostics;
using Chatbot_GF.MessengerManager;

namespace Chatbot_GF.Controllers
{
    [Route("api/[controller]")]
    public class MessengerController : Controller
    {

        [HttpGet]
        public ActionResult Get()
        {

            var allUrlKeyValues = Request.Query;
            if (allUrlKeyValues["hub.mode"] == "subscribe" && allUrlKeyValues["hub.verify_token"] == "test123")
            {
                var returnVal = allUrlKeyValues["hub.challenge"];
                return Json(int.Parse(returnVal));
            }
            return NotFound();
        }


        [HttpPost]
        public ActionResult Post([FromBody] MessengerData data)
        {

            Task.Factory.StartNew(() =>
            {
                foreach (var entry in data.entry)
                {

                    foreach (var message in entry.messaging)
                    {
                        if (message.postback != null && message.postback.payload == "GET_STARTED_PAYLOAD")
                        {
                            var jsn = new JsonBuilder(message);
                            //String es = PostRawAsync("https://graph.facebook.com/v2.6/me/messages?access_token=EAADbmmTTQZBkBAGCYtymjKzMGGTr817rNVgsqNMAFxxVZCkrvKN5dkJfj88rhy3onuVwCAziCWPB1sBl3Jf5C6FujRZC1g6lRaRk1yW0M5EQvSQiKLFtkbNAYSqFpRZAsuBDqUXYpQz2K5PwZCopyzC5skFa1e7LOUhEZAdelk2QZDZD", jsn.Json()).Result;
                            Manager manager = new Manager();
                            manager.changeUserState(long.Parse(message.sender.id), message.postback.payload);
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(message?.message?.text))
                                continue;

                            var json = new JsonBuilder(message);
                            Console.Write(json);
                            String res = PostRawAsync("https://graph.facebook.com/v2.6/me/messages?access_token=EAADbmmTTQZBkBAGCYtymjKzMGGTr817rNVgsqNMAFxxVZCkrvKN5dkJfj88rhy3onuVwCAziCWPB1sBl3Jf5C6FujRZC1g6lRaRk1yW0M5EQvSQiKLFtkbNAYSqFpRZAsuBDqUXYpQz2K5PwZCopyzC5skFa1e7LOUhEZAdelk2QZDZD", json.Json()).Result;
                        }
                    }
                }
            });

            return Ok();
        }

        public static async Task<String> PostRawAsync(string url, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            using (var requestWriter = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                requestWriter.Write(data);
            }

            var response = (HttpWebResponse)await request.GetResponseAsync();
            if (response == null)
                throw new InvalidOperationException("GetResponse returns null");

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }

    }
}
