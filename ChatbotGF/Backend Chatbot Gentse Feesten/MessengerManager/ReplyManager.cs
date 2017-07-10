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
        public string Language_choice { get; set; }

        public ReplyManager()
        {
            api = RestClientBuilder.GetMessengerApi();
            Language_choice = "GENTS";  //default Gentse chatbot
        }

        public void SendWelcomeMessage(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Search_location", Language_choice), "SEND_LOCATION_CHOICE"));
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Search_Date", Language_choice), "SEND_DATE_CHOICE"));
            reply.Add(new QuickReply("text", "English", "SET_LANGUAGE_EN"));
            reply.Add(new QuickReply("text", "Nederlands", "SET_LANGUAGE_NL"));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Welcome", Language_choice), reply);
            Console.WriteLine("Welcome message: " + api.SendMessageToUser(message).Result);
        }

        public void SendLocationsChoice(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Choose_list", Language_choice), "GET_EVENT_HERE_NOW-0"));
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Choose_card", Language_choice), "GET_USER_LOCATION"));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Choose_options", Language_choice), reply);
            Console.WriteLine("Location choice: " + api.SendMessageToUser(message).Result);
        }
        public void SendLocationResult(long id, SearchableLocation loc)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Yes", Language_choice), $"DEVELOPER_DEFINED_LOCATION-{loc.Name}"));
            reply.Add(new QuickReply("text", DataConstants.GetMessage("No", Language_choice), "SEND_LOCATION_CHOICE"));
            var text = DataConstants.GetMessage("Nearest_location", Language_choice) + $"{loc.PrettyName}" + DataConstants.GetMessage("This_location", Language_choice);
            //$"Je bent het dichtst bij {loc.PrettyName}. Wil je op deze locatie zoeken?"
            GenericMessage message = new GenericMessage(id, text, reply);
            Console.WriteLine("Approval of using this location: " + api.SendMessageToUser(message).Result);
        }

        public void SendTextMessage(long id, string text)
        {
            GenericMessage message = new GenericMessage(id, text);
            Console.WriteLine("Basic message: " + api.SendMessageToUser(message).Result);
        }

        public void SendGetLocationButton(long id)
        {
            GenericMessage message = LocationFactory.makeLocationButton(id);
            Console.WriteLine("Location map button: " + api.SendMessageToUser(message).Result);
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
                    reply.Add(new QuickReply("text", DataConstants.GetMessage("More", Language_choice), "GET_EVENT_HERE_NOW-" + 1));
                }
                else
                {
                    reply.Add(new QuickReply("text", DataConstants.GetMessage("Previous", Language_choice), "GET_EVENT_HERE_NOW-" + (page - 1)));
                }
                GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Which_location", Language_choice), reply);

                Console.WriteLine("All squares: " + api.SendMessageToUser(message).Result);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void SendConfirmation(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Yes", Language_choice), "SEND_LOCATION_CHOICE"));
            reply.Add(new QuickReply("text", DataConstants.GetMessage("No", Language_choice), "DEVELOPER_DEFINED_SEARCHFALSE"));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Other_location", Language_choice), reply);
            Console.WriteLine("Confirmation: " + api.SendMessageToUser(message).Result);
        }

        public void SendInfoForEnding(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", "Begin opnieuw", "GET_STARTED_PAYLOAD"));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage( "Welcome", Language_choice), reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendNoEventFound(long id)
        {
            SendTextMessage(id, DataConstants.GetMessage("Not_found", Language_choice));
        }

        public void SendDayOption(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            string[] data = DataConstants.GetMessage("Day_Block", Language_choice).Split(',');
            foreach (var block in data)
            {
                reply.Add(new QuickReply("text", block, "DEVELOPER_DEFINED_DAY-" + block.Split(' ')[1]));
            }
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Previous_Block", Language_choice), "SEND_DATE_CHOICE"));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Choice_For_Date", Language_choice), reply);
            Console.WriteLine("Pick a date: " + api.SendMessageToUser(message).Result);
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
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Previous_Block", Language_choice), "DEVELOPER_DEFINED_DATE_SPECIFIC"));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Time_Periods", Language_choice), reply);
            Console.WriteLine("Pick a time block: " + api.SendMessageToUser(message).Result);
        }

        public void SendDateChoice(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Now", Language_choice), "DEVELOPER_DEFINED_LOCATION-ALL"));
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Choice_For_Date", Language_choice), "DEVELOPER_DEFINED_DATE_SPECIFIC"));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Date_Choice", Language_choice), reply);
            Console.WriteLine("All now or date specific: " + api.SendMessageToUser(message).Result);
        }

        public void SendHoursChoice(long id, string looping, string value)
        {
            if (!value.Contains("T"))
            {
                value = $"2017-07-{value}T";
            }
            string[] loop = looping.Split('~');
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            for (int i = Convert.ToInt32(loop[0]); i <= Convert.ToInt32(loop[1]); i++)
            {
                string view = (i.ToString().Length == 1) ? "0" + i + ":00" : i + ":00";
                reply.Add(new QuickReply("text", view, "DEVELOPER_DEFINED_HOURS_COMP-" + view + "|" + value));
            }
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Previous_Block", Language_choice), "DEVELOPER_DEFINED_DAY-" + value));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Time_Periods", Language_choice), reply);
            Console.WriteLine("Choice hour: " + api.SendMessageToUser(message).Result);
        }

        public void SendImage(long id, string url)
        {
            Console.WriteLine("Smile " + api.SendMessageToUser(new GenericMessage(id, new Attachment("image", new PayloadImage(url)))).Result);
        }
    }
}
