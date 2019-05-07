using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CSCommon
{
    public static class TPLExterns
    {
        public static void LogExceptions(this Task task)
        {
            task.ContinueWith(t =>
            {
                var aggException = t.Exception.Flatten();
                foreach (var exception in aggException.InnerExceptions)
                {
                    Console.WriteLine(exception);
                }
            },
            TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
