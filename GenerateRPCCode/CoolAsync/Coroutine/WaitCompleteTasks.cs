
namespace Cool.Coroutine
{

    public class WaitCompleteTasks
    {
        private MyTask[] m_aTasks;
        private int m_iNextFreeID;

        public WaitCompleteTasks(int iCapacity)
        {
            m_aTasks = new MyTask[iCapacity];
        }

        int GetNextFreeID()
        {
            do
            {
                ++m_iNextFreeID;
                if (m_iNextFreeID >= m_aTasks.Length)
                {
                    m_iNextFreeID = 0;
                }
            }
            while (m_aTasks[m_iNextFreeID] != null);
            return m_iNextFreeID;
        }

        public WaitCompleteTask<T> WaitComplete<T>()
        {
            int iNextFreeID = GetNextFreeID();
            WaitCompleteTask<T> myTask = new WaitCompleteTask<T>(iNextFreeID);
            
            m_aTasks[iNextFreeID] = myTask;

            return myTask;
        }

        public void OnComplete<T>(int iID, ref T result)
        {
            WaitCompleteTask<T> task = m_aTasks[iID] as WaitCompleteTask<T>;
            if (task == null)
                return;

            m_aTasks[iID] = null;
            task.SetResult(ref result);
        }

    }
}
