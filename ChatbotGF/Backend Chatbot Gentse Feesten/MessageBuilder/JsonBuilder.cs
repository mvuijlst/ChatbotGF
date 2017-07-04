using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Chatbot_GF.BotData.MessengerData;

namespace Chatbot_GF.Controllers
{
    public class JsonBuilder
    {
        private string json;
        public JsonBuilder(Messaging message)
        {
            json = $@" {{recipient: {{  id: {message.sender.id}}},message: {{text: ""{message.message.text}"" }}}}";
        }

        public string Json {get;}
    }
}

