using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LiteWebSocket.Routing
{
    public interface IMessageControllerResolutionConvention
    {
        Dictionary<MethodInfo, Type> GetControllerActions(MessageController controller);
        Dictionary<MethodInfo, Type> GetControllerActions(Type type);
        Dictionary<MethodInfo, Type> GetControllerActions<T>() where T : MessageController;
    }
}
