
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace Shittopia_Server
{
    internal class Client
    {
        public static int dataBufferSize = 65536;
        public int id;
        public bool isActive;
        public bool isGhost;
        public bool isNoclip;
        public string nationality;
        public Player player;
        public Trade currentTrade;
        public Account account;
        public World world;
        public Client.TCP tcp;
        public Client.UDP udp;
        public bool enteringWorld;
        public string lastFriendRequestName;
        public string guidID;
        public DateTime chatCooldown;

        public Client(int _clientId)
        {
            this.id = _clientId;
            this.tcp = new Client.TCP(this.id);
            this.udp = new Client.UDP(this.id);
        }

        public void SetAccount(
          int _fromClient,
          string _username,
          string _password,
          string _email,
          bool _isLogin,
          string _version)
        {
            if (Server.clients[_fromClient].isActive || !(_username != "") || !(_password != ""))
                return;
            if (_isLogin)
            {
                int _replyCode = Auth.Login(_username, _password, this);
                if (Server.clients[_fromClient].account != null)
                {
                    if (Server.clients[_fromClient].account.accountLevel != 7 && _version != Server.version)
                    {
                        ServerSend.SendNotification(_fromClient, "<color=red>Error", "Outdated version, please visit " + ChatManager.coloredTexts["blue"] + "www.Build4fun.co", 3f);
                        return;
                    }
                }
                else if (_version != Server.version)
                {
                    ServerSend.SendNotification(_fromClient, "<color=red>Error", "Outdated version, please visit " + ChatManager.coloredTexts["blue"] + "www.Build4fun.co", 3f);
                    return;
                }
                ServerSend.AuthUser(_fromClient, _username, _password, _replyCode);
                ServerSend.SendPackData(_fromClient);
            }
            else
            {
                int _replyCode = Auth.Register(_username, _password, _email, this);
                if (Server.clients[_fromClient].account != null)
                {
                    if (Server.clients[_fromClient].account.accountLevel != 7 && _version != Server.version)
                    {
                        ServerSend.SendNotification(_fromClient, "<color=red>Error", "Outdated version, please visit " + ChatManager.coloredTexts["blue"] + "www.Build4Fun.com", 3f);
                        return;
                    }
                }
                else if (_version != Server.version)
                {
                    ServerSend.SendNotification(_fromClient, "<color=red>Error", "Outdated version, please visit " + ChatManager.coloredTexts["blue"] + "www.Build4Fun.com", 3f);
                    return;
                }
                ServerSend.AuthUser(_fromClient, _username, _password, _replyCode);
                ServerSend.SendPackData(_fromClient);
                if (_replyCode != 0)
                    return;
                ServerSend.JoinWorld(_fromClient, "START");
            }
        }

        public void SendIntoGame(Account _account, string _worldName, string _nationality)
        {
            string username = _account.username;
            bool doesExistForLock = false;
            foreach (World world in Server.worlds.Values)
            {
                if (_worldName == world.name)
                {
                    doesExistForLock = true;
                    if (world.worldState == 1 && _account.accountLevel != 7 && _account.accountLevel != 5 && _account.accountLevel != 4)
                    {
                        this.enteringWorld = false;
                        ServerSend.SendNotification(this.id, "<color=red>Warning", "This world is banned", 1.5f);
                        ServerSend.Reply(this.id, 3);
                        return;
                    }
                }
            }

            if (!doesExistForLock)
            {
                if (File.Exists(Server.path + @"/Worlds/" + _worldName + ".shit"))
                {
                    string _worldData = File.ReadAllText(Server.path + @"Worlds\" + _worldName + ".shit");
                    string[] _datas = _worldData.Split('/');

                    if (_datas[5] == "1")
                    {
                        if (_account.accountLevel == 7 || _account.accountLevel == 5 || _account.accountLevel == 4)
                        {

                        }
                        else
                        {
                            enteringWorld = false;
                            return;
                        }
                    }
                }
            }

            bool doesExist = false;
            foreach (World world in Server.worlds.Values)
            {
                if (_worldName == world.name)
                {
                    if (_worldName == world.name && world.clients.Count > 0)
                    {
                        doesExist = true;

                        this.world = world;
                        world.clients.Add(this);

                        ServerSend.SendWorldData(id, world);

                        using (Dictionary<int, DroppedItem>.ValueCollection.Enumerator enumerator = world.droppedItems.Values.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                                ServerSend.SendDroppedItemData(this.id, enumerator.Current);
                            break;
                        }
                    }
                }
            }

                if (!doesExist)
            {
                foreach (World world in Server.worlds.Values)
                {
                    if (world.clients.Count == 0)
                    {
                        world.name = _worldName;
                        world.worth = 10000 / world.name.Length;
                        this.world = world;
                        world.clients.Add(this);

                        if (File.Exists(Server.path + @"/Worlds/" + _worldName + ".shit"))
                        {
                            world.LoadWorld(id);
                        }
                        else
                        {
                            // default : 100x60
                            world.GenerateWorld(100, 60);
                            world.SaveWorld();

                        }

                        ServerSend.SendWorldData(id, world);

                        Server.Log($"{world.name} is created by {username}!", ConsoleColor.Blue);
                        break;
                    }
                }
            }

            Server.Log($"{username} joined to {world.name}!", ConsoleColor.Green);

            this.nationality = _nationality;
            this.player = !this.isGhost ? new Player(this.id, username, this.world.whiteDoorPosition, 100) : new Player(this.id, username, new Vector2(-1f, -1f), 100);
            _account.inventory.player = this.player;
            account = _account;

            ServerSend.SendChatMessage(this.id, $"Welcome to the {ChatManager.coloredTexts["red"]}Build4fun</color>!");

            if (world.clients.Count == 1)
            {
                ServerSend.SendChatMessage(id, $"You have entered to world{ChatManager.coloredTexts["blue"]} " + world.name + $"</color>, {ChatManager.coloredTexts["green"]}" + world.clients.Count + "</color> player is here.");
            }
            else if (world.clients.Count > 1)
            {
                ServerSend.SendChatMessage(id, $"You have entered to world{ChatManager.coloredTexts["blue"]} " + world.name + $"</color>, {ChatManager.coloredTexts["green"]}" + world.clients.Count + "</color> players are here.");
            }

            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    if (_client.id != id)
                    {
                        if (_client.world == world)
                        {
                            ServerSend.SpawnPlayer(id, _client.player);
                            if (world.clients.Count == 1 && !isGhost)
                            {
                                ServerSend.SendChatMessage(_client.id, $"{ChatManager.coloredTexts["orange"]}" + player.username + $"</color> has joined, {ChatManager.coloredTexts["green"]}" + world.clients.Count + "</color> player is here.");
                            }
                            else if (world.clients.Count > 1 && !isGhost)
                            {
                                ServerSend.SendChatMessage(_client.id, $"{ChatManager.coloredTexts["orange"]}" + player.username + $"</color> has joined, {ChatManager.coloredTexts["green"]}" + world.clients.Count + "</color> players are here.");
                            }
                        }
                    }
                }
            }

            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    if (_client.world == world)
                    {
                        ServerSend.SpawnPlayer(_client.id, player);
                    }
                }
            }
        }

        public void LeaveFromWorld()
        {
            int count = Server.clients[this.id].world.clients.Count;
            if (Server.clients[this.id].world != null && count - 1 != 0)
            {
                if (count - 1 == 1 && !this.isGhost)
                {
                    foreach (Client client in Server.clients[this.id].world.clients)
                        ServerSend.SendChatMessage(client.id, ChatManager.coloredTexts["orange"] + this.player.username + "</color> has left, " + ChatManager.coloredTexts["green"] + (count - 1).ToString() + "</color> player is here.");
                }
                else if (count - 1 > 1 && !this.isGhost)
                {
                    foreach (Client client in Server.clients[this.id].world.clients)
                        ServerSend.SendChatMessage(client.id, ChatManager.coloredTexts["orange"] + this.player.username + "</color> has left, " + ChatManager.coloredTexts["green"] + (count - 1).ToString() + "</color> players are here.");
                }
            }
            ServerSend.DisconnectUser(this.id);
            World world = Server.clients[this.id].world;
            world.clients.Remove(Server.clients[this.id]);
            if (world.clients.Count == 0)
            {
                Server.Log(world.name + " is removed.", ConsoleColor.Blue);
                world.name = (string)null;
                world.worldLayers[0] = (Block[,])null;
                Server.worlds[world.id] = new World(world.id);
            }
            ServerSend.Reply(this.id, 1);
            this.enteringWorld = false;
        }
        public async void Disconnect()
        {
            Client client1 = this;
            if (client1.tcp.socket == null)
                return;
            if (client1.world != null)
            {
                int count = client1.world.clients.Count;
                if (count - 1 != 0)
                {
                    if (count - 1 == 1 && !client1.isGhost)
                    {
                        foreach (Client client2 in client1.world.clients)
                        {
                            if (client1.player != null && client2.player != null)
                                ServerSend.SendChatMessage(client2.id, ChatManager.coloredTexts["orange"] + client1.player.username + "</color> has left, " + ChatManager.coloredTexts["green"] + (count - 1).ToString() + "</color> player is here.");
                        }
                    }
                    else if (count - 1 > 1 && !client1.isGhost)
                    {
                        foreach (Client client3 in client1.world.clients)
                        {
                            if (client3.player != null)
                                ServerSend.SendChatMessage(client3.id, ChatManager.coloredTexts["orange"] + client1.player.username + "</color> has left, " + ChatManager.coloredTexts["green"] + (count - 1).ToString() + "</color> players are here.");
                        }
                    }
                }
            }
            if (client1.account != null && client1.player != null)
                client1.account.inventory.SetItem(28, 0);
            if (client1.currentTrade != null)
                TradeManager.CancelTrade(client1.currentTrade, false);
            client1.isActive = false;
            client1.enteringWorld = false;
            if (client1.currentTrade != null)
                TradeManager.CancelTrade(client1.currentTrade, true);
            if (client1.account != null)
                client1.account.Save();
            if (client1.world != null)
            {
                client1.world.clients.Remove(client1);
                if (client1.world.clients.Count == 0)
                {
                    client1.world.SaveWorld().Join();
                    Server.Log(client1.world.name + " is removed.", ConsoleColor.Blue);
                    client1.world.name = (string)null;
                    client1.world.worldLayers[0] = (Block[,])null;
                    Server.worlds[client1.world.id] = new World(client1.world.id);
                }
            }
            Server.Log(string.Format("{0} has disconnected!", (object)client1.tcp.socket.Client.RemoteEndPoint), ConsoleColor.Red);
            Security.ClientDisconnect(client1.tcp.socket.Client.RemoteEndPoint.ToString().Split(":")[0]);
            if (client1.world != null)
                ServerSend.DisconnectUser(client1.id);
            client1.isNoclip = false;
            client1.isGhost = false;
            client1.player = (Player)null;
            client1.account = (Account)null;
            client1.tcp.Disconnect();
            client1.udp.Disconnect();
        }

        public class TCP
        {
            public TcpClient socket;
            private readonly int id;
            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;

            public TCP(int _id) => this.id = _id;

            public void Connect(TcpClient _socket)
            {
                this.socket = _socket;
                this.socket.ReceiveBufferSize = Client.dataBufferSize;
                this.socket.SendBufferSize = Client.dataBufferSize;
                this.stream = this.socket.GetStream();
                this.receivedData = new Packet();
                this.receiveBuffer = new byte[Client.dataBufferSize];
                this.stream.BeginRead(this.receiveBuffer, 0, Client.dataBufferSize, new AsyncCallback(this.ReceiveCallback), (object)null);
                ServerSend.Welcome(this.id, "You have connected to the server!", Server.itemsTextureURL);
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    if (this.socket == null)
                        return;
                    this.stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), (AsyncCallback)null, (object)null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error sending data to player {0} via TCP: {1}", (object)this.id, (object)ex));
                }
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    if (this.stream != null)
                    {
                        int length = this.stream.EndRead(_result);
                        if (length <= 0)
                        {
                            Server.clients[this.id].Disconnect();
                        }
                        else
                        {
                            byte[] numArray = new byte[length];
                            Array.Copy((Array)this.receiveBuffer, (Array)numArray, length);
                            this.receivedData.Reset(this.HandleData(numArray));
                            this.stream.BeginRead(this.receiveBuffer, 0, Client.dataBufferSize, new AsyncCallback(this.ReceiveCallback), (object)null);
                        }
                    }
                    else
                        this.Disconnect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error receiving TCP data: {0}", (object)ex));
                    Server.clients[this.id].Disconnect();
                }
            }

            private bool HandleData(byte[] _data)
            {
                try
                {
                    int _length = 0;
                    this.receivedData.SetBytes(_data);
                    if (this.receivedData.UnreadLength() >= 4)
                    {
                        _length = this.receivedData.ReadInt();
                        if (_length <= 0)
                            return true;
                    }
                    while (_length > 0 && _length <= this.receivedData.UnreadLength())
                    {
                        byte[] _packetBytes = this.receivedData.ReadBytes(_length);
                        ThreadManager.ExecuteOnMainThread((Action)(() =>
                       {
                           using (Packet _packet = new Packet(_packetBytes))
                           {
                               try
                               {
                                   int key = _packet.ReadInt();
                                   Server.packetHandlers[key](this.id, _packet);
                               }
                               catch (Exception ex)
                               {
                                   Server.Log("An error occured", ex.ToString());
                               }
                           }
                       }));
                        _length = 0;
                        if (this.receivedData.UnreadLength() >= 4)
                        {
                            _length = this.receivedData.ReadInt();
                            if (_length <= 0)
                                return true;
                        }
                    }
                    return _length <= 1;
                }
                catch (Exception ex)
                {
                    Server.Log("An error occured", ex.ToString());
                    return false;
                }
            }

            public void Disconnect()
            {
                if (this.socket != null)
                    this.socket.Close();
                this.stream = (NetworkStream)null;
                this.receivedData = (Packet)null;
                this.receiveBuffer = (byte[])null;
                this.socket = (TcpClient)null;
            }
        }

        public class UDP
        {
            public IPEndPoint endPoint;
            private int id;

            public UDP(int _id) => this.id = _id;

            public void Connect(IPEndPoint _endPoint) => this.endPoint = _endPoint;

            public void SendData(Packet _packet) => Server.SendUDPData(this.endPoint, _packet);

            public void HandleData(Packet _packetData)
            {
                int _length = _packetData.ReadInt();
                byte[] _packetBytes = _packetData.ReadBytes(_length);
                ThreadManager.ExecuteOnMainThread((Action)(() =>
               {
                   using (Packet _packet = new Packet(_packetBytes))
                   {
                       int key = _packet.ReadInt();
                       Server.packetHandlers[key](this.id, _packet);
                   }
               }));
            }

            public void Disconnect() => this.endPoint = (IPEndPoint)null;
        }
    }
}
