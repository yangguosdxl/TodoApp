using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NetWorkInterface
{
    public interface ISocketConnectorTask
    {
        ISocketTask Connect(EndPoint endPoint);
    }
}
