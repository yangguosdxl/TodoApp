using Cool.Coroutine;
using CoolRpcInterface;
using RpcTestInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientTest
{
    class CHelloService : ICHelloService
    {
        public ISerializer Serializer { get; set; }
        public ICallAsync CallAsync { get; set; }
        public int ChunkType { get; set; }

        public void Hello()
        {
            Console.WriteLine("client: recv hello");
        }

        public void Hello2(Param p)
        {
            Console.WriteLine($"client: recv hello2 param {p.a}");

            p.a = 2;
        }

        public MyTask<Param> Hello3(Param p)
        {
            Console.WriteLine($"client: recv hello3 param {p.a}");

            p.a = 3;
            return MyTask.FromResult(p);
        }

        public MyTask<(int, int)> HelloInt(int a)
        {
            Console.WriteLine($"client: recv helloint {a}");

            return MyTask.FromResult((a, a));
        }
    }
}
