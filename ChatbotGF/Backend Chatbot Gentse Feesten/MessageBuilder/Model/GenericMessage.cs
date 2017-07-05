using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class GenericMessage
    {
        public GenericMessage(long id, string text, List<QuickReply> quick_replies)
        {
            this.recipient = new Recipient(id);
            this.message = new Message(text, quick_replies);
        }

        public GenericMessage(long id, string text)
        {
            this.recipient = new Recipient(id);
            this.message = new Message(text);
        }
        public GenericMessage(long id, Attachment attachment)
        {
            this.recipient = new Recipient(id);
            this.message = new Message(attachment);
        }

        public GenericMessage(long id)
        {
            this.recipient = new Recipient(id);
            this.message = new Message("Welkom!");
        }

        public class Recipient
        {
            public Recipient(long id)
            {
                this.id = id;
            }
            public long id { get; set; }
        }

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

        public class Attachment
        {
            public Attachment(string type, Payload payload)
            {
                this.type = type; // required
                this.payload = payload; // required
            }
            public string type { get; set; }  
            public Payload payload { get; set; } 
            
        }

        public class Payload
        {
            public Payload(string template_type, List<Element> elements, bool sharable, string image_aspect_ratio)
            {
                this.template_type = template_type; // required
                this.elements = elements; // required
                this.sharable = sharable; // not required
                this.image_aspect_ratio = image_aspect_ratio; // not required
            }
            public string template_type { get; set; }
            public List<Element> elements { get; set; }
            public bool sharable { get; set; }
            public string image_aspect_ratio { get; set; }
        }

        public class Element
        {
            public Element(string title, string image_url, string subtitle, List<Button> buttons, DefaultAction default_action)
            {
                this.title = title; // required
                this.image_url = image_url; // not required
                this.buttons = buttons; // not required
                this.default_action = default_action; // not required
                this.subtitle = subtitle; // not required
            }
            public string title { get; set; }
            public string image_url { get; set; }
            public string subtitle { get; set; }
            public DefaultAction default_action { get; set; }
            public List<Button> buttons { get; set; }
        }

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

        public Recipient recipient { get; set; }
        public Message message { get; set; }


    }
}
