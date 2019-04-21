
using Orleans;
using System;

namespace GrainInterface
{
    public interface IClientSession : IGrainObserver
    {
        void Recv(int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len);
        void Send(int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len);
    }
}
