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
            Console.Write(message);
            json = $@" {{recipient: {{  id: {message.sender.id}}},message: {{text: ""{message.message.text}"" }}}}";
            Console.Write(json);

        }



        public JsonBuilder(long id, string s)
        {
            json = $@" {{recipient: {{  id: {id}}},message: {{text: ""{s}"" }}}}";
        }

        public string Json { get { return json; } }
             
    }
}

