using Cool.Coroutine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoolRpcInterface
{
    public interface ICallAsync
    {
        void SendWithoutResponse(int iChunkType, int iCommunicateID, int iProtoID, byte[] bytes, int iStart, int len);
        Task<(byte[], int, int)> SendWithResponse(int iChunkType, int iProtoID, byte[] bytes, int iStart, int len);

        void SendWithoutResponse(int iChunkType, int iCommunicateID, int iProtoID, Func<byte[], int, ValueTuple<byte[], int, int>> action);
        MyTask<IMessage> SendWithResponse(int iChunkType, int iProtoID, Func<byte[], int, ValueTuple<byte[], int, int>> action);

        void AddProtocolHandler(int iProtoID, ProtocolHandler h);
        void AddProtocolDeserializer(int iProtoID, ProtocolDeserializer deserializer);

        void Update();
        void OnMessage(int iChunkType, int iProtocolID, int iCommunicateID, byte[] messageBuff, int start, int len);

    }
}
