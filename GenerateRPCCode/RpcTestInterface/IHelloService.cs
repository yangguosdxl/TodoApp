using Cool.Coroutine;
using Cool.Interface.Rpc;
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
        void Hello();

        MyTask<(int,int)> HelloInt(int a);

        void Hello2(Param p);
        MyTask<Param> Hello3(Param p);
    }

    public interface ISHelloService : ICoolRpc
    {
        void Hello();

        MyTask<(int, int)> HelloInt(int a);

        void Hello2(Param p);
        MyTask<Param> Hello3(Param p);
    }
}
