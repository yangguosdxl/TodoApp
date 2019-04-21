using NetWorkInterface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyNetWork
{
    public class DefaultSocketAcceptor : ISocketAcceptorTask
    {
        CancellationTokenSource m_CancelTS = new CancellationTokenSource();

        public event Action<ISocketTask> OnNewConnection;

        ISocketAcceptor m_Acceptor;

        public DefaultSocketAcceptor(ISocketAcceptor acceptor)
        {
            m_Acceptor = acceptor;
        }

        public void Startup()
        {
            Task.Factory.StartNew(AcceptLoop, m_CancelTS.Token, m_CancelTS.Token, TaskCreationOptions.None, TaskScheduler.Default);
        }

        private async Task AcceptLoop(object state)
        {
            CancellationToken cancelTocken = (CancellationToken)state;
            while (true)
            {
                cancelTocken.ThrowIfCancellationRequested();

                ISocket c = await m_Acceptor.AcceptAsync();

                OnNewConnection?.Invoke(new DefaultSocket(c));
            }
        }
    }
}
