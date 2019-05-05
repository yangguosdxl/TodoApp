using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    public class ICHelloService_HelloInt_MsgIn
    {
        [MessagePack.Key(1)]
        public System.Int32 a;
    }
}
