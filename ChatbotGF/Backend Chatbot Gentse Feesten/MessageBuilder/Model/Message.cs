using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class Message
    {
        public Message(string text, List<QuickReply> quick_replies)
        {
            // message with text and buttons
            this.text = text;
            this.quick_replies = quick_replies;
            this.attachment = null;
        }
        public Message(string text)
        {
            // message with only text
            this.text = text;
            this.attachment = null;
            this.quick_replies = null;
        }
        public Message(Attachment attachment)
        {
            // for a carousel message
            this.text = null;
            this.attachment = attachment;
            this.quick_replies = null;
        }
        public Attachment attachment { get; set; }
        public string text { get; set; }
        public List<QuickReply> quick_replies { get; set; }
    }
}
