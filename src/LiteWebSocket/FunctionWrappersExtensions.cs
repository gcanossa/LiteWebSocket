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
                    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                    await ext(context);
                    context.Response.Body.Close();
                }
                else if (context.Request.Method.ToUpper() == "OPTIONS" && context.Request.Headers.ContainsKey("Access-Control-Request-Method"))
                {
                    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");//TODO: make the origin an option value/list
                    context.Response.Headers.AppendList("Access-Control-Allow-Methods", new string[] { "POST", "OPTIONS" });
                    context.Response.Headers.Append("Access-Control-Allow-Headers", "*");
                    context.Response.Headers.Append("Access-Control-Max-Age", "86400");

                    context.Response.Body.Close();
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.Body.Close();
                }
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
