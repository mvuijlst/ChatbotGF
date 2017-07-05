using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class ButtonUrl : IButton
    {
        public ButtonUrl(string title, string type, string url, bool messenger_extensions)
        {
            this.type = type; // required
            this.url = url; // required for web_url type
            this.messenger_extensions = messenger_extensions; // not required
            this.title = title;  // required
        }
        public string type { get; set; }
        public bool messenger_extensions { get; set; }
        public string url { get; set; }
        public string title { get; set; }
    }
}
