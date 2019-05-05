﻿using Cool.Coroutine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoolRpcInterface
{
    public interface ICallAsync
    {
        void SendWithoutResponse(int iChunkType, int iCommunicateID, int iProtoID, byte[] bytes, int iStart, int len);
        Task<(byte[], int, int)> SendWithResponse(int iChunkType, int iProtoID, byte[] bytes, int iStart, int len);

        void SendWithoutResponse(int iChunkType, int iCommunicateID, int iProtoID, Func<(byte[],int,int)> action);
        MyTask<T> SendWithResponse<T>(int iChunkType, int iProtoID, Func<(byte[], int, int)> action);

        void AddProtocolHandler(int iProtoID, handler h);
        void AddDeserializeFunc(int iProtoID, DeserializeFunc deserializer);
    }
}
