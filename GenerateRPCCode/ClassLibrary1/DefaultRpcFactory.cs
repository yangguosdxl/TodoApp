using System;
using System.Collections.Generic;
using System.Text;

namespace CoolRpcInterface
{
    public class DefaultRpcFactory : IRpcFactory
    {
        public ISerializer serializer { get; set; }
        public ICallAsync callAsync { get; set; }

        public IServiceProvider serviceProvider { get; set; }

        public T Get<T>()
        {
            object o;
            if (m_RpcServices.TryGetValue(typeof(T)))
        }
    }
}
