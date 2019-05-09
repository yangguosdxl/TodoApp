﻿using System;
using System.Threading.Tasks;
using Cool;
using Cool.Coroutine;
using CoolRpcInterface;
using GrainsTest;
using RpcTestInterface;

namespace GrainsTest
{
    class SHelloService : ISHelloService
    {
        public ISerializer Serializer { get ; set ; }
        public ICallAsync CallAsync { get ; set ; }
        public int ChunkType { get ; set ; }

        public ClientSessionGrain clientSessionGrain;

        public void Hello()
        {
            CoolLog.WriteLine("server: recv hello");

            clientSessionGrain.CHelloService.Hello();
        }

        public void Hello2(Param p)
        {
            CoolLog.WriteLine($"server: recv hello2 param {p.a}");

            p.a = 2;
        }

        public MyTask<Param> Hello3(Param p)
        {
            CoolLog.WriteLine($"server: recv hello3 param {p.a}");

            p.a = 3;
            return MyTask.FromResult(p);
        }

        public MyTask<(int, int)> HelloInt(int a)
        {
            CoolLog.WriteLine($"server: recv helloint {a}");

            MyTask.Run(async delegate (object o)
            {
                var (c, d) = await clientSessionGrain.CHelloService.HelloInt(1);
                CoolLog.WriteLine($"recv client a: {c}, b: {d}");

            }, null);

            return MyTask.FromResult((a, a));
        }
        
    }
}
