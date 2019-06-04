using System;
using Cool.Interface.Rpc;
using System.Threading.Tasks;
using Cool.Coroutine;
using RpcTestInterface;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    internal class ISHelloService_Hello_MsgIn : IMessage
    {
    }
}
