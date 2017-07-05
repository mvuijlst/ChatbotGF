using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class GenericMessage
    {
        public GenericMessage(long id, string text)
        {
            this.recipient = new Recipient(id);
            this.message = new Message(text);
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
            public Message(string text)
            {
                this.text = text;
            }
            public string text { get; set; }
        }

        public Recipient recipient { get; set; }
        public Message message { get; set; }

    }
}
