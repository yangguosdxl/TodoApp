using System;
using Cool.Interface.Rpc;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;
using RpcTestInterface;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    internal class ICHelloService_Hello_MsgIn : IMessage
    {
    }
}
