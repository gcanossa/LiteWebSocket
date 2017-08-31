using LiteWebSocket.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Filters
{
    public class MethodFilter : ActionFilterAttribute
    {
        public override void OnExecuted(ActionContext context)
        {
            Console.WriteLine();
        }

        public override void OnExecuting(ActionContext context)
        {
            Console.WriteLine();
        }
    }
}
