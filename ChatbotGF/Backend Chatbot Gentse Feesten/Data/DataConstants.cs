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
        public const int numberLocations = 16;
        public const int numberMessages = 16;
        private static IConfigurationRoot LocationsStore;
        private static IConfigurationRoot MessagesStore;


        public static List<SearchableLocation> Locations
        {
            get {
                if (locations == null)
                    initLocations();
                return locations;
            }
        }



        private static List<SearchableLocation> locations;
        

        private static void initMessages()
        {
            try
            {
                    var builder = new ConfigurationBuilder().
                        SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("messages.json");

                MessagesStore = builder.Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void initLocations()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("locations.json");

                LocationsStore = builder.Build();
                

                locations = new List<SearchableLocation>();
                for (int i = 0; i < numberLocations; i++)
                {
                    locations.Add(new SearchableLocation
                    {
                        Name = LocationsStore[$"locations:{i}:Name"],
                        PrettyName = LocationsStore[$"locations:{i}:PrettyName"],
                        Id = LocationsStore[$"locations:{i}:Id"],
                        Lat = double.Parse(LocationsStore[$"locations:{i}:Lat"]),
                        Lon = double.Parse(LocationsStore[$"locations:{i}:lon"])
                    });
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            //Console.WriteLine(JsonConvert.SerializeObject(locations));
        }

        public static string GetMessage(string name, string locale)
        {
            if(MessagesStore == null)
            {
                initMessages();
            }
            return MessagesStore[$"messages:{name}:{locale}"];
            
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
