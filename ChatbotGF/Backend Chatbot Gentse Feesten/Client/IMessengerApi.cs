using Chatbot_GF.MessageBuilder.Model;
using RestEase;
using System;
using System.Threading.Tasks;

namespace Chatbot_GF.Client
{
    public interface IMessengerApi
    {
        [Post]
        Task<String> SendMessageToUser([Body] GenericMessage message);
    }
}
