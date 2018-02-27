using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace TcpServer
{
    class Server
    {
        public static ManualResetEvent clientConnected =
        new ManualResetEvent(false);

        public static void StartServer(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start(1000);

            Worker worker = new Worker(mainWork);
            Console.WriteLine("\nServer is running\n");

            while (true)
            {
                Socket socket = listener.AcceptSocket();
                Console.WriteLine("Client connected");
                worker.EnqueueTask(socket);
            }

        }

        public static void mainWork(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];
            byte[] bytesToSend;
            string[] files = null;

            int bytesRecieved = clientSocket.Receive(buffer);

            string path = Encoding.ASCII.GetString(
                buffer, 0, bytesRecieved);

            try
            {
                files = Directory.GetFiles(path);
            }
            catch (DirectoryNotFoundException e)
            {
                bytesToSend = Encoding.ASCII.GetBytes(e.Message);
                clientSocket.Send(bytesToSend);
            }
            catch (UnauthorizedAccessException e)
            {
                bytesToSend = Encoding.ASCII.GetBytes(e.Message);
                clientSocket.Send(bytesToSend);
            }


            if (files != null)
            {
                StringBuilder names = new StringBuilder();
                foreach (string s in files)
                {
                    names.Append(Regex.Match(s, @"\\(?:.(?!\\))+$").Value + '\n');
                }

                bytesToSend = Encoding.ASCII.GetBytes(names.ToString());
                clientSocket.Send(bytesToSend);
            }

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
}
