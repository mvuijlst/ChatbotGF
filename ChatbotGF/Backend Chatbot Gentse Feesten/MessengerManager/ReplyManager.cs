using Chatbot_GF.Client;
using Chatbot_GF.Data;
using Chatbot_GF.MessageBuilder.Factories;
using Chatbot_GF.MessageBuilder.Model;
using Chatbot_GF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessengerManager
{
    public class ReplyManager
    {
        private IMessengerApi api;
        private string hmess;

        public ReplyManager()
        {
            api = RestClientBuilder.GetMessengerApi();
        }

        public void SendWelcomeMessage(long id)
        {
            try
            {
                List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
                hmess = DataConstants.GetMessage("Search_location").GENTS;
                reply.Add(new QuickReply("text", hmess, "SEND_LOCATION_CHOICE"));
                hmess = DataConstants.GetMessage("Now").GENTS;
                reply.Add(new QuickReply("text", hmess, "DEVELOPER_DEFINED_LOCATION-ALL"));
                hmess = DataConstants.GetMessage("Welcome").GENTS;
                GenericMessage message = new GenericMessage(id, hmess, reply);
                Console.WriteLine(api.SendMessageToUser(message).Result);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void SendLocationsChoice(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            hmess = DataConstants.GetMessage("Choose_list").GENTS;
            reply.Add(new QuickReply("text", hmess, "GET_EVENT_HERE_NOW-0"));
            hmess = DataConstants.GetMessage("Choose_card").GENTS;
            reply.Add(new QuickReply("text", hmess, "GET_USER_LOCATION"));
            GenericMessage message = new GenericMessage(id, "Je kan ofwel een locatie kiezen, ofwel je eigen locatie gebruiken.", reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }
        public void SendLocationResult(long id, SearchableLocation loc)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            hmess = DataConstants.GetMessage("Ja").GENTS;
            reply.Add(new QuickReply("text", hmess, $"DEVELOPER_DEFINED_LOCATION-{loc.Name}"));
            hmess = DataConstants.GetMessage("My_location").GENTS;
            reply.Add(new QuickReply("text", hmess, "DEVELOPER_DEFINED_SEARCHFALSE"));
            hmess = DataConstants.GetMessage("Nearest_location").GENTS;
            string h = DataConstants.GetMessage("This_location").GENTS;
            GenericMessage message = new GenericMessage(id, $"Je bent het dichtst bij {loc.PrettyName}. Wil je op deze locatie zoeken?", reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendTextMessage(long id, string text)
        {
            GenericMessage message = new GenericMessage(id, text);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendGetLocationButton(long id)
        {
            GenericMessage message = LocationFactory.makeLocationButton(id);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendLocationQuery(long id, int page)
        {
            try
            {
                List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
                int lastindex = (page * 8 + 8) > DataConstants.Locations.Count ? DataConstants.Locations.Count : (page * 8 + 8);
                for (int i = page * 8; i < lastindex; i++)
                {
                    string l = DataConstants.Locations[i].PrettyName;
                    reply.Add(new QuickReply("text", l, "DEVELOPER_DEFINED_LOCATION-" + l));
                }
                //Max 10 quickreplies, we got more locations. When at first page, add extra button to show second page
                if(page == 0)
                {
                    hmess = DataConstants.GetMessage("More").GENTS;
                    reply.Add(new QuickReply("text", hmess, "GET_EVENT_HERE_NOW-" + 1));
                }
                else
                {
                    hmess = DataConstants.GetMessage("Previous").GENTS;
                    reply.Add(new QuickReply("text", hmess, "GET_EVENT_HERE_NOW-" + (page - 1)));
                }
                hmess = DataConstants.GetMessage("Which_location").GENTS;
                GenericMessage message = new GenericMessage(id, hmess, reply);

                Console.WriteLine(api.SendMessageToUser(message).Result);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        public void SendConfirmation(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            hmess = DataConstants.GetMessage("Yes").GENTS;
            reply.Add(new QuickReply("text", hmess, "SEND_LOCATION_CHOICE"));
            hmess = DataConstants.GetMessage("No").GENTS;
            reply.Add(new QuickReply("text", hmess, "DEVELOPER_DEFINED_SEARCHFALSE"));
            hmess = DataConstants.GetMessage("Other_location").GENTS;
            GenericMessage message = new GenericMessage(id, hmess, reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendInfoForEnding(long id)
        {
            hmess = DataConstants.GetMessage("Restart").GENTS;
            SendTextMessage(id, hmess);
            // fotos voor waar de knop is
        }


        public void SendNoEventFound(long id)
        {
            hmess = DataConstants.GetMessage("Not_found").GENTS;
            SendTextMessage(id, hmess);
        }
    }
}
