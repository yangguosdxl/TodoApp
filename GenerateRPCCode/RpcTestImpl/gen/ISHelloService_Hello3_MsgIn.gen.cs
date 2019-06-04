using System;
using Cool.Interface.Rpc;
using System.Threading.Tasks;
using Cool.Coroutine;
using RpcTestInterface;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    internal class ISHelloService_Hello3_MsgIn : IMessage
    {
        [MessagePack.Key(1)]
        public RpcTestInterface.Param p;
    }
}
