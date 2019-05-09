using System;
using System.Collections.Generic;
using System.Text;

namespace Cool
{
    public class CoolLog
    {
        public static void WriteLine(string format)
        {
            //Console.WriteLine(format, args);
        }

        public static void WriteLine(string format, params object[] args)
        {
            //Console.WriteLine(format, args);
        }

        public static void WriteLine<T>(T o)
        {
            //Console.WriteLine(o);
        }
    }
}
