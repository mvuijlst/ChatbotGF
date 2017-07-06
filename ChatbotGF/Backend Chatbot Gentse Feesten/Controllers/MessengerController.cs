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
        private MessageHandler mhandler;
        private PayloadHandler phandler;

        public MessengerController()
        {
            mhandler = new MessageHandler();
            phandler = new PayloadHandler();
        }

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
                        //Check current message if text is recognized and sets corresponding payload
                        Messaging currentMessage = mhandler.MessageRecognized(message);

                        if (currentMessage.postback != null)
                        {
                            phandler.handle(message);

                        }
                        else if (!string.IsNullOrWhiteSpace(currentMessage?.message?.quick_reply?.payload))
                        {
                            //set the quick reply payload as the message payload
                            currentMessage.postback = new Postback { payload = message.message.quick_reply.payload };
                            phandler.handle(message);
                        }
                        else if(currentMessage?.message?.attachments != null)
                        {
                            MessengerData.Attachment locationAtt = currentMessage?.message?.attachments[0];
                            Coordinates coords = locationAtt.payload?.coordinates;
                            
                        }
                        else
                        {
                            mhandler.ReplyRecieved(currentMessage);
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
