using System;
using System.Collections.Generic;
using System.Text;
using LiteWebSocket.Models;

namespace LiteWebSocket
{
    public class SimpleOperationResult : IOperationResult
    {
        protected List<Message> _results;

        public SimpleOperationResult(Message message)
        {
            _results = new List<Message>
            {
                message
            };
        }
        public SimpleOperationResult(IEnumerable<Message> messages)
        {
            _results = new List<Message>(messages);
        }

        public IEnumerable<Message> GetMessages()
        {
            return _results;
        }
    }
}
