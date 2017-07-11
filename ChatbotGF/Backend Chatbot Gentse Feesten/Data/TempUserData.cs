using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.Data
{
    public class TempUserData
    {
        private Dictionary<long, string> UserLanguage;

        private Dictionary<DateTime, long> LastConnected;
        private Dictionary<long, DateTime> InverseLastConnected;

        private static TempUserData instance;
        private static readonly int MAX_USERS = 1;
        private static readonly int KEEP_ALIVE_MINUTES = 2;
        public TempUserData()
        {
            UserLanguage = new Dictionary<long,string>();
            LastConnected = new Dictionary<DateTime, long>();
            InverseLastConnected = new Dictionary<long, DateTime>();
                       
        }
    
        /// <summary>
        /// Removes all users that did not perform any action in x minutes 
        /// </summary>
        /// <param name="minutes"></param>
        public void CleanMaps(int minutes)
        {
            DateTime threshhold = DateTime.Now.AddMinutes(minutes * 2);
            foreach(DateTime time in LastConnected.Keys)
            {

                if(DateTime.Compare(time,threshhold) < 0) //date is before threshhold
                {
                    Remove(LastConnected[time]);
                }
            }

        }


        public static TempUserData Instance {
        get {
                if (instance == null)
                    instance = new TempUserData();
                return instance;

            }
        }

        public string GetLanguage(long id)
        {
            if (UserLanguage.ContainsKey(id))
            {
                string lang = UserLanguage[id];
                Remove(id);
                return lang;
            }
            else
            {
                return null;
            }
        }

        public void Remove(long id)
        {
            if (UserLanguage.ContainsKey(id))//Data could already be deleted
            {
                LastConnected.Remove(InverseLastConnected[id]);
                InverseLastConnected.Remove(id);
                UserLanguage.Remove(id);
            }
        }

        public void Add(long id, string lang)
        {
            Remove(id);
            DateTime now = DateTime.Now;
            UserLanguage.Add(id, lang);
            LastConnected.Add(now, id);
            InverseLastConnected.Add(id, now);

            if(UserLanguage.Count > MAX_USERS)
                CleanMaps(KEEP_ALIVE_MINUTES); //remove users that did not connect in more than 10 minutes
        }



    }
}
