using Cool.Coroutine;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoolRpcInterface
{
    public delegate MyTask ProtocolHandler(int iCommunicateID, IMessage _msg);

    public delegate IMessage ProtocolDeserializer(byte[] bytes, int iStartIndex, int iCount);

    public interface IRPCHandlerMap
    {
        //handler Get(int id);
    }
}
