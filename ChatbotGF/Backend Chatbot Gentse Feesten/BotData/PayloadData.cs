using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot_GF.BotData
{
    public class PayloadData  : List<string>
    {
        public PayloadData(string input)
        {
            string[] list = input.Split('°');
            Payload = list[0];
            Value = list[1];
            Language = list[2];
        }

        public string Payload { get; set; }
        public string Value { get; set; }
        public string Language { get; set; }
    }
}
