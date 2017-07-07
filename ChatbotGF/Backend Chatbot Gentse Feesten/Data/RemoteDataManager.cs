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


        public void GetEventsHereNow(User user)
        {
            string formattedTime = user.date.ToString("yyyy-MM-ddTHH:mm:sszzz");
            string locationfilter = "str(?location) = \"" + user.location + "\"";
            string startdatefilter = "?startdate < \"" + formattedTime + "\" ^^ xsd:dateTime";
            string enddatefilter = "?enddate > \"" + formattedTime + "\" ^^ xsd:dateTime";

            string query = DataConstants.GetQuery("base") + string.Format(DataConstants.GetQuery("EventsNowHere"), locationfilter, startdatefilter, enddatefilter);
            System.Console.WriteLine(query);
            endpoint.QueryWithResultSet(query, new SparqlResultsCallback(callback), user);
        }

        public void GetEventsNow(User user)
        {
            string formattedTime = user.date.ToString("yyyy-MM-ddTHH:mm:sszzz");
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
            System.Console.WriteLine(query);
            endpoint.QueryWithResultSet(query, new SparqlResultsCallback(callback), user);
        }
        public  void GetNextEvents(string locationurl,string date, int count, long id)
        {
            string query = DataConstants.GetQuery("base") + string.Format(DataConstants.GetQuery("NextEventsOnLocation"), locationurl, date, count);
            System.Console.WriteLine(query);
            endpoint.QueryWithResultSet(query, new SparqlResultsCallback(callback), new User(id));
        }


        public void callback(SparqlResultSet results, Object u)
        {
            try
            {
                List<Event> events = new List<Event>();
                IMessengerApi api = RestClientBuilder.GetMessengerApi();
                System.Console.WriteLine("Query Callback");
                
                if (results.Count > 0 && u is User)
                {
                    User user = (User)u;
                    System.Console.WriteLine("Found Results");
                    foreach (SparqlResult res in results)
                    {
                        Event e = ResultParser.GetEvent(res);
                        events.Add(e);
                    }
                    hmess = DataConstants.GetMessage("Found", Language_choice);
                    rm.SendTextMessage(user.id, hmess);
                    System.Console.WriteLine(JsonConvert.SerializeObject(CarouselFactory.makeCarousel(user.id, events)));
                    String result = api.SendMessageToUser(CarouselFactory.makeCarousel(user.id, events)).Result;
                }
                else if(u is User)
                {
                    User user = (User)u;
                    
                    rm.SendNoEventFound(user.id);
                    rm.SendConfirmation(user.id);
                }
                else if (u is VDS.RDF.AsyncError)
                {
                    VDS.RDF.AsyncError error = (VDS.RDF.AsyncError)u;
                    User user = (User)error.State;
                    hmess = DataConstants.GetMessage("Error", Language_choice);
                    rm.SendTextMessage(user.id, hmess);
                }
                System.Console.WriteLine("End of query method");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }

        }
    }
}
