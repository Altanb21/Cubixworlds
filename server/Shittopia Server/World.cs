
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Security.Permissions;
using System.Text.Json;
using System.Threading;

namespace Shittopia_Server
{
    internal class World
    {
        public int id;
        public int worth;
        public string name;
        public string ownerName;
        public string data;
        public string worldBackgroundHexCode = "#292929FF";
        public int biomeID;
        public List<string> accessedPlayers = new List<string>();
        public bool privateState;
        public int worldState;
        public List<Client> clients = new List<Client>();
        public Block[][,] worldLayers = new Block[2][,];
        public Dictionary<int, DroppedItem> droppedItems = new Dictionary<int, DroppedItem>();
        public Vector2 whiteDoorPosition = new Vector2(0.0f, 59f);
        public Dictionary<Vector2, string> signs = new Dictionary<Vector2, string>();
        private int saveID;
        private int blockSaveID;
        private int dropSaveID;
        private int accessSaveID;
        private int signSaveID;
        public double currentTerrainHeight = 40.0;
        public int terrainHeight = 40;
        public double generationscale = 10.0;
        public double generationXScale = 0.2;
        public int waterLevel = 39;
        public int seed;

        public World(int _id) => this.id = _id;

        public void GenerateWorld(int _worldWidth, int _worldHeight)
        {
            seed = new Random().Next(0, 100000);
            worldLayers[0] = new Block[_worldWidth, _worldHeight];
            worldLayers[1] = new Block[_worldWidth, _worldHeight];

            World _world = this;
            int _id = 0;
            biomeID = _id;
            WorldGeneration.Biomes[_id].Invoke(ref _world, _worldWidth, _worldHeight);
        }

        [SecurityPermission((SecurityAction)2, ControlThread = true)]
        public Thread SaveWorld()
        {
            ++this.saveID;
            ++this.blockSaveID;
            ++this.dropSaveID;
            ++this.accessSaveID;
            ++this.signSaveID;
            Thread thread = new Thread((ThreadStart)(() => this.Save(true, true, true, true, this.saveID, this.blockSaveID, this.dropSaveID, this.accessSaveID, this.signSaveID)));
            thread.Start();
            return thread;
        }

        [SecurityPermission((SecurityAction)2, ControlThread = true)]
        public void SaveDropData()
        {
            ++this.dropSaveID;
            new Thread((ThreadStart)(() => this.Save(false, true, false, false, this.saveID, this.blockSaveID, this.dropSaveID, this.accessSaveID, this.signSaveID))).Start();
        }

        [SecurityPermission((SecurityAction)2, ControlThread = true)]
        public void SaveBlockData()
        {
            ++this.blockSaveID;
            new Thread((ThreadStart)(() => this.Save(true, false, false, false, this.saveID, this.blockSaveID, this.dropSaveID, this.accessSaveID, this.signSaveID))).Start();
        }

        [SecurityPermission((SecurityAction)2, ControlThread = true)]
        public void SaveAccessData()
        {
            ++this.accessSaveID;
            new Thread((ThreadStart)(() => this.Save(false, false, true, false, this.saveID, this.blockSaveID, this.dropSaveID, this.accessSaveID, this.signSaveID))).Start();
        }

        [SecurityPermission((SecurityAction)2, ControlThread = true)]
        public void SaveSignsData()
        {
            ++this.signSaveID;
            new Thread((ThreadStart)(() => this.Save(false, false, false, true, this.saveID, this.blockSaveID, this.dropSaveID, this.accessSaveID, this.signSaveID))).Start();
        }

