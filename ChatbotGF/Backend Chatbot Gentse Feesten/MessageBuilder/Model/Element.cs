using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class Element
    {
        public Element(string title, string image_url, string subtitle, List<IButton> buttons, DefaultAction default_action)
        {
            this.title = title; // required
            this.image_url = image_url; // not required
            this.buttons = buttons; // not required
            this.subtitle = subtitle;
            this.default_action = default_action; // not required
        }
        public string title { get; set; }
        public string image_url { get; set; }
        public string subtitle { get; set; }
        public DefaultAction default_action { get; set; }
        public List<IButton> buttons { get; set; }
    }
}
