using System;
using Cool.Interface.Rpc;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;
using RpcTestInterface;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    internal class ICHelloService_Hello2_MsgIn : IMessage
    {
        [MessagePack.Key(1)]
        public RpcTestInterface.Param p;
    }
}
