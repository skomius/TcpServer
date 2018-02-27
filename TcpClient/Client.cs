using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCClient
{
    class Client
    {
        public static void StartClient(int portToConnect, string directoryPath)
        {
            try
            {
                using (TcpClient client = new TcpClient("localhost", portToConnect))
                {
                    Socket socket = client.Client;

                    byte[] path = Encoding.ASCII.GetBytes(directoryPath);
                    socket.Send(path);

                    int bytesRecieved = 0;
                    byte[] buffer = new byte[1024];
                    string stringRecieved;
                    StringBuilder content = new StringBuilder();

                    do
                    {
                        bytesRecieved = socket.Receive(buffer);
                        stringRecieved = Encoding.ASCII.GetString(buffer, 0, bytesRecieved);
                        content.Append(stringRecieved);
                    }
                    while (bytesRecieved != 0);

                    Console.WriteLine("\nAtsakymas " + Thread.CurrentThread.ManagedThreadId +
                           '\n' + content.ToString());
                }
            }
            catch(SocketException e)
            {
                Console.WriteLine("\nSeveris isjungtas bandykit veliau\n");
            }

        }
    }
}
