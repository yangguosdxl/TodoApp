using CSCommon;
using CSRPC;
using GrainInterface;
using Orleans;
using System;
using System.Threading.Tasks;

namespace GrainsTest
{
    public class ClientSessionGrain : Grain, IClientSessionGrain
    {
        public Guid SessionID { get; set; }

        public object State { get; set; }

        IGatewayGrainObserver m_GateWayGrain;

        ISHelloService_HandlerMap m_SHelloServiceHandlers;
        CallAsync m_CallAsync;

        public override Task OnActivateAsync()
        {
            SHelloService sHelloService = new SHelloService();
            sHelloService.Serializer = new Serializer();
            sHelloService.CallAsync = m_CallAsync = new CallAsync(this);

            m_SHelloServiceHandlers = new ISHelloService_HandlerMap(sHelloService);



            return base.OnActivateAsync();
        }

        public Task Subscribe(IGatewayGrainObserver gateway)
        {
            m_GateWayGrain = gateway;
            return Task.CompletedTask;
        }

        public Task UnSubscribe(IGatewayGrainObserver gateway)
        {
            m_GateWayGrain = null;
            return Task.CompletedTask;
        }

        public Task Recv(ChunkType eChunkType, int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len)
        {
            m_CallAsync.OnMessage((int)eChunkType, iProtocolID, iCommunicationID, bytes, start, len);

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
            throw new NotImplementedException();
        }
        
    }
}
