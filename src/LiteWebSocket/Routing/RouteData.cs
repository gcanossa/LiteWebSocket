using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket.Routing
{
    public class RouteData
    {
        public string[] Scopes { get; set; }
        public string Name { get; set; }

        public string MessageType => string.Join(":", string.Join(":", Scopes??new string[0]), Name);
        public string ScopesPath => string.Join(':', Scopes);
    }
}
