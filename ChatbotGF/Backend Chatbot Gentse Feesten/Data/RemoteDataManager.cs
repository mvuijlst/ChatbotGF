using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Query;

namespace Chatbot_GF.Data
{
    public class RemoteDataManager
    {
        public RemoteDataManager()
        {
            TripleStore store = new TripleStore();
            store.AddFromUri(new Uri("http://stad.gent/gentse-feesten/"));
            Object results = store.ExecuteQuery("SELECT * WHERE {?s ?p ?o}");
            if (results is SparqlResultSet)
            {
                //Print out the Results
                SparqlResultSet rset = (SparqlResultSet)results;
                foreach (SparqlResult result in rset)
                {
                    System.Console.WriteLine(result.ToString());
                }
            }
        }


    }
}
