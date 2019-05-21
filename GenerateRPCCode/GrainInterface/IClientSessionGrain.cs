
using Orleans;
using System;
using System.Threading.Tasks;

namespace GrainInterface
{
    public interface IClientSessionGrain //: IGrainWithGuidKey
    {

        Task Subscribe(IGatewayGrainObserver gateway);

        Task UnSubscribe(IGatewayGrainObserver gateway);

        Task Recv(ChunkType eChunkType, int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len);

        Task Send(int iProtocolID, int iCommunicateID, byte[] bytes, int start, int len);

        Task SetSessionID(Guid sessionID);

        Task OnDisconnect();
    }
}
