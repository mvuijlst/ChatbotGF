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

<<<<<<< HEAD
        public string Json()
        {
            return $@" {{recipient: {{  id: {message.sender.id}}},message: {{text: ""{message.message.text}"" }}}}";
        }


=======
        public JsonBuilder(long id, string s)
        {
            json = $@" {{recipient: {{  id: {id}}},message: {{text: ""{s}"" }}}}";
        }

        public string Json {get;}
>>>>>>> fe03658f20710a3add2cfc0eacc0be1a0cccd825
    }
}

