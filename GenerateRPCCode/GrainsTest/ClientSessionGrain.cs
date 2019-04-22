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
            throw new NotImplementedException();
        }

        public void Send(int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len)
        {
            if (m_GateWayGrain != null)
                m_GateWayGrain.Send(SessionID, iCommunicationID, iProtocolID, bytes, start, len);
        }

        public void SetSessionID(Guid sessionID)
        {
            SessionID = sessionID;
        }
    }
}
