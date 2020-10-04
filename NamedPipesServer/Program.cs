using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NamedPipesServer
{
    class Program
    {
        static void Main(string[] args)
        {
            NamedPipeServer namedPipeServer = new NamedPipeServer();
            namedPipeServer.Received += NamedPipeServer_Received;
            namedPipeServer.Start();

            while (true)
            {
                Console.WriteLine("Main is Alive");
                Thread.Sleep(1000);
            }
        }

        private static void NamedPipeServer_Received(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine($"{sender} says {e.Data}");
        }
    }
}
