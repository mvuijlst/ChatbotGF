using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Model
{
    public class LocationElement : Element
    {
        public LocationElement(string title, string image_url, string subtitle, List<IButton> buttons, string item_url) : base(title, image_url, subtitle, buttons, null)
        {
            this.item_url = item_url;
        }

        public string item_url { get; set; }
    }
}
