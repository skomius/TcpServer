using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TCClient
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Client");
                Console.WriteLine("Iveskite direktorija : ");
                string path = Regex.Replace(Console.ReadLine(), @"\\", "\\\\");

                Console.WriteLine("Iveskite serverio porto numeri: ");
                int port;
                int.TryParse(Console.ReadLine(), out port);

                Client.StartClient(port, path);
            }
        }
    }
}
