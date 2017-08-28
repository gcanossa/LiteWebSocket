using System;
using System.Collections.Generic;
using System.Text;
using LiteWebSocket.Models;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace LiteWebSocket
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        public IEnumerable<Message> Deserialize(string messages, Dictionary<string, Type> supportedTypes)
        {
            if (string.IsNullOrEmpty(messages))
                yield return null;
            else
            {
                foreach (JObject item in JsonConvert.DeserializeObject<object[]>(messages))
                {
                    if (!supportedTypes.ContainsKey(item["type"].Value<string>()))
                        yield return null;
                    else
                    {
                        yield return (Message)item.ToObject(supportedTypes[item["type"].Value<string>()]);
                    }
                }
            }
        }

        public string Serialize(IEnumerable<Message> messages, Dictionary<string, Type> supportedTypes)
        {
            if (messages == null)
                return null;
            else
            {
                List<object> tmp = new List<object>();
                foreach (Message item in messages)
                {
                    if(supportedTypes.ContainsValue(item.GetType()))
                    {
                        JObject obj = JObject.FromObject(item);
                        obj.Add("type", supportedTypes.First(p=>p.Value==item.GetType()).Key);
                        tmp.Add(obj.ToObject<object>());
                    }
                }

                return JsonConvert.SerializeObject(tmp);
            }
        }
    }
}
