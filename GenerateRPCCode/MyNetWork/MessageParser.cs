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
        int m_iHeaderLeftBytes = 6;
        int m_iProtocolID;
        uint m_iResponseIndex = 0;
        bool m_bIsReadHead = true;

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
                    Array.Copy(buff, m_OneMessgeBuffer, iCopyLen);
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




                if (m_iLeftBytes != 0)
                {

                }

                if (m_iLeftBytes == 0)
                { // 下一个协议开始
                    unsafe
                    {
                        fixed(byte* bytes = seg.Array)
                        {
                            m_iLeftBytes = ReadLittleEndian(seg.Array);
                        }
                        
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
