using Cool;
using Cool.Coroutine;
using Cool.Interface.Rpc;
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
            Logger.Debug("client: recv hello");
        }

        public void Hello2(Param p)
        {
            Logger.Debug($"client: recv hello2 param {p.a}");

            p.a = 2;
        }

        public MyTask<Param> Hello3(Param p)
        {
            Logger.Debug($"client: recv hello3 param {p.a}");

            p.a = 3;
            return MyTask.FromResult(p);
        }

        public MyTask<(int, int)> HelloInt(int a)
        {
            Logger.Debug($"client: recv helloint {a}");

            return MyTask.FromResult((a, a));
        }

        public void Init(ISerializer serializer, ICallAsync callAsync, int iChunkType)
        {
            Serializer = serializer;
            CallAsync = callAsync;
            ChunkType = iChunkType;
        }
    }
}
