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
            Serializer serializer = new Serializer();
            var msgIn = serializer.Deserialize<HelloIntMsgIn>(bytes, iStart, len);

            Console.WriteLine("process msg: " + msgIn.eProtoID + ", " + msgIn.a);

            HelloIntMsgOut ret = new HelloIntMsgOut();
            ret.Value = (msgIn.a + 100, 2);

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
        static IRpcFactory rpcFactory;
        static void Main(string[] args)
        {
            ServiceCollection collection = new ServiceCollection();

            CSRPC.RpcServicesConfiguration.AddAllRpcServices(collection);
            collection.AddSingleton<IRpcFactory, DefaultRpcFactory>();

            var serviceProvider = collection.BuildServiceProvider();

            rpcFactory = new DefaultRpcFactory(new Serializer(), new CallAsync(), serviceProvider);
#if false


            var hs = serviceProvider.GetService<IHelloService>();
            var (a, b) = hs.HelloInt(1).GetAwaiter().GetResult();
#endif

            Hello().GetAwaiter().GetResult();

        }

        static async Task Hello()
        {
            var helloServer = rpcFactory.Get<IHelloService>();
            var ret = await helloServer.HelloInt(1);
            Console.WriteLine("" + ret.Item1 + ", " + ret.Item2);

            await helloServer.Hello2(new Param());
        }

        static void AddServices(IServiceCollection sc)
        {
            
        }

        

        
    }
}
