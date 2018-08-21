using CoolRpcInterface;
using System;
using System.Threading.Tasks;

namespace RpcTestInterface
{
    public interface IHelloService : ICoolRpc
    {
        Task Hello();

        Task<(int,int)> HelloInt(int a);
    }
}
