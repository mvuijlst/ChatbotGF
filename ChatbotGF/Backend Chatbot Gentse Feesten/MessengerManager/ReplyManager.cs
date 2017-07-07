using Chatbot_GF.Client;
using Chatbot_GF.Data;
using Chatbot_GF.MessageBuilder.Factories;
using Chatbot_GF.MessageBuilder.Model;
using Chatbot_GF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessengerManager
{
    public class ReplyManager
    {
        private IMessengerApi api;
        private string hmess;
        public string Language_choice { get; set; }

        public ReplyManager()
        {
            api = RestClientBuilder.GetMessengerApi();
            Language_choice = "GENTS";  //default Gentse chatbot
        }

        public void SendWelcomeMessage(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            hmess = DataConstants.GetMessage("Search_location", Language_choice);
            reply.Add(new QuickReply("text", hmess, "SEND_LOCATION_CHOICE"));

            /* hmess = DataConstants.GetMessage("Now", "GENTS");
            reply.Add(new QuickReply("text", hmess, "DEVELOPER_DEFINED_LOCATION-ALL")); */

            // test geval knoppen op gsm word dan schuiven dus minder goed
            hmess = DataConstants.GetMessage("Search_Date", Language_choice);
            reply.Add(new QuickReply("text", hmess, "SEND_DATE_CHOICE"));

            hmess = DataConstants.GetMessage("Welcome", Language_choice);
            GenericMessage message = new GenericMessage(id, hmess, reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendLocationsChoice(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            hmess = DataConstants.GetMessage("Choose_list", Language_choice);
            reply.Add(new QuickReply("text", hmess, "GET_EVENT_HERE_NOW-0"));
            hmess = DataConstants.GetMessage("Choose_card", Language_choice);
            reply.Add(new QuickReply("text", hmess, "GET_USER_LOCATION"));
            GenericMessage message = new GenericMessage(id, "Je kan ofwel een locatie kiezen, ofwel je eigen locatie gebruiken.", reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }
        public void SendLocationResult(long id, SearchableLocation loc)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            hmess =DataConstants.GetMessage("Yes", Language_choice);
            reply.Add(new QuickReply("text", hmess, $"DEVELOPER_DEFINED_LOCATION-{loc.Name}"));
            hmess = DataConstants.GetMessage("No", Language_choice);
            reply.Add(new QuickReply("text", hmess, "DEVELOPER_DEFINED_SEARCHFALSE"));
            GenericMessage message = new GenericMessage(id, $"Je bent het dichtst bij {loc.PrettyName}. Wil je op deze locatie zoeken?", reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendTextMessage(long id, string text)
        {
            GenericMessage message = new GenericMessage(id, text);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendGetLocationButton(long id)
        {
            GenericMessage message = LocationFactory.makeLocationButton(id);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendLocationQuery(long id, int page)
        {
            try
            {
                List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
                int lastindex = (page * 8 + 8) > DataConstants.Locations.Count ? DataConstants.Locations.Count : (page * 8 + 8);
                for (int i = page * 8; i < lastindex; i++)
                {
                    string l = DataConstants.Locations[i].PrettyName;
                    reply.Add(new QuickReply("text", l, "DEVELOPER_DEFINED_LOCATION-" + l));
                }
                //Max 10 quickreplies, we got more locations. When at first page, add extra button to show second page
                if(page == 0)
                {
                    hmess = DataConstants.GetMessage("More", Language_choice);
                    reply.Add(new QuickReply("text", hmess, "GET_EVENT_HERE_NOW-" + 1));
                }
                else
                {
                    hmess = DataConstants.GetMessage("Previous", Language_choice);
                    reply.Add(new QuickReply("text", hmess, "GET_EVENT_HERE_NOW-" + (page - 1)));
                }
                hmess = DataConstants.GetMessage("Which_location", Language_choice);
                GenericMessage message = new GenericMessage(id, hmess, reply);

                Console.WriteLine(api.SendMessageToUser(message).Result);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void SendConfirmation(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            hmess = DataConstants.GetMessage("Yes", Language_choice);
            reply.Add(new QuickReply("text", hmess, "SEND_LOCATION_CHOICE"));
            hmess = DataConstants.GetMessage("No", Language_choice);
            reply.Add(new QuickReply("text", hmess, "DEVELOPER_DEFINED_SEARCHFALSE"));
            hmess = DataConstants.GetMessage("Other_location", Language_choice);
            GenericMessage message = new GenericMessage(id, hmess, reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendInfoForEnding(long id)
        {
            hmess = DataConstants.GetMessage("Restart", Language_choice);
            SendTextMessage(id, hmess);
            // fotos voor waar de knop is
        }

        public void SendNoEventFound(long id)
        {
            hmess = DataConstants.GetMessage("Not_found", Language_choice);
            SendTextMessage(id, hmess);
        }

        public void SendDayOption(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            string[] data = DataConstants.GetMessage("Day_Block", Language_choice).Split(',');
            foreach (var block in data)
            {
                reply.Add(new QuickReply("text", block, "DEVELOPER_DEFINED_DAY-" + block.Split(' ')[1]));
            }
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Choice_For_Date", Language_choice), reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendTimePeriod(long id, string value)
        {
            // value is om bij te houden opwelke dag ze hebben gekozen
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            string[] data = DataConstants.GetMessage("Time_Block", Language_choice).Split('|');
            foreach (var block in data)
            {
                string[] blockinfo = block.Split(':');
                reply.Add(new QuickReply("text", blockinfo[0], "DEVELOPER_DEFINED_HOURS-" + blockinfo[1] + "|" + value));
            }
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Time_Periods", Language_choice), reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendDateChoice(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            hmess = DataConstants.GetMessage("Now", Language_choice);
            reply.Add(new QuickReply("text", hmess, "DEVELOPER_DEFINED_LOCATION-ALL"));

            hmess = DataConstants.GetMessage("Choice_For_Date", Language_choice);
            reply.Add(new QuickReply("text", hmess, "DEVELOPER_DEFINED_DATE_SPECIFIC"));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Date_Choice", Language_choice), reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendHoursChoice(long id, string looping, string value)
        {
            // value opnieuw informatie // date: 2017-07-16T19:30:00+02:00
            value = $"2017-07-{value}T";
            string[] loop = looping.Split('~');
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            for (int i = Convert.ToInt32(loop[0]); i <= Convert.ToInt32(loop[1]); i++)
            {
                string view = (i.ToString().Length == 1) ? "0" + i + ":00" : i + ":00";
                reply.Add(new QuickReply("text", view, "DEVELOPER_DEFINED_HOURS_COMP-" + view + "|" + value));
            }
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Previous_Block", Language_choice), "DEVELOPER_DEFINED_DAY-" + value));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Time_Periods", Language_choice), reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }
    }
}
