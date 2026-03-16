using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    internal class RequestHandler
    {
        static ConcurrentDictionary<string, string> cache = new ConcurrentDictionary<string, string>();
        static string rootFolder = @"D:\SisProjApp1\Sis-prog-prvi-proj\ServerApp";
        public static void ProcessRequest(object state)
        {
            var context = (HttpListenerContext)state;
            string filename = context.Request.Url.AbsolutePath.TrimStart('/');
            Logger.Log($"Zahtev za fajlom {filename}");
            if (cache.ContainsKey(filename))
            {
                Logger.Log($"Kesiran odgovor: {filename}");
                SendResponse(context, cache[filename]);
                return;
            }
            string filePath = FindFile(rootFolder, filename);
            if (filePath == null)
            {
                Logger.Log($"Fajl nije pronađen: {filename}");
                SendError(context, 404, "Fajl nije pronađen");
                return;
            }
            try
            {
                int count = CountWords(filePath);
                string response = $"Broj reci koje pocinju velikim slovom, a duze su od 5 slova su: {count}";
                cache[filename] = response;
                Logger.Log($"Obradjen fajl: {filename} | Reci: {count}");
                SendResponse(context, response);
            }
            catch (Exception e)
            {
                Logger.Log($"Doslo je do greske {e.Message}");
                SendError(context, 500, "Greska u obradi");
            }
        }
        static string FindFile(string root, string targetFile)
        {
            foreach (var file in Directory.GetFiles(root, "*", SearchOption.AllDirectories))
            {
                if (Path.GetFileName(file).Equals(targetFile, StringComparison.OrdinalIgnoreCase))
                    return file;
            }
            return null;
        }
        static int CountWords(string filePath)
        {
            int count = 0;
            string[] words = File.ReadAllText(filePath).Split(new[] { ' ', '\n', '\r', '\t', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (word.Length > 5 && char.IsUpper(word[0]))
                    count++;
            }
            return count;
        }

        static void SendResponse(HttpListenerContext context, string message)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }
        static void SendError(HttpListenerContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            SendResponse(context, message);
        }
    }
}