        public void Save(
          bool _isBlockDataChanged,
          bool _isDropDataChanged,
          bool _isAccessDataChanged,
          bool _isSignsDataChanged,
          int _saveID,
          int _blockSaveID,
          int _dropSaveID,
          int _accessSaveID,
          int _SignSaveID)
        {
            try
            {
                string _worldForegroundData = "worldForegroundData";
                string _worldBackgroundData = "worldBackgroundData";
                string _dropData = "dropData";
                string _accessedPlayersData = "accessedPlayersData";
                string _signsData = "signsData";
                if (this.worldLayers[0] == null)
                    return;
                int length1 = this.worldLayers[0].Length;
                int length2 = this.worldLayers[1].Length;
                int _worldWidth = this.worldLayers[0].GetLength(0);
                int _worldHeight = this.worldLayers[0].GetLength(1);
                if (_isBlockDataChanged)
                {
                    try
                    {
                        for (int index1 = 0; index1 < length1; ++index1)
                        {
                            if (_saveID != this.saveID || _blockSaveID != this.blockSaveID)
                                return;
                            int index2 = index1 % _worldWidth;
                            int index3 = (int)Math.Floor((double)(index1 / _worldWidth));
                            _worldForegroundData = _worldForegroundData + "|" + this.worldLayers[0][index2, index3].id.ToString();
                        }
                        for (int index4 = 0; index4 < length2; ++index4)
                        {
                            if (_saveID != this.saveID || _blockSaveID != this.blockSaveID)
                                return;
                            int index5 = index4 % _worldWidth;
                            int index6 = (int)Math.Floor((double)(index4 / _worldWidth));
                            _worldBackgroundData = _worldBackgroundData + "|" + this.worldLayers[1][index5, index6].id.ToString();
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    _worldForegroundData = this.data.Split('/')[1];
                    _worldBackgroundData = this.data.Split('/')[2];
                }
                if (_isDropDataChanged)
                {
                    for (int key = 0; key < this.droppedItems.Values.Count; ++key)
                    {
                        if (_dropSaveID != this.dropSaveID)
                            return;
                        if (this.droppedItems.ContainsKey(key))
                            _dropData = _dropData + "|" + JsonSerializer.Serialize<DroppedItem>(this.droppedItems[key]);
                    }
                }
                else
                    _dropData = this.data.Split('/')[3];
                if (_isAccessDataChanged)
                {
                    for (int index = 0; index < this.accessedPlayers.Count; ++index)
                    {
                        if (_accessSaveID != this.accessSaveID)
                            return;
                        _accessedPlayersData = _accessedPlayersData + "|" + this.accessedPlayers[index];
                    }
                }
                else
                    _accessedPlayersData = this.data.Split('/')[4];
                if (_isSignsDataChanged)
                {
                    foreach (KeyValuePair<Vector2, string> sign in this.signs)
                    {
                        if (_SignSaveID != this.signSaveID)
                            return;
                        string[] strArray = new string[7];
                        strArray[0] = _signsData;
                        strArray[1] = "|";
                        Vector2 key = sign.Key;
                        strArray[2] = key.X.ToString();
                        strArray[3] = ":";
                        key = sign.Key;
                        strArray[4] = key.Y.ToString();
                        strArray[5] = ":^";
                        strArray[6] = sign.Value;
                        _signsData = string.Concat(strArray);
                    }
                }
                else
                    _signsData = this.data.Split('/')[8];
                try
                {
                    ThreadManager.ExecuteOnMainThread((Action)(() => File.WriteAllText(Server.path + "Worlds\\" + this.name + ".shit", Compress.CompressString(_worldWidth.ToString() + ":" + _worldHeight.ToString() + "/" + _worldForegroundData + "/" + _worldBackgroundData + "/" + _dropData + "/" + _accessedPlayersData + "/" + this.worldState.ToString() + "/" + this.ownerName + "/" + this.biomeID.ToString() + "/" + _signsData))));
                    this.data = _worldWidth.ToString() + ":" + _worldHeight.ToString() + "/" + _worldForegroundData + "/" + _worldBackgroundData + "/" + _dropData + "/" + _accessedPlayersData + "/" + this.worldState.ToString() + "/" + this.ownerName + "/" + this.biomeID.ToString() + "/" + _signsData;
                }
                catch (Exception ex)
                {
                    Server.Log("An error occured!", ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Server.Log("An error occured!", ex.ToString());
            }
        }

        public void LoadWorld(int _fromClient)
        {
            string str1 = Compress.DecompressString(File.ReadAllText(Server.path + "Worlds\\" + this.name + ".shit"));
            this.data = str1;
            string[] strArray1 = str1.Split('/');
            string[] strArray2 = strArray1[0].Split(':');
            int length1 = int.Parse(strArray2[0]);
            int length2 = int.Parse(strArray2[1]);
            string[] strArray3 = strArray1[1].Split('|');
            string[] strArray4 = strArray1[2].Split('|');
            string str2 = strArray1[3];
            string str3 = strArray1[4];
            if (str2 != "dropData")
            {
                string[] strArray5 = str2.Split('|');
                for (int index = 1; index < strArray5.Length; ++index)
                {
                    DroppedItem droppedItem = JsonSerializer.Deserialize<DroppedItem>(strArray5[index]);
                    this.CreateDroppedItem(_fromClient, droppedItem.item.id, droppedItem.item.quantity, new Vector2(droppedItem.positionX, droppedItem.positionY));
                }
            }
            if (str3 != "accessedPlayersData")
            {
                this.privateState = true;
                string[] strArray6 = str3.Split('|');
                for (int index = 1; index < strArray6.Length; ++index)
                    this.accessedPlayers.Add(strArray6[index]);
            }
            this.worldLayers = new Block[2][,];
            this.worldLayers[0] = new Block[length1, length2];
            this.worldLayers[1] = new Block[length1, length2];
            for (int index = 1; index < strArray3.Length; ++index)
            {
                int x = (index - 1) % length1;
                int y = (int)Math.Floor((Decimal)((index - 1) / length1));
                this.worldLayers[0][x, y] = new Block();
                this.worldLayers[0][x, y].id = int.Parse(strArray3[index]);
                this.worldLayers[0][x, y].health = GameData.items[this.worldLayers[0][x, y].id].health;
                if (this.worldLayers[0][x, y].id == 2)
                    this.whiteDoorPosition = new Vector2((float)x, (float)y);
            }
            for (int index1 = 1; index1 < strArray4.Length; ++index1)
            {
                int index2 = (index1 - 1) % length1;
                int index3 = (int)Math.Floor((Decimal)((index1 - 1) / length1));
                this.worldLayers[1][index2, index3] = new Block();
                this.worldLayers[1][index2, index3].id = int.Parse(strArray4[index1]);
                this.worldLayers[1][index2, index3].health = GameData.items[this.worldLayers[1][index2, index3].id].health;
            }
            this.worldState = int.Parse(strArray1[5]);
            this.biomeID = int.Parse(strArray1[7]);
            if (strArray1[6] != "")
            {
                this.ownerName = strArray1[6];
                this.privateState = true;
            }
            if (strArray1.Length < 9)
                return;
            string[] strArray7 = strArray1[8].Split("|");
            for (int index4 = 1; index4 < strArray7.Length; ++index4)
            {
                Vector2 key = new Vector2(float.Parse(strArray7[index4].Split(":")[0]), float.Parse(strArray7[index4].Split(":")[1]));
                string[] strArray8 = strArray7[index4].Split("^");
                string str4 = "";
                for (int index5 = 0; index5 < strArray8.Length; ++index5)
                    str4 = strArray8[index5];
                this.signs.Add(key, str4);
            }
        }

        public void CreateDroppedItem(int _fromClient, int _itemID, int _quantity, Vector2 _position)
        {
            if (_quantity > 0)
            {
                DroppedItem _droppedItem = new DroppedItem();
                _droppedItem.item = new GameItem()
                {
                    id = _itemID,
                    quantity = _quantity
                };
                _droppedItem.positionX = _position.X;
                _droppedItem.positionY = _position.Y;
                for (int key = 0; key < int.MaxValue; ++key)
                {
                    if (!this.droppedItems.ContainsKey(key))
                    {
                        _droppedItem.id = key;
                        break;
                    }
                }
                this.droppedItems.Add(_droppedItem.id, _droppedItem);
                if (_fromClient != -1)
                    ServerSend.CreateDroppedItem(_fromClient, _droppedItem);
            }
            this.SaveDropData();
        }

        public void RemoveDroppedItem(int _id)
        {
            this.droppedItems.Remove(_id);
            this.SaveDropData();
        }

        public void AddAccess(int _clientID)
        {
            if (this.accessedPlayers.Count == 0)
            {
                this.privateState = true;
                this.accessedPlayers.Add(Server.clients[_clientID].player.username);
            }
            else
                this.accessedPlayers.Add(Server.clients[_clientID].player.username);
            this.SaveAccessData();
        }

        public void AddAccess(string _username)
        {
            if (this.accessedPlayers.Count == 0)
            {
                this.privateState = true;
                this.accessedPlayers.Add(_username);
            }
            else
                this.accessedPlayers.Add(_username);
            this.SaveAccessData();
        }

        public void RemoveAccess(int _clientID)
        {
            if (this.accessedPlayers.Count == 1 && this.accessedPlayers.Count != 0)
            {
                this.privateState = false;
                this.accessedPlayers.Remove(Server.clients[_clientID].player.username);
            }
            else
                this.accessedPlayers.Remove(Server.clients[_clientID].player.username);
            this.SaveAccessData();
        }

        public void RemoveAccess(string _username)
        {
            if (this.accessedPlayers.Count == 0)
            {
                this.privateState = true;
                this.accessedPlayers.Remove(_username);
            }
            else
                this.accessedPlayers.Remove(_username);
            this.SaveAccessData();
        }

        public int ContainsPlayer(string _username)
        {
            foreach (Client client in this.clients)
            {
                if (client.player.username.ToLower() == _username.ToLower())
                    return client.id;
            }
            return 0;
        }
    }
}
