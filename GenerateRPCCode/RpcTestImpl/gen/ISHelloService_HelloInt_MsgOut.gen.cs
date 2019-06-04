using System;
using Cool.Interface.Rpc;
using System.Threading.Tasks;
using Cool.Coroutine;
using RpcTestInterface;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    internal class ISHelloService_HelloInt_MsgOut : IMessage
    {
        [MessagePack.Key(1)]
        public ValueTuple<System.Int32, System.Int32> Value;
    }
}
