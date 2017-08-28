using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LiteWebSocket.Routing
{
    public interface IMessageControllerResolutionConvention
    {
        Dictionary<MethodInfo, Type> GetControllerActions(SocketController controller);
        Dictionary<MethodInfo, Type> GetControllerActions(Type type);
        Dictionary<MethodInfo, Type> GetControllerActions<T>() where T : SocketController;
    }
}
