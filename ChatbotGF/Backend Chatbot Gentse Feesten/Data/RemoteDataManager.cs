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
        public RemoteDataManager()
        {
            
        }

        public void ReadFromOnlineStore()
        {
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("https://stad.gent/sparql"), "http://stad.gent/gentse-feesten/");
            System.Console.WriteLine("Starting query");
            endpoint.QueryWithResultSet("PREFIX schema: <http://schema.org/> SELECT * WHERE { ?sub a schema:Event . ?sub schema:name ?name. ?sub schema:location ?location. filter(str(?location) = \"https://gentsefeesten.stad.gent/api/v1/location/f2e7a735-7632-486c-b70d-7e7340bfd340\") }", new SparqlResultsCallback(callback),"test");            


        }

        public void callback(SparqlResultSet results,Object state)
        {
            foreach (SparqlResult result in results)
            {
                System.Console.WriteLine(result.ToString());
            }
        }
    }
}
