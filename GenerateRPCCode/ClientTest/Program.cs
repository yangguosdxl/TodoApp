using CoolRpcInterface;
using CSCommon;
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
            var helloServer = rpcFactory.Get<ISHelloService>();
            var ret = await helloServer.HelloInt(1);
            Console.WriteLine("" + ret.Item1 + ", " + ret.Item2);

            await helloServer.Hello2(new Param());
        }

        static void AddServices(IServiceCollection sc)
        {
            
        }

        

        
    }
}
