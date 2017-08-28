using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket.Routing
{
    public interface IMessageNameResolutionConvention
    {
        RouteData GetRouteData(Message message);
        RouteData GetRouteData(Type type);
        RouteData GetRouteData<T>() where T : Message;
    }
}
