using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class Attachment
    {
        public Attachment(string type, IPayload payload)
        {
            this.type = type; // required
            this.payload = payload; // required
        }
        public string type { get; set; }
        public IPayload payload { get; set; }

    }
}
