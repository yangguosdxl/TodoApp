using System;
using System.Collections.Generic;
using System.Text;

namespace MyNetWork
{
    public class NetworkConfig
    {
        //public static bool IsLittleEndian = true;
        public const byte MESSAGE_HEAD_BYTES = 7;
        public const short MESSAGE_MAX_BYTES = 1024;
        public const short MESSAGE_BODY_BYTES = MESSAGE_MAX_BYTES - MESSAGE_HEAD_BYTES;
        
    }
}
