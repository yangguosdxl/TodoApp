using Cool.Coroutine.CompilerServices;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;


namespace Cool.Coroutine
{
    [AsyncMethodBuilder(typeof(AsyncUniTaskMethodBuilder))]
    public class WaitCompleteTask<T> : MyTask<T>
    {
        WaitCompleteTasks m_WaitCompleteTasks;
        public int ID { get; set; }

        public WaitCompleteTask(int iID)
        {
            ID = iID;
        }

        public new void SetCancel()
        {
            Contract.Requires(false, "Can not cancel this task");
        }
    }
}
