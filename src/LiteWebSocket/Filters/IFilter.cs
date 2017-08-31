using LiteWebSocket.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LiteWebSocket.Filters
{
    public interface IFilter
    {
        void Accept(Action<ActionContext> handler, ActionContext context);
        Task AcceptAsync(Func<ActionContext, Task> handler, ActionContext context);
    }
}
