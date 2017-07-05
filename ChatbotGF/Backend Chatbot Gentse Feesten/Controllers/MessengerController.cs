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
using Chatbot_GF.MessageBuilder.Model;
using Chatbot_GF.Client;
using static Chatbot_GF.MessageBuilder.Model.GenericMessage;

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
                            /*
                            Manager manager = new Manager();
                            manager.changeUserState(message.sender.id, message.postback.payload);
                            */
                            Manager manager = new Manager();
                            manager.startUser(message.sender.id);
                            startSearch(message.sender.id);

                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(message?.message?.text))
                                continue;
                            GenericMessage toSend = new GenericMessage(message.sender.id, message.message.text);
                            IMessengerApi api = RestClientBuilder.GetMessengerApi();

                            String result = api.SendMessageToUser(toSend).Result;
                            System.Console.WriteLine(result);
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
        

        public void startSearch(long id)
        {
            // welcome message, hardcode in GenericMessage
            GenericMessage welcomeMessage = new GenericMessage(id);
            GenericMessage welcomeImage = new GenericMessage(id, new Attachment("image", new PayloadImage("https://cdn.pastemagazine.com/www/system/images/photo_albums/cuberdons/large/cuberdons-1.jpg?1384968217")));
            IMessengerApi api = RestClientBuilder.GetMessengerApi();
            String resultWelcomeMessage = api.SendMessageToUser(welcomeMessage).Result;
            String resultWecomeImage = api.SendMessageToUser(welcomeImage).Result;
        }
    }
}
