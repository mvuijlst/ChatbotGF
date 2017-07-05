using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class Button
    {
        public Button(string title, string type, string url, string payload, bool messenger_extensions)
        {
            this.type = type; // required
            this.url = url; // required for web_url type
            this.payload = payload; // not required unless the type is = postback
            this.messenger_extensions = messenger_extensions; // not required
            this.title = title;  // required
        }
        public string type { get; set; }
        public bool messenger_extensions { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string payload { get; set; }
    }
}
