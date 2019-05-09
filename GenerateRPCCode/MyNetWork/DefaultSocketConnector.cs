using Cool;
using NetWorkInterface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MyNetWork
{
    public class DefaultSocketConnector
    {
        public ISocketTask Connect(EndPoint endPoint, NetType netType)
        {
            ISocket socket = null;

            try
            {
                ISocketConnector connector = null;
                if (netType == NetType.KCP)
                    connector = new Kcp.KcpConnector();
                else
                    connector = new Tcp.TcpConnector();

                socket = connector.Connect(endPoint);

                ISocketTask socketTask = new DefaultSocket(socket);

                return socketTask;
            }
            catch(SocketException e)
            {
                socket = null;
                CoolLog.WriteLine($"socket error code: {e.SocketErrorCode}. {e}");
            }
            catch(Exception e)
            {
                socket = null;
                CoolLog.WriteLine($"{e}");
            }

            return null;
        }

        public ISocketTask Connect(string host, int port, NetType netType)
        {
            ISocket socket = null;

            try
            {
                ISocketConnector connector = null;
                if (netType == NetType.KCP)
                    connector = new Kcp.KcpConnector();
                else
                    connector = new Tcp.TcpConnector();

                socket = connector.Connect(host, port);

                ISocketTask socketTask = new DefaultSocket(socket);

                return socketTask;
            }
            catch (SocketException e)
            {
                socket = null;
                CoolLog.WriteLine($"socket error code: {e.SocketErrorCode}. {e}");
            }
            catch (Exception e)
            {
                socket = null;
                CoolLog.WriteLine($"{e}");
            }

            return null;
        }
    }
}
