using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LiteWebSocket.Filters
{
    public interface IActionFilter : IFilter
    {
        void OnExecuting(ActionContext context);
        Task OnExecutingAsync(ActionContext context);
        void OnExecuted(ActionContext context);
        Task OnExecutedAsync(ActionContext context);
    }
}
