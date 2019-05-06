using CoolRpcInterface;
using CSCommon;
using CSRPC;
using GrainInterface;
using Microsoft.Extensions.DependencyInjection;

using RpcTestInterface;
using System;
using System.Threading.Tasks;

namespace ClientTest
{

    class Program
    {
        static IRpcFactory rpcFactory;
        static void Main(string[] args)
        {
            ISerializer serializer = new Serializer();
            ICallAsync callAsync = new CallAsync("127.0.0.1", 1234, NetWorkInterface.NetType.TCP);

            CHelloService cHelloService = new CHelloService();
            cHelloService.Serializer = serializer;
            cHelloService.CallAsync = callAsync;
            cHelloService.ChunkType = (int)ChunkType.BASE;

            IRPCHandlerMap handlers = new ICHelloService_HandlerMap(cHelloService);

            ISHelloService sHelloService = new CSRPC.SHelloService();
            sHelloService.Serializer = serializer;
            sHelloService.CallAsync = callAsync;
            sHelloService.ChunkType = (int)ChunkType.BASE;

            sHelloService.Hello();

            while(true)
            {
                callAsync.Update();
            }
        }
    }
}
