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
        public string lang { get; set; }

        public ReplyManager()
        {
            api = RestClientBuilder.GetMessengerApi();
            lang = "GENTS";  //default Gentse chatbot
        }

        public void SendWelcomeMessage(long id,string lang)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Search_location", lang), "SEND_LOCATION_CHOICE°°" + lang));
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Search_Date", lang), "SEND_DATE_CHOICE°°" + lang));
            reply.Add(new QuickReply("text", "Language", "SEND_LANGUAGE_OPTIONS°°" + lang));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Welcome", lang), reply);
            Console.WriteLine("Welcome message: " + api.SendMessageToUser(message).Result);
        }

        public void ChangeLanguage(long id)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", "English", "SET_LANGUAGE°EN°" + lang));
            reply.Add(new QuickReply("text", "Nederlands", "SET_LANGUAGE°NL°" + lang));
            reply.Add(new QuickReply("text", "Gents", "SET_LANGUAGE°GENTS°" + lang));
            reply.Add(new QuickReply("text", "Deutsch", "SET_LANGUAGE°DE°" + lang));
            GenericMessage message = new GenericMessage(id, $"Kies u taal: ", reply);
            Console.WriteLine("Language message: " + api.SendMessageToUser(message).Result);
        }

        public void SendLocationsChoice(long id, string lang)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Choose_list", lang), "GET_EVENT_HERE_NOW°0°" + lang));
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Choose_card", lang), "GET_USER_LOCATION°°" + lang));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Choose_options", lang), reply);
            Console.WriteLine("Location choice: " + api.SendMessageToUser(message).Result);
        }
        public void SendLocationResult(long id, SearchableLocation loc, string lang)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Yes", lang), $"DEVELOPER_DEFINED_LOCATION°{loc.Name}°{lang}"));
            reply.Add(new QuickReply("text", DataConstants.GetMessage("No", lang), "SEND_LOCATION_CHOICE°°" + lang));
            var text = DataConstants.GetMessage("Nearest_location", lang) + $"{loc.PrettyName}" + DataConstants.GetMessage("This_location", lang);
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

        public void SendLocationQuery(long id, int page, string lang)
        {
            try
            {
                List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
                int lastindex = (page * 8 + 8) > DataConstants.Locations.Count ? DataConstants.Locations.Count : (page * 8 + 8);
                for (int i = page * 8; i < lastindex; i++)
                {
                    string l = DataConstants.Locations[i].PrettyName;
                    reply.Add(new QuickReply("text", l, "DEVELOPER_DEFINED_LOCATION°" + l + "°" + lang));
                }
                //Max 10 quickreplies, we got more locations. When at first page, add extra button to show second page
                if(page == 0)
                {
                    reply.Add(new QuickReply("text", DataConstants.GetMessage("More", lang), "GET_EVENT_HERE_NOW°" + 1 + "°" + lang));
                }
                else
                {
                    reply.Add(new QuickReply("text", DataConstants.GetMessage("Previous", lang), "GET_EVENT_HERE_NOW°" + (page - 1) + "°" + lang));
                }
                GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Which_location", lang), reply);

                Console.WriteLine("All squares: " + api.SendMessageToUser(message).Result);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void SendConfirmation(long id, string lang)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Yes", lang), "SEND_LOCATION_CHOICE°°" + lang));
            reply.Add(new QuickReply("text", DataConstants.GetMessage("No", lang), "DEVELOPER_DEFINED_SEARCHFALSE°°" + lang));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Other_location", lang), reply);
            Console.WriteLine("Confirmation: " + api.SendMessageToUser(message).Result);
        }

        public void SendInfoForEnding(long id, string lang)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", "Begin opnieuw", "GET_STARTED_PAYLOAD°°" + lang));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage( "Welcome", lang), reply);
            Console.WriteLine(api.SendMessageToUser(message).Result);
        }

        public void SendNoEventFound(long id, string lang)
        {
            SendTextMessage(id, DataConstants.GetMessage("Not_found", lang));
        }

        public void SendDayOption(long id, string lang)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            string[] data = DataConstants.GetMessage("Day_Block", lang).Split(',');
            foreach (var block in data)
            {
                reply.Add(new QuickReply("text", block, "DEVELOPER_DEFINED_DAY°" + block.Split(' ')[1]  + "°" + lang));
            }
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Previous_Block", lang), "SEND_DATE_CHOICE°°" + lang));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Choice_For_Date", lang), reply);
            Console.WriteLine("Pick a date: " + api.SendMessageToUser(message).Result);
        }

        public void SendTimePeriod(long id, string value, string lang)
        {
            // value is om bij te houden opwelke dag ze hebben gekozen
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            string[] data = DataConstants.GetMessage("Time_Block", lang).Split('|');
            foreach (var block in data)
            {
                string[] blockinfo = block.Split(':');
                reply.Add(new QuickReply("text", blockinfo[0], "DEVELOPER_DEFINED_HOURS°" + blockinfo[1] + "|" + value + "°" + lang));
            }
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Previous_Block", lang), "DEVELOPER_DEFINED_DATE_SPECIFIC°°" + lang));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Time_Periods", lang), reply);
            Console.WriteLine("Pick a time block: " + api.SendMessageToUser(message).Result);
        }

        public void SendDateChoice(long id, string lang)
        {
            List<SimpleQuickReply> reply = new List<SimpleQuickReply>();
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Now", lang), "DEVELOPER_DEFINED_LOCATION°ALL°" + lang));
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Choice_For_Date", lang), "DEVELOPER_DEFINED_DATE_SPECIFIC°°" + lang));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Date_Choice", lang), reply);
            Console.WriteLine("All now or date specific: " + api.SendMessageToUser(message).Result);
        }

        public void SendHoursChoice(long id, string looping, string value, string lang)
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
                reply.Add(new QuickReply("text", view, "DEVELOPER_DEFINED_HOURS_COMP°" + view + "|" + value + "°" + lang));
            }
            reply.Add(new QuickReply("text", DataConstants.GetMessage("Previous_Block", lang), "DEVELOPER_DEFINED_DAY°" + value + "°" + lang));
            GenericMessage message = new GenericMessage(id, DataConstants.GetMessage("Time_Periods", lang), reply);
            Console.WriteLine("Choice hour: " + api.SendMessageToUser(message).Result);
        }

        public void SendImage(long id, string url)
        {
            Console.WriteLine("Smile " + api.SendMessageToUser(new GenericMessage(id, new Attachment("image", new PayloadImage(url)))).Result);
        }
    }
}
