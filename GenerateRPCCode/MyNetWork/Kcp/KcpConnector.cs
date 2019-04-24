using NetWorkInterface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MyNetWork.Kcp
{
    class KcpConnector : ISocketConnector
    {
        public ISocket Connect(EndPoint endPoint)
        {
            throw new NotImplementedException();
        }

        public ISocket Connect(string host, int port)
        {
            throw new NotImplementedException();
        }
    }
}
