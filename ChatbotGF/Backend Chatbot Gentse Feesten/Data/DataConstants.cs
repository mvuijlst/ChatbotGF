using Chatbot_GF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.Data
{
    public class DataConstants
    {
        //TODO: add to config files
        private readonly static string[] locationsStrings = { "BAUDELOHOF", "BEVERHOUTPLEINPLACEMUSETTE", "SINTJACOBS", "CENTRUM", "STADSHAL", "EMILE BRAUNPLEIN", "LUISTERPLEIN", "GROENTENMARKT", "KORENLEI-GRASLEI", "KORENMARKT", "SINTBAAFSPLEIN", "STVEERLEPLEIN", "VLASMARKT", "VRIJDAGMARKT", "WILLEM DE BEERSTEEG" };
        private readonly static string[] prettyName = { "Baudelohof", "Beverhoutplein", "Sint-Jacobs", "Centrum", "Stadshal", "Emile Braunplein", "Luisterplein", "Groentemarkt", "Graslei", "Korenmarkt", "St.Baafsplein", "St.Veerleplein", "Vlasmarkt", "Vrijdagmarkt", "W. De Beersteeg" };
        private readonly static string[] urls =
        {
            "https://gentsefeesten.stad.gent/api/v1/location/ffc36b40-f30a-4f4c-9512-ee1367c45fc3",
            "https://gentsefeesten.stad.gent/api/v1/location/a9b88475-05d5-4d52-8787-764f75827599",
            "https://gentsefeesten.stad.gent/api/v1/location/2917fe2d-8597-46ab-be01-a5e0a97447c2",
            "https://gentsefeesten.stad.gent/api/v1/location/f792034c-a8e5-4260-ab6b-2adeecf759a6",
            "https://gentsefeesten.stad.gent/api/v1/location/15864533-d5da-4425-897c-162d5e155558",
            "https://gentsefeesten.stad.gent/api/v1/location/c9218a9b-0340-40ed-b2f8-67c74271cb58",
            "https://gentsefeesten.stad.gent/api/v1/location/39a81c28-09ac-496d-acb6-f10aa9db0aea",
            "https://gentsefeesten.stad.gent/api/v1/location/41f39d3d-e906-4333-b590-f273a0a979e1",
            "https://gentsefeesten.stad.gent/api/v1/location/5a123130-716e-4923-a59d-3bbcb863890d",
            "https://gentsefeesten.stad.gent/api/v1/location/bfbb81fb-ede4-4bb9-b09a-ba40390c6596",
            "https://gentsefeesten.stad.gent/api/v1/location/eb4ea42e-e8e0-4945-b098-8b8c98bdb4d1",
            "https://gentsefeesten.stad.gent/api/v1/location/f614c847-2cab-432a-b0f0-6639f8a5ac57",
            "https://gentsefeesten.stad.gent/api/v1/location/0d5f41fa-c8bc-47e3-aac2-c88fcdb29352",
            "https://gentsefeesten.stad.gent/api/v1/location/29e0000a-5cf8-467f-98b8-a57a92ef0298",
            "https://gentsefeesten.stad.gent/api/v1/location/c2559115-bfcb-4cf3-9d4f-a5147f99e371"
        };

        public static List<SearchableLocation> Locations
        {
            get {
                if (locations == null)
                    initLocations();
                return locations;
            }
        }

        private static List<SearchableLocation> locations;


        private static void initLocations()
        {
            locations = new List<SearchableLocation>();
            for (int i = 0; i < locationsStrings.Length; i++)
            {
                locations.Add(new SearchableLocation
                {
                    Name = locationsStrings[i],
                    PrettyName = prettyName[i],
                    Id = urls[i]
                });
            }
        }

        public static SearchableLocation GetLocation(string name){
            if (locations == null)
                initLocations();

            foreach(SearchableLocation loc in locations)
            { 
            
                if (loc.Name.Equals(name) || loc.PrettyName.Equals(name))
                {
                    return loc;
                }
            }

            return null;
        }

    }
}
