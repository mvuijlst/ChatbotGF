using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class PayloadImage : IPayload
    {
        public PayloadImage(string url)
        {
            this.url = url;
        }
        public string url { get; set; }
    }
}
