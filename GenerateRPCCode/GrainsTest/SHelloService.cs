using System;
using System.Threading.Tasks;
using Cool;
using Cool.Coroutine;
using Cool.Interface.Rpc;
using GrainsTest;
using RpcTestInterface;

namespace GrainsTest
{
    class SHelloService : ISHelloService
    {
        public ISerializer Serializer { get ; set ; }
        public ICallAsync CallAsync { get ; set ; }
        public int ChunkType { get ; set ; }

        private PlayerGrain m_PlayerGrain;

        public SHelloService(PlayerGrain playerGrain)
        {
            m_PlayerGrain = playerGrain;
        }

        public void Hello()
        {
            Logger.Debug("server: recv hello");

            CallAsync.GetRpc<ICHelloService>().Hello();
        }

        public void Hello2(Param p)
        {
            Logger.Debug($"server: recv hello2 param {p.a}");

            p.a = 2;
        }

        public MyTask<Param> Hello3(Param p)
        {
            Logger.Debug($"server: recv hello3 param {p.a}");

            p.a = 3;
            return MyTask.FromResult(p);
        }

        public MyTask<(int, int)> HelloInt(int a)
        {
            Logger.Debug($"server: recv helloint {a}");

            MyTask.Run(async delegate (object o)
            {
                var (c, d) = await CallAsync.GetRpc<ICHelloService>().HelloInt(1);
                Logger.Debug($"recv client a: {c}, b: {d}");

            }, null);

            return MyTask.FromResult((a, a));
        }

        public void Init(ISerializer serializer, ICallAsync callAsync, int iChunkType)
        {
            this.Serializer = serializer;
            this.CallAsync = callAsync;
            this.ChunkType = iChunkType;
        }
    }
}
