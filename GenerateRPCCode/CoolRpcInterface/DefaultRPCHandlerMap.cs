using System;
using System.Collections.Generic;
using System.Text;

namespace CoolRpcInterface
{
    public class DefaultRPCHandlerMap : IRPCHandlerMap
    {
        private ProtocolHandler[] m_Handlers;

        public DefaultRPCHandlerMap(int count)
        {
            m_Handlers = new ProtocolHandler[count];
        }

        public void Add(int id, ProtocolHandler h)
        {
            m_Handlers[id] = h;
        }

        public ProtocolHandler Get(int id)
        {
            return m_Handlers[id];
        }
    }
}
