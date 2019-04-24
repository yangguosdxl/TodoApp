using GrainInterface;
using NetWorkInterface;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gateway
{
    class ClientSession : IGatewayGrainObserver
    {
        ISocketTask m_Socket;
        IClientSessionGrain m_ClientSessionGrain;
        IGatewayGrainObserver m_gatewayGrainObserver;

        public Guid SessionID { get; set; }

        public ClientSession(ISocketTask socket, Guid sessionID, IClusterClient clusterClient)
        {
            m_Socket = socket;
            SessionID = sessionID;

            m_Socket.OnDisconnect += OnDisconnect;
            m_Socket.OnMessage += OnMessage;

            m_ClientSessionGrain = clusterClient.GetGrain<IClientSessionGrain>(sessionID);

            var obj = clusterClient.CreateObjectReference<IGatewayGrainObserver>(this).GetAwaiter().GetResult();
            m_ClientSessionGrain.Subscribe(obj).GetAwaiter().GetResult();
        }

        private void OnMessage(int iProtocolID, int iCommunicateID, byte[] messageBuff, int start, int len)
        {
            m_ClientSessionGrain.Send(iCommunicateID, iProtocolID, messageBuff, start, len);
        }

        private void OnDisconnect()
        {
            Task.Run(ProcessDisconnectAsync);
        }

        private async Task ProcessDisconnectAsync()
        {
            await m_ClientSessionGrain.OnDisconnect();

            ClientSession session;
            SessionMgr.Inst.TryRemove(SessionID, out session);
        }

        public void Send(byte[] bytes, int start, int len)
        {
            m_Socket.Send(bytes, start, len);
        }
    }
}
