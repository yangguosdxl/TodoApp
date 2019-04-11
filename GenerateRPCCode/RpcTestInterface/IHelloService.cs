using CoolRpcInterface;
using System;
using System.Threading.Tasks;

namespace RpcTestInterface
{
    public class Param
    {
        public int a;
    }
    public interface IHelloService : ICoolRpc
    {
        Task Hello();

        Task<(int,int)> HelloInt(int a);

        Task Hello2(Param p);
        Task<Param> Hello3(Param p);
    }
}
