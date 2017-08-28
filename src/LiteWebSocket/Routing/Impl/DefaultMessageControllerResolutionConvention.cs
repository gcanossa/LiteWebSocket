using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LiteWebSocket.Routing.Impl
{
    public class DefaultMessageControllerResolutionConvention : IMessageControllerResolutionConvention
    {
        public Dictionary<MethodInfo, Type> GetControllerActions(SocketController controller)
        {
            return GetControllerActions(controller.GetType());
        }        
        public Dictionary<MethodInfo, Type> GetControllerActions<T>() where T : SocketController
        {
            return GetControllerActions(typeof(T));
        }
        public Dictionary<MethodInfo, Type> GetControllerActions(Type type)
        {
            if (!typeof(SocketController).IsAssignableFrom(type))
                throw new ArgumentException($"type must inherit from {typeof(SocketController).FullName} type", nameof(type));
            if (!type.IsClass)
                throw new ArgumentException($"type must be a class", nameof(type));
            if (type.IsAbstract)
                throw new ArgumentException($"type must not be abstract", nameof(type));

            return type.GetMethods()
                .Where(m => m.GetParameters().Any(p => typeof(Message).IsAssignableFrom(p.ParameterType)))
                .ToDictionary(p => p, m => m.GetParameters().First(p => typeof(Message).IsAssignableFrom(p.ParameterType)).ParameterType);
        }
    }
}
