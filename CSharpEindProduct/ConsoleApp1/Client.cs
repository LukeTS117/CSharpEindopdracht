using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static ServerClient.ServerClient;

namespace Client
{
    class Client
    {

        private static string hostname = "192.168.178.117";
        private static int port = 1337;
        bool done;

        TcpClient client;

        static void Main(string[] args)
        {
            new Client();
        }

        public Client()
        {
            client = new TcpClient();
            client.Connect(hostname, port);
            NetworkStream nws = client.GetStream();

            Console.WriteLine("Connected to Server");


            Thread thread = new Thread(HandleRead);
            thread.Start(nws);
            done = false;
            while (!done)
            {
                string input = Console.ReadLine();
                SendTaggedMessage(nws, Tag.msg, input);
            }
        }

        public void HandleRead(object o)
        {
            NetworkStream nws = o as NetworkStream;
            if (nws == null)
                return;


            while (!done)
            {
                TaggedMessage msg = ReadTaggedMessage(nws);
                HandlePacket(msg);
            }
            

        }


        public static void HandlePacket(TaggedMessage taggedmsg)
        {
            switch (taggedmsg.tag)
            {
                case Tag.msg:
                    Console.WriteLine(taggedmsg.message);
                    break;
                default:
                    Console.WriteLine("Something went Wrong in the Tag...");
                    Console.WriteLine(taggedmsg.tag);
                    Console.WriteLine(taggedmsg.message);
                    break;
            }
        }

    }
}
