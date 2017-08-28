using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LiteWebSocket
{
    public class OperationContext
    {
        internal List<object> _results;

        public OperationContext()
        {
            _results = new List<object>();
        }

        internal void AddResult(object result)
        {
            if(result.GetType() != typeof(void))
                _results.Add(result);

        }

        public IEnumerable<Message> Results
        {
            get
            {
                List<Message> results = new List<Message>();
                foreach (object item in _results)
                {
                    Type type = item.GetType();
                    if (typeof(Task).IsAssignableFrom(type) && type.IsGenericType && typeof(IOperationResult).IsAssignableFrom(type.GetGenericArguments().First()))
                    {
                        MethodInfo convert_method = typeof(FunctionWrappersExtensions).GetMethods().First(p=>p.Name==nameof(FunctionWrappersExtensions.WaitTaskResults) && p.IsGenericMethod).MakeGenericMethod(type.GetGenericArguments().First());
                        results.AddRange(((IOperationResult)convert_method.Invoke(null, new object[] { item })).GetMessages());
                    }
                    else  if (typeof(Task).IsAssignableFrom(type))
                    {
                        ((Task)item).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    else
                    {
                        results.AddRange(((IOperationResult)item).GetMessages());
                    }
                }

                return results;
            }
        }
    }
}
