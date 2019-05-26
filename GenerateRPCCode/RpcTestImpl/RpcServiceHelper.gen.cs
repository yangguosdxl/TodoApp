using System;
using Cool.Interface.Rpc;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;
using RpcTestInterface;

namespace CSRPC
{
    public static partial class RpcServiceHelper
    {
        static RpcServiceHelper()
        {
            RpcServiceHelperHodler<ICHelloService>.ID = 1;
            RpcServiceHelperHodler<ICHelloService>.CreateRpcFunc = delegate()
            {
                return new CHelloService();
            };
            RpcServiceHelperHodler<ICHelloService>.CreateRpcHandlerMapFunc = delegate(ICoolRpc rpcHandler)
            {
                return new ICHelloService_HandlerMap(rpcHandler);
            };
            RpcServiceHelperHodler<ISHelloService>.ID = 2;
            RpcServiceHelperHodler<ISHelloService>.CreateRpcFunc = delegate()
            {
                return new SHelloService();
            };
            RpcServiceHelperHodler<ISHelloService>.CreateRpcHandlerMapFunc = delegate(ICoolRpc rpcHandler)
            {
                return new ISHelloService_HandlerMap(rpcHandler);
            };
            s_iRpcServiceCount = 3;
            s_iProtoCount = (int)ProtoID.COUNT;
        }
    }
}
