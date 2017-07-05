using Chatbot_GF.MessageBuilder.Model;
using Chatbot_GF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
                // voor aantal knopen if doen wachten op event link 
                List<IButton> buttons = new List<IButton>();
                // link aanpassen 
                DefaultAction defaultAction = new DefaultAction("web_url", "https://gentsefeesten.stad.gent", true);
                buttons.Add(new ButtonUrl("Meer info", "web_url", "https://gentsefeesten.stad.gent", true));
                buttons.Add(new ButtonPayload("Kleine uitleg", "postback", "DEVELOPER_DEFINED_DESCRIPTION-" + /*eve.url*/ "https://gentsefeesten.stad.gent"));
                buttons.Add(new ButtonPayload("Wanneer is het?", "postback", "DEVELOPER_DEFINED_HOURS-" + /*eve.*/ "https://gentsefeesten.stad.gent"));
                elements.Add(new Element(eve.name.nl, "https://cdn.pastemagazine.com/www/system/images/photo_albums/cuberdons/large/cuberdons-1.jpg?1384968217"/*eve.image*/, eve.organizer, buttons, defaultAction));
            }
            IPayload payload = new PayloadMessage("generic", elements, true, "horizontal");
            Attachment attachment = new Attachment("template", payload);
            return new GenericMessage(id, attachment);
        }
    }
}
