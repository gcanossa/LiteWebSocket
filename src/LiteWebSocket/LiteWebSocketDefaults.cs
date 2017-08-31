using LiteWebSocket.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket
{
    public static class LiteWebSocketDefaults
    {
        public static class Endpoints
        {
            public const string SyncEndpoint = "/sync";
            public const string MonitorEndpoint = "/monitor";
            public const string SocketEndpoint = "/socket";
            public const string SocketWsEndpoint = "/socket-ws";
        }

        public static IList<IFilter> Filters { get; private set; } = new List<IFilter>();
    }
}
