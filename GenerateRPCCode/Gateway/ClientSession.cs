using GrainInterface;
using MyNetWork;
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

        public Guid SessionID { get; set; }

        public ClientSession(ISocketTask socket, Guid sessionID, IClusterClient clusterClient)
        {
            m_Socket = socket;
            SessionID = sessionID;

            socket.MessageEncoder = new MessageEncoder();
            socket.MessageDecoder = new MessageDecoder();
            socket.Startup();

            m_Socket.OnDisconnect += OnDisconnect;
            m_Socket.OnMessage += OnMessage;


            try
            {
                m_ClientSessionGrain = clusterClient.GetGrain<IClientSessionGrain>(sessionID);

                var obj = clusterClient.CreateObjectReference<IGatewayGrainObserver>(this).GetAwaiter().GetResult();
                m_ClientSessionGrain.Subscribe(obj).GetAwaiter().GetResult();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }


        }

        private void OnMessage(int iChunkType, int iProtocolID, int iCommunicateID, byte[] messageBuff, int start, int len)
        {
            m_ClientSessionGrain.Recv((ChunkType)iChunkType, iCommunicateID, iProtocolID, messageBuff, start, len);
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

        public void Send(int iProtocolID, int iCommunicateID, byte[] bytes, int start, int len)
        {
            m_Socket.Send(0, iCommunicateID, iProtocolID, delegate(byte[] sendBuffer, int offset)
            {
                Array.Copy(bytes, start, sendBuffer, offset, len);
                return (sendBuffer, offset, len);
            });
        }
    }
}
