using System;
using System.Threading.Tasks;

namespace NetWorkInterface
{
    public interface IChannel
    {
        void Start();


        Task SendAsync(byte[] buff, int start, int len);
        Task<(byte[], int, int)> RecvAsync();

        void Send(byte[] buff, int start, int len);
        (byte[], int, int) Recv();

        event Action ConnectedEvent;
        event Action DisconnectedEvent;
    }
}
