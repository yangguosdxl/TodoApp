
using System;
using System.Net;


namespace NetWorkInterface
{
    public interface ISocketAcceptorTask
    {
        void Startup();

        // 要求回调函数线程安全
        event Action<ISocketTask> OnNewConnection;
    }
}
