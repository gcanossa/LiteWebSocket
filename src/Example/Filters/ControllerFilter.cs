using LiteWebSocket.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Filters
{
    public class ControllerFilter : ExceptionFilterAttribute
    {
        public override void OnException(ActionContext context)
        {
            Console.WriteLine();
        }
    }
}
