using CoolRpcInterface;
using CSRPC;
using Microsoft.Extensions.DependencyInjection;

using RpcTestInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest
{
    class CallAsync : ICallAsync
    {
        int rpcResponseIndex = 0;
        Dictionary<int, Action> m_MapRpcResponseProcessor = new Dictionary<int, Action>();

        public async Task SendWithoutResponse(byte[] bytes, int iStart, int len)
        {
            
        }

        public async Task<(byte[], int, int)> SendWithResponse(byte[] bytes, int iStart, int len)
        {
            HelloIntMsgOut ret = new HelloIntMsgOut();
            ret.Value = (1,2);
            Serializer serializer = new Serializer();

            return await Task.FromResult(serializer.Serialize(ret));
        }
    }

    class Serializer : ISerializer
    {
        public T Deserialize<T>(byte[] bytes, int iStartIndex, int iCount)
        {

            MemoryStream msRet = new MemoryStream(bytes, iStartIndex, iCount, false);

            return MessagePack.MessagePackSerializer.Deserialize<T>(msRet);
        }

        public (byte[], int, int) Serialize<T>(T o)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                MessagePack.MessagePackSerializer.Serialize(ms, o);
                return (ms.GetBuffer(), 0, (int)ms.Position);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

#if false
            ServiceCollection collection = new ServiceCollection();
            collection.AddSingleton<ICallAsync, CallAsync>();
            collection.AddSingleton<ISerializer, Serializer>();
            ConfigServices.AddAllRpcService(collection);

            ServiceProvider serviceProvider = collection.BuildServiceProvider();

            var hs = serviceProvider.GetService<IHelloService>();
            var (a, b) = hs.HelloInt(1).GetAwaiter().GetResult();
#endif

            Hello().GetAwaiter().GetResult();

        }

        static async Task Hello()
        {
            var helloServer = new CSRPC.HelloService(new CallAsync(), new Serializer());
            var ret = await helloServer.HelloInt(1);
            Console.WriteLine("" + ret.Item1 + ", " + ret.Item2);
        }

        static void AddServices(IServiceCollection sc)
        {
            
        }

        

        
    }
}
