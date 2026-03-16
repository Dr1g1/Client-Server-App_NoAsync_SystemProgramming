using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    internal class Logger
    {
        static object logLock = new object();
        public static void Log(string message)
        {
            lock (logLock)
            {
                Console.WriteLine($"[{DateTime.Now}] {message}");
            }
        }
    }
}
