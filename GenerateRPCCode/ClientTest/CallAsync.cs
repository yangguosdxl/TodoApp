using CoolRpcInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest
{
    class CallAsync : ICallAsync
    {
        int rpcResponseIndex = 0;
        Dictionary<int, Action> m_MapRpcResponseProcessor = new Dictionary<int, Action>();

        public async Task SendWithoutResponse(byte[] bytes, int iStart, int len)
        {

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
    }

}
