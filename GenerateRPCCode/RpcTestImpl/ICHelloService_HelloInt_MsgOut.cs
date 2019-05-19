using System;
using Cool.Interface.Rpc;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    public class ICHelloService_HelloInt_MsgOut : IMessage
    {
        [MessagePack.Key(1)]
        public ValueTuple<System.Int32, System.Int32> Value;
    }
}
