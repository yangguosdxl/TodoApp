using System;

namespace GrainInterface
{
    public interface IClientSession
    {
        void Recv(int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len);
        void Send(int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len);
    }
}
