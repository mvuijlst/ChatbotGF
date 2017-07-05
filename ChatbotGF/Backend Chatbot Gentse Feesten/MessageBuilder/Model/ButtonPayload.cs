using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class ButtonPayload : IButton
   {
        public ButtonPayload(string title, string type, string payload)
        {
            this.type = type; // required
            this.payload = payload; // not required unless the type is = postback
            this.title = title;  // required
        }
        public string type { get; set; }
        public string title { get; set; }
        public string payload { get; set; }
    }
}
