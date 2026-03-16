using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;

namespace ServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServer server = new WebServer();
            server.Start();
        }
    }
}