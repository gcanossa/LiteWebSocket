using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket.Exception
{
    public class ControllerNotFoundException : InvalidOperationException
    {
        public ControllerNotFoundException()
        {
        }

        public ControllerNotFoundException(string message)
            : base(message)
        {

        }
        public ControllerNotFoundException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
