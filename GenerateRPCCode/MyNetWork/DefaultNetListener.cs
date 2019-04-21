using MyNetWork.Kcp;
using MyNetWork.Tcp;
using NetWorkInterface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MyNetWork
{
    public enum NetType
    {
        KCP,
        TCP,
    }
    public class DefaultNetListener
    {
        ISocketAcceptorTask m_Acceptor;
        public void Init(EndPoint endPonit, NetType netType)
        {
            if (netType == NetType.KCP)
                m_Acceptor = new DefaultSocketAcceptor(new KcpAcceptor(endPonit));
            else if (netType == NetType.TCP)
                m_Acceptor = new DefaultSocketAcceptor(new TcpAcceptor(endPonit));
            else
                throw new Exception($"Unknow net type {netType}");

            m_Acceptor.OnNewConnection += OnNewConnection;
        }

        // 是可重入
        private void OnNewConnection(ISocketTask obj)
        {
            throw new NotImplementedException();
        }

        public void Startup()
        {
            m_Acceptor.Startup();
        }
    }
}
