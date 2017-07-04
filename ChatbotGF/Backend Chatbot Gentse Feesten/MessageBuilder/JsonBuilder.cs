using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.Controllers
{
    public class JsonBuilder
    {
        private string json;
        public JsonBuilder(string user_id, string msg)
        {
            json = $@" {{recipient: {{  id: {user_id}}},message: {{text: ""{msg}"" }}}}";
        }

        public string Json {get;}
    }
}
