using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    public class ICHelloService_Hello3_MsgOut : IMessage
    {
        [MessagePack.Key(1)]
        public RpcTestInterface.Param Value;
    }
}
