using System;

namespace CoolRpcInterface
{
    public interface ICoolRpc
    {
        ISerializer Serializer { get; set; }
        ICallAsync CallAsync { get; set; }
    }
}
