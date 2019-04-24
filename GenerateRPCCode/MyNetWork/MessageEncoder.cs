
using NetWorkInterface;
using System;
using System.Diagnostics.Contracts;

namespace MyNetWork
{
    public class MessageEncoder : IMessageEncoder
    {

        public MessageEncoder()
        {

        }

        public (byte[] buff, int start, int len) Encode(int iChunkType, int iCommunicateID, int iProtoID, byte[] messageBytes, int iStart, int len)
        {
            Contract.Requires(iStart >= NetworkConfig.MESSAGE_HEAD_BYTES);
            unsafe
            {
                fixed (byte* bytes = messageBytes)
                {
                    WriteUShortLittleEndian(bytes+iStart-NetworkConfig.MESSAGE_HEAD_BYTES, (ushort)len);
                    WriteUShortLittleEndian(bytes + iStart - NetworkConfig.MESSAGE_HEAD_BYTES + 2, (ushort)iProtoID);
                    WriteUShortLittleEndian(bytes + iStart - NetworkConfig.MESSAGE_HEAD_BYTES + 4, (ushort)iCommunicateID);
                    bytes[iStart - NetworkConfig.MESSAGE_HEAD_BYTES + 6] = (byte)iChunkType;
                }
            }

            return (messageBytes, iStart - NetworkConfig.MESSAGE_HEAD_BYTES, len + NetworkConfig.MESSAGE_HEAD_BYTES);
        }

        private unsafe static void WriteUShortLittleEndian(byte* bytes, ushort value)
        {
            unchecked
            {
                bytes[0] = (byte)value;
                bytes[1] = (byte)(value >> 8);
            }

        }
    }
}
