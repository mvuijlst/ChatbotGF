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
        private static IConfigurationRoot LocationsStore;
        private static IConfigurationRoot MessagesStore;
        private static IConfigurationRoot QueryStore;

        public static List<SearchableLocation> Locations
        {
            get {
                if (locations == null)
                    initLocations();
                return locations;
            }
        }
        
        private static List<SearchableLocation> locations;
        private static List<SearchableLocation> toilets;
        private static readonly int TOILET_COUNT = 171;

        public static List<SearchableLocation> Toilets {
            get {
                if (toilets == null)
                    initToilets();
                return toilets;
            }
        }

        private static void initQueries()
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("queries.json");
            QueryStore = builder.Build();
        }

        private static void initToilets()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("locations.json");

                IConfiguration toiletStore = builder.Build();
                toilets = new List<SearchableLocation>();

                for (int i = 0; i < TOILET_COUNT; i++)
                {
                    //Console.WriteLine("Toilet toegevoegd");
                    toilets.Add(new SearchableLocation { Lon = double.Parse(toiletStore[$"toilets:{i}:{0}"]), Lat = double.Parse(toiletStore[$"toilets:{i}:{1}"]) });
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static SearchableLocation GetClosestsToilet(double Lon, double Lat)
        {
            return GetClosestLocation(Toilets, new Coordinates { lon = Lon, lat = Lat });
        }

        public static String GetQuery(string name)
        {
            if(QueryStore == null)
            {
                initQueries();
            }
            return QueryStore[name];
        }

        public static DateTime Now
        {
            get { return DateTime.Now.AddDays(9).AddHours(6); }
        }

        private static void initMessages()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
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

        public static List<SearchableLocation> GetClosestLocation(Coordinates coors, int count)
        {
            List<SearchableLocation> closests = new List<SearchableLocation>();
            List<SearchableLocation> locations = new List<SearchableLocation>(Locations); //shallow clone
            for(int i = 0; i < count; i++)
            {
                SearchableLocation close = GetClosestLocation(locations, coors);
                locations.Remove(close);
                closests.Add(close);
            }
            return closests;
        }

        public static SearchableLocation GetClosestLocation(List<SearchableLocation> locations,Coordinates coors)
        {
            SearchableLocation closests = locations[0];
            double dx = Locations[0].Lon - coors.lon;
            double dy = Locations[0].Lat - coors.lat;
            double shortestDistance = Math.Sqrt(dx * dx + dy * dy);
            for(int i = 1; i < locations.Count; i++)
            {
                dx = locations[i].Lon - coors.lon;
                dy = locations[i].Lat - coors.lat;
                double distance = Math.Sqrt(dx * dx + dy * dy);
                if(distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closests = locations[i];
                }
            }
            return closests;
        }
    }
}
