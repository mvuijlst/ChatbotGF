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
        public Dictionary<long, User> activeUsers;
        private RemoteDataManager dataDAO;

        public Manager()
        {
            dataDAO = new RemoteDataManager();
            activeUsers = new Dictionary<long, User>();
        }
        /// <summary>
        /// Method to be called when user pushes on "get Started" or after
        /// --> makes a new user in the list + saves his id and timestamp + gives a welcom message
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
            Console.WriteLine("Stap 1.5");
            User currentUser; ;   //contains the user object linked to the messengerperson who sends an event

            if (!activeUsers.ContainsKey(id))
            {
                currentUser = new User(id);
                Console.WriteLine("Stap 2");
                activeUsers.Add(id, currentUser);
            }
            else{
                currentUser = activeUsers[id];
            }
      

            dataDAO.GetEventsHereNow(currentUser,"https://gentsefeesten.stad.gent/api/v1/location/0d5f41fa-c8bc-47e3-aac2-c88fcdb29352", DateTime.Now.AddDays(10).AddHours(14));


        }
        

        





    }
}
