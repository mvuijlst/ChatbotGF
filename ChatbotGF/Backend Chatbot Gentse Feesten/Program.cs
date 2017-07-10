using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Chatbot_GF.Data;
using Chatbot_GF.SQLite;
using Chatbot_GF.Model;

namespace Chatbot_GF
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://*:50000")
                .UseApplicationInsights()
                .Build();

            host.Run();

            
        }
    }
}
