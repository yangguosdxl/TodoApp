using CoolRpcInterface;
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
        int rpcResponseIndex = 0;
        Dictionary<int, Action> m_MapRpcResponseProcessor = new Dictionary<int, Action>();

        ISocketTask m_Socket;

        public CallAsync(string ip, int port, NetType netType)
        {
            DefaultSocketConnector socketConnector = new DefaultSocketConnector();
            m_Socket = socketConnector.Connect(ip, port, netType);

            if (m_Socket == null)
                throw new Exception($"failed connect to socket {ip}:{port}:{netType}");
        }

        public void AddProtocolHandler(int iProtoID, handler h)
        {
            throw new NotImplementedException();
        }

        public Task SendWithoutResponse(int iCommunicateID, int iProtoID, byte[] bytes, int iStart, int len)
        {
            throw new NotImplementedException();
        }

        public async Task<(byte[], int, int)> SendWithResponse(byte[] bytes, int iStart, int len)
        {
            //Serializer serializer = new Serializer();

            //var msgIn = serializer.Deserialize<HelloIntMsgIn>(bytes, iStart, len);

            //Console.WriteLine("process msg: " + msgIn.eProtoID + ", " + msgIn.a);

            //HelloIntMsgOut ret = new HelloIntMsgOut();
            //ret.Value = (msgIn.a + 100, 2);

            //return await Task.FromResult(serializer.Serialize(ret));

            var ret = ((byte[])null, 0, 0);
            return await Task.FromResult(ret);
        }

        public Task<(byte[], int, int)> SendWithResponse(int iCommunicateID, int iProtoID, byte[] bytes, int iStart, int len)
        {
            throw new NotImplementedException();
        }
    }

}
