using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public interface Message
    {       
    }

    public class MessageAttachment
    {
        public MessageAttachment(Attachment attachment)
        {
            // for a carousel message
            this.attachment = attachment;
        }
        public Attachment attachment { get; set; }
    }

    public class MessageText
    {
        public MessageText(string text)
        {
            // message with only text
            this.text = text;
        }
        public string text { get; set; }
    }

    public class MessageQuickReply
    {
        public MessageQuickReply(string text, List<QuickReply> quick_replies)
        {
            // message with text and buttons

            this.text = text;
            this.quick_replies = quick_replies;
        }
        public string text { get; set; }
        public List<QuickReply> quick_replies { get; set; }
    }
}
