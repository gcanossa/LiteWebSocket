using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiteWebSocket.Models
{
    public abstract class Message
    {
        [JsonProperty("sid", Required = Required.Always)]
        public string SessionId { get; set; }
        [JsonProperty("ts", Required = Required.Always)]
        public DateTimeOffset Timestamp { get; set; }
        [JsonProperty("seqn", Required = Required.Always)]
        public uint SequenceNumber { get; set; }
    }
}
