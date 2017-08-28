using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LiteWebSocket.Models;

namespace LiteWebSocket.Routing
{
    public static class MessageNameResolutionExtensions
    {
        public static RouteData GetRouteData(this IEnumerable<IMessageNameResolutionConvention> ext, Message message)
        {
            if (ext == null)
                return null;
            else
            {
                foreach (IMessageNameResolutionConvention item in ext)
                {
                    RouteData data = ext.GetRouteData(message);
                    if (data != null)
                        return data;
                }
                return null;
            }
        }

        public static RouteData ConcatRouteData(this string[] ext, string[] route)
        {
            return new RouteData()
            {
                Name = route.Last(),
                Scopes = ext.Union(route.ToList().GetRange(0, route.Length-1)).ToArray()
            };
        }
    }
}
