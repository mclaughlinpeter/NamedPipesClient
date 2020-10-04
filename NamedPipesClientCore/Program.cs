using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace NamedPipesClientCore
{
    class Program
    {
        static void Main(string[] args)
        {
            NamedPipeClient namedPipeClient1 = new NamedPipeClient();
            namedPipeClient1.Send("Hello There 1");
            Thread.Sleep(1000);
            namedPipeClient1.Send("Hello There 2");
            Thread.Sleep(5000);
            namedPipeClient1.Send("Hello There 3");

            Console.ReadLine();
        }
    }

    public interface IIpcClient
    {
        void Send(string data);
    }

    public class NamedPipeClient : IIpcClient
    {
        public void Send(string data)
        {
            using (var client = new NamedPipeClientStream(".", typeof(IIpcClient).Name, PipeDirection.Out))
            {
                client.Connect();

                using (var writer = new StreamWriter(client))
                {
                    writer.WriteLine(data);
                }
            }
        }
    }
}
