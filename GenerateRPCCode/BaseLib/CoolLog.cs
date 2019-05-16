using System;
using System.Collections.Generic;
using System.Text;

namespace Cool
{
    public class Logger
    {
        private static NLog.Logger Default = NLog.LogManager.GetCurrentClassLogger();

        public static void Debug(object o)
        {
            Default.Debug(o);
        }

        public static void Debug(string msg, params object[] args)
        {
            Default.Debug(msg, args);
        }

        public static void Debug(Exception err, string msg, params object[] args)
        {
            Default.Debug(err, msg, args);
        }

        public static void Info(object o)
        {
            Default.Info(o);
        }

        public static void Info(string msg, params object[] args)
        {
            Default.Info(msg, args);
        }

        public static void Info(Exception err, string msg, params object[] args)
        {
            Default.Info(err, msg, args);
        }

        public static void Trace(object o)
        {
            Default.Trace(o);
        }

        public static void Trace(string msg, params object[] args)
        {
            Default.Trace(msg, args);
        }

        public static void Trace(Exception err, string msg, params object[] args)
        {
            Default.Trace(err, msg, args);
        }

        public static void Warn(object o)
        {
            Default.Warn(o);
        }

        public static void Warn(string msg, params object[] args)
        {
            Default.Warn(msg, args);
        }

        public static void Warn(Exception err, string msg, params object[] args)
        {
            Default.Warn(err, msg, args);
        }

        public static void Error(object o)
        {
            Default.Error(o);
        }

        public static void Error(string msg, params object[] args)
        {
            Default.Error(msg, args);
        }

        public static void Error(Exception err, string msg, params object[] args)
        {
            Default.Error(err, msg, args);
        }

        public static void Fatal(object o)
        {
            Default.Fatal(o);
        }

        public static void Fatal(string msg, params object[] args)
        {
            Default.Fatal(msg, args);
        }

        public static void Fatal(Exception err, string msg, params object[] args)
        {
            Default.Fatal(err, msg, args);
        }
    }
}
