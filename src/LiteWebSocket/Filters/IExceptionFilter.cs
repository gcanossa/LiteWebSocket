using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LiteWebSocket.Filters
{
    interface IExceptionFilter : IFilter
    {
        void OnException(ActionContext context);
        Task OnExceptionAsync(ActionContext context);
    }
}
