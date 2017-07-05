using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Chatbot_GF.BotData.MessengerData;

namespace Chatbot_GF.MessengerManager
{
    public class PayloadHandler
    {
        private UserManager umanager;
        private ReplyManager rmanager;

        public PayloadHandler()
        {
            umanager = new UserManager();
            rmanager = new ReplyManager();
            
        }

        public void handle(Messaging message)
        {
            switch (message.postback.payload)
            {
                case "GET_STARTED_PAYLOAD":
                    umanager.startUser(message.sender.id);
                    break;
                case "GET_EVENT_HERE_NOW":
                    rmanager.SendLocationQuery(message.sender.id);
                    break;

                default:
                    //contains information for user
                    handleInformation(message.sender.id, message.postback.payload);
                    break;
            }

        }

        private void handleInformation(long id, string payload)
        {
            //payload indicates which category data in messengers has been given
            int pos = payload.IndexOf("-");
            string category = payload.Substring(0, pos);
            string value = payload.Substring(pos + 1);
            switch (category)
            {
                case "DEVELOPER_DEFINED_LOCATION":
                    umanager.setUserLocation(id, value);
                    break;
                case "DEVELOPER_DEFINED_TIME":
                    umanager.setUserTime(id, value);
                    break;
                case "DEVELOPER_DEFINED_SEARCH":
                    umanager.searchResults(id);
                    break;
                default:
                   //do nothing
                    break;
            }

        }
    }
}
