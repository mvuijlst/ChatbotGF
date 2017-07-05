using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class QuickReply
    {
        public QuickReply(string content, string title, string payload)
        {
            this.content_type = content;
            this.title = title;
            this.payload = payload;
        }
        public string content_type { get; set; }
        public string title { get; set; }
        public string payload { get; set; }
    }
}
