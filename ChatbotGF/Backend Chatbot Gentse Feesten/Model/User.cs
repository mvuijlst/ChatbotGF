using Chatbot_GF.MessengerManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.Model
{
    public class User
    {
        public long id { get; set; }                    //Messenger id from user
        
        public DateTime date { get; set; }
        public string location { get; set; }    //needs to be URL's of the locations
        public IList<string> keywords { get; set; }

        public User(long messId)
        {
            id = messId;
            date = DateTime.Now.AddDays(10).AddHours(7);
            keywords = new List<string>();
        }


    }
}
