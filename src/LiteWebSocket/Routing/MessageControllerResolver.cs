using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteWebSocket.Routing;
using LiteWebSocket.Exception;
using System.Reflection;

namespace LiteWebSocket.Routing
{
    public class MessageControllerResolver
    {
        protected Dictionary<MethodInfo, Type> RegistredHandlers { get; set; }
        protected IMessageNameResolutionConvention _nameConvention;
        protected IServiceProvider _serviceProvider;

        public MessageControllerResolver(IMessageNameResolutionConvention nameConvention, IServiceProvider serviceProvider)
        {
            RegistredHandlers = new Dictionary<MethodInfo, Type>();

            _nameConvention = nameConvention;
            _serviceProvider = serviceProvider;
        }
        

        protected Func<Message, IEnumerable<Message>> GetAction(Message message)
        {
            if (!RegistredHandlers.Values.Any(p => p == message.GetType()))
                throw new NotSupportedMessageTypeException($"type: {message.GetType().FullName}");

            List<MethodInfo> mts = RegistredHandlers.Where(p => p.Value == message.GetType()).Select(p=>p.Key).ToList();

            return (Message msg) =>
            {
                return null;
                //List<>
                //(IEnumerable<Message>)action.Invoke(result, new object[] { msg });
            };
        }
    }
}
