using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class PayloadMessage : IPayload
    {
        public PayloadMessage(string template_type, List<Element> elements, bool sharable, string image_aspect_ratio)
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
}
