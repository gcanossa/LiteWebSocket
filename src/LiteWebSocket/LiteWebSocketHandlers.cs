using LiteWebSocket.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LiteWebSocket
{//TODO: see wrapping and error handling
    internal class LiteWebSocketHandlers
    {
        public static Action<IApplicationBuilder> SyncHandler(MessageControllerResolver resolver)
        {
            return (Action<IApplicationBuilder>)(app =>
            {
                app.Run(FunctionWrappersExtensions.AsHttpEndpoint(async context =>
                {
                    await context.Response.WriteBodyAsStringAsync(resolver.AcceptSerializedBulk(await context.Request.ReadBodyAsStringAsync()), "application/json");
                }));
            });
        }
        public static Action<IApplicationBuilder> MonitorHandler(MessageControllerResolver resolver)
        {
            return (Action<IApplicationBuilder>)(app =>
            {
                app.Run(FunctionWrappersExtensions.AsHttpEndpoint(async context =>
                {
                    //resolver.Accept()
                    await context.Response.WriteAsync("Monitor");
                }));
            });
        }
        public static Action<IApplicationBuilder> SocketHandler(MessageControllerResolver resolver)
        {
            return (Action<IApplicationBuilder>)(app =>
            {
                app.Run(FunctionWrappersExtensions.AsHttpEndpoint(async context =>
                {
                    //resolver.Accept()
                    await context.Response.WriteAsync("Socket");
                }));
            });
        }
        public static Action<IApplicationBuilder> SocketWsHandler(MessageControllerResolver resolver)
        {
            return (Action<IApplicationBuilder>)(app =>
            {
                app.Run(FunctionWrappersExtensions.AsWsEndpoint(async (context, webSocket) =>
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "result.CloseStatusDescription", CancellationToken.None);
                }));
            });
        }
    }
}
