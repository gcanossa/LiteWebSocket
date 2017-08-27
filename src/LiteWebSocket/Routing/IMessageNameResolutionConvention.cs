using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket.Routing
{
    public interface IMessageNameResolutionConvention
    {
        RouteData GetRouteData(Message message);
    }
}
