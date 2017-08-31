using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
        public static IServiceCollection AddLiteWebSocket(this IServiceCollection services)
        {
            services.AddTransient<Protocol.Controller>();

            return services
                .AddTransient<OperationContext, OperationContext>()
                .AddSingleton<Routing.IMessageNameResolutionConvention, Routing.Impl.DefaultMessageNameResolutionConvention>()
                .AddSingleton<Routing.IMessageControllerResolutionConvention, Routing.Impl.DefaultMessageControllerResolutionConvention>()
                .AddTransient<IMessageSerializer, JsonMessageSerializer>()
                .AddSingleton<Routing.MessageControllerResolver>();
        }

        public static IApplicationBuilder UseLiteWebSocket(this IApplicationBuilder ext, LiteWebSocketOptions options)
        {
            Routing.MessageControllerResolver resolver = ext.ApplicationServices.GetService<Routing.MessageControllerResolver>();

            resolver.AddSupportedMessage<Protocol.Messages.Sync_RequestMessage>();
            resolver.AddSupportedMessage<Protocol.Messages.Sync_ResponseMessage>();
            resolver.RegisterController<Protocol.Controller>();

            return ext
                .UseWebSockets(new WebSocketOptions() {})//TODO: configure
                .Map(options.BasePath.Add(new PathString(LiteWebSocketDefaults.Endpoints.SyncEndpoint)), LiteWebSocketHandlers.SyncHandler(resolver))
                .Map(options.BasePath.Add(new PathString(LiteWebSocketDefaults.Endpoints.MonitorEndpoint)), LiteWebSocketHandlers.MonitorHandler(resolver))
                .Map(options.BasePath.Add(new PathString(LiteWebSocketDefaults.Endpoints.SocketEndpoint)), LiteWebSocketHandlers.SocketHandler(resolver))
                .Map(options.BasePath.Add(new PathString(LiteWebSocketDefaults.Endpoints.SocketWsEndpoint)), LiteWebSocketHandlers.SocketWsHandler(resolver));
        }
    }
}
