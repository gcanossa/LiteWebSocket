using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LiteWebSocket
{
    public static class LiteWebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder UseLiteWebSocket(this IApplicationBuilder ext, LiteWebSocketOptions options)
        {
            return ext
                .UseWebSockets(new WebSocketOptions() {})
                .Map(options.BasePath.Add(new PathString(LiteWebSocketDefaults.SyncEndpoint)), LiteWebSocketHandlers.SyncHandler)
                .Map(options.BasePath.Add(new PathString(LiteWebSocketDefaults.MonitorEndpoint)), LiteWebSocketHandlers.MonitorHandler)
                .Map(options.BasePath.Add(new PathString(LiteWebSocketDefaults.SocketEndpoint)), LiteWebSocketHandlers.SocketHandler)
                .Map(options.BasePath.Add(new PathString(LiteWebSocketDefaults.SocketWsEndpoint)), LiteWebSocketHandlers.SocketWsHandler);
        }
    }
}
