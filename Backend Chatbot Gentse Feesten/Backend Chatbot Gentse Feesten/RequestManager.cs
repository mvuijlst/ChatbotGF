using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using Backend_Chatbot_Gentse_Feesten.Model;
using System.Runtime.Serialization.Json;

namespace Backend_Chatbot_Gentse_Feesten
{
    public class RequestManager<T>
    {

        public RequestManager()
        {
            
                        
        }


        public T getResultAsObject(String url)
        {
            return ProcessRepositories(url).Result;
        }

        private static async Task<T> ProcessRepositories(String url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            var serializer = new DataContractJsonSerializer(typeof(List<Event>));
            var streamTask = client.GetStreamAsync(url);
            var events = serializer.ReadObject(await streamTask);
            return (T) events;
        }

    }


}
