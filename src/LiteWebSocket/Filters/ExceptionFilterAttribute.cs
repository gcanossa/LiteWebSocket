using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiteWebSocket.Models;

namespace LiteWebSocket.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void Accept(Action<ActionContext> handler, ActionContext context)
        {
            try
            {
                handler(context);
            }
            catch(Exception e)
            {
                context.Exception = e;
                OnException(context);
            }
        }

        public async Task AcceptAsync(Func<ActionContext, Task> handler, ActionContext context)
        {
            try
            {
                await handler(context);
            }
            catch (Exception e)
            {
                context.Exception = e;
                await OnExceptionAsync(context);
            }
        }

        public abstract void OnException(ActionContext context);

        public virtual async Task OnExceptionAsync(ActionContext context)
        {
            await Task.Run(() => OnException(context));
        }
    }
}
