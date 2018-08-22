using System;

namespace CoolRpcInterface
{
    public interface ICoolRpc
    {
        ISerializer serializer { get; set; }
        ICallAsync callAsync { get; set; }
    }
}
