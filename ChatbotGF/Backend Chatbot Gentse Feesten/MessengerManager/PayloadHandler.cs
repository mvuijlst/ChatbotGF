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
            //payload indicates which category data in messengers has been given
            int pos = message.postback.payload.IndexOf("-");
            String category, value;
            if (pos != -1)
            {
                category = message.postback.payload.Substring(0, pos);
                value = message.postback.payload.Substring(pos + 1);
            }
            else
            {
                category = message.postback.payload;
            }

            Console.WriteLine(category);

            switch (message.postback.payload)
            {
                case "GET_STARTED_PAYLOAD":
                    manager.startUser(message.sender.id);
                    break;
                case "DEVELOPER_DEFINED_LOCATION":
                    break;
                case "GET_EVENT_HERE_NOW":
                    rmanager.SendLocationQuery(message.sender.id);
                    break;

                default:
                    //do nothing
                    break;
            }

        }
    }
}
