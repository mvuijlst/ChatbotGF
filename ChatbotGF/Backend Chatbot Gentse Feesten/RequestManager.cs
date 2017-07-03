using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using Chatbot_GF.Model;
using System.Runtime.Serialization.Json;

namespace Chatbot_GF
{

    /// <summary>
    /// Generic rest client, T schould be a valid model class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequestManager<T>
    {

        public RequestManager()
        {
            
                        
        }

        /// <summary>
        /// Fetches data at url and returns it as a defined model class
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public T getResultAsObject(String url)
        {
           return ProcessRepositories(url).Result;
        }

        private static async Task<T> ProcessRepositories(String url)
        {
           
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                var serializer = new DataContractJsonSerializer(typeof(T));
                var streamTask = client.GetStreamAsync(url);
                var events = serializer.ReadObject(await streamTask);
                return (T)events;
            
        }

    }


}
