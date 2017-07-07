using Chatbot_GF.Data;
using Chatbot_GF.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.MessengerManager
{
    public class UserManager
    {
        private static UserManager instance;
        private ReplyManager reply;
        public Dictionary<long, User> activeUsers;
        private RemoteDataManager dataDAO;
        

        public UserManager()
        {
            dataDAO = RemoteDataManager.GetInstance();
            activeUsers = new Dictionary<long, User>();
            reply = new ReplyManager();
        }

        public static UserManager GetInstance()
        {
            if(instance == null)
            {
                instance = new UserManager();
            }
            return instance;
        }


        /// <summary>
        /// Method to be called when user pushes on "get Started" or after
        /// --> makes a new user in the list + saves his id and timestamp + gives a welcom message etc
        /// </summary>
        /// <param name="id"></param>
        public void startUser(long id)
        {
            //activeUsers.Add(id, new User(id));
            //saveUsers(id, DateTime.Now);
            Console.WriteLine("Stap 50");
            reply.SendWelcomeMessage(id);
        }

        
        /// <summary>
        /// Saves all the userid's (app specific) + timestamps for case of emergency + knowing how many users
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stamp"></param>
        private void saveUsers(long id, DateTime stamp)
        {
            string path = Path.GetTempFileName();
            string line = "" + id + "\t" + stamp;
            // This text is always added, making the file longer over time
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(line);
            }

        }

        public void setUserTime(long id, string value)
        {
            User user = activeUsers[id];
            try
            {
                DateTime dt = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:sszzz", null);
                user.date = DateTime.Now.AddDays(10).AddHours(9);
            } catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void searchResults(long id)
        {
            //contains the user object linked to the messengerperson who sends an event
            User user = activeUsers[id];
            dataDAO.GetEventsHereNow(user);
            activeUsers.Remove(id);
           
        }

        public void setUserLocation(long id,String value)
        {
            try
            {
                Console.WriteLine("stap 98");
                //contains the user object linked to the messengerperson who sends an event
                User user = new User(id);
                Console.WriteLine("stap 98.5");
                // user has clicked on location button: three possibilities
                if(value.Equals("ALL")){
                    dataDAO.GetEventsNow(user);
                    Console.Write("Alles nu");
                }
                else
                {
                    Console.WriteLine("stap 99");
                    //specific location: use hashmap to get link for request
                    user.location = DataConstants.GetLocation(value).Id;
                    Console.WriteLine("stap 100");
                    dataDAO.GetEventsHereNow(user);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        
    }
}
