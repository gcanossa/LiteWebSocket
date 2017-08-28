using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LiteWebSocket.Protocol
{
    public class Controller : MessageController
    {
        public async Task<IOperationResult> SyncXY_sssss(Messages.SyncMessage msg)
        {
            await Task.Delay(1000);
            return Result<Messages.Sync_ResponseMessage>(new Messages.Sync_ResponseMessage() { Valore = "resp" });
        }
    }
}
