using Cool.Interface.Rpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpcTestImpl
{
    public static partial class ProtocolHandlerMap
    {
        static ProtocolHandler[] s_aProtocolHandlers;
        static ProtocolDeserializer[] s_aProtocolDeserializers;

        public static ProtocolHandler GetProtocolHandler(int iProtoID)
        {
            return s_aProtocolHandlers[iProtoID];
        }

        public static ProtocolDeserializer GetProtocolDeserializer(int iProtoID)
        {
            return s_aProtocolDeserializers[iProtoID];
        }
    }
}
