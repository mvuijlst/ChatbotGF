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

        public ReplyManager()
        {
            api = RestClientBuilder.GetMessengerApi();
        }

        public void SendWelcomeMessage(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", "Zoek op locatie", "SEND_LOCATION_CHOICE"));
            reply.Add(new QuickReply("text", "Wat is nu bezig?", "DEVELOPER_DEFINED_LOCATION-ALL"));
            GenericMessage message = new GenericMessage(id,"Hallo. Hieronder ziet u een aantal suggesties. U kunt ook altijd opnieuw beginnen door op de knop te drukken.",reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendLocationsChoice(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", "Kies uit lijst", "GET_EVENT_HERE_NOW-0"));
            reply.Add(new QuickReply("text", "Kies op kaart.", "GET_USER_LOCATION"));
            GenericMessage message = new GenericMessage(id, "Je kan ofwel een locatie kiezen, ofwel je eigen locatie gebruiken.", reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }
        public void SendLocationResult(long id, SearchableLocation loc)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", "Ja", $"DEVELOPER_DEFINED_LOCATION-{loc.Name}"));
            reply.Add(new QuickReply("text", "Gebruik mijn locatie.", "DEVELOPER_DEFINED_SEARCHFALSE"));
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
                    string mes = DataConstants.GetMessage("More").GENTS;
                    reply.Add(new QuickReply("text", mes, "GET_EVENT_HERE_NOW-" + 1));
                }
                else
                {
                    string mes = DataConstants.GetMessage("Previous").GENTS;
                    reply.Add(new QuickReply("text", mes, "GET_EVENT_HERE_NOW-" + (page - 1)));
                }
                GenericMessage message = new GenericMessage(id, "Welke locatie wil je bezoeken?", reply);

                Console.WriteLine(api.SendMessageToUser(message).Result);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        public void SendConfirmation(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", "Ja", "SEND_LOCATION_CHOICE"));
            reply.Add(new QuickReply("text", "Nee", "DEVELOPER_DEFINED_SEARCHFALSE"));
            GenericMessage message = new GenericMessage(id, "Wil je een andere locatie bekijken?", reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendInfoForEnding(long id)
        {
            SendTextMessage(id, "Typ \"opnieuw\" of klik Begin opnieuw in het menu naast het tekstvak als je een nieuwe zoekopdracht wil starten.");
            // fotos voor waar de knop is
        }


        public void SendNoEventFound(long id)
        {
            SendTextMessage(id, "Woeps! Ik kon jammer genoeg geen evenementen vinden.");
        }
    }
}
