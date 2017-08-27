using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using LiteWebSocket.Models;

namespace LiteWebSocket.Routing.Impl
{
    public class DefaultMessageNameResolutionConvention : IMessageNameResolutionConvention
    {
        //search for attributes then search for underscores and nested types underscores
        public RouteData GetRouteData(Message message)
        {
            Type msg_type = message.GetType();

            MessageTypeAttribute mt_attr = msg_type.GetCustomAttributes(typeof(MessageTypeAttribute), true).FirstOrDefault() as MessageTypeAttribute;

            if(mt_attr!=null)
            {
                return new RouteData()
                {
                    MessageType = string.Join(":", mt_attr.Scopes, mt_attr.Name),
                    MessageScopes = mt_attr.Scopes,
                    MessageName = mt_attr.Name,
                    Controller = "",
                    Action = ""
                };
            }
            else
            {
                List<string> scopes = new List<string>();
                Type tmp = msg_type;
                while(tmp.IsNested)
                {
                    scopes.Add(Regex.Replace(Regex.Replace(tmp.Name, "Message$",""),"MessageScope$",""));
                    tmp = tmp.DeclaringType;
                }

                scopes = scopes.Reverse<string>().ToList();

                return new RouteData()
                {
                    MessageType = string.Join(":",scopes),
                    MessageScopes = scopes.GetRange(0,scopes.Count-1).ToArray(),
                    MessageName = scopes.Last(),
                    Controller = "",
                    Action = ""
                };
            }
        }
    }
}
