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
        public static T WaitTaskResults<T>(Task<T> task)
        {
            return task.ConfigureAwait(false).GetAwaiter().GetResult();
        }

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

        public static async Task<string> ReadBodyAsStringAsync(this HttpRequest ext)
        {
            if (!ext.ContentLength.HasValue)
                return null;
            else
            {
                byte[] data = new byte[ext.ContentLength.Value];
                await ext.Body.ReadAsync(data, 0, data.Length);

                return Encoding.UTF8.GetString(data);
            }
        }

        public static async Task WriteBodyAsStringAsync(this HttpResponse ext, string content, string contentType)
        {
            ext.ContentType = contentType;

            byte[] data = Encoding.UTF8.GetBytes(content);
            await ext.Body.WriteAsync(data, 0, data.Length);
        }
    }
}
