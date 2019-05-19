using System;
using Cool.Interface.Rpc;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    public class ISHelloService_Hello3_MsgOut : IMessage
    {
        [MessagePack.Key(1)]
        public RpcTestInterface.Param Value;
    }
}
