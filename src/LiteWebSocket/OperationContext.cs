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
        //TODO: thick if something more can be usefull
        private List<List<Message>> _results = new List<List<Message>>();
        
        internal void AddResult(List<Message> result)
        {
            _results.Add(result);
        }

        public IEnumerable<Message> Results
        {
            get
            {
                return _results?.SelectMany(p => p);
            }
        }
    }
}
