using LiteWebSocket.Models;
using LiteWebSocket.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket.Protocol
{
    [MessageTypePrefix("protocol")]
    public class Messages
    {
        public class SyncMessage : Message
        {
            public string Valore { get; set; }
        }
        public class Sync_ResponseMessage : Message
        {
            public string Valore { get; set; }
        }
    }
}
