using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;

namespace CSRPC
{
    public static class RpcServicesConfiguration
    {
        public static void AddAllRpcServices(IServiceCollection ServiceCollection)
        {
            ServiceCollection.AddSingleton<RpcTestInterface.ICHelloService, CHelloService>();
            ServiceCollection.AddSingleton<RpcTestInterface.ISHelloService, SHelloService>();
        }
    }
}
