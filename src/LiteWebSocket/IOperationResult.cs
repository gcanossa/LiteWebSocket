using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket
{
    public interface IOperationResult
    {
        IEnumerable<Message> GetMessages();
    }
}
