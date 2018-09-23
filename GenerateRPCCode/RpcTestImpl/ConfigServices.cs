using Microsoft.Extensions.DependencyInjection;
using RpcTestInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpcTestImpl
{
    public static class ConfigServices
    {
        public static void AddAllRpcService(this IServiceCollection sc)
        {
            sc.AddSingleton<IHelloService, HelloServiceRpc>();
        }
    }
}
