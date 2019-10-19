using ServerClient;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using static ServerClient.ServerClient;

namespace LibClient
{
    public class Client
    {
        private static string hostname = "192.168.178.117";
        private static int port = 1337;
        bool done;
        public string Username { get; set; }
        TcpClient client;

        public Client(string username)
        {
            client = new TcpClient();
            client.Connect(hostname, port);
            NetworkStream nws = client.GetStream();
            Username = username;

            //Console.WriteLine("Connected to Server");


            Thread thread = new Thread(HandleRead);
            thread.Start(nws);
            done = false;
            SendTaggedMessage(nws, Tag.sun, Username);
        }

        public async void HandleRead(object o)
        {
            NetworkStream nws = o as NetworkStream;
            if (nws == null)
                return;


            while (!done)
            {
                Task<TaggedMessage> read = ReadTaggedMessageAsync(nws);
                TaggedMessage msg = await read;
                HandlePacket(msg);
            }


        }


        private static void HandlePacket(TaggedMessage taggedmsg)
        {
            switch (taggedmsg.tag)
            {
                case Tag.msg:
                   // Console.WriteLine(taggedmsg.message);
                    break;
                case Tag.mts:

                default:
                    Console.WriteLine("Something went Wrong in the Tag...");
                    Console.WriteLine(taggedmsg.tag);
                    Console.WriteLine(taggedmsg.message);
                    break;
            }
        }

        public void SendMessage(Tag tag, string message)
        {
            NetworkStream nws = this.client.GetStream();
            SendTaggedMessage(nws, tag, message);

        }

        
    }
}
