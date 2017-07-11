using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.Model
{   
    public class SearchableLocation
    {
        public string Name { get; set; }
        public string PrettyName { get; set; }
        public string Id { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
        public List<string> Search { get; set; }

    }
}
