using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Chatbot_GF.BotData.MessengerData;

namespace Chatbot_GF.MessengerManager
{
    public class PayloadHandler
    {
        private Manager manager;
        private ReplyManager rmanager;

        public PayloadHandler()
        {
            manager = new Manager();
            rmanager = new ReplyManager();
            
        }

        public void handle(Messaging message)
        {
            switch (message.postback.payload)
            {
                case "GET_STARTED_PAYLOAD":
                    manager.startUser(message.sender.id);
                    break;
                default:
                    //do nothing
                    break;
            }

        }
    }
}
