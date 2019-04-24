using NetWorkInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoolRpcInterface
{
    public class DefaultRpcFactory : IRpcFactory
    {
        public ISerializer serializer { get; set; }
        public ICallAsync callAsync { get; set; }

        Dictionary<Type, ICoolRpc> m_RpcServices = new Dictionary<Type, ICoolRpc>();
        IServiceProvider serviceProvider { get; set; }

        public DefaultRpcFactory(ISerializer serializer, ICallAsync callAsync, IServiceProvider serviceProvider)
        {
            this.serializer = serializer;
            this.callAsync = callAsync;
            this.serviceProvider = serviceProvider;
        }

        public T Get<T>() where T : class, ICoolRpc
        {
            ICoolRpc rpcService;
            Type rpcType = typeof(T);
            if (m_RpcServices.TryGetValue(rpcType, out rpcService) == false)
            {
                rpcService = serviceProvider.GetService(rpcType) as T;

                rpcService.Serializer = serializer;
                rpcService.CallAsync = callAsync;

                m_RpcServices.Add(rpcType, rpcService);
            }

            return rpcService as T;
        }
    }
}
