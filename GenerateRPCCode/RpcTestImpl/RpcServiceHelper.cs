using Cool.Interface.Rpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSRPC
{
    public static partial class RpcServiceHelper
    {
        private static int s_iRpcServiceCount;
        private static int s_iProtoCount;

        public static int RpcServiceCount
        {
            get { return s_iRpcServiceCount; }
        }

        public static int ProtoCount
        {
            get
            {
                return s_iProtoCount;
            }
        }

        public static int GetID<T>()
        {
            return RpcServiceHelperHodler<T>.ID;
        }

        public static T CreateRpc<T>()
        {
            return RpcServiceHelperHodler<T>.CreateRpcFunc();
        }

        public static IRPCHandlerMap CreateRpcHandlerMap<T>(ICoolRpc rpcHandler)
        {
            return RpcServiceHelperHodler<T>.CreateRpcHandlerMapFunc(rpcHandler);
        }
    }

    public class RpcServiceHelperHodler<T>
    {
        public static int ID;
        public static Func<T> CreateRpcFunc;
        public static Func<ICoolRpc, IRPCHandlerMap> CreateRpcHandlerMapFunc;
    }
}
