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
            RpcServiceHelperHodler<ICHelloService>.CreateFunc = delegate()
            {
                return new CHelloService();
            };
            RpcServiceHelperHodler<ISHelloService>.ID = 2;
            RpcServiceHelperHodler<ISHelloService>.CreateFunc = delegate()
            {
                return new SHelloService();
            };
            s_iRpcServiceCount = 3;
        }
    }
}
