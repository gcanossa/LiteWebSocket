using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LiteWebSocket.Routing.Impl
{
    public class DefaultMessageControllerResolutionConvention : IMessageControllerResolutionConvention
    {
        public Dictionary<MethodInfo, Type> GetControllerActions(MessageController controller)
        {
            return GetControllerActions(controller.GetType());
        }        
        public Dictionary<MethodInfo, Type> GetControllerActions<T>() where T : MessageController
        {
            return GetControllerActions(typeof(T));
        }
        public Dictionary<MethodInfo, Type> GetControllerActions(Type type)
        {
            if (!typeof(MessageController).IsAssignableFrom(type))
                throw new ArgumentException($"type must inherit from {typeof(MessageController).FullName} type", nameof(type));
            if (!type.IsClass)
                throw new ArgumentException($"type must be a class", nameof(type));
            if (type.IsAbstract)
                throw new ArgumentException($"type must not be abstract", nameof(type));
            
            return type.GetMethods()
                .Where(m => 
                    m.IsPublic && 
                    m.GetParameters().Count()==1 && 
                    m.GetParameters().Any(p => typeof(Message).IsAssignableFrom(p.ParameterType) &&
                    (
                        typeof(IOperationResult).IsAssignableFrom(m.ReturnType) || 
                        (typeof(Task).IsAssignableFrom(m.ReturnType) && m.ReflectedType.IsGenericType && typeof(IOperationResult).IsAssignableFrom(m.ReturnType.GetGenericArguments().First())) ||
                        m.ReturnType == typeof(void) ||
                        typeof(Task).IsAssignableFrom(m.ReturnType))
                    ))
                .ToDictionary(p => p, m => m.GetParameters().First(p => typeof(Message).IsAssignableFrom(p.ParameterType)).ParameterType);
        }
    }
}
