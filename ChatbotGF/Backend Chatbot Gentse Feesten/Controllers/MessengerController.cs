using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Chatbot_GF.BotData.MessengerData;
using Chatbot_GF.BotData;
using Chatbot_GF.MessengerManager;

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
            phandler = PayloadHandler.Instance;
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
                        // Check current message if text is recognized and sets corresponding payload
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
                            try
                            {
                                Attachment locationAtt = currentMessage?.message?.attachments[0];
                                Coordinates coords = locationAtt.payload?.coordinates;
                                Console.WriteLine($"Coordinates Received: {coords.lon} {coords.lat}");
                                string lang = phandler.GetLanguage(currentMessage.sender.id);
                                if (string.IsNullOrWhiteSpace(lang))
                                    lang = "";

                                currentMessage.postback = new Postback { payload = $"DEVELOPER_DEFINED_COORDINATES°{coords.lon}:{coords.lat}°{lang}" };
                                Console.WriteLine(currentMessage.postback);
                                phandler.handle(message);
                            }catch(Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                            
                        }
                        else
                        {
                            mhandler.CheckForKnowText(currentMessage);
                        }
                    }
                }
            });
            return Ok();
        }
    }
}