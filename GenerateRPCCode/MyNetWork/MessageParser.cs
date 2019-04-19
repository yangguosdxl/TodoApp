using CoolRpcInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNetWork
{
    public class MessageParser : IMessageParser
    {
        byte[] m_OneMessgeBuffer;
        int m_iMessageBufferLen;

        int m_iBodyLeftBytes = 0;
        int m_iProtocolID;
        int m_iResponseIndex = 0;

        public event Action<int, int, byte[], int, int> OnMessage;

        public MessageParser()
        {
            m_OneMessgeBuffer = new byte[NetworkConfig.MESSAGE_MAX_BYTES];
        }
        public void Process(byte[] buff, int start, int len)
        {
            while(len > 0)
            {
                if (m_iMessageBufferLen < NetworkConfig.MESSAGE_HEAD_BYTES)
                {
                    int left = NetworkConfig.MESSAGE_HEAD_BYTES - m_iMessageBufferLen;
                    int iCopyLen = Math.Min(len, left);
                    Array.Copy(buff, start, m_OneMessgeBuffer, m_iMessageBufferLen, iCopyLen);
                    m_iMessageBufferLen += iCopyLen;
                    len -= iCopyLen;
                    start += iCopyLen;

                    if (iCopyLen < left)
                        return;

                    // 可以解析MessageHeader了
                    unsafe
                    {
                        fixed (byte* bytes = m_OneMessgeBuffer)
                        {
                            m_iBodyLeftBytes = ReadUShortLittleEndian(bytes);
                            if (m_iBodyLeftBytes > NetworkConfig.MESSAGE_BODY_BYTES)
                                throw new ErrMessageBodyLenException();
                            m_iProtocolID = ReadUShortLittleEndian(bytes + 2);
                            m_iResponseIndex = ReadUShortLittleEndian(bytes + 2);
                        }
                    }
                }

                if (len == 0)
                    return;

                // 可以解析MessageBody了
                {
                    int iCopyLen = Math.Min(len, m_iBodyLeftBytes);
                    Array.Copy(buff, start, m_OneMessgeBuffer, m_iMessageBufferLen, iCopyLen);
                    start += iCopyLen;
                    len -= iCopyLen;
                    m_iMessageBufferLen += iCopyLen;
                    m_iBodyLeftBytes -= iCopyLen;

                    if (m_iBodyLeftBytes == 0)
                    { // 完整的协议已解析出来
                        OnMessage(m_iProtocolID, m_iResponseIndex, m_OneMessgeBuffer, NetworkConfig.MESSAGE_HEAD_BYTES, m_iMessageBufferLen);

                        // 清理
                        m_iMessageBufferLen = 0;
                        m_iProtocolID = 0;
                        m_iResponseIndex = 0;
                    }
                }
            }

        }

        private unsafe static ushort ReadUShortLittleEndian(byte* bytes)
        {
            unchecked
            {
                ushort value = 0;

                value = bytes[1];
                value <<= 8;
                value = bytes[0];

                return value;
            }

        }


    }
}
