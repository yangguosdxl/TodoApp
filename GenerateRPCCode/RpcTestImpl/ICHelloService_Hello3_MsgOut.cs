using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    public class ICHelloService_Hello3_MsgOut
    {
        [MessagePack.Key(1)]
        public RpcTestInterface.Param Value;
    }
}