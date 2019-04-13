using System;
using System.Collections.Generic;
using System.Text;

namespace CoolRpcInterface
{
    public class DefaultRPCHandlerMap : IRPCHandlerMap
    {
        private handler[] m_Handlers;

        public DefaultRPCHandlerMap(int count)
        {
            m_Handlers = new handler[count];
        }

        public void Add(int id, handler h)
        {
            m_Handlers[id] = h;
        }

        public handler Get(int id)
        {
            return m_Handlers[id];
        }
    }
}
