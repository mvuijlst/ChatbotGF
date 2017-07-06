using Chatbot_GF.Client;
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
            reply.Add(new QuickReply("text", "Wat gebeurt hier?", "GET_EVENT_HERE_NOW"));
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
            string[] locaties = {"BAUDELOHOF", "BEVERHOUTPLEINPLACEMUSETTE", "SINTJACOBS", "CENTRUM","STADSHAL", "EMILE BRAUNPLEIN", "LUISTERPLEIN", "GROENTENMARKT", "KORENLEI-GRASLEI", "KORENMARKT", "SINTBAAFSPLEIN", "STVEERLEPLEIN", "VLASMARKT", "VRIJDAGMARKT", "WILLEM DE BEERSTEEG" };
            try
            {
                List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
                for (int i = page * 10; i < (page * 10 + 10); i++)
                {
                    string l = locaties[i].ToLowerInvariant();
                    reply.Add(new QuickReply("text", l, "DEVELOPER_DEFINED_LOCATION-" + locaties[i]));

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
