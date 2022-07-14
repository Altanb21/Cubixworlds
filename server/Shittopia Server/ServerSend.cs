
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace Shittopia_Server
{
    internal class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int key = 1; key <= Server.MaxPlayers; ++key)
                Server.clients[key].tcp.SendData(_packet);
        }

        private static void SendTCPDataToWorld(World _world, Packet _packet)
        {
            _packet.WriteLength();
            foreach (Client client in _world.clients)
                client.tcp.SendData(_packet);
        }

        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int key = 1; key <= Server.MaxPlayers; ++key)
            {
                if (key != _exceptClient)
                    Server.clients[key].tcp.SendData(_packet);
            }
        }

        private static void SendTCPDataToWorld(World _world, int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            foreach (Client client in _world.clients)
            {
                if (client.id != _exceptClient)
                    client.tcp.SendData(_packet);
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int key = 1; key <= Server.MaxPlayers; ++key)
                Server.clients[key].udp.SendData(_packet);
        }

        private static void SendUDPDataToWorld(World _world, Packet _packet)
        {
            _packet.WriteLength();
            foreach (Client client in _world.clients)
                client.udp.SendData(_packet);
        }

        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int key = 1; key <= Server.MaxPlayers; ++key)
            {
                if (key != _exceptClient)
                    Server.clients[key].udp.SendData(_packet);
            }
        }

        private static void SendUDPDataToWorld(World _world, int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            foreach (Client client in _world.clients)
            {
                if (client.id != _exceptClient)
                    client.udp.SendData(_packet);
            }
        }

        public static void Welcome(int _toClient, string _msg, string _itemsTextureURL)
        {
            using (Packet _packet = new Packet(1))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);
                _packet.Write(_itemsTextureURL);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void SpawnPlayer(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet(2))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(Server.clients[_player.id].account.accountLevel);
                _packet.Write("NL");
                if (Server.clients[_player.id].isGhost && _player.id == _toClient)
                    _packet.Write(Server.clients[_player.id].world.whiteDoorPosition);
                else
                    _packet.Write(_player.position);
                _packet.Write(_player.size);
                _packet.Write(_player.collisionSize);
                string str = "costumeData";
                for (int index = 0; index < Server.clients[_player.id].account.clothes.Length; ++index)
                    str = str + ":" + Server.clients[_player.id].account.clothes[index].ToString();
                _packet.Write(str);
                _packet.Write(Server.clients[_player.id].account.bio);
                _packet.Write(Server.clients[_player.id].account.level);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void PlayerPosition(Player _player)
        {
            using (Packet _packet = new Packet(3))
            {
                try
                {
                    if (_player == null)
                        return;
                    _packet.Write(_player.id);
                    _packet.Write(_player.position);
                    ServerSend.SendUDPDataToWorld(Server.clients[_player.id].world, _player.id, _packet);
                }
                catch (Exception ex)
                {
                    Server.Log("An error occured while sending player position!", ex.ToString());
                }
            }
        }

        public static void PlayerSize(Player _player)
        {
            using (Packet _packet = new Packet(4))
            {
                if (_player == null)
                    return;
                _packet.Write(_player.id);
                _packet.Write(_player.size);
                ServerSend.SendTCPDataToWorld(Server.clients[_player.id].world, _player.id, _packet);
            }
        }

        public static void DisconnectUser(int _clientID)
        {
            using (Packet _packet = new Packet(5))
            {
                _packet.Write(_clientID);
                ServerSend.SendTCPDataToWorld(Server.clients[_clientID].world, _clientID, _packet);
            }
        }

        public static void SendItemData(int _toClient)
        {
            using (Packet _packet = new Packet(6))
            {
                int length = GameData.items.Length;
                _packet.Write(length);
                foreach (Item obj in GameData.items)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<Item>(obj));
                    _packet.Write(bytes.Length);
                    _packet.Write(bytes);
                }
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void SendWorldData(int _toClient, World _world)
        {
            using (Packet _packet = new Packet(30))
            {
                string text1 = "worldData";
                int length1 = _world.worldLayers[0].GetLength(0);
                int length2 = _world.worldLayers[0].GetLength(1);
                int num;
                for (int index1 = 0; index1 < _world.worldLayers[0].Length; ++index1)
                {
                    int index2 = index1 % length1;
                    int index3 = (int)Math.Floor((double)(index1 / length1));
                    string[] strArray = new string[5]
                    {
            text1,
            "|",
            null,
            null,
            null
                    };
                    num = _world.worldLayers[0][index2, index3].id;
                    strArray[2] = num.ToString();
                    strArray[3] = ":";
                    num = _world.worldLayers[0][index2, index3].health;
                    strArray[4] = num.ToString();
                    text1 = string.Concat(strArray);
                }
                string text2 = "worldData";
                int length3 = _world.worldLayers[1].GetLength(0);
                _world.worldLayers[1].GetLength(1);
                for (int index4 = 0; index4 < _world.worldLayers[1].Length; ++index4)
                {
                    int index5 = index4 % length3;
                    int index6 = (int)Math.Floor((double)(index4 / length3));
                    string[] strArray = new string[5]
                    {
            text2,
            "|",
            null,
            null,
            null
                    };
                    num = _world.worldLayers[1][index5, index6].id;
                    strArray[2] = num.ToString();
                    strArray[3] = ":";
                    num = _world.worldLayers[1][index5, index6].health;
                    strArray[4] = num.ToString();
                    text2 = string.Concat(strArray);
                }
                string str = "signsData";
                foreach (KeyValuePair<Vector2, string> sign in Server.clients[_toClient].world.signs)
                {
                    string[] strArray = new string[7];
                    strArray[0] = str;
                    strArray[1] = "|";
                    Vector2 key = sign.Key;
                    strArray[2] = key.X.ToString();
                    strArray[3] = ":";
                    key = sign.Key;
                    strArray[4] = key.Y.ToString();
                    strArray[5] = ":^";
                    strArray[6] = ChatManager.wrapString(sign.Value, 30);
                    str = string.Concat(strArray);
                }
                _packet.Write(_world.name);
                _packet.Write(length1);
                _packet.Write(length2);
                byte[] bytes1 = Encoding.UTF8.GetBytes(Compress.CompressString(text1));
                byte[] bytes2 = Encoding.UTF8.GetBytes(Compress.CompressString(text2));
                _packet.Write(bytes1.Length);
                _packet.Write(bytes2.Length);
                _packet.Write(bytes1);
                _packet.Write(bytes2);
                _packet.Write(_world.worldBackgroundHexCode);
                _packet.Write(str);
                if (_world.ownerName != null)
                    _packet.Write(0);
                else
                    _packet.Write(_world.worth);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void EditWorldData(
          int _toClient,
          int _blockID,
          int _layer,
          int _blockHealth,
          Vector2 _blockPosition)
        {
            using (Packet _packet = new Packet(8))
            {
                _packet.Write(_blockID);
                _packet.Write(_blockHealth);
                _packet.Write(_blockPosition);
                _packet.Write(_layer);
                ServerSend.SendTCPDataToWorld(Server.clients[_toClient].world, _packet);
            }
        }

        public static void SendInventoryData(int _toClient)
        {
            using (Packet _packet = new Packet(9))
            {
                IEnumerable<GameItem> source = (IEnumerable<GameItem>)((IEnumerable<GameItem>)Server.clients[_toClient].account.inventory.items).ToList<GameItem>().OrderBy<GameItem, int>((Func<GameItem, int>)(x => GameData.items[x.id].inventoryOrder));
                _packet.Write(Compress.CompressString(new Inventory()
                {
                    player = Server.clients[_toClient].player,
                    maxSlots = 500,
                    items = source.ToArray<GameItem>()
                }.ToJson()));
                _packet.Write(Compress.CompressString(Server.clients[_toClient].account.inventory.ToJson()));
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void EditInventoryData(int _toClient, int _itemID, int _quantity)
        {
            using (Packet _packet = new Packet(10))
            {
                _packet.Write(_itemID);
                _packet.Write(_quantity);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void SendDroppedItemData(int _toClient, DroppedItem _droppedItem)
        {
            using (Packet _packet = new Packet(11))
            {
                _packet.Write(_droppedItem.id);
                _packet.Write(_droppedItem.item.id);
                _packet.Write(_droppedItem.item.quantity);
                _packet.Write(new Vector2(_droppedItem.positionX, _droppedItem.positionY));
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void CreateDroppedItem(int _toClient, DroppedItem _droppedItem)
        {
            using (Packet _packet = new Packet(11))
            {
                _packet.Write(_droppedItem.id);
                _packet.Write(_droppedItem.item.id);
                _packet.Write(_droppedItem.item.quantity);
                _packet.Write(new Vector2(_droppedItem.positionX, _droppedItem.positionY));
                ServerSend.SendTCPDataToWorld(Server.clients[_toClient].world, _packet);
            }
        }

        public static void RemoveDroppedItem(int _toClient, DroppedItem _droppedItem)
        {
            using (Packet _packet = new Packet(12))
            {
                _packet.Write(_droppedItem.id);
                ServerSend.SendTCPDataToWorld(Server.clients[_toClient].world, _packet);
            }
        }

        public static void SendChatMessage(int _toClient, string _text)
        {
            using (Packet _packet = new Packet(13))
            {
                _packet.Write(_text);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void SendChatBubble(int _toClient, int _clientID, string _text, float _time)
        {
            using (Packet _packet = new Packet(14))
            {
                _packet.Write(_clientID);
                _packet.Write(_text);
                _packet.Write(_time);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void AuthUser(int _toClient, string _username, string _password, int _replyCode)
        {
            using (Packet _packet = new Packet(15))
            {
                _packet.Write(_username);
                _packet.Write(_password);
                _packet.Write(_replyCode);
                if (_replyCode == 0)
                {
                    _packet.Write(Server.clients[_toClient].account.bio);
                    _packet.Write(Server.clients[_toClient].account.level);
                    _packet.Write(Server.clients[_toClient].account.accountLevel);
                    _packet.Write(Server.clients[_toClient].account.cubix);
                    string str1 = "";
                    string str2 = "";
                    if (Server.clients[_toClient].account.friendList != null)
                    {
                        foreach (string friend in Server.clients[_toClient].account.friendList)
                            str1 = !(str1 == "") ? str1 + ", " + friend : str1 + friend;
                    }
                    if (Server.clients[_toClient].account.ownedWorlds != null)
                    {
                        foreach (string ownedWorld in Server.clients[_toClient].account.ownedWorlds)
                            str2 = !(str2 == "") ? str2 + ", " + ownedWorld : str2 + ownedWorld;
                    }
                    _packet.Write(str1);
                    _packet.Write(str2);
                }
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void Reply(int _toClient, int _reply)
        {
            using (Packet _packet = new Packet(16))
            {
                _packet.Write(_reply);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void SendPackData(int _toClient)
        {
            using (Packet _packet = new Packet(17))
            {
                string str1 = "";
                foreach (Pack pack in GameData.packs)
                {
                    string str2 = JsonSerializer.Serialize<Pack>(pack);
                    str1 = !(str1 == "") ? str1 + "|" + str2 : str1 + str2;
                }
                _packet.Write(str1);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void SetPosition(int _toClient)
        {
            using (Packet _packet = new Packet(18))
            {
                if (Server.clients[_toClient].player == null)
                    return;
                _packet.Write(Server.clients[_toClient].player.position);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void CreateTrade(int _toClient)
        {
            using (Packet _packet = new Packet(19))
            {
                _packet.Write(0);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void RemoveTrade(int _toClient)
        {
            using (Packet _packet = new Packet(22))
            {
                _packet.Write(0);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void AddItemToTrade(
          int _toClient,
          int _itemID,
          int _quantity,
          int _row,
          int _clientID)
        {
            using (Packet _packet = new Packet(20))
            {
                _packet.Write(_itemID);
                _packet.Write(_quantity);
                _packet.Write(_row);
                _packet.Write(_clientID);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void SendNotification(int _toClient, string _title, string _text, float _time)
        {
            using (Packet _packet = new Packet(23))
            {
                _packet.Write(_title);
                _packet.Write(ChatManager.wrapString(_text, 40));
                _packet.Write(_time);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void JoinWorld(int _toClient, string _worldName)
        {
            using (Packet _packet = new Packet(24))
            {
                _packet.Write(_worldName);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void SignData(
          int _toClient,
          Vector2 _signPosition,
          string _signText,
          int _actionID)
        {
            using (Packet _packet = new Packet(25))
            {
                _packet.Write(_signPosition);
                _packet.Write(ChatManager.wrapString(_signText, 30));
                _packet.Write(_actionID);
                ServerSend.SendTCPDataToWorld(Server.clients[_toClient].world, _packet);
            }
        }

        public static void EditCostumeData(int _clientID, int _costumeID, int _costumeRowID)
        {
            using (Packet _packet = new Packet(26))
            {
                _packet.Write(_clientID);
                _packet.Write(_costumeID);
                _packet.Write(_costumeRowID);
                ServerSend.SendTCPDataToWorld(Server.clients[_clientID].world, _packet);
            }
        }

        public static void ChangeAnimation(int _clientID, int _animationID)
        {
            using (Packet _packet = new Packet(27))
            {
                _packet.Write(_clientID);
                _packet.Write(_animationID);
                ServerSend.SendTCPDataToWorld(Server.clients[_clientID].world, _packet);
            }
        }

        public static void SetHealth(int _clientID, int _health)
        {
            using (Packet _packet = new Packet(28))
            {
                _packet.Write(_clientID);
                _packet.Write(_health);
                ServerSend.SendTCPDataToWorld(Server.clients[_clientID].world, _packet);
            }
        }

        public static void SetStatus(int _clientID, int _statusID, bool _status)
        {
            using (Packet _packet = new Packet(29))
            {
                _packet.Write(_clientID);
                _packet.Write(_statusID);
                _packet.Write(_status);
                ServerSend.SendTCPDataToWorld(Server.clients[_clientID].world, _packet);
            }
        }

        public static void SendActiveWorlds(int _clientID)
        {
            using (Packet _packet = new Packet(31))
            {
                _packet.Write(Server.popularWorld);
                _packet.Write(Server.GetRandomWorlds(20));
                ServerSend.SendTCPData(_clientID, _packet);
            }
        }

        public static void SendNews(int _clientID)
        {
            using (Packet _packet = new Packet(33))
            {
                string str = "";
                foreach (News news in GameData.news)
                    str = !(str == "") ? str + "|" + JsonSerializer.Serialize<News>(news) : str + JsonSerializer.Serialize<News>(news);
                _packet.Write(str);
                ServerSend.SendTCPData(_clientID, _packet);
            }
        }

        public static void SendCubixCount(int _clientID)
        {
            using (Packet _packet = new Packet(34))
            {
                _packet.Write(Server.clients[_clientID].account.cubix);
                ServerSend.SendTCPData(_clientID, _packet);
            }
        }

        public static void UpdateAccountData(int _toClient)
        {
            using (Packet _packet = new Packet(35))
            {
                string str1 = "";
                string str2 = "";
                if (Server.clients[_toClient].account.friendList != null)
                {
                    foreach (string friend in Server.clients[_toClient].account.friendList)
                        str1 = !(str1 == "") ? str1 + ", " + friend : str1 + friend;
                }
                if (Server.clients[_toClient].account.ownedWorlds != null)
                {
                    foreach (string ownedWorld in Server.clients[_toClient].account.ownedWorlds)
                        str2 = !(str2 == "") ? str2 + ", " + ownedWorld : str2 + ownedWorld;
                }
                _packet.Write(Server.clients[_toClient].account.cubix);
                _packet.Write(Server.clients[_toClient].account.level);
                _packet.Write(Server.clients[_toClient].account.accountLevel);
                _packet.Write(str1);
                _packet.Write(str2);
                ServerSend.SendTCPData(_toClient, _packet);
            }
        }

        public static void UpdateClientData(int _toClient)
        {
            using (Packet _packet = new Packet(36))
            {
                _packet.Write(_toClient);
                _packet.Write(Server.clients[_toClient].account.bio);
                _packet.Write(Server.clients[_toClient].account.accountLevel);
                _packet.Write(Server.clients[_toClient].account.level);
                ServerSend.SendTCPDataToWorld(Server.clients[_toClient].world, _packet);
            }
        }
    }
}
