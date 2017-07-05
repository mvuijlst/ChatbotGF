using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class QuickReply : SimpleQuickReply
    {
        public QuickReply(string content_type, string title, string payload) : base(content_type)
        {
            this.title = title;
            this.payload = payload;
        }
        public string title { get; set; }
        public string payload { get; set; }
    }

    public class SimpleQuickReply
    {
        public SimpleQuickReply(string content_type)
        {
            this.content_type = content_type;
        }
        public string content_type { get; set; }

    }

}
