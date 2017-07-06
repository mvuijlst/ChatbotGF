using Chatbot_GF.Client;
using Chatbot_GF.Data;
using Chatbot_GF.MessageBuilder.Model;
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
            reply.Add(new QuickReply("text", "Wat gebeurt hier?", "GET_EVENT_HERE_NOW-0"));
            reply.Add(new QuickReply("text", "Wat gebeurt nu?", "DEVELOPER_DEFINED_LOCATION-ALL"));
            GenericMessage message = new GenericMessage(id,"Hallo. Onderaan zie je een aantal suggesties. Je kan ook altijd opnieuw beginnen door op de knop te drukken.",reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendTextMessage(long id, string text)
        {
            GenericMessage message = new GenericMessage(id, text);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendLocationQuery(long id, int page)
        {
            try
            {

                List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
                int lastindex = (page * 9 + 9) > DataConstants.Locations.Count ? DataConstants.Locations.Count : (page * 9 + 9);
                for (int i = page * 9; i < lastindex; i++)
                {
                    string l = DataConstants.Locations[i].PrettyName;
                    reply.Add(new QuickReply("text", l, "DEVELOPER_DEFINED_LOCATION-" + l));

                }
                //Max 10 quickreplies, we got more locations. When at first page, add extra button to show second page
                if(page == 0)
                {
                    reply.Add(new QuickReply("text", "Meer", "GET_EVENT_HERE_NOW-" + 1));
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
            reply.Add(new QuickReply("text", "Ja", "GET_EVENT_HERE_NOW"));
            reply.Add(new QuickReply("text", "Nee", "DEVELOPER_DEFINED_SEARCHFALSE"));
            GenericMessage message = new GenericMessage(id, "Wilt je een andere locatie bekijken?", reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }
        public void SendInfoForEnding(long id)
        {
            SendTextMessage(id, "ls je opnieuw wilt zoeken kan in het menu klikken op de knop Begin Opnieuw");
            // fotos voor waar de knop is
        }


        public void SendNoEventFound(long id)
        {
            SendTextMessage(id, "Wauw! Ik kon jammer genoeg geen evenementen vinden.");
        }
    }
}
