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
            GenericMessage message = new GenericMessage(id,"Hallo, mijn naam is Cubje. Onderaan zie je een aantal suggesties. Je kan ook altijd opnieuw beginnen door op de knop te drukken.",reply);
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
                {
                    
                }
                GenericMessage message = new GenericMessage(id, "Welke locatie wil je bezoeken?", reply);

                Console.WriteLine(api.SendMessageToUser(message).Result);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

    }
}
