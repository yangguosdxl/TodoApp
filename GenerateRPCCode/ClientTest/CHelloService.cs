using CoolRpcInterface;
using RpcTestInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest
{
    class CHelloService : ICHelloService
    {
        public ISerializer Serializer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICallAsync CallAsync { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task Hello()
        {
            Console.WriteLine("client: recv hello");
            return null;
        }

        public Task Hello2(Param p)
        {
            Console.WriteLine($"client: recv hello2 param {p.a}");

            p.a = 2;
            return null;
        }

        public Task<Param> Hello3(Param p)
        {
            Console.WriteLine($"client: recv hello3 param {p.a}");

            p.a = 3;
            return Task.FromResult(p);
        }

        public Task<(int, int)> HelloInt(int a)
        {
            Console.WriteLine($"client: recv helloint {a}");

            return Task.FromResult((a, a));
        }
    }
}
