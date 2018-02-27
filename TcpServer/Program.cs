using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server");
            Console.WriteLine("Iveskite porto numeri: ");
            int port;
            int.TryParse(Console.ReadLine(), out port);

            new Thread(() => Server.StartServer(port)).Start();
            Thread.Sleep(500);

            Console.ReadKey();
        }
    }
}

