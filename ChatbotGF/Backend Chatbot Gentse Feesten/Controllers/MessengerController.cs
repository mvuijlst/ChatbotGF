using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using static Chatbot_GF.BotData.MessengerData;
using System.Net;



namespace Chatbot_GF.Controllers
{
    [Route("api/[controller]")]
    public class MessengerController : Controller
    {
        // GET: api/values
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


        [ActionName("Receive")]
        [HttpPost]
        public ActionResult ReceivePost(BotRequest data)
        {
            Task.Factory.StartNew(async () =>
            {
                foreach (var entry in data.entry)
                {
                    foreach (var message in entry.messaging)
                    {
                        if (string.IsNullOrWhiteSpace(message?.message?.text))
                            continue;

                        var msg = "You said: " + message.message.text;
                        var json = $@" {{recipient: {{  id: {message.sender.id}}},message: {{text: ""{msg}"" }}}}";
                        await PostRawAsync("https://graph.facebook.com/v2.6/me/messages?access_tokenEAADbmmTTQZBkBAGCYtymjKzMGGTr817rNVgsqNMAFxxVZCkrvKN5dkJfj88rhy3onuVwCAziCWPB1sBl3Jf5C6FujRZC1g6lRaRk1yW0M5EQvSQiKLFtkbNAYSqFpRZAsuBDqUXYpQz2K5PwZCopyzC5skFa1e7LOUhEZAdelk2QZDZD", json);
                    }
                }
            });

            return Ok();
        }

        private async Task<string> PostRawAsync(string url, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            using (var requestWriter = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                requestWriter.Write(data);
            }

            var response = (HttpWebResponse) await request.GetResponseAsync();
            if (response == null)
                throw new InvalidOperationException("GetResponse returns null");

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }

    }
}
