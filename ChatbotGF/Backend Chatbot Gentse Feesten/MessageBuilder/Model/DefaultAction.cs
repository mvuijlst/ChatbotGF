using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class DefaultAction
    {
        public DefaultAction(string type, string url, bool messenger_extensions)
        {
            this.type = type; // required
            this.url = url; // required
            this.messenger_extensions = messenger_extensions; // not required
        }
        public string type { get; set; }
        public string url { get; set; }
        public bool messenger_extensions { get; set; }

    }
}
