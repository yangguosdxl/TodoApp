using Cool.Coroutine;
using CoolRpcInterface;

namespace NetWorkInterface
{
    public class WaitMessageCompleteTask : WaitCompleteTask<IMessage>
    {
        public int ProtoIDRet { get; set; }
        public WaitMessageCompleteTask(int iID, int iProtoIDRet) : base(iID)
        {
            ProtoIDRet = iProtoIDRet;
        }
    }
}
