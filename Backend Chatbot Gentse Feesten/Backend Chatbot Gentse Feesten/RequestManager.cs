using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace Backend_Chatbot_Gentse_Feesten
{
    public class RequestManager
    {

        public RequestManager()
        {
            ProcessRepositories().Wait();
        }


        private static async Task ProcessRepositories()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();

            var stringTask = client.GetStringAsync("https://datatank.stad.gent/4/cultuursportvrijetijd/gentsefeestenlocaties.json");

            var msg = await stringTask;
            Debug.Write(msg);
        }

    }


}
