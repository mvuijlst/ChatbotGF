using Chatbot_GF.Data;
using Chatbot_GF.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessengerManager
{
    public enum SearchState { START, PROGRESS, READYTO}
   

    public class Manager
    {
        public static Dictionary<String, String> locations_WithURL;
        public Dictionary<long, User> activeUsers;
        private RemoteDataManager dataDAO;

        public Manager()
        {
            dataDAO = new RemoteDataManager();
            activeUsers = new Dictionary<long, User>();
            locations_WithURL = new Dictionary<String, String>();
            //hier nog locations initialiseren!
        }
        /// <summary>
        /// Method to be called when user pushes on "get Started" or after
        /// --> makes a new user in the list + saves his id and timestamp + gives a welcom message etc
        /// </summary>
        /// <param name="id"></param>
        public void startUser(long id)
        {
            activeUsers.Add(id, new User(id));
            saveUsers(id, DateTime.Now);
        }

        /// <summary>
        /// Saves all the users + timestamps for case of emergency + knowing how many users
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stamp"></param>
        private void saveUsers(long id, DateTime stamp)
        {
            string path = Path.GetTempFileName();
            string line = "" + id + "\t" + stamp;
            // This text is a1lways added, making the file longer over time
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine("This");
            }

        }

        /// <summary>
        /// Method for each event call from messenger
        /// </summary>
        /// <param name="id"></param>
        public void changeUserState(long id, string payload)
        {
            User currentUser; ;   //contains the user object linked to the messengerperson who sends an event

            if (!activeUsers.ContainsKey(id))
            {
                currentUser = new User(id);
                activeUsers.Add(id, currentUser);
            }
            else{
                currentUser = activeUsers[id];
            }

            //payload indicates which category data in messengers has been given
            int pos = payload.IndexOf("-");
            String category = payload.Substring(0,pos);
            String value = payload.Substring(pos + 1);

            if (category.Equals("DEVELOPER_DEFINED_LOCATION"))
            { //location button choosed
                setLocationChoice(currentUser,value);
            } else if (category.Equals("DEVELOPER_DEFINED_SEARCH")){
                searchResults(currentUser);
            } else if (category.Equals("DEVELOPER_DEFINED_TIME"))
            {
                setTimeChoice(currentUser,value);
            }
            
            //hardcoded example
          //  dataDAO.GetEventsHereNow(currentUser,"https://gentsefeesten.stad.gent/api/v1/location/0d5f41fa-c8bc-47e3-aac2-c88fcdb29352", DateTime.Now.AddDays(10).AddHours(14));


        }

        private void setTimeChoice(User user, string value)
        {
            //hier tijd invoeren, voorlopig nog basisuur
            user.dates.Add(DateTime.Now.AddDays(10).AddHours(12));
        }

        private void searchResults(User user)
        {
            //hier effectieve zoekmethode uitvoeren
            
            //voorlopig enkel 1e locatie toelaten
            dataDAO.GetEventsHereNow(user, user.locations[0], DateTime.Now.AddDays(10).AddHours(12));
           
        }

        private void setLocationChoice(User user,String value)
        {
            // user has clicked on location button: three possibilities
            if (value.Equals("ALL"))
            {
                //list van locations toevoegen!
                //voorlopig hardcodering vlasmarkt als locatie
                user.locations.Add(locations_WithURL["VLASMARKT"]);
            }
            else if (value.Equals("MY_LOCATION"))
            {
                //eigen locatie bepalen en toevoegen
                //voorlopig hardcodering vlasmarkt als locatie
                user.locations.Add(locations_WithURL["VLASMARKT"]);
            }
            else
            {
                //specific location: use hashmap to get link for request
                user.locations.Add(locations_WithURL[value]);
            }
        }
        





    }
}
