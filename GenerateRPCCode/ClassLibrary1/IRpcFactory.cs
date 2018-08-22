using System;
using System.Collections.Generic;
using System.Text;

namespace CoolRpcInterface
{
    public interface IRpcFactory
    {
        ISerializer serializer { get; set; }
        ICallAsync callAsync { get; set; }
        T Get<T>();
    }
}
