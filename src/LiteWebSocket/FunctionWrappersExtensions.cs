using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace LiteWebSocket
{
    internal static class FunctionWrappersExtensions
    {
        public static RequestDelegate AsHttpEndpoint(this RequestDelegate ext)
        {
            return async context =>
            {
                if (context.Request.Method.ToUpper() == "POST")
                {
                    await ext(context);
                }
                else
                    context.Response.StatusCode = 404;
            };
        }

        public static RequestDelegate AsWsEndpoint(this Func<HttpContext, WebSocket, Task> ext)
        {
            return async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await ext(context, webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            };
        }
    }
}
