using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoolRpcInterface
{
    public interface ICallAsync
    {
        Task SendWithoutResponse(int iCommunicateID, int iProtoID, byte[] bytes, int iStart, int len);
        Task<(byte[], int, int)> SendWithResponse(int iCommunicateID, int iProtoID, byte[] bytes, int iStart, int len);
        void AddProtocolHandler(int iProtoID, handler h);
    }
}
