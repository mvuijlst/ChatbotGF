using Chatbot_GF.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessengerManager
{
    public class FreeTextHandler
    {
        private static IConfiguration ReplyStore;
        private static ReplyManager RMmanager;

        private static void InitReplies()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("replies.json");
            ReplyStore = builder.Build();

            RMmanager = new ReplyManager();
        }


        public static void CheckText(long id,string text)
        {
            Console.Write("INput?: " + text);
            string res;
            if (!string.IsNullOrEmpty(DataConstants.GetLocationBySearchTag(text)?.Id))
            {
                RemoteDataManager.GetInstance().GetEventsHereNow(id, DataConstants.GetLocationBySearchTag(text).Id, DataConstants.Now, "NL");
            }
            else
            {

                if (ReplyStore == null)
                {
                    InitReplies();
                }

                res = GetResponse(text);
                if (res != null)
                {
                    RMmanager.SendTextMessage(id, res);
                }

                //No way to know which language is prefered, default to dutch
                /*RMmanager.SendTextMessage(id, DataConstants.GetMessage("Donot_understand", "NL"));
                RMmanager.SendLocationQuery(id, 0, "NL");*/
                //Console.WriteLine("Zoeken naar event: " + text);
                RemoteDataManager.GetInstance().GetEventByName(text, id, "NL");
            }
        }
        
        private static string RemoveNonAlphanumerics(string text)
        {
            char[] arr = text.Where(c => (char.IsLetterOrDigit(c) ||
                             char.IsWhiteSpace(c) ||
                             c == '-')).ToArray();

            return new string(arr);
        }

        public static string GetResponse(string text)
        {
            
            try
            {
                text = RemoveNonAlphanumerics(text); 

                List<string> words = text.ToLower().Split(' ').ToList();
                List<string> KeywordsFound = new List<string>();
                KeywordsFound.Add("keywords");
                int count = 0;
                while (count < words.Count)
                {                    
                    string query = string.Join(":", KeywordsFound);
                    //Console.WriteLine("Searching " + query + ":" + words[count]);
                    //Console.WriteLine(ReplyStore["keywords:feestje:waar:response:nl"]);
                    if (ReplyStore.GetSection(query + ":" + words[count]).GetValue<string>("response") != null)
                    {
                       // Console.WriteLine(query + ":" + words[count] + "Keyword found!");
                        KeywordsFound.Add(words[count]);
                        words.RemoveAt(count);
                        count = 0; //restart
                    }
                    else if (ReplyStore[query + ":haschildren"] != null && ReplyStore[query + ":haschildren"] == "false")
                    {
                       // Console.WriteLine("No children, strop recursion");
                        break; //stop recursion, object has no child keywords
                    }
                    else
                    {
                        count++;
                    }
                }
                //Console.WriteLine(string.Join(":", KeywordsFound) + ":response");
                return ReplyStore[string.Join(":", KeywordsFound) + ":response"];
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static string GetPayload(string text)
        {
            if (ReplyStore == null)
            {
                InitReplies();
            }

            try
            {
                text = RemoveNonAlphanumerics(text);

                List<string> words = text.ToLower().Split(' ').ToList();
                List<string> KeywordsFound = new List<string>();
                KeywordsFound.Add("payloads");
                int count = 0;
                while (count < words.Count)
                {
                    string query = string.Join(":", KeywordsFound);
                   // Console.WriteLine("Searching " + query + ":" + words[count]);
                    //Console.WriteLine(ReplyStore["keywords:feestje:waar:response:nl"]);
                    if (ReplyStore.GetSection(query + ":" + words[count]).GetValue<string>("payload") != null)
                    {
                        //Console.WriteLine(query + ":" + words[count] + "Keyword found!");
                        KeywordsFound.Add(words[count]);
                        words.RemoveAt(count);
                        count = 0; //restart
                    }
                    else if (ReplyStore[query + ":haschildren"] != null && ReplyStore[query + ":haschildren"] == "false")
                    {
                       // Console.WriteLine("No children, stop recursion");
                        break; //stop recursion, object has no child keywords
                    }
                    else
                    {
                        count++;
                    }
                }
                //Console.WriteLine(string.Join(":", KeywordsFound) + ":payload");
                return ReplyStore[string.Join(":", KeywordsFound) + ":payload"];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static bool IsEqual(string first, string second, int tollerance)
        {
            return string.Compare(first, second) < tollerance;
        }

    }
}
