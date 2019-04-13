using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    public class ISHelloService_HelloInt_MsgIn
    {
        [MessagePack.Key(1)]
        public System.Int32 a;
    }
}
