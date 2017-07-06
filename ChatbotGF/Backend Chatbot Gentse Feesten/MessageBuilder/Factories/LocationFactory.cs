using Chatbot_GF.MessageBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Factories
{
    public class LocationFactory
    {
        public static GenericMessage makeLocationButton(long id)
        {
            List<SimpleQuickReply> quick_replies = new List<SimpleQuickReply>();
            quick_replies.Add(new SimpleQuickReply("location"));
            return new GenericMessage(id, "Duid een locatie aan op de kaart.", quick_replies);
        }
    }
}
