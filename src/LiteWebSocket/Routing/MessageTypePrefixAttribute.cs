using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteWebSocket.Routing
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageTypePrefixAttribute : Attribute
    {
        public string[] Scopes { get; protected set; }

        public MessageTypePrefixAttribute(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path must be not null", nameof(path));
            Scopes = path.ToLower().Replace('_', '-').Split(':');
        }

        public MessageTypePrefixAttribute(params string[] scopes)
        {
            if (scopes.Length < 1)
                throw new ArgumentException("At least one scope must exist", nameof(scopes));
            if (scopes.Any(p => string.IsNullOrEmpty(p)))
                throw new ArgumentException("evey scope must be not null", nameof(scopes));

            Scopes = scopes.Select(p => p.ToLower().Replace('_', '-')).ToArray();
        }
    }
}
