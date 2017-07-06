using Chatbot_GF.MessageBuilder.Model;
using Chatbot_GF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chatbot_GF.MessageBuilder.Factories
{
    public class CarouselFactory
    {
        public static GenericMessage makeCarousel(long id, List<Event> events)
        {
            if (events.Count > 10)
            {
                events = events.GetRange(0, 10);
            }
            List<Element> elements = new List<Element>();
            foreach (var eve in events)
            {
                List<IButton> buttons = new List<IButton>();
                DefaultAction defaultAction;
                //if (eve.url == null)
                //{
                   // buttons.Add(new ButtonUrl("Meer info", "web_url", "https://gentsefeesten.stad.gent", true));
                /*}
                else
                {
                    // misschien moet je de url nog whitelisten !!!!!!!!!!!!!!
                    buttons.Add(new ButtonUrl("Meer info", "web_url", eve.url, true));
                }*/
                defaultAction = new DefaultAction("web_url", "https://gentsefeesten.stad.gent", true);
                // buttons.Add(new ButtonPayload("Kleine uitleg", "postback", "DEVELOPER_DEFINED_DESCRIPTION-" + eve.description.nl));
                buttons.Add(new ButtonPayload("Wanneer is het?", "postback", "DEVELOPER_DEFINED_HOURS-" + eve.startDate + "////****////" + eve.endDate));
                var image = eve.image;
                if (string.IsNullOrEmpty(image))
                {
                    image = /* keuze */ "https://gentsefeesten.stad.gent/sites/default/files/styles/large_3_2/public/2017-04/foto%20campagne%20timeline.png?itok=xs3Q9p3L";
                }
                
                elements.Add(new Element(eve.name.nl, image, buttons, defaultAction));
            }
            IPayload payload = new PayloadMessage("generic", elements, true, "horizontal");
            Attachment attachment = new Attachment("template", payload);
            return new GenericMessage(id, attachment);
        }
        
    }
}
