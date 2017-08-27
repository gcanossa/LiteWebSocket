using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket.Models
{
    public abstract class Message
    {
        public DateTimeOffset Timestamp { get; set; }
        public uint SequenceNumber { get; set; }
    }
}
