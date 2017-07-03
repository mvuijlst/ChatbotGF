using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Chatbot_Gentse_Feesten.Model
{
    public class Name
    {
        public string nl { get; set; }
    }

    public class Contributor
    {
        public string @type { get; set; }
        public Name name { get; set; }
    }

    public class Description
    {
        public string nl { get; set; }
    }

    public class Image
    {
        public string url { get; set; }
        public string thumbnailUrl { get; set; }
        public string caption { get; set; }
    }

public class Event
    {
        public Contributor contributor { get; set; }
        public Description description { get; set; }
        public string duration { get; set; }
        public DateTime endDate { get; set; }
        public Image image { get; set; }
        public IList<string> inLanguage { get; set; }
        public bool isAccessibleForFree { get; set; }
        public IList<string> isPartOf { get; set; }
        public bool isWheelchairUnfriendly { get; set; }
        public IList<string> keywords { get; set; }
        public string location { get; set; }
        public Name name { get; set; }
        public IList<object> offers { get; set; }
        public string organizer { get; set; }
        public object startDate { get; set; }
        public IList<string> subEvent { get; set; }
        public string superEvent { get; set; }
        public IList<string> theme { get; set; }
        public object typicalAgeRange { get; set; }
        public string url { get; set; }
        public IList<object> video { get; set; }
        public DateTime? changed { get; set; }
        public string @id { get; set; }
    }
}
