using System;
using System.Threading.Tasks;

namespace NetWorkInterface
{
    public interface IChannel
    {
        void Start();
        void Send(byte[] buffer, int start, int len);

        event Action ConnectedEvent;
        event Action DisconnectedEvent;
    }
}
