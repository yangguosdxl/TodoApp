using System;
using System.Threading.Tasks;
using Cool.Coroutine;
using CoolRpcInterface;
using RpcTestInterface;

namespace ServerTest
{
    class SHelloService : ISHelloService
    {
        public ISerializer Serializer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICallAsync CallAsync { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ChunkType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Hello()
        {
            Console.WriteLine("server: recv hello");
        }

        public void Hello2(Param p)
        {
            Console.WriteLine($"server: recv hello2 param {p.a}");

            p.a = 2;
        }

        public MyTask<Param> Hello3(Param p)
        {
            Console.WriteLine($"server: recv hello3 param {p.a}");

            p.a = 3;
            return MyTask.FromResult(p);
        }

        public MyTask<(int, int)> HelloInt(int a)
        {
            Console.WriteLine($"server: recv helloint {a}");

            return MyTask.FromResult((a, a));
        }
        
    }
}
