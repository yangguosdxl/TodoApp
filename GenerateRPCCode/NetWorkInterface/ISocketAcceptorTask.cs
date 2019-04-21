
using System;
using System.Net;


namespace NetWorkInterface
{
    public interface ISocketAcceptorTask
    {
        void Startup();

        event Action<ISocketTask> OnNewConnection;
    }
}
