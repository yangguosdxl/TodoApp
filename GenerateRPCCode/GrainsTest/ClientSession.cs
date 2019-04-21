using GrainInterface;
using System;

namespace GrainsTest
{
    public class ClientSession : IClientSession
    {
        public void Recv(int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len)
        {
            throw new NotImplementedException();
        }

        public void Send(int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len)
        {
            throw new NotImplementedException();
        }
    }
}
