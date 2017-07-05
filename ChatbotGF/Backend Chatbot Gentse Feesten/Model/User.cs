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
        public SearchState state { get; set; }
        public IList<DateTime> dates { get; set; }
        public IList<string> locations { get; set; }    //needs to be URL's of the locations
        public IList<string> keywords { get; set; }

        public User(long messId)
        {
            state = SearchState.START;
            id = messId;
            dates = new List<DateTime>();
            dates.Add(DateTime.Now);
            locations = new List<string>();
            keywords = new List<string>();
        }


    }
}
