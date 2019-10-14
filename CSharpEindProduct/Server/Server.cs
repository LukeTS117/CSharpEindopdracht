using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static ServerClient.ServerClient;

namespace Server
{
    class Server
    {
        private TcpListener listener;
        private bool running;
        private static IPAddress ipAdress = IPAddress.Any;
        private static int port = 1337;
        private object _sync = new object();
        private int clientID;

        private ConcurrentDictionary<int, TcpClient> lobby;
        


        static void Main(string[] args)
        {
            new Server();
        }

        Server()
        {
            running = true;
            clientID = 0;

            lobby = new ConcurrentDictionary<int, TcpClient>();
            listener = new TcpListener(ipAdress, port);
            listener.Start();
            Console.WriteLine("Start Listening...");

            Thread t = new Thread(BroadcastThread);
            t.Start();

            while (running)
            {
                TcpClient client = listener.AcceptTcpClient();
                int key = this.GenerateClientID();
                lobby.TryAdd(key , client);

                Thread thread = new Thread(HandleTcpClient);
                thread.Start(lobby.GetValueOrDefault(key));
            }
            
        }

        private void BroadcastThread()
        {
            while (running)
            {
                string input = Console.ReadLine();
                BroadCast(input);
            }
            
        }

        private void HandleTcpClient(object obj)
        {
            
            TcpClient client = obj as TcpClient;
            if (client == null)
                return;

            Console.WriteLine("Client connected");

            NetworkStream nws = client.GetStream();
            SendTaggedMessage(nws, Tag.msg,"Testing... Attention please!");

            bool done = false;
            while (!done)
            {
                TaggedMessage recieved = ReadTaggedMessage(nws);
                this.HandleMessage(client, recieved);
            }


            client.Close();
            Console.WriteLine("Connection closed");
            nws.Close();
            Console.WriteLine("Networkstream closed");
        }

        public void HandleMessage(TcpClient client, TaggedMessage taggedmsg)
        {

            switch (taggedmsg.tag)
            {
                case Tag.msg:
                    Console.WriteLine(taggedmsg.tag);
                    Console.WriteLine(taggedmsg.message);
                    break;
                default:
                    Console.WriteLine("Something went Wrong in the Tag...");
                    Console.WriteLine(taggedmsg.tag);
                    break;
            }        
        }
        


        private int GenerateClientID()
        {
            if(clientID > 255) { clientID = 0; }
            bool notfound = lobby.ContainsKey(clientID);

            while (notfound)
            {
                notfound = lobby.ContainsKey(++clientID);
            }    

            return clientID++;
        }

        public void BroadCast(string input)
        {
            foreach(var entry in lobby)
            {
                TcpClient client = entry.Value as TcpClient;

                NetworkStream nws = client.GetStream();

                SendTaggedMessage(nws, Tag.msg, input);
            }
        }
       
    }
}



/* Data Protocol 
 * 
 *  From Client:                    param:
 *     <loc> = location             (float x, float y) 
 *     <dir> = direction            (byte)  
 *     <msg> = text message         (String)
 *     <bmb> = bomb placed          (byte x, byte y)
 *     <exp> = exploded             (byte) 
 *     <ded> = dead                 -
 */


/*
 * 
 * 
 *              ______                                 
 *             /     /                            ________________________________________________________________________________
 *            /     /                            /________________________________     __________________________________________/
 *           /     /   ______   ____           ____    _____   ______________    /    /  _____         _____   _________________
 *          /     /   /      \  \   \         /   /   /____/  /    _________/   /    /  /    /        /    /  /    ____________/
 *         /     /   /   __   \  \   \       /   /   _____   /    /            /    /  /    /        /    /  /    /
 *        /     /   /   /  \   \  \   \     /   /   /    /  /    /            /    /  /    /        /    /  /    /___________
 *       /     /   /   /____\   \  \   \   /   /   /    /  /    /            /    /  /    /        /    /  /____________    /
 *      /     /   /   ________   \  \   \_/   /   /    /  /    /            /    /  /    /        /    /               /   /
 *     /     /   /   /        \   \  \       /   /    /  /    /_________   /    /  /    /________/    /  _____________/   /
 *    /     /   /___/          \___\  \_____/   /____/  /______________/  /____/  /__________________/  /________________/
 *   /     /_________________________________________________________________________________________________________________
 *  /________________________________________________________________________________________________________________________/
 * 
*/

