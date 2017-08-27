using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteWebSocket.Routing
{
    public class MessageControllerResolver
    {
        protected List<Type> SupportedMessageTypes { get; set; }

        public MessageControllerResolver()
        {
            SupportedMessageTypes = new List<Type>();
        }

        //protected Func<IEnumerable<Message>> GetAction(Message msg)
        //{
        //    Type type = SupportedMessageTypes.FirstOrDefault(p => p == msg.GetType());
        //    if(type != null)
        //    {

        //    }
        //}
    }
}
