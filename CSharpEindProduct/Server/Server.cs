using ServerClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private int sessionID;
        private int maxPlayers;

        private ConcurrentDictionary<int, Player> lobby;
        private ConcurrentDictionary<int, Session> sessions;

        public class Session
        {
           private ConcurrentDictionary<int, Player> _session;
           public int iPlayers;
           public int SessionID;

            public Session(int sessionID)
            {
                _session = new ConcurrentDictionary<int, Player>();
                iPlayers = 0;
                SessionID = sessionID;
            }

            public void AddPlayer(ConcurrentDictionary<int, Player> currentlobby, Player player)
            {
                if (this._session.TryAdd(player.ID, player))
                {
                    Player outPlayer;
                    currentlobby.TryRemove(player.ID, out outPlayer);
                    SendTaggedMessage(player.Client.GetStream(), Tag.mts, SessionID.ToString());
                    ++iPlayers;
                };
            }

            public Player RemovePlayer(int playerID)
            {
                Player player;
                this._session.TryRemove(playerID, out player);
                --iPlayers;
                return player;
            }

            public void NotifyList()
            {

            }
        }

        public class Player
        {
            public TcpClient Client { get; }
            public int ID { get; }
            public string UserName { get; set; }

            public Player(TcpClient client, int iD)
            {
                Client = client;
                ID = iD;
            }
        }

        static void Main(string[] args)
        {
            new Server();
        }

        Server()
        {
            running = true;
            clientID = 0;
            sessionID = 0;
            maxPlayers = 4;

            lobby = new ConcurrentDictionary<int, Player>();
            listener = new TcpListener(ipAdress, port);
            listener.Start();
            Console.WriteLine("Start Listening...");

            //Thread t = new Thread(BroadcastThread);
            //t.Start();

            while (running)
            {
                TcpClient client = listener.AcceptTcpClient();
                int key = this.GenerateClientID();

                Player player = new Player(client, key);
                lobby.TryAdd(key , player);

                Thread thread = new Thread(HandlePlayerClient);
                thread.Start(lobby.GetValueOrDefault(key));
            }
            
        }

        //private void BroadcastThread()
        //{
        //    while (running)
        //    {
        //        string input = Console.ReadLine();
        //        BroadCast(input);
        //    }
            
        //}

        private async void HandlePlayerClient(object obj)
        {

            Player player = obj as Player;
            if (player == null)
                return;

            Console.WriteLine($"<{player.ID}>Client connected");

            NetworkStream nws = player.Client.GetStream();
            SendTaggedMessage(nws, Tag.msg,"Testing... Attention please!");

            bool done = false;
            while (!done)
            {
                Task<TaggedMessage> read = ReadTaggedMessageAsync(nws);
                TaggedMessage recieved = await read;
                this.HandleMessage(player, recieved);

            }


            player.Client.Close();
            Console.WriteLine("Connection closed");
            nws.Close();
            Console.WriteLine("Networkstream closed");
        }

        public void HandleMessage(Player player, TaggedMessage taggedmsg)
        {

            switch (taggedmsg.tag)
            {
                case Tag.msg:
                    Console.WriteLine($"<{player.ID}>" + taggedmsg.tag);
                    Console.WriteLine($"<{player.ID}>" + taggedmsg.message);
                    break;
                case Tag.sun:
                    player.UserName = taggedmsg.message;
                    break;
                case Tag.cns:
                    int newSessionID = ++sessionID;
                    Session newSession = new Session(newSessionID);
                    newSession.AddPlayer(lobby, player);
                    sessions.TryAdd(newSessionID, newSession);
                    break;
                case Tag.jas:
                    break;
                default:
                    Console.WriteLine($"<{player.ID}>" + "Something went Wrong in the Tag...");
                    Console.WriteLine($"<{player.ID}>" + taggedmsg.tag);
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

        public void BroadCast(ConcurrentDictionary<int, Player> players, Tag tag, string message)
        {
            foreach(var entry in players)
            {
                Player player = entry.Value as Player;

                NetworkStream nws = player.Client.GetStream();

                SendTaggedMessage(nws, tag, message);
            }
        }
       
    }
}



/* Data Protocol 
 * 
 *  From Client:                    param:
 *     <loc> = location                 (float x, float y) 
 *     <dir> = direction                (byte)  
 *     <msg> = text message             (String)
 *     <bmb> = bomb placed              (byte x, byte y)
 *     <exp> = exploded                 (byte) 
 *     <ded> = dead                     -
 *     <sun> = set username             string
 *     <cns> = creat new session        -
 *     <jas> = join available session   int
 *     
 *  From Server:                    param:
 *     <nsl> = notify session list      (int sessionid, int playeramount)
 *     <npj> = notify player join       (int, string)
 *     <npl> = notify player left       (int)
 */


/*
 * 
 * 
 *              ______                                 
 *             /     /                           _________________________________________________________________________________
 *            /     /                           /_________________________________     __________________________________________/
 *           /     /   ______   ____           ____    _____   ______________    /    /  _____         _____   _________________
 *          /     /   /      \  \   \         /   /   /____/  /    _________/   /    /  /    /        /    /  /    ____________/
 *         /     /   /   __   \  \   \       /   /   _____   /    /            /    /  /    /        /    /  /    /
 *        /     /   /   /  \   \  \   \     /   /   /    /  /    /            /    /  /    /        /    /  /    /___________
 *       /     /   /   /____\   \  \   \   /   /   /    /  /    /            /    /  /    /        /    /  /____________    /
 *      /     /   /   ________   \  \   \_/   /   /    /  /    /            /    /  /    /        /    /               /   /
 *     /     /   /   /        \   \  \       /   /    /  /    /_________   /    /  /    /________/    /  _____________/   /
 *    /     /   /___/          \___\  \_____/   /____/  /______________/  /____/  /__________________/  /________________/
 *   /     /____________________________________________________________________________________________________________
 *  /__________________________________________________________________________________________________________________/
 * 
*/

