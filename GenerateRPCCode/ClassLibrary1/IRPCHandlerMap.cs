using System;
using System.Collections.Generic;
using System.Text;

namespace CoolRpcInterface
{
    public delegate void handler(int iCommunicateID, byte[] bytes, int iStartIndex, int iCount);

    public interface IRPCHandlerMap
    {
        //handler Get(int id);
    }
}
