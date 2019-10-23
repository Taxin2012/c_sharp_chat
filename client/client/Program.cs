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
        static TcpClient client = null;
        static NetworkStream stream = null;
        static Byte[] bytes = new Byte[256];
        static Byte[] data = new Byte[256];

        static void Connect(String ipaddr)
        {
            try
            {
                client = new TcpClient(ipaddr, 13000);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                Console.ReadLine();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                Console.ReadLine();
            }

            stream = client.GetStream();
            Console.WriteLine("\n Connected to " + ipaddr + "!");

            Thread receiveThread = new Thread(new ThreadStart(GetMsgs));
            receiveThread.Start();

            while (true)
            {
                string Msg = Console.ReadLine();
                SendMsg(Msg);
            }
        }

        static void GetMsgs()
        {
            while (true)
            {
                try
                {
                    if (stream.CanRead)
                    {
                        String responseData = String.Empty;
                        int i;

                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            Int32 bytes = stream.Read(data, 0, data.Length);
                            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                            Console.WriteLine("Received: {0}", responseData);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!");
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        static void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
            Environment.Exit(0);
        }

        static void SendMsg(String Msg)
        {
            try
            {
                if ( stream.CanWrite )
                {
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(Msg);

                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Sent: {0}", Msg);
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                Console.ReadLine();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                Console.ReadLine();
            }
        }

        static void TryConnect()
        {
            Console.WriteLine("\nConnect to:\n");
            string line = Console.ReadLine();

            try
            {
                IPAddress address = IPAddress.Parse(line);
            }

            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException caught!!!");
                TryConnect();
            }

            catch (FormatException e)
            {
                Console.WriteLine("FormatException caught!!!");
                TryConnect();
            }

            catch (Exception e)
            {
                Console.WriteLine("Unknown Error!!!");
                TryConnect();
            }

            Connect(line);
        }

       static void Main(string[] args)
       {
            TryConnect();
       }
    }
}