using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteWebSocket.Routing
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageTypeAttribute :Attribute
    {
        public string[] Scopes { get; protected set; }
        public string Name { get; protected set; }

        public MessageTypeAttribute(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path must be not null", nameof(path));
            string[] tmp = path.ToLower().Replace('_', '-').Split(':');

            Scopes = tmp.ToList().GetRange(0, tmp.Length - 1).ToArray();
            Name = tmp.Last();
        }

        public MessageTypeAttribute(string name, params string[] scopes)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name must be not null", nameof(name));
            if (scopes.Length < 1)
                throw new ArgumentException("At least one scope must exist", nameof(scopes));
            if (scopes.Any(p=>string.IsNullOrEmpty(p)))
                throw new ArgumentException("evey scope must be not null", nameof(scopes));

            Name = name.ToLower().Replace('_', '-');
            Scopes = scopes.Select(p=>p.ToLower().Replace('_', '-')).ToArray();
        }
    }
}
