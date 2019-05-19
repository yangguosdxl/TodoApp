using System;
using Cool.Interface.Rpc;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    public class ICHelloService_Hello3_MsgIn : IMessage
    {
        [MessagePack.Key(1)]
        public RpcTestInterface.Param p;
    }
}
