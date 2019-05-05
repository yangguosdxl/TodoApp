using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    public class ISHelloService_Hello2_MsgIn
    {
        [MessagePack.Key(1)]
        public RpcTestInterface.Param p;
    }
}
