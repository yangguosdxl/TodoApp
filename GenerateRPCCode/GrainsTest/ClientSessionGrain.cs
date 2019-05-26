using Cool;
using Cool.CSCommon;
using CSRPC;
using GrainInterface;
using Orleans;
using RpcTestInterface;
using System;
using System.Threading.Tasks;
using SHelloService = GrainsTest.SHelloService;

namespace GrainsTest
{
    public class ClientSessionGrain : Grain, IClientSessionGrain
    {
        public Guid SessionID { get; set; }

        public object State { get; set; }

        IGatewayGrainObserver m_GateWayGrain;

        public CallAsync CallAsync { get; set; }

        public override Task OnActivateAsync()
        {
            Logger.Info($"OnActivateAsync, session {IdentityString}");

            CallAsync = new CallAsync(this);

            //this.RegisterTimer(async delegate (object o)
            //{
            //    if (Console.KeyAvailable)
            //    {
            //        if (Console.ReadKey().Key == ConsoleKey.Spacebar)
            //        {
            //            m_CHelloService.Hello();

            //            var (a,b) = await m_CHelloService.HelloInt(1);
            //            CoolLog.WriteLine($"recv client a: {a}, b: {b}");
            //        }
            //    }
            //}, null, new TimeSpan(0), new TimeSpan(0,0,0,0,100));

            return Task.CompletedTask;
        }

        public override Task OnDeactivateAsync()
        {
            Logger.Info($"OnDeactivateAsync, session {IdentityString}");

            return base.OnDeactivateAsync();
        }

        public Task Subscribe(IGatewayGrainObserver gateway)
        {
            Logger.Info($"subscribe, session {IdentityString}");

            m_GateWayGrain = gateway;
            return Task.CompletedTask;
        }

        public Task UnSubscribe(IGatewayGrainObserver gateway)
        {
            Logger.Info($"unsubscribe, session {IdentityString}");

            m_GateWayGrain = null;
            return Task.CompletedTask;
        }

        public Task Recv(ChunkType eChunkType, int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len)
        {
            CallAsync.OnMessage((int)eChunkType, iProtocolID, iCommunicationID, bytes, start, len);

            return Task.CompletedTask;
        }

        public Task Send(int iProtocolID, int iCommunicateID, byte[] bytes, int start, int len)
        {
            if (m_GateWayGrain != null)
                m_GateWayGrain.Send(iProtocolID, iCommunicateID, bytes, start, len);
            return Task.CompletedTask;
        }

        public Task SetSessionID(Guid sessionID)
        {
            SessionID = sessionID;
            return Task.CompletedTask;
        }

        public Task OnDisconnect()
        {
            Logger.Info($"Disconnection, remove session, guid {IdentityString}");
            return Task.CompletedTask;
        }
        
    }
}
