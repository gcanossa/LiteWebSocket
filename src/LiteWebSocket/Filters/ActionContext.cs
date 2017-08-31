using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LiteWebSocket.Filters
{
    public class ActionContext
    {
        public ActionContext(MessageController controller, MethodInfo action, Message message, OperationContext context)
        {
            Controller = controller;
            Action = action;
            HandledMessage = message;
            Context = context;
        }

        public MessageController Controller { get; protected set; }
        public MethodInfo Action { get; protected set; }
        public Message HandledMessage { get; protected set; }
        public OperationContext Context { get; protected set; }

        public List<Message> Results { get; protected set; } = new List<Message>();
        public Exception Exception { get; set; }
    }
}
