using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteWebSocket.Routing;
using LiteWebSocket.Exceptions;
using System.Reflection;
using System.Threading.Tasks;
using LiteWebSocket.Filters;

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
        
        protected IEnumerable<IFilter> GetFilters(MethodInfo action)
        {
            List<IFilter> results = new List<IFilter>();

            results.AddRange(LiteWebSocketDefaults.Filters);

            results.AddRange(action.DeclaringType.GetCustomAttributes().Where(p => p is IFilter).Select(p => p as IFilter));

            results.AddRange(action.GetCustomAttributes().Where(p => p is IFilter).Select(p => p as IFilter));

            return results;
        }

        protected Action<ActionContext> WrapAction(MethodInfo method, object target, Message arg)
        {
            if (typeof(Task).IsAssignableFrom(method.ReturnType))
                return null;
            if (method.ReturnType == typeof(void))
                return ctx => { };
            else if (typeof(IOperationResult).IsAssignableFrom(method.ReturnType))
                return ctx => { ctx.Results.AddRange((method.Invoke(target, new object[] { arg }) as IOperationResult).GetMessages()); };
            else
                return null;//TODO: maybe an exception would be better
        }
        protected Func<ActionContext, Task> WrapActionAsync(MethodInfo method, object target, Message arg)
        {
            if (!typeof(Task).IsAssignableFrom(method.ReturnType))
                return null;//TODO: maybe an exception would be better
            else if (method.ReturnType.IsGenericType && typeof(IOperationResult).IsAssignableFrom(method.ReturnType.GetGenericArguments().First()))
                return async ctx =>
                {
                    ctx.Results.AddRange((await (method.Invoke(target, new object[] { arg }) as Task<IOperationResult>)).GetMessages());
                };
            else
                return async ctx =>
                {
                    await (method.Invoke(target, new object[] { arg }) as Task);
                };
        }

        protected Action<ActionContext> WrapFilter(Action<ActionContext> handler, IFilter filter)
        {
            return (ActionContext ctx) =>
            {
                filter.Accept(handler, ctx);
            };
        }
        protected Func<ActionContext, Task> WrapFilterAsync(Func<ActionContext, Task> handler, IFilter filter)
        {
            return async (ActionContext ctx) =>
            {
                await filter.AcceptAsync(handler, ctx);
            };
        }

        protected async Task ExecuteAction(MethodInfo method, MessageController target, Message message, OperationContext ctx)
        {
            IEnumerable<IFilter> filters = GetFilters(method);
            object func = null;
            if ((func = WrapAction(method, target, message)) != null)
            {
                Action<ActionContext> action = func as Action<ActionContext>;
                if (filters != null)
                {
                    foreach (IFilter item in filters)
                    {
                        action = WrapFilter(action, item);
                    }
                }

                ActionContext a_ctx = new ActionContext(target, method, message, ctx);
                action(a_ctx);

                ctx.AddResult(a_ctx.Results);
            }
            else if ((func = WrapActionAsync(method, target, message)) != null)
            {
                Func<ActionContext, Task> action = func as Func<ActionContext, Task>;
                if (filters != null)
                {
                    foreach (IFilter item in filters)
                    {
                        action = WrapFilterAsync(action, item);
                    }
                }

                ActionContext a_ctx = new ActionContext(target, method, message, ctx);
                await action(a_ctx);

                ctx.AddResult(a_ctx.Results);
            }
            else
                throw new Exception(); //TODO: decide which exception
        }

        protected async virtual Task<IEnumerable<Message>> AcceptMessage(Message message)
        {
            if (!_registredHandlers.Values.Any(p => p == message.GetType()))
                throw new NotSupportedMessageTypeException($"type: {message.GetType().FullName}");

            List<MethodInfo> mts = _registredHandlers.Where(p => p.Value == message.GetType()).Select(p=>p.Key).ToList();

            Dictionary<MethodInfo, MessageController> ctrls = mts.ToDictionary(p=>p, p => _serviceProvider.GetService(p.DeclaringType) as MessageController);

            OperationContext ctx = GetContext();
            foreach (KeyValuePair<MethodInfo, MessageController> item in ctrls)
            {
                await ExecuteAction(item.Key, item.Value, message, ctx);
            }

            return ctx.Results;
        }

        //TODO: verify if it is necessary
        public string AcceptSerializedBulk(string messages)
        {
            IEnumerable<Message> msgs = _serializer.Deserialize(messages, _supportedTypes);
            if (msgs == null)
                return null;

            List<Task<IEnumerable<Message>>> _results = new List<Task<IEnumerable<Message>>>();
            foreach (Message item in msgs)
            {
                _results.Add(AcceptMessage(item));
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
