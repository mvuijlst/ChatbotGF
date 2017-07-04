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
        private static string BASE_QUERY = "PREFIX schema: <http://schema.org/> SELECT * WHERE { ?sub a schema:Event . ?sub schema:name ?name. ?sub schema:startDate ?startdate. ?sub schema:endDate ?enddate. ?sub schema:description ?description. ?sub schema:location ?location. ?sub schema:isAccessibleForFree ?isFree. ?sub schema:organizer ?organizer. OPTIONAL { ?sub schema:image ?image. } ";
        private SparqlRemoteEndpoint endpoint;
        public RemoteDataManager()
        {
            endpoint = new SparqlRemoteEndpoint(new Uri("https://stad.gent/sparql"), "http://stad.gent/gentse-feesten/");


        }

        public void ReadFromOnlineStore()
        {
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("https://stad.gent/sparql"), "http://stad.gent/gentse-feesten/");
            System.Console.WriteLine("Starting query");
            endpoint.QueryWithResultSet("PREFIX schema: <http://schema.org/> SELECT * WHERE { ?sub a schema:Event . ?sub schema:name ?name. ?sub schema:location ?location. filter(str(?location) = \"https://gentsefeesten.stad.gent/api/v1/location/f2e7a735-7632-486c-b70d-7e7340bfd340\") }", new SparqlResultsCallback(callback),"test");            


        }

        public void GetEventsHereNow(String location, DateTime time) 
        {
            string formattedTime = time.ToString("yyyy-MM-ddTHH:mm:sszzz");
            string locationfilter = "str(?location) = \""+ location + "\"";
            string startdatefilter = "?startdate < \"" + formattedTime + "\" ^^ xsd:dateTime";
            string enddatefilter = "?enddate > \"" + formattedTime + "\" ^^ xsd:dateTime";

            string query = BASE_QUERY + " FILTER(" + locationfilter + " && " + startdatefilter + " && " + enddatefilter + ") }";
            System.Console.WriteLine(query);
            endpoint.QueryWithResultSet(query, new SparqlResultsCallback(callback), "test");
        }

        public void GetEventsByKeywords(List<String> keywords)
        {

            string filterString = "";
            if(keywords.Count != 0)
            {
                filterString += "contains(lcase(STR(?keywords)), \"" + keywords[0] + "\")";
                keywords.RemoveAt(0);
            }
            foreach(string keyword in keywords)
            {
                filterString += "&& contains(lcase(STR(?keywords)), \"" + keyword + "\")";
            }


            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("https://stad.gent/sparql"), "http://stad.gent/gentse-feesten/");
            
            String query = "PREFIX schema: <http://schema.org/> SELECT * WHERE { ?sub a schema:Event . ?sub schema:name ?name. ?sub schema:location ?location. ?sub schema:keywords ?keywords. filter(" + filterString + ") }";
            
            endpoint.QueryWithResultSet(query, new SparqlResultsCallback(callback), "test");


        }

        public void callback(SparqlResultSet results,Object state)
        {
            foreach (SparqlResult result in results)
            {
                System.Console.WriteLine(result.Variables.ToString());
                foreach( string s in result.Variables)
                {
                    System.Console.WriteLine(s);
                }
                Event e = new Event();
                e.name.nl = result["name"].ToString();
                e.startDate = result["startdate"].ToString();
                e.endDate = result["enddate"].ToString();
                e.description.nl = result["description"].ToString();
                e.organizer = result["organizer"].ToString();
                e.image = (Image) result["image"];

            }
        }


    }
}
