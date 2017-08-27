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
    internal class LiteWebSocketHandlers
    {
        public static void SyncHandler(IApplicationBuilder app)
        {
            app.Run(FunctionWrappersExtensions.AsHttpEndpoint(async context =>
            {
                await context.Response.WriteAsync("Sync");
            }));
        }
        public static void MonitorHandler(IApplicationBuilder app)
        {
            app.Run(FunctionWrappersExtensions.AsHttpEndpoint(async context =>
            {
                await context.Response.WriteAsync("Monitor");
            }));
        }
        public static void SocketHandler(IApplicationBuilder app)
        {
            app.Run(FunctionWrappersExtensions.AsHttpEndpoint(async context =>
            {
                await context.Response.WriteAsync("Socket");
            }));
        }
        public static void SocketWsHandler(IApplicationBuilder app)
        {
            app.Run(FunctionWrappersExtensions.AsWsEndpoint(async (context, webSocket) =>
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "result.CloseStatusDescription", CancellationToken.None);
            }));
        }
    }
}
