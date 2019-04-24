using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NetWorkInterface
{
    public interface ISocketConnector
    {
        ISocket Connect(EndPoint endPoint);
        ISocket Connect(string host, int port);
    }
}
