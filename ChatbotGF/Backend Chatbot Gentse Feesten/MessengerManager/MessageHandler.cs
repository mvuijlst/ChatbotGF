using Chatbot_GF.Client;
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
                replyManager.SendTextMessage(message.sender.id, message.message.text);
            }                
            
        }

    }
}
