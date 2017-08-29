using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LiteWebSocket.Routing.Impl
{
    public class DefaultMessageNameResolutionConvention : IMessageNameResolutionConvention
    {
        public RouteData GetRouteData(Message message)
        {
            return GetRouteData(message.GetType());
        }
        
        public RouteData GetRouteData<T>() where T : Message
        {
            return GetRouteData(typeof(T));
        }

        public RouteData GetRouteData(Type type)
        {
            if (!typeof(Message).IsAssignableFrom(type))
                throw new ArgumentException($"type must inherit from {typeof(Message).FullName} type", nameof(type));
            if(!type.IsClass)
                throw new ArgumentException($"type must be a class", nameof(type));
            if (type.IsAbstract)
                throw new ArgumentException($"type must not be abstract", nameof(type));

            RouteData result = new RouteData();

            while (TryAddScopesAndName(ref result, type) && type.IsNested)
                type=type.DeclaringType;

            return result;
        }
        
        protected bool TryAddScopesAndName(ref RouteData data, Type type)
        {
            if (type == null)
                return false;

            MessageTypeAttribute mt_attr = type.GetCustomAttributes(typeof(MessageTypeAttribute), true).FirstOrDefault() as MessageTypeAttribute;
            if (mt_attr != null)
            {
                if(!string.IsNullOrEmpty(data.Name))
                        throw new InvalidOperationException("Message nesting hierarchy cannot have multiple MessageTypeAttribute");

                data.Scopes = mt_attr.Scopes;
                data.Name = mt_attr.Name;

                return false;
            }
            else
            {
                MessageTypePrefixAttribute mp_attr = type.GetCustomAttributes(typeof(MessageTypePrefixAttribute), true).FirstOrDefault() as MessageTypePrefixAttribute;
                if(mp_attr != null)
                {
                    if (string.IsNullOrEmpty(data.Name))
                        throw new ArgumentException("The MessageTypePrefixAttribute cannot be used on a terminal Message type");

                    data.Scopes = mp_attr.Scopes.Concat(data.Scopes ?? new string[0]).ToArray();

                    return false;
                }
                else
                {
                    if (string.IsNullOrEmpty(data.Name))
                        data.Name = Regex.Replace(Regex.Replace(type.Name, "[\\-_]?Message$", ""), "[\\-_]?MessageScope$", "").Replace('_', '-').ToLower();
                    else
                        data.Scopes = new string[] { Regex.Replace(Regex.Replace(type.Name, "[\\-_]?Message$", ""), "[\\-_]?MessageScope$", "").Replace('_', '-').ToLower() }.Concat(data.Scopes??new string[0]).ToArray();

                    return true;
                }
            }
        }
    }
}
