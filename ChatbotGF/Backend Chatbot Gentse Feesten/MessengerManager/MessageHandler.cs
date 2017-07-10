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

        public string Language_choice { get; set; }

        public MessageHandler()
        {
            replyManager = new ReplyManager();
            Language_choice = "GENTS";
        }

        /*public void ReplyRecieved(Messaging message)
        {
            if (!string.IsNullOrWhiteSpace(message?.message?.text))
            {
                // replyManager.SendImage(message.sender.id, "https://media.giphy.com/media/xUA7beWTUYAcGOkWIM/giphy.gif");
                replyManager.SendTextMessage(message.sender.id, DataConstants.GetMessage("Donot_understand", Language_choice));
                replyManager.SendLocationQuery(message.sender.id, 0);
            }                
            
        }*/

        public void CheckForKnowText(Messaging message)
        {
            if (!string.IsNullOrWhiteSpace(message?.message?.text))
            {
                string txt = message.message.text;
                FreeTextHandler.CheckText(message.sender.id, txt);
            }
        }
        /// <summary>
        /// If messages corresponds to any of the alread defined payloads, set the payload
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Messaging MessageRecognized(Messaging message)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(message?.message?.text))
                {
                    Console.WriteLine("Zoeken naar payload");
                    string response = FreeTextHandler.GetPayload(message.message.text);
                    if (response != null)
                    {
                        SetPayload(message, response);
                    }
                }
                return message;
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public void SetPayload(Messaging msg, String pl)
        {            
            msg.postback = new Postback { payload = pl };            
        }

    }
}
