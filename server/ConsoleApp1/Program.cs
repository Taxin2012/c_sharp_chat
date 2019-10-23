using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace server
{
    class Program
    {
        static TcpListener server = null;
        static Byte[] bytes = new Byte[256];
        static String data = null;

        static void Host()
        {
            try
            {
                server = new TcpListener( IPAddress.Parse("127.0.0.1"), 13000 );
                server.Start();

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    NetworkStream stream = client.GetStream();

                    int i;

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        static void ReadMsg()
        {

        }

        static void Main(string[] args)
        {
            Console.Write("Waiting for a connection... ");

            Thread clientThread = new Thread(new ThreadStart(Host));
            clientThread.Start();
        }
    }
}
