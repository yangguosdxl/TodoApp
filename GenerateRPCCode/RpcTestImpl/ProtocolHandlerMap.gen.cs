using CSRPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpcTestImpl
{
    public static partial class ProtocolHandlerMap
    {
        static ProtocolHandlerMap()
        {
            s_aProtocolHandlers = new Cool.Interface.Rpc.ProtocolHandler[(int)ProtoID.COUNT];
            s_aProtocolDeserializers = new Cool.Interface.Rpc.ProtocolDeserializer[(int)ProtoID.COUNT];
        }
    }
}
