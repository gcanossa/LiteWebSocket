using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket.Routing
{
    public class MessageTypeAttribute :Attribute
    {
        public string[] Scopes { get; protected set; }
        public string Name { get; protected set; }

        public MessageTypeAttribute(string name, params string[] scopes)
        {
            Name = name;
            Scopes = scopes;
        }
    }
}
