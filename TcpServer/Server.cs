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
            listener.Start(100);

            while (true)
            {
                clientConnected.Reset();
                //Console.WriteLine("Waiting for a connection...");

                listener.BeginAcceptSocket(
                    new AsyncCallback(Callback), listener);
                clientConnected.WaitOne();
            }

        }

        public static void Callback(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            Socket clientSocket = listener.EndAcceptSocket(ar);
            //Console.WriteLine("Client connected completed");

            clientConnected.Set();

            byte[] buffer = new byte[1024];
            int bytesRecieved = clientSocket.Receive(buffer);

            string path = Encoding.ASCII.GetString(
                buffer, 0, bytesRecieved);

            
            string[] files = Directory.GetFiles(path);
            StringBuilder names = new StringBuilder();
            foreach (string s in files)
            {
                names.Append(Regex.Match(s, @"\\(?:.(?!\\))+$").Value + '\n');
            }

            byte[] bytesToSend = Encoding.ASCII.GetBytes(names.ToString());
            clientSocket.Send(bytesToSend);

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();

        }
    }
}
