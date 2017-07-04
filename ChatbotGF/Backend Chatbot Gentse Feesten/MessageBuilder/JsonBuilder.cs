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
        private Messaging message;
        public JsonBuilder(Messaging message)
        {
            this.message = message;
            //Console.Write(message);
        }

        public string Json()
        {
            return $@" {{recipient: {{  id: {message.sender.id}}},message: {{text: ""{message.message.text}"" }}}}";
        }


    }
}

