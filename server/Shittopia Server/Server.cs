
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Shittopia_Server
{
    internal class Server
    {
        public static string version = "PBT5";
        public static string itemsTextureURL = "https://cdn.discordapp.com/attachments/480307512713805824/947621167089455154/unknown.png";
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public static Dictionary<int, World> worlds = new Dictionary<int, World>();
        public static Dictionary<int, Server.PacketHandler> packetHandlers;
        public static string[] badWords = new string[15]
        {
      "fuck",
      "f*ck",
      "fvck",
      "dick",
      "bastard",
      "asshole",
      "bitch",
      "nigga",
      "nigge",
      "nigg4",
      "niggerz",
      "pussy",
      "penis",
      "motherfucker",
      "shitass"
        };
        public static string path = "C:\\CubixWorlds\\";
        public static string popularWorld = "START";
        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        public static int MaxPlayers { get; private set; }

        public static int MaxWorlds { get; private set; }

        public static int Port { get; private set; }

        public static void Start(int _maxPlayers, int _maxWorlds, int _port)
        {
            Server.MaxPlayers = _maxPlayers;
            Server.MaxWorlds = _maxWorlds;
            Server.Port = _port;
            Economy.Load();
            Server.Log("Starting the server...", ConsoleColor.Yellow);
            Server.InitializeServerData();
            Server.tcpListener = new TcpListener(IPAddress.Any, Server.Port);
            Server.tcpListener.Start();
            new Thread((ThreadStart)(() => Server.tcpListener.BeginAcceptTcpClient(new AsyncCallback(Server.TCPConnectCallback), (object)null))).Start();
            Server.udpListener = new UdpClient(Server.Port);
            new Thread((ThreadStart)(() => Server.udpListener.BeginReceive(new AsyncCallback(Server.UDPReceiveCallback), (object)null))).Start();
            Server.Log(string.Format("Server started on port {0}.", (object)Server.Port), ConsoleColor.Green);
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _socket = Server.tcpListener.EndAcceptTcpClient(_result);
            Server.tcpListener.BeginAcceptTcpClient(new AsyncCallback(Server.TCPConnectCallback), (object)null);
            Server.Log(string.Format("Incoming connection from {0}...", (object)_socket.Client.RemoteEndPoint), ConsoleColor.Yellow);
            if (!Security.ClientConnect(_socket.Client.RemoteEndPoint.ToString().Split(":")[0]))
                Server.Log("An error occured", _socket.Client.RemoteEndPoint.ToString() + " has tried more than 3 connections.");
            else if (Security.CheckLoadBalancer(_socket.Client.RemoteEndPoint.ToString().Split(":")[0]))
            {
                Server.Log("Error", "Client got refused by Load Balancer");
            }
            else
            {
                for (int key = 1; key <= Server.MaxPlayers; ++key)
                {
                    if (Server.clients[key].tcp.socket == null)
                    {
                        Server.clients[key].tcp.Connect(_socket);
                        return;
                    }
                }
                Security.ClientDisconnect(_socket.Client.RemoteEndPoint.ToString().Split(":")[0]);
                Server.Log(string.Format("{0} failed to connect:", (object)_socket.Client.RemoteEndPoint), "Server is full!");
            }
        }

        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = Server.udpListener.EndReceive(_result, ref remoteEP);
                Server.udpListener.BeginReceive(new AsyncCallback(Server.UDPReceiveCallback), (object)null);
                if (_data.Length < 4)
                    return;
                using (Packet _packetData = new Packet(_data))
                {
                    int key = _packetData.ReadInt();
                    if (key == 0)
                        return;
                    if (Server.clients[key].udp.endPoint == null)
                    {
                        Server.clients[key].udp.Connect(remoteEP);
                    }
                    else
                    {
                        if (!(Server.clients[key].udp.endPoint.ToString() == remoteEP.ToString()))
                            return;
                        Server.clients[key].udp.HandleData(_packetData);
                    }
                }
            }
            catch (Exception ex)
            {
                Server.Log("Error receiving UDP data:", ex.ToString());
            }
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint == null)
                    return;
                Server.udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, (AsyncCallback)null, (object)null);
            }
            catch (Exception ex)
            {
                Server.Log(string.Format("Error sending data to {0} via UDP:", (object)_clientEndPoint), ex.ToString());
            }
        }

        private static void InitializeServerData()
        {
            for (int index = 1; index <= Server.MaxPlayers; ++index)
                Server.clients.Add(index, new Client(index));
            for (int index = 1; index <= Server.MaxWorlds; ++index)
            {
                Server.worlds.Add(index, new World(index));
                Server.worlds[index].name = (string)null;
            }
            Server.packetHandlers = new Dictionary<int, Server.PacketHandler>()
      {
        {
          1,
          new Server.PacketHandler(ServerHandle.WelcomeReceived)
        },
        {
          2,
          new Server.PacketHandler(ServerHandle.PlayerMovement)
        },
        {
          3,
          new Server.PacketHandler(ServerHandle.EditWorldData)
        },
        {
          4,
          new Server.PacketHandler(ServerHandle.TakeDroppedItem)
        },
        {
          5,
          new Server.PacketHandler(ServerHandle.GetChatMessage)
        },
        {
          6,
          new Server.PacketHandler(ServerHandle.AuthUser)
        },
        {
          7,
          new Server.PacketHandler(ServerHandle.ItemAction)
        },
        {
          8,
          new Server.PacketHandler(ServerHandle.LeaveFromWorld)
        },
        {
          9,
          new Server.PacketHandler(ServerHandle.BuyPack)
        },
        {
          10,
          new Server.PacketHandler(ServerHandle.ButtonClick)
        },
        {
          12,
          new Server.PacketHandler(ServerHandle.TradeAction)
        },
        {
          11,
          new Server.PacketHandler(ServerHandle.AddItemToTrade)
        },
        {
          13,
          new Server.PacketHandler(ServerHandle.PlayerSize)
        },
        {
          14,
          new Server.PacketHandler(ServerHandle.EditSignText)
        },
        {
          15,
          new Server.PacketHandler(ServerHandle.WearCostume)
        },
        {
          16,
          new Server.PacketHandler(ServerHandle.ChangeAnimation)
        },
        {
          18,
          new Server.PacketHandler(ServerHandle.ChangeBio)
        },
        {
          19,
          new Server.PacketHandler(ServerHandle.ChangeAuthData)
        }
      };
            Server.Log("Initialized packets.", ConsoleColor.Green);
        }

        public static int FindIDFromUsername(string _username)
        {
            for (int key = 1; key < Server.clients.Count; ++key)
            {
                if (Server.clients[key].player != null && Server.clients[key].player.username.ToLower() == _username.ToLower())
                    return key;
            }
            return 0;
        }

        public static int FindIDFromUsernameNonPlayer(string _username)
        {
            for (int key = 1; key < Server.clients.Count; ++key)
            {
                if (Server.clients[key].account != null && Server.clients[key].account.username.ToLower() == _username.ToLower())
                    return key;
            }
            return 0;
        }

        public static string GetRandomWorlds(int _count)
        {
            string randomWorlds = "";
            List<World> worldList = new List<World>();
            foreach (World world in Server.worlds.Values)
            {
                if (world.clients.Count > 0)
                    worldList.Add(world);
            }
            int num = worldList.Count >= _count ? _count : worldList.Count;
            for (int index1 = 0; index1 < num; ++index1)
            {
                if (randomWorlds == "")
                {
                    int index2 = new Random().Next(0, worldList.Count);
                    randomWorlds = randomWorlds + worldList[index2].name + ":" + worldList[index2].clients.Count.ToString();
                    worldList.Remove(worldList[index2]);
                }
                else
                {
                    int index3 = new Random().Next(0, worldList.Count);
                    randomWorlds = randomWorlds + "|" + worldList[index3].name + ":" + worldList[index3].clients.Count.ToString();
                    worldList.Remove(worldList[index3]);
                }
            }
            return randomWorlds;
        }

        public static void Log(string _text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[" + DateTime.Now.ToString() + "] " + _text);
        }

        public static void Log(string _text, ConsoleColor _color)
        {
            Console.ForegroundColor = _color;
            Console.WriteLine("[" + DateTime.Now.ToString() + "] " + _text);
        }

        public static void Log(string _text, string _error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[" + DateTime.Now.ToString() + "] " + _text + " [" + _error + "]");
        }

        public static void Log(object _value)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[" + DateTime.Now.ToString() + "] " + _value.ToString());
        }

        public delegate void PacketHandler(int _fromClient, Packet _packet);
    }
}
