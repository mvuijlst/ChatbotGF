using Chatbot_GF.Client;
using Chatbot_GF.Data;
using Chatbot_GF.MessageBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Chatbot_GF.BotData.MessengerData;

namespace Chatbot_GF.MessengerManager
{
    public class MessageHandler
    {

        private ReplyManager replyManager;

        public MessageHandler()
        {
            replyManager = new ReplyManager();
        }

        public void ReplyRecieved(Messaging message)
        {
            if (!string.IsNullOrWhiteSpace(message?.message?.text))
            {
                replyManager.SendTextMessage(message.sender.id, "Ik begrijp niet wat je bedoelt, wilt u weten wat hier gebeurt?");
                replyManager.SendLocationQuery(message.sender.id, 0);
            }                
            
        }
        /// <summary>
        /// If messages corresponds to any of the alread defined payloads, set the payload
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Messaging MessageRecognized(Messaging message)
        {
            if (!string.IsNullOrWhiteSpace(message?.message?.text)){
                string value = message?.message?.text;
                if (value.ToLower().Contains("opnieuw")){
                    SetPayload(message, "GET_STARTED_PAYLOAD");   
                }
                else if(value.Length > 4 && DataConstants.GetLocation(value) != null) //the typed message is a valid location
                {
                    SetPayload(message, "DEVELOPER_DEFINED_LOCATION-" + DataConstants.GetLocation(value).Name);
                }
            }
            return message;
        }

        public void SetPayload(Messaging msg, String pl)
        {            
            msg.postback = new Postback { payload = pl };            
        }

    }
}
