using System;
using Cool.Interface.Rpc;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;
using RpcTestInterface;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    public class ICHelloService_HelloInt_MsgIn : IMessage
    {
        [MessagePack.Key(1)]
        public System.Int32 a;
    }
}
