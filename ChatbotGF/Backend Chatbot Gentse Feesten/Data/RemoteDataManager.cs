using Chatbot_GF.Client;
using Chatbot_GF.Controllers;
using Chatbot_GF.MessageBuilder.Factories;
using Chatbot_GF.MessageBuilder.Model;
using Chatbot_GF.MessengerManager;
using Chatbot_GF.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace Chatbot_GF.Data
{
    public class RemoteDataManager
    {
        private string hmess;
        public string Language_choice { get; set; }
        private static RemoteDataManager instance;
        private static string BASE_QUERY = "PREFIX schema: <http://schema.org/> SELECT * WHERE { ?sub a schema:Event . OPTIONAL {?sub schema:url ?url. } OPTIONAL {?sub schema:name ?name.} OPTIONAL {?sub schema:startDate ?startdate. } OPTIONAL {?sub schema:endDate ?enddate. } OPTIONAL {?sub schema:description ?description. } OPTIONAL {?sub schema:location ?location. } OPTIONAL {?sub schema:isAccessibleForFree ?isFree.} OPTIONAL {?sub schema:organizer ?organizer. } OPTIONAL { ?sub schema:image/schema:url ?image. } ";
        //private static string BASE_IMG = "https://stad.gent/cultuur-sport-vrije-tijd/nieuws-evenementen/gentse-feestengangers-vormen-basis-van-gentse-feestencampagne-2017";
        private SparqlRemoteEndpoint endpoint;
        ReplyManager rm;
        public RemoteDataManager()
        {
            endpoint = new SparqlRemoteEndpoint(new Uri("https://stad.gent/sparql"), "http://stad.gent/gentse-feesten/");
            rm = new ReplyManager();
            Language_choice = "GENTS"; //default Gentse chatbot
        }

        public static RemoteDataManager GetInstance()
        {
            if (instance == null)
            {
                instance = new RemoteDataManager();
            }
            return instance;
        }

        public void GetEventsHereNow(long id,string location,DateTime now,string language)
        {
            string formattedTime = now.ToString("yyyy-MM-ddTHH:mm:sszzz");
            string locationfilter = "str(?location) = \"" + location + "\"";
            string startdatefilter = "?startdate < \"" + formattedTime + "\" ^^ xsd:dateTime";
            string enddatefilter = "?enddate > \"" + formattedTime + "\" ^^ xsd:dateTime";
            string query = DataConstants.GetQuery("base") + string.Format(DataConstants.GetQuery("EventsNowHere"), locationfilter, startdatefilter, enddatefilter);
            //System.Console.WriteLine(query);
            endpoint.QueryWithResultSet(query, new SparqlResultsCallback(callback), new CallbackData { Id = id, Language = language });
        }

        public void GetEventsAtTime(long id, string date,string language)
        {
            string startdatefilter = "?startdate < \"" + date + "\" ^^ xsd:dateTime";
            string enddatefilter = "?enddate > \"" + date + "\" ^^ xsd:dateTime";
            List<SearchableLocation> locations = DataConstants.Locations;
            string locationFilters = "str(?location) = \"" + locations[0].Id + "\"";
            for (int i = 1; i < locations.Count; i++)
            {
                locationFilters += " || str(?location) = \"" + locations[i].Id + "\"";
            }
            string query = DataConstants.GetQuery("base") + string.Format(DataConstants.GetQuery("EventsNowHere"), locationFilters, startdatefilter, enddatefilter);
           // System.Console.WriteLine(query);
            endpoint.QueryWithResultSet(query, new SparqlResultsCallback(callback), new CallbackData {Id = id, Language = language });
        }

        public void GetEventsNow(long id, string lang, DateTime time)
        {
            string formattedTime = time.ToString("yyyy-MM-ddTHH:mm:sszzz");
            string startdatefilter = "?startdate < \"" + formattedTime + "\" ^^ xsd:dateTime";
            string enddatefilter = "?enddate > \"" + formattedTime + "\" ^^ xsd:dateTime";
            //filter over defined locations only
            List<SearchableLocation> locations = DataConstants.Locations;
            string locationFilters = "str(?location) = \"" + locations[0].Id + "\"";
            for(int i = 1; i<locations.Count; i++)
            {
                locationFilters += " || str(?location) = \"" + locations[i].Id + "\"";
            }
            string query = DataConstants.GetQuery("base") + string.Format(DataConstants.GetQuery("EventsNowHere"),locationFilters,startdatefilter,enddatefilter);
            //System.Console.WriteLine(query);
            endpoint.QueryWithResultSet(query, new SparqlResultsCallback(callback), new CallbackData {Id = id, Language = lang });
        }
        public  void GetNextEvents(string locationurl,string date, int count, long id,string lang)
        {
            string query = DataConstants.GetQuery("base") + string.Format(DataConstants.GetQuery("NextEventsOnLocation"), locationurl, date, count);
            //System.Console.WriteLine(query);
            endpoint.QueryWithResultSet(query, new SparqlResultsCallback(callback), new CallbackData { Id = id, Language = lang });
        }
        public void GetEventByName(string locationName, long id, string lang)
        {
            try
            {
                string formattedTime = DataConstants.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
                string query = DataConstants.GetQuery("base") + string.Format(DataConstants.GetQuery("SearchByName"), locationName.ToLower(), formattedTime);
                System.Console.WriteLine(query);
                endpoint.QueryWithResultSet(query, new SparqlResultsCallback(callback), new CallbackData { Id = id, Language = lang });
            }catch(Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }


        public void callback(SparqlResultSet results, Object u)
        {
            try
            {
                List<Event> events = new List<Event>();
                IMessengerApi api = RestClientBuilder.GetMessengerApi();
                System.Console.WriteLine("Query Callback");
                
                if (results.Count > 0 && u is CallbackData)
                {
                    CallbackData user = (CallbackData)u;
                    System.Console.WriteLine("Found Results");
                    foreach (SparqlResult res in results)
                    {
                        try
                        {
                            Event e = ResultParser.GetEvent(res);
                            events.Add(e);
                            //System.Console.WriteLine("Datum: " + e.startDate);
                            //System.Console.WriteLine("Dend: " + e.endDate);
                        } catch (Exception ex)
                        {
                            System.Console.WriteLine(ex);
                        }
                    }
                    rm.SendTextMessage(user.Id, DataConstants.GetMessage("Found", user.Language));
                   // System.Console.WriteLine(JsonConvert.SerializeObject(CarouselFactory.makeCarousel(user.Id, events,user.Language)));
                    String result = api.SendMessageToUser(CarouselFactory.makeCarousel(user.Id, events,user.Language)).Result;
                }
                else if(u is CallbackData)
                {
                    CallbackData user = (CallbackData)u;
                    rm.SendNoEventFound(user.Id, user.Language);
                    rm.SendConfirmation(user.Id, user.Language);
                }
                else if (u is VDS.RDF.AsyncError)
                {
                    VDS.RDF.AsyncError error = (VDS.RDF.AsyncError)u;
                    CallbackData user = (CallbackData)error.State;
                    hmess = DataConstants.GetMessage("Error", user.Language);
                    rm.SendTextMessage(user.Id, hmess);
                }
                System.Console.WriteLine("End of query method");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        public class CallbackData
        {
            public string Language { get; set; }
            public long Id { get; set; }
        }
    }
}
