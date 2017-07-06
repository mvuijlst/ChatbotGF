using Chatbot_GF.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Chatbot_GF.BotData.MessengerData;

namespace Chatbot_GF.Data
{
    public class DataConstants
    {
        private static IConfigurationRoot configuration;
        //TODO: add to config files
        private readonly static string[] locationsStrings = { "BAUDELOHOF", "BEVERHOUTPLEINPLACEMUSETTE", "SINTJACOBS", "CENTRUM", "STADSHAL", "EMILE BRAUNPLEIN", "LUISTERPLEIN", "GROENTENMARKT", "KORENLEI-GRASLEI", "KORENMARKT", "SINTBAAFSPLEIN", "STVEERLEPLEIN", "VLASMARKT", "VRIJDAGMARKT", "WILLEM DE BEERSTEEG" };
        private readonly static string[] prettyName = { "Baudelohof", "Beverhoutplein", "Sint-Jacobs", "Centrum", "Stadshal", "Emile Braunplein", "Luisterplein", "Groentemarkt", "Graslei", "Korenmarkt", "St.Baafsplein", "St.Veerleplein", "Vlasmarkt", "Vrijdagmarkt", "W. De Beersteeg" };
        private readonly static double[] lats = { 51.058362, 51.057191, 51.056523, 51.047455, 51.053945, 51.053605, 51.051073, 51.055942, 51.054710, 51.054633, 51.053530, 51.056690, 51.056110, 51.057149, 51.059811 };
        private readonly static double[] lons = { 3.730361, 3.727982, 3.727681, 3.730360, 3.724448, 3.723850, 3.728229, 3.722008, 3.720760, 3.721943, 3.725285, 3.721284, 3.728240, 3.726169, 3.727214 };
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
            try
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("locations.json");

                configuration = builder.Build();

                locations = new List<SearchableLocation>();
                for (int i = 0; i < locationsStrings.Length; i++)
                {
                    locations.Add(new SearchableLocation
                    {
                        Name = configuration[$"locations:{i}:Name"],
                        PrettyName = configuration[$"locations:{i}:PrettyName"],
                        Id = configuration[$"locations:{i}:Id"],
                        Lat = double.Parse(configuration[$"locations:{i}:Lat"]),
                        Lon = double.Parse(configuration[$"locations:{i}:lon"])
                    });
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            //Console.WriteLine(JsonConvert.SerializeObject(locations));
        }

        public static SearchableLocation GetLocation(string name){
            if (locations == null)
                initLocations();

            foreach(SearchableLocation loc in locations)
            { 
            
                if (loc.Id == name || loc.Name.Contains(name) || loc.PrettyName.Contains(name))
                {
                    return loc;
                }
            }

            return null;
        }
        public static SearchableLocation GetClosestLocation(Coordinates coors)
        {
            SearchableLocation closests = Locations[0];
            double dx = Locations[0].Lon - coors.lon;
            double dy = Locations[0].Lat - coors.lat;
            double shortestDistance = Math.Sqrt(dx * dx + dy * dy);
            for(int i = 1; i < Locations.Count; i++)
            {
                dx = Locations[i].Lon - coors.lon;
                dy = Locations[i].Lat - coors.lat;
                double distance = Math.Sqrt(dx * dx + dy * dy);
                if(distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closests = Locations[i];
                }
            }
            return closests;
        }

    

    }
}
