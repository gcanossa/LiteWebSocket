using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket
{
    public interface IMessageSerializer
    {
        string Serialize(IEnumerable<Message> message, Dictionary<string, Type> supportedTypes);
        IEnumerable<Message> Deserialize(string messages, Dictionary<string, Type> supportedTypes);
    }
}
