using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpServer
{
    public class Worker: IDisposable
    {
        EventWaitHandle wh = new AutoResetEvent(false);
        Thread worker;
        readonly object locker = new object();
        Queue<Socket> tasks = new Queue<Socket>();

        public Worker(HardWork hard)
        {
            worker = new Thread(() => Work(hard));
            worker.Start();
        }

        public void EnqueueTask(Socket socket)
        {
            lock (locker) tasks.Enqueue(socket);
            wh.Set();
        }

        public void Dispose()
        {
            EnqueueTask(null);    
            worker.Join();         
            wh.Close();          
        }

        void Work(HardWork hard)
        {
            while (true)
            {
                Socket socket = null;
                lock (locker)
                    if (tasks.Count > 0)
                    {
                        socket = tasks.Dequeue();
                        if (socket == null) return;
                    }
                if (socket != null)
                {
                    hard(socket);  
                }
                else
                    wh.WaitOne();      
            }
        }

        public delegate void HardWork(Socket socket);
    }
}
