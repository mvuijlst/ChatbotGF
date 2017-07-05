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
            List<QuickReply> reply = new List<QuickReply>();
            reply.Add(new QuickReply("text", "Wat gebeurt hier?", "GET_EVENT_HERE_NOW"));
            GenericMessage message = new GenericMessage(id,"Hallo, mijn naam is Cubje. Onderaan zie je een aantal suggesties. Je kan ook altijd opnieuw beginnen door op de knop te drukken.",reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

    }
}
