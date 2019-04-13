using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CSRPC
{
    [MessagePack.MessagePackObject]
    public class ISHelloService_HelloInt_MsgOut
    {
        [MessagePack.Key(1)]
        public ValueTuple<System.Int32, System.Int32> Value;
    }
}
