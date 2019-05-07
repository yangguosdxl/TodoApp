using CoolRpcInterface;
using CSCommon;
using CSRPC;
using GrainInterface;
using RpcTestInterface;
using System;

namespace ClientTest
{

    class Program
    {
        static IRPCHandlerMap CHelloServiceHandlers;
        static ICallAsync callAsync;
        static ISHelloService sHelloService;


        static void Main(string[] args)
        {
            

            while(true)
            {
                if (Console.ReadKey().Key ==  ConsoleKey.Spacebar)
                {
                    sHelloService.Hello();
                }
                else if (Console.ReadKey().Key == ConsoleKey.A)
                {
                    ISerializer serializer = new Serializer();
                    callAsync = new CallAsync("127.0.0.1", 1234, NetWorkInterface.NetType.TCP);

                    CHelloService cHelloService = new CHelloService();
                    cHelloService.Serializer = serializer;
                    cHelloService.CallAsync = callAsync;
                    cHelloService.ChunkType = (int)ChunkType.BASE;

                    CHelloServiceHandlers = new ICHelloService_HandlerMap(cHelloService);

                    sHelloService = new CSRPC.SHelloService();
                    sHelloService.Serializer = serializer;
                    sHelloService.CallAsync = callAsync;
                    sHelloService.ChunkType = (int)ChunkType.BASE;
                }

                if (callAsync != null)
                    callAsync.Update();
            }
        }
    }
}
