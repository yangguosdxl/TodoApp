using System;
using System.Collections.Generic;
using System.Text;

namespace CSRPC
{
    public static partial class RpcServiceHelper
    {
        private static int s_iRpcServiceCount;

        public static int GetRpcServiceCount()
        {
            return s_iRpcServiceCount;
        }

        public static int GetID<T>()
        {
            return RpcServiceHelperHodler<T>.ID;
        }

        public static T Create<T>()
        {
            return RpcServiceHelperHodler<T>.CreateFunc();
        }
    }

    public class RpcServiceHelperHodler<T>
    {
        public static int ID;
        public static Func<T> CreateFunc;
    }
}
