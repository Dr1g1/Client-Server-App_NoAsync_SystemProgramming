using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    internal class WebServer
    {
        static HttpListener listener = new HttpListener();
        public void Start()
        {
            listener.Prefixes.Add("http://localhost:5050/");
            listener.Start();
            Console.WriteLine("Server pokrenut na http://localhost:5050/");

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                ThreadPool.QueueUserWorkItem(RequestHandler.ProcessRequest, context);
            }
        }
    }
}
