using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class ButtonTemplate : IPayload
    {
        public ButtonTemplate(string template_type, List<IButton> buttons, string text)
        {
            this.template_type = template_type; // required
            this.buttons = buttons; // required
            this.text = text;
        }

        public string template_type { get; set; }
        public List<IButton> buttons { get; set; }
        public string text { get; set; }
    }
}

