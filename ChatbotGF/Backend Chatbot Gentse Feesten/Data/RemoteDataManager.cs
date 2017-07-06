using Chatbot_GF.Client;
using Chatbot_GF.Controllers;
using Chatbot_GF.MessageBuilder.Factories;
using Chatbot_GF.MessageBuilder.Model;
using Chatbot_GF.MessengerManager;
using Chatbot_GF.Model;
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
        private static string BASE_QUERY = "PREFIX schema: <http://schema.org/> SELECT * WHERE { ?sub a schema:Event . ?sub schema:url ?url. ?sub schema:name ?name. ?sub schema:startDate ?startdate. ?sub schema:endDate ?enddate. ?sub schema:description ?description. ?sub schema:location ?location. ?sub schema:isAccessibleForFree ?isFree. ?sub schema:organizer ?organizer. OPTIONAL { ?sub schema:image/schema:url ?image. } ";
        private static string BASE_IMG = "https://stad.gent/cultuur-sport-vrije-tijd/nieuws-evenementen/gentse-feestengangers-vormen-basis-van-gentse-feestencampagne-2017";
        private SparqlRemoteEndpoint endpoint;
        public RemoteDataManager()
        {
            endpoint = new SparqlRemoteEndpoint(new Uri("https://stad.gent/sparql"), "http://stad.gent/gentse-feesten/");


        }

       
        public void GetEventsHereNow(User user)
        {
            string formattedTime = user.date.ToString("yyyy-MM-ddTHH:mm:sszzz");
            string locationfilter = "str(?location) = \""+ user.location + "\"";
            string startdatefilter = "?startdate < \"" + formattedTime + "\" ^^ xsd:dateTime";
            string enddatefilter = "?enddate > \"" + formattedTime + "\" ^^ xsd:dateTime";

            string query = BASE_QUERY + " FILTER(" + locationfilter + " && " + startdatefilter + " && " + enddatefilter + ") }";
            System.Console.WriteLine(query);
            endpoint.QueryWithResultSet(query, new SparqlResultsCallback(callback), user);
        }

        
        public void callback(SparqlResultSet results, Object u)
        {
            System.Console.WriteLine("Query Callback");
            User user = (User)u;
            if (results.Count > 0 && u != null)
            {
                System.Console.WriteLine("Found Results");

                    


                    foreach (SparqlResult res in results)
                    {

                        Event e = ResultParser.GetEvent(res);



                        GenericMessage toSend = new GenericMessage(user.id, e.name.nl);
                        IMessengerApi api = RestClientBuilder.GetMessengerApi();
                        System.Console.WriteLine("stap 6");
                        String result = api.SendMessageToUser(toSend).Result;
                        List<Event> events = new List<Event>();
                        events.Add(e);
                        CarouselFactory.makeCarousel(user.id, events);
                        System.Console.WriteLine("stap 7");
                        System.Console.WriteLine(result);
                        System.Console.WriteLine(CarouselFactory.makeCarousel(user.id, events));

                    }
                }            
            else
            {
                ReplyManager rm = new ReplyManager();
                rm.SendNoEventFound(user.id);
            }

            System.Console.WriteLine("End of query method");
        }

        

    }
}
