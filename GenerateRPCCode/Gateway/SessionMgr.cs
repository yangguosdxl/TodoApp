using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace Gateway
{
    class SessionMgr : ConcurrentDictionary<Guid, ClientSession>
    {
        public static SessionMgr Inst = new SessionMgr();
    }
}
