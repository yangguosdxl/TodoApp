
using Orleans;
using System;
using System.Threading.Tasks;

namespace GrainInterface
{
    public interface IClientSessionGrain : IGrainWithGuidKey
    {
        Guid SessionID { get; set; }

        object State { get; set; }

        Task Subscribe(IGatewayGrainObserver gateway);

        Task UnSubscribe(IGatewayGrainObserver gateway);

        void Recv(ChunkType eChunkType, int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len);

        void Send(byte[] bytes, int start, int len);

        void SetSessionID(Guid sessionID);

        Task OnDisconnect();
    }
}
