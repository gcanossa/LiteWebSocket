using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket.Routing
{
    public class RouteData
    {
        public string[] Scopes { get; set; }
        public string Name { get; set; }

        public string MessageType => string.Join(":", ScopesPath??"", Name??"");
        public string ScopesPath => Scopes!=null?string.Join(':', Scopes):null;
    }
}
