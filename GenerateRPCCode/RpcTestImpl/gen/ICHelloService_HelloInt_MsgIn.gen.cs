using System;
using Cool.Interface.Rpc;
using System.Threading.Tasks;
using Cool.Coroutine;
using RpcTestInterface;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    internal class ICHelloService_HelloInt_MsgIn : IMessage
    {
        [MessagePack.Key(1)]
        public System.Int32 a;
    }
}
