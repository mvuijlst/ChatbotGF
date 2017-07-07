using Chatbot_GF.Data;
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
                DefaultAction defaultAction = new DefaultAction("web_url", "https://gentsefeesten.stad.gent", true);
                
                if (!string.IsNullOrWhiteSpace(eve.description.nl))
                {
                    buttons.Add(new ButtonPayload(DataConstants.GetMessage("What_Is_It", "NL"), "postback", "DEVELOPER_DEFINED_DESCRIPTION-" + eve.description.nl));
                }
                buttons.Add(new ButtonPayload(DataConstants.GetMessage("NEXT", "NL"), "postback", "DEVELOPER_DEFINED_NEXT-" + eve.name.nl + "-_-" + eve.location));

                var image = eve.image;
                if (string.IsNullOrEmpty(image))
                {
                    image = /* keuze */ "https://gentsefeesten.stad.gent/sites/default/files/styles/large_3_2/public/2017-04/foto%20campagne%20timeline.png?itok=xs3Q9p3L";
                }

                string dates = "";
                DateTime start = ResultParser.normalizeDate(eve.startDate.ToString());
                DateTime end = ResultParser.normalizeDate(eve.endDate.ToString());
                if (start.Day == end.Day)
                {
                    dates = $" Op de {start.Day}e van {start.ToString("HH:mm")} tot {end.ToString("HH:mm")}";
                }
                else
                {
                    dates = $" Vanaf de {start.Day}e om {start.ToString("HH:mm")} tot de {end.Day}e om {end.ToString("HH:mm")}";
                }

                string subtitle = DataConstants.GetLocation(eve.location).PrettyName + dates;
                elements.Add(new Element(eve.name.nl, image, subtitle, buttons, defaultAction));
            }
            IPayload payload = new PayloadMessage("generic", elements, true, "horizontal");
            Attachment attachment = new Attachment("template", payload);
            return new GenericMessage(id, attachment);
        }
        
    }
}
