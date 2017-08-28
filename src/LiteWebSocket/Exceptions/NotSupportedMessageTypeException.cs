using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket.Exceptions
{
    public class NotSupportedMessageTypeException : NotSupportedException
    {
        public NotSupportedMessageTypeException()
        {
        }

        public NotSupportedMessageTypeException(string message)
            : base(message)
        {

        }
        public NotSupportedMessageTypeException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
