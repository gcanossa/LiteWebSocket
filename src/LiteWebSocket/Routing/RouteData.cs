using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket.Routing
{
    public class RouteData
    {
        public string MessageType { get; set; }
        public string[] MessageScopes { get; set; }
        public string MessageName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
