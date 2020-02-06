using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Misc
{
    public class LogStatics
    {
        static public Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();
        static public void Trace(string message)
        {
            Logger.Trace(message);
        }
        static public void Trace(string message, params object[] args)
        {
            Logger.Trace(message, args);
        }

        static public void Debug(string message)
        {
            Logger.Debug(message);
        }
        static public void Debug(string message, params object[] args)
        {
            Logger.Debug(message, args);
        }

        static public void Info(string message)
        {
            Logger.Info(message);
        }
        static public void Info(string message, params object[] args)
        {
            Logger.Info(message, args);
        }

        static public void Warn(string message)
        {
            Logger.Warn(message);
        }
        static public void Warn(string message, params object[] args)
        {
            Logger.Warn(message, args);
        }

        static public void Error(string message)
        {
            Logger.Error(message);
        }
        static public void Error(Exception exception, string message)
        {
            Logger.Error(exception, message);
        }
        static public void Error(string message, params object[] args)
        {
            Logger.Error(message, args);
        }
        static public void Error(Exception exception, string message, params object[] args)
        {
            Logger.Error(exception, message, args);
        }

        static public void Fatal(string message)
        {
            Logger.Fatal(message);
        }
        static public void Fatal(Exception exception, string message)
        {
            Logger.Fatal(exception, message);
        }
        static public void Fatal(string message, params object[] args)
        {
            Logger.Fatal(message, args);
        }
        static public void Fatal(Exception exception, string message, params object[] args)
        {
            Logger.Fatal(exception, message, args);
        }
    }
}
