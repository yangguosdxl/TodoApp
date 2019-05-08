using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyNetWork
{
    public static class NetHelper
    {
        // 表示要透传的CommunicateID
        public static int ConvertToRequestCommunicateID(int id)
        {
            Contract.Requires(id <= 0x7FFF && IsValidCommunicateID(id));

            id = id | (0x1 << 15);
            return id;
        }

        public static int ConvertToResponseCommunicateID(int id)
        {
            Contract.Requires(IsValidCommunicateID(id));

            id = id & 0x7FFF;
            return id;
        }

        public static bool IsResponseCommunicateID(int id)
        {
            Contract.Requires(IsValidCommunicateID(id));

            return (id & (0x1 << 15)) == 0;
        }

        public static bool IsValidCommunicateID(int id)
        {
            return id > 0 && id <= 0xFFFF;
        }
    }
}
