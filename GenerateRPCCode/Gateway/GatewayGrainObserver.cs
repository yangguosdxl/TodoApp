using GrainInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gateway
{
    class GatewayGrainObserver : IGatewayGrainObserver
    {
        public void Send(Guid sessionID, int iCommunicationID, int iProtocolID, byte[] bytes, int start, int len)
        {
            throw new NotImplementedException();
        }
    }
}
