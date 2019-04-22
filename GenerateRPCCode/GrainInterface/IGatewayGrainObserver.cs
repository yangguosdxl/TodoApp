using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrainInterface
{
    public interface IGatewayGrainObserver : IGrainObserver
    {
        void Send(Guid sessionID, int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len);
    }
}
