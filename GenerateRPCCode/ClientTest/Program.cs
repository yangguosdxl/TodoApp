using Cool;
using Cool.Coroutine;
using Cool.Interface.Rpc;
using Cool.CSCommon;
using CSRPC;
using GrainInterface;
using RpcTestInterface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClientTest
{

    class Program
    {
        static IRPCHandlerMap s_CHelloServiceHandlers;
        static ICallAsync s_callAsync;
        static ISHelloService s_sHelloService;
        static Random s_Random = new Random();

        static void Main(string[] args)
        {
            for(int i = 0; i < 500; ++i)
            {
                Task.Run(StartClient);
            }

            Console.ReadKey();
#if false
            ISerializer serializer = new Serializer();
            s_callAsync = new CallAsync("127.0.0.1", 1234, Cool.Interface.NetWork.NetType.TCP);

            ClientTest.CHelloService cHelloService = new ClientTest.CHelloService();
            cHelloService.Serializer = serializer;
            cHelloService.CallAsync = s_callAsync;
            cHelloService.ChunkType = (int)ChunkType.BASE;

            s_CHelloServiceHandlers = new ICHelloService_HandlerMap(cHelloService);

            s_sHelloService = new CSRPC.SHelloService(serializer, s_callAsync);
            s_sHelloService.ChunkType = (int)ChunkType.BASE;

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                    {
                        s_sHelloService.Hello();
                        MyTask.Run(async delegate (object state)
                        {
                            var (a, b) = await s_sHelloService.HelloInt(1);
                            CoolLog.WriteLine($"a:{a}, b:{b}");

                            //return null;
                        }, null);
                    }
                    else if (Console.ReadKey().Key == ConsoleKey.A)
                    {

                    }
                }


                if (s_callAsync != null)
                    s_callAsync.Update();
            }

#endif
        }

        static async Task StartClient()
        {
            ISerializer serializer = new Serializer();
            ICallAsync callAsync = new CallAsync("127.0.0.1", 1234, Cool.Interface.NetWork.NetType.TCP);

            ClientTest.CHelloService cHelloService = new ClientTest.CHelloService();
            cHelloService.Serializer = serializer;
            cHelloService.CallAsync = callAsync;
            cHelloService.ChunkType = (int)ChunkType.BASE;

            IRPCHandlerMap cHelloServiceHandlerMap = new ICHelloService_HandlerMap(cHelloService);

            ISHelloService sHelloService = new CSRPC.SHelloService(serializer, callAsync);
            sHelloService.ChunkType = (int)ChunkType.BASE;

            while (true)
            {
                await Task.Delay(10);

                MyTask.Run(RunRequest, sHelloService);
                

                if (callAsync != null)
                    callAsync.Update();
            }
        }

        static async MyTask RunRequest(object state)
        {
            ISHelloService sHelloService = state as ISHelloService;
            int choose = s_Random.Next() % 4;
            switch (choose)
            {
                case 0:
                    {
                        sHelloService.Hello();
                        break;
                    }
                case 1:
                    {
                        var (a, b) = await sHelloService.HelloInt(1);
                        Logger.Debug($"a:{a}, b:{b}");
                        
                        break;
                    }
                case 2:
                    {
                        Param p = new Param();
                        p.a = 2;
                        sHelloService.Hello2(p);
                        break;
                    }
                case 3:
                    {
                        Param p = new Param();
                        p.a = 3;
                        Param ret = await sHelloService.Hello3(p);
                        break;
                    }

            }
        }
    }
}
