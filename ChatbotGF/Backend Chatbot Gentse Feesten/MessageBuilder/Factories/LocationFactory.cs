using Chatbot_GF.Data;
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
            String hmess = DataConstants.GetMessage("Pick_map", "GENTS");
            return new GenericMessage(id, hmess, quick_replies);
        }
    }
}
