using Cool.Coroutine;
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
            ISerializer serializer = new Serializer();
            callAsync = new CallAsync("127.0.0.1", 1234, NetWorkInterface.NetType.TCP);

            ClientTest.CHelloService cHelloService = new ClientTest.CHelloService();
            cHelloService.Serializer = serializer;
            cHelloService.CallAsync = callAsync;
            cHelloService.ChunkType = (int)ChunkType.BASE;

            CHelloServiceHandlers = new ICHelloService_HandlerMap(cHelloService);

            sHelloService = new CSRPC.SHelloService(serializer, callAsync);
            sHelloService.ChunkType = (int)ChunkType.BASE;

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                    {
                        sHelloService.Hello();
                        MyTask.Run(async delegate (object state)
                        {
                            var (a, b) = await sHelloService.HelloInt(1);
                            Console.WriteLine($"a:{a}, b:{b}");

                            //return null;
                        }, null);
                    }
                    else if (Console.ReadKey().Key == ConsoleKey.A)
                    {

                    }
                }


                if (callAsync != null)
                    callAsync.Update();
            }

            
        }
    }
}
