using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.Model
{
    public class Address
    {
        public string @type { get; set; }
        public string addressCountry { get; set; }
        public string addressLocality { get; set; }
        public object addressRegion { get; set; }
        public string postalCode { get; set; }
        public string streetAddress { get; set; }
    }

    public class Location
    {
        public Address address { get; set; }
        public String changed { get; set; }
        public object containedInPlace { get; set; }
        public IList<object> containsPlace { get; set; }
        public String created { get; set; }
        public string email { get; set; }
        public Boolean? isWheelchairUnfriendly { get; set; }
        public Name name { get; set; }
        public Boolean? outDoors { get; set; }
        public string telephone { get; set; }
        public string url { get; set; }
    }
}
