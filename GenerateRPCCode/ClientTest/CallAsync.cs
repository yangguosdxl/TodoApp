using CoolRpcInterface;
using CSRPC;
using MyNetWork;
using NetWorkInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest
{
    class CallAsync : ICallAsync
    {
        int m_iRpcCommuniationID = 0;
        Dictionary<int, TaskCompletionSource<(byte[], int, int)>> m_MapRpcResponseProcessor = new Dictionary<int, TaskCompletionSource<(byte[], int, int)>>();

        ISocketTask m_Socket;

        IMessageEncoder m_MessageCoder = new MessageEncoder();

        handler[] m_ProtocoHandlers = new handler[(int)ProtoID.COUNT]; 

        int GetNextRpcCommuniationID()
        {
            if (m_iRpcCommuniationID == int.MaxValue)
                m_iRpcCommuniationID = 0;
            return ++m_iRpcCommuniationID;
        }

        public CallAsync(string ip, int port, NetType netType)
        {
            DefaultSocketConnector socketConnector = new DefaultSocketConnector();
            m_Socket = socketConnector.Connect(ip, port, netType);

            if (m_Socket == null)
                throw new Exception($"failed connect to socket {ip}:{port}:{netType}");

            m_Socket.OnMessage += OnMessage;
            m_Socket.OnDisconnect += OnDisconnect;
        }

        private void OnDisconnect()
        {
            throw new NotImplementedException();
        }

        private void OnMessage(int iProtocolID, int iCommunicateID, byte[] messageBuff, int start, int len)
        {
            if (iCommunicateID != 0)
            {
                TaskCompletionSource<(byte[], int, int)> taskCompletionSource;
                if (m_MapRpcResponseProcessor.TryGetValue(iCommunicateID, out taskCompletionSource))
                {
                    taskCompletionSource.SetResult((messageBuff, start, len));
                }
                else
                {
                    Console.WriteLine($"Can not find communicate id's processor, {iCommunicateID}");
                }
            }
            else
            {
                if (iProtocolID >= 0 && iProtocolID < (int)ProtoID.COUNT)
                {
                    handler h = m_ProtocoHandlers[iProtocolID];
                    h(iCommunicateID, messageBuff, start, len);
                }
            }
        }

        public void AddProtocolHandler(int iProtoID, handler h)
        {
            m_ProtocoHandlers[iProtoID] = h;
        }

        public async Task SendWithoutResponse(int iChunkType, int iCommunicateID, int iProtoID, byte[] bytes, int iStart, int len)
        {
            (byte[] bytes2, int start2, int len2) = m_MessageCoder.Encode(iChunkType, iCommunicateID, iProtoID, bytes, iStart, len);
            m_Socket.Send(bytes2, start2, len2);
            await Task.CompletedTask;
        }

        public async Task<(byte[], int, int)> SendWithResponse(int iChunkType, int iProtoID, byte[] bytes, int iStart, int len)
        {
            int iCommunicateID = GetNextRpcCommuniationID();

            (byte[] bytes2, int start2, int len2) = m_MessageCoder.Encode(iChunkType, iCommunicateID, iProtoID, bytes, iStart, len);
            m_Socket.Send(bytes2, start2, len2);

            TaskCompletionSource<(byte[], int, int)> taskCompletionSource = new TaskCompletionSource<(byte[], int, int)>();

            m_MapRpcResponseProcessor.Add(iCommunicateID, taskCompletionSource);

            return await taskCompletionSource.Task;
        }
    }

}
