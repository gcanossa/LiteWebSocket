using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteWebSocket.Routing;
using LiteWebSocket.Exceptions;
using System.Reflection;
using System.Threading.Tasks;

namespace LiteWebSocket.Routing
{
    public class MessageControllerResolver
    {
        protected Dictionary<MethodInfo, Type> _registredHandlers;
        protected Dictionary<string, Type> _supportedTypes;

        protected IMessageNameResolutionConvention _messageConvention;
        protected IMessageControllerResolutionConvention _controllerConvention;
        protected IMessageSerializer _serializer;
        protected IServiceProvider _serviceProvider;

        public MessageControllerResolver(
            IMessageSerializer serializer,
            IMessageControllerResolutionConvention controllerConvention, 
            IMessageNameResolutionConvention messageConvention, 
            IServiceProvider serviceProvider)
        {
            _registredHandlers = new Dictionary<MethodInfo, Type>();
            _supportedTypes = new Dictionary<string, Type>();

            _serializer = serializer;
            _messageConvention = messageConvention;
            _controllerConvention = controllerConvention;
            _serviceProvider = serviceProvider;
        }
        
        protected virtual IEnumerable<Message> AcceptMessage(Message message)
        {
            if (!_registredHandlers.Values.Any(p => p == message.GetType()))
                throw new NotSupportedMessageTypeException($"type: {message.GetType().FullName}");

            List<MethodInfo> mts = _registredHandlers.Where(p => p.Value == message.GetType()).Select(p=>p.Key).ToList();

            Dictionary<MethodInfo, MessageController> ctrls = mts.ToDictionary(p=>p, p => _serviceProvider.GetService(p.DeclaringType) as MessageController);

            OperationContext ctx = GetContext();
            foreach (KeyValuePair<MethodInfo, MessageController> item in ctrls)
            {
                //TODO: wrapping
                item.Value.Context = ctx;
                ctx.AddResult(item.Key.Invoke(item.Value, new object[] { message }));
            }

            return ctx.Results;
        }

        public string AcceptSerializedBulk(string messages)
        {
            IEnumerable<Message> msgs = _serializer.Deserialize(messages, _supportedTypes);
            if (msgs == null)
                return null;

            List<Task<IEnumerable<Message>>> _results = new List<Task<IEnumerable<Message>>>();
            foreach (Message item in msgs)
            {
                //TODO: add wrapping
                _results.Add(Task.Run(() => AcceptMessage(item)));
            }

            return _serializer.Serialize(_results.Select(p=>p.ConfigureAwait(false).GetAwaiter().GetResult()).SelectMany(p=>p), _supportedTypes);
        }

        protected OperationContext GetContext()
        {
            return (OperationContext)_serviceProvider.GetService(typeof(OperationContext));
        }
        
        public void AddSupportedMessage<T>() where T : Message
        {
            RouteData data = _messageConvention.GetRouteData<T>();
            if (!_supportedTypes.ContainsKey(data.MessageType))
                _supportedTypes.Add(data.MessageType, typeof(T));
        }

        public void RemoveSupportedMessage<T>() where T : Message
        {
            RouteData data = _messageConvention.GetRouteData<T>();
            _supportedTypes.Remove(data.MessageType);
        }

        public void ClearSupportedMessage()
        {
            _supportedTypes.Clear();
        }

        public void RegisterController<T>() where T : MessageController
        {
            foreach (KeyValuePair<MethodInfo, Type> item in _controllerConvention.GetControllerActions<T>())
            {
                if (!_registredHandlers.ContainsKey(item.Key))
                {
                    _registredHandlers.Add(item.Key, item.Value);
                }
            }
        }

        public void UnregisterController<T>() where T : MessageController
        {
            foreach (KeyValuePair<MethodInfo, Type> item in _controllerConvention.GetControllerActions<T>())
            {
                _registredHandlers.Remove(item.Key);
            }
        }

        public void ClearRegisteredControllers()
        {
            _registredHandlers.Clear();
        }
    }
}
