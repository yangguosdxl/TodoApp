using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetWorkInterface
{
    public interface ISocketTask
    {
        void Startup();
        void Send(int iChunkType, int iCommunicateID, int iProtoID, Func<byte[], int, (byte[], int, int)> f);

        IMessageDecoder MessageDecoder { get; set; }
        IMessageEncoder MessageEncoder { get; set; }

        event Action OnDisconnect;
        // int iChunkType, int iProtocolID, int iCommunicateID, byte[] messageBuff, int start, int len
        event Action<int, int, int, byte[], int,int> OnMessage;
    }
}
