using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket
{
    public abstract class MessageController
    {
        public OperationContext Context { get; internal set; }

        protected virtual IOperationResult Result<T>(T message) where T : Message
        {
            return new SimpleOperationResult(message);
        }
        protected virtual IOperationResult Result<T>(IEnumerable<T> messages) where T : Message
        {
            return new SimpleOperationResult(messages);
        }
    }
}
