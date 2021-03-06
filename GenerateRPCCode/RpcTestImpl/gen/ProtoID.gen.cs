using System;
using Cool.Interface.Rpc;
using System.Threading.Tasks;
using Cool.Coroutine;
using RpcTestInterface;

namespace CSRPC
{
    internal enum ProtoID
    {
        EICHelloService_Hello_MsgIn,
        EICHelloService_HelloInt_MsgIn,
        EICHelloService_HelloInt_MsgOut,
        EICHelloService_Hello2_MsgIn,
        EICHelloService_Hello3_MsgIn,
        EICHelloService_Hello3_MsgOut,
        EISHelloService_Hello_MsgIn,
        EISHelloService_HelloInt_MsgIn,
        EISHelloService_HelloInt_MsgOut,
        EISHelloService_Hello2_MsgIn,
        EISHelloService_Hello3_MsgIn,
        EISHelloService_Hello3_MsgOut,
        COUNT
    }
}
