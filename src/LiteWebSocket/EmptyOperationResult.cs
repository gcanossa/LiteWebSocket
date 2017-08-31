using System;
using System.Collections.Generic;
using System.Text;
using LiteWebSocket.Models;

namespace LiteWebSocket
{
    public class EmptyOperationResult : IOperationResult
    {
        private static Message[] Empty = new Message[0];
        
        public IEnumerable<Message> GetMessages()
        {
            return Empty;
        }
    }
}
