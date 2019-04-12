using CoolRpcInterface;
using System;
using System.Threading.Tasks;

namespace RpcTestInterface
{
    [MessagePack.MessagePackObject]
    public class Param
    {
        [MessagePack.Key(1)]
        public int a;
    }
    public interface ICHelloService : ICoolRpc
    {
        Task Hello();

        Task<(int,int)> HelloInt(int a);

        Task Hello2(Param p);
        Task<Param> Hello3(Param p);
    }

    public interface ISHelloService : ICoolRpc
    {
        Task Hello();

        Task<(int, int)> HelloInt(int a);

        Task Hello2(Param p);
        Task<Param> Hello3(Param p);
    }
}
