using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Chatbot_GF.BotData.MessengerData;
using Chatbot_GF.BotData;
using Chatbot_GF.MessengerManager;
using Chatbot_GF.Data;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Chatbot_GF.Controllers
{
    [Route("api/[controller]")]
    public class MessengerController : Controller
    {
        private MessageHandler mhandler;
        private PayloadHandler phandler;
        private readonly ILogger _logger;

        public MessengerController(ILogger<MessengerController> logger)
        {
            mhandler = new MessageHandler();
            phandler = PayloadHandler.Instance;
            _logger = logger;
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
            _logger.LogInformation("Messenger data received");
            //System.Console.WriteLine(JsonConvert.SerializeObject(data));

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
                                //Console.WriteLine($"Coordinates Received: {coords.lon} {coords.lat}");
                                string lang = TempUserData.Instance.GetLanguage(currentMessage.sender.id);
                                if (string.IsNullOrWhiteSpace(lang))
                                    lang = "";
                                if (!TempUserData.Instance.WantsToilet(message.sender.id))
                                {
                                    currentMessage.postback = new Postback { payload = $"DEVELOPER_DEFINED_COORDINATES°{coords.lon}:{coords.lat}°{lang}" };
                                    //Console.WriteLine("False " + currentMessage.postback);
                                    phandler.handle(message);
                                }
                                else
                                {
                                    currentMessage.postback = new Postback { payload = $"GET_TOILET°{coords.lon}:{coords.lat}°{lang}" };
                                    //Console.WriteLine("True " +  currentMessage.postback);
                                    phandler.handle(message);
                                }
                                TempUserData.Instance.Remove(message.sender.id); //Remove the user from the set
                            }
                            catch(Exception ex)
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