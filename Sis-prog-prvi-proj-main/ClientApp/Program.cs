using System;
using System.Threading;

class ClientApp
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Unesite naziv fajla (npr. fajl.txt) ili 'exit' za izlaz:");
            string fileName = Console.ReadLine();

            if (fileName.ToLower() == "exit")
                break;

            Thread thread = new Thread(() => SendRequest(fileName));
            thread.Start();
            thread.Join();
        }
    }
    static void SendRequest(string fileName)
    {
        string url = $"http://localhost:5050/{fileName}";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result; 
                string responseBody = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Odgovor za '{fileName}':");
                Console.WriteLine(responseBody);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Greška: {ex.Message}");
        }
    }

}