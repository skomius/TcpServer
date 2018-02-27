using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpServer
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Iveskite direktorija (nenaudokite backslash \\ ): ");
            string path = Console.ReadLine();

            Console.WriteLine("Iveskite porto numeri: ");
            int port;
            int.TryParse(Console.ReadLine(), out port);


            new Thread(() => Server.StartServer(port)).Start();
            Thread.Sleep(500);
            new Thread(() => Client.StartClient(port, path)).Start();
        }
    }
}

