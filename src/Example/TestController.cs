using Example.Filters;
using LiteWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example
{
    [ControllerFilter]
    public class TestController : MessageController
    {
        public async Task<IOperationResult> Echo(TestMessageScope.Test1 msg)
        {
            await Task.Delay(1);
            return Result<TestMessageScope.Test1>(msg);
        }
        [MethodFilter]
        public IOperationResult Next(TestMessageScope.Test1 msg)
        {
            return Result(new TestMessageScope.Test2() { SessionId="af", SequenceNumber=2, Timestamp=DateTime.UtcNow });
        }
        public void XY(TestMessageScope.Test3 msg)
        {

        }
        public async Task XY_Async(TestMessageScope.Test3 msg)
        {
            await Task.Delay(1);
        }
    }
}
