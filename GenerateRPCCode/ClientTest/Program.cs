using CoolRpcInterface;
using Microsoft.Extensions.DependencyInjection;
using RpcTestImpl;
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
        public Task Send(byte[] bytes, int iStart, int len)
        {
            throw new NotImplementedException();
        }

        public Task<(byte[], int, int)> SendWithResponse(byte[] bytes, int iStart, int len)
        {
            throw new NotImplementedException();
        }
    }

    class Serializer : ISerializer
    {
        public T Deserialize<T>(byte[] bytes, int iStartIndex, int iCount)
        {

            MemoryStream msRet = new MemoryStream(bytes, iStartIndex, iCount, false);

            return  ProtoBuf.Serializer.Deserialize<T>(msRet);
        }

        public (byte[], int, int) Serialize<T>(T o)
        {
            throw new NotImplementedException();
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
        }

        static void AddServices(IServiceCollection sc)
        {
            
        }

        

        
    }
}
