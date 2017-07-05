using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestEase;

namespace Chatbot_GF.Client
{
    public class RestClientBuilder
    {

        public static IMessengerApi GetMessengerApi()
        {
            return RestClient.For<IMessengerApi>("https://graph.facebook.com/v2.6/me/messages?access_token=EAADbmmTTQZBkBAGCYtymjKzMGGTr817rNVgsqNMAFxxVZCkrvKN5dkJfj88rhy3onuVwCAziCWPB1sBl3Jf5C6FujRZC1g6lRaRk1yW0M5EQvSQiKLFtkbNAYSqFpRZAsuBDqUXYpQz2K5PwZCopyzC5skFa1e7LOUhEZAdelk2QZDZD");
        }
    }
}
