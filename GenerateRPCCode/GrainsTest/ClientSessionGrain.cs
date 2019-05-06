using CSRPC;
using GrainInterface;
using System;
using System.Threading.Tasks;

namespace GrainsTest
{
    public class ClientSessionGrain : IClientSessionGrain
    {
        public Guid SessionID { get; set; }

        public object State { get; set; }

        IGatewayGrainObserver m_GateWayGrain;

        ISHelloService_HandlerMap m_SHelloServiceHandlers

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

        public void Recv(ChunkType eChunkType, int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len)
        {
        
        }

        public void Send(int iProtocolID, int iCommunicateID, byte[] bytes, int start, int len)
        {
            if (m_GateWayGrain != null)
                m_GateWayGrain.Send(iProtocolID, iCommunicateID, bytes, start, len);
        }

        public void SetSessionID(Guid sessionID)
        {
            SessionID = sessionID;
        }

        public Task OnDisconnect()
        {
            throw new NotImplementedException();
        }
    }
}
