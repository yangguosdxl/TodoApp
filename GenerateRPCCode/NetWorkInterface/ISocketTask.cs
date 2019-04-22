﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetWorkInterface
{
    public interface ISocketTask
    {
        void Startup();
        void Send(byte[] buffer, int start, int len);

        event Action OnDisconnect;
        // int iProtocolID, int iCommunicateID, byte[] messageBuff, int start, int len
        event Action<int, int, byte[], int,int> OnMessage;
    }
}
