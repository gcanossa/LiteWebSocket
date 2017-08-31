using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiteWebSocket.Models;

namespace LiteWebSocket.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class ActionFilterAttribute : Attribute, IActionFilter
    {
        public void Accept(Action<ActionContext> handler, ActionContext context)
        {
            bool failed = false;
            try
            {
                OnExecuting(context);
                handler(context);
            }
            catch (Exception e)
            {
                failed = true;
                context.Exception = e;
                OnExecuted(context);
            }
            finally
            {
                if (!failed)
                    OnExecuted(context);
            }
        }

        public async Task AcceptAsync(Func<ActionContext, Task> handler, ActionContext context)
        {
            bool failed = false;
            try
            {
                await OnExecutingAsync(context);
                await handler(context);
            }
            catch (Exception e)
            {
                failed = true;
                context.Exception = e;
                await OnExecutedAsync(context);
            }
            finally
            {
                if (!failed)
                    await OnExecutedAsync(context);
            }
        }

        public abstract void OnExecuted(ActionContext context);

        public virtual async Task OnExecutedAsync(ActionContext context)
        {
            await Task.Run(() => OnExecuted(context));
        }

        public abstract void OnExecuting(ActionContext context);

        public virtual async Task OnExecutingAsync(ActionContext context)
        {
            await Task.Run(() => OnExecuting(context));
        }
    }
}
