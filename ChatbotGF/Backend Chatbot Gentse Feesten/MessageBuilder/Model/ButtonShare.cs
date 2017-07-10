using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class ButtonShare : IButton
    {
        public ButtonShare()
        {
            this.type = "element_share"; // required
        }
        public string type { get; set; }
    }
}
