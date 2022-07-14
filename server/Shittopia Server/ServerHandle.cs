
using System;
using System.Numerics;
using System.Threading;

namespace Shittopia_Server
{
    internal class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _toClient = _packet.ReadInt();
            string str1 = _packet.ReadString();
            string str2 = _packet.ReadString();
            string _nationality = _packet.ReadString();
            if (str2.Length > 12)
            {
                ServerSend.SendNotification(_toClient, "<color=red>Error", "World name is too long!", 2f);
                ServerSend.Reply(_toClient, 3);
            }
            else
            {
                foreach (char ch in str2)
                {
                    bool flag = false;
                    foreach (char allowedCharacter in Auth.allowedCharacters)
                    {
                        if (ch.ToString().ToLower() == allowedCharacter.ToString().ToLower())
                            flag = true;
                    }
                    if (!flag)
                    {
                        ServerSend.SendNotification(_toClient, "<color=red>Error", "You can't have <color=red>invalid symbols</color> in your world name!", 2f);
                        ServerSend.Reply(_toClient, 3);
                        return;
                    }
                }
                if (Server.clients[_fromClient].enteringWorld)
                    return;
                Server.Log(string.Format("{0} connected successfully and is now player {1}.", (object)Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint, (object)_fromClient), ConsoleColor.Green);
                if (_fromClient != _toClient)
                    Server.Log(string.Format("Player \"{0}\" (ID: {1}) has assumed the wrong client ID ({2})!", (object)str1, (object)_fromClient, (object)_toClient), ConsoleColor.Red);
                Server.clients[_fromClient].enteringWorld = true;
                Server.clients[_fromClient].SendIntoGame(Server.clients[_fromClient].account, str2.ToUpper(), _nationality);
            }
        }

        public static void AuthUser(int _fromClient, Packet _packet)
        {
            string _username = _packet.ReadString();
            string _password = _packet.ReadString();
            string _email = _packet.ReadString();
            bool _isLogin = _packet.ReadBool();
            string _version = _packet.ReadString();
            string _guid = _packet.ReadString();
            BanCheckResult banCheckResult = Security.CheckBan(Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint.ToString().Split(":")[0], _guid);
            if (banCheckResult.isGuidBanned)
                ServerSend.SendNotification(_fromClient, "<color=red>Error", string.Format("Sorry, this device's location has been banned for breaking game rules. The ban will expire in {0} Days, {1} Hours, {2} Minutes.", (object)(banCheckResult.guidExpireTime - DateTime.Now).Days, (object)(banCheckResult.guidExpireTime - DateTime.Now).Hours, (object)(banCheckResult.guidExpireTime - DateTime.Now).Minutes), 6f);
            else if (banCheckResult.isIPBanned)
            {
                ServerSend.SendNotification(_fromClient, "<color=red>Error", string.Format("Sorry, this device's location has been banned for breaking game rules. The ban will expire in {0} Days, {1} Hours, {2} Minutes.", (object)(banCheckResult.ipExpireTime - DateTime.Now).Days, (object)(banCheckResult.ipExpireTime - DateTime.Now).Hours, (object)(banCheckResult.ipExpireTime - DateTime.Now).Minutes), 6f);
            }
            else
            {
                Server.clients[_fromClient].guidID = _guid;
                Server.clients[_fromClient].SetAccount(_fromClient, _username, _password, _email, _isLogin, _version);
                GameManager.SendItemsDataToClient(_fromClient);
                if (Server.clients[_fromClient].account != null)
                    Server.clients[_fromClient].account.Save();
                ServerSend.SendInventoryData(_fromClient);
                ServerSend.SendNews(_fromClient);
            }
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            Vector2 _newPosition = _packet.ReadVector2();
            try
            {
                if (Server.clients[_fromClient].world != null)
                {
                    if (Server.clients[_fromClient].player != null)
                    {
                        if (Server.clients[_fromClient].world.worldLayers[0] != null)
                        {
                            if (Server.clients[_fromClient].isGhost)
                                Server.clients[_fromClient].player.position = new Vector2(-1f, -1f);
                            else if (Server.clients[_fromClient].account.accountLevel == 4 || Server.clients[_fromClient].account.accountLevel == 5 || Server.clients[_fromClient].account.accountLevel == 7)
                                Server.clients[_fromClient].player.position = _newPosition;
                            else if (!Security.CheckNoclip(Server.clients[_fromClient].player.position, _newPosition, _fromClient))
                            {
                                int index1 = (int)Math.Round((double)_newPosition.X);
                                int index2 = (int)Math.Round((double)_newPosition.Y);
                                Server.clients[_fromClient].player.damageTake = GameData.items[Server.clients[_fromClient].world.worldLayers[0][index1, index2].id].damage;
                                if (GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Round((double)_newPosition.X - 0.200000002980232), (int)Math.Round((double)_newPosition.Y)].id].waterPhysics || GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Round((double)_newPosition.X - 0.200000002980232), (int)Math.Round((double)_newPosition.Y)].id].waterPhysics)
                                {
                                    if (!Server.clients[_fromClient].player.isInWater)
                                        Server.clients[_fromClient].player.isInWater = true;
                                }
                                else if (Server.clients[_fromClient].player.isInWater)
                                {
                                    Server.clients[_fromClient].player.isInWater = false;
                                    Server.clients[_fromClient].player.maxY = float.MaxValue;
                                }
                                if (GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Round((double)_newPosition.X + 0.200000002980232), (int)Math.Round((double)_newPosition.Y - 0.550000011920929)].id].isSolid || GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Round((double)_newPosition.X - 0.200000002980232), (int)Math.Round((double)_newPosition.Y - 0.550000011920929)].id].isSolid)
                                {
                                    if (!Server.clients[_fromClient].player.isGrounded)
                                    {
                                        Server.clients[_fromClient].player.isGrounded = true;
                                        Server.clients[_fromClient].player.OnPlayerGrounded();
                                    }
                                }
                                else if (Server.clients[_fromClient].player.isGrounded)
                                {
                                    Server.clients[_fromClient].player.isGrounded = false;
                                    Server.clients[_fromClient].player.OnPlayerUnGrounded();
                                }
                                else if ((double)Server.clients[_fromClient].player.maxY < (double)_newPosition.Y && !Server.clients[_fromClient].player.isInWater)
                                {
                                    Server.clients[_fromClient].player.SetPosition(new Vector2(Server.clients[_fromClient].player.position.X, Server.clients[_fromClient].player.maxY));
                                    Server.clients[_fromClient].player.position = new Vector2(Server.clients[_fromClient].player.position.X, Server.clients[_fromClient].player.maxY);
                                    return;
                                }
                                if (!Server.clients[_fromClient].player.isGrounded && !Server.clients[_fromClient].player.isInWater)
                                {
                                    float num = MathF.Abs(_newPosition.X - Server.clients[_fromClient].player.position.X);
                                    Server.clients[_fromClient].player.triangleY -= num - 0.08f;
                                    if ((double)Server.clients[_fromClient].player.triangleY < (double)_newPosition.Y)
                                    {
                                        Server.clients[_fromClient].player.SetPosition(new Vector2(_newPosition.X, Server.clients[_fromClient].player.triangleY));
                                        Server.clients[_fromClient].player.position = new Vector2(_newPosition.X, Server.clients[_fromClient].player.triangleY);
                                        return;
                                    }
                                }
                                Server.clients[_fromClient].player.position = _newPosition;
                            }
                            else
                                Server.clients[_fromClient].player.SetPosition(Server.clients[_fromClient].player.position);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Server.Log("An error has occured", ex.ToString());
            }
            ServerSend.PlayerPosition(Server.clients[_fromClient].player);
        }

        public static void PlayerSize(int _fromClient, Packet _packet)
        {
            Vector2 vector2 = _packet.ReadVector2();
            try
            {
                if (Server.clients[_fromClient].player != null)
                    Server.clients[_fromClient].player.size = vector2;
            }
            catch (Exception ex)
            {
                Server.Log("An error has occured", ex.ToString());
            }
            if (Server.clients[_fromClient].isGhost)
                return;
            ServerSend.PlayerSize(Server.clients[_fromClient].player);
        }

        public static void EditWorldData(int _fromClient, Packet _packet)
        {
            try
            {
                int _itemID = _packet.ReadInt();
                Vector2 _mouseCoordinates = _packet.ReadVector2();
                if (Server.clients[_fromClient].world != null)
                {
                    if (Server.clients[_fromClient].player != null)
                    {
                        if ((double)_mouseCoordinates.X <= Math.Round((double)Server.clients[_fromClient].player.position.X) + 4.0)
                        {
                            if ((double)_mouseCoordinates.X >= Math.Round((double)Server.clients[_fromClient].player.position.X) - 3.0)
                            {
                                if ((double)_mouseCoordinates.Y <= (double)MathF.Round(Server.clients[_fromClient].player.position.Y) + 4.0)
                                {
                                    if ((double)_mouseCoordinates.Y >= Math.Round((double)Server.clients[_fromClient].player.position.Y) - 3.0)
                                    {
                                        bool flag = false;
                                        foreach (string accessedPlayer in Server.clients[_fromClient].world.accessedPlayers)
                                        {
                                            if (Server.clients[_fromClient].player.username == accessedPlayer)
                                            {
                                                flag = true;
                                                break;
                                            }
                                        }
                                        if (Server.clients[_fromClient].world.ownerName == Server.clients[_fromClient].player.username)
                                            flag = true;
                                        if (!Server.clients[_fromClient].world.privateState)
                                            flag = true;
                                        if (flag)
                                        {
                                            if (_itemID != 7)
                                            {
                                                if (Server.clients[_fromClient].account.inventory.items[_itemID].quantity > 0)
                                                {
                                                    if (GameData.items[_itemID].typeID == 1)
                                                    {
                                                        if (GameData.items[_itemID].layer == 0)
                                                        {
                                                            if (Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id == 0)
                                                            {
                                                                int _blockID = _itemID;
                                                                if ((int)Math.Floor((double)_mouseCoordinates.Y) != 0 && GameData.items[_itemID].isSolid && Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y) - 1].id == 3)
                                                                {
                                                                    Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y) - 1].id = 4;
                                                                    Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].isIDChanged = true;
                                                                    ServerSend.EditWorldData(_fromClient, 4, 0, Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y) - 1].health, new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)((int)Math.Floor((double)_mouseCoordinates.Y) - 1)));
                                                                }
                                                                if (_itemID == 3 && GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y) + 1].id].isSolid)
                                                                    _blockID = 4;
                                                                if (GameData.items[_blockID].isSign)
                                                                {
                                                                    Server.clients[_fromClient].world.signs.Add(new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)(int)Math.Floor((double)_mouseCoordinates.Y)), "Sign");
                                                                    ServerSend.SignData(_fromClient, new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)(int)Math.Floor((double)_mouseCoordinates.Y)), "Sign", 0);
                                                                }
                                                                Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id = _blockID;
                                                                Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].isIDChanged = true;
                                                                Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health = GameData.items[_blockID].health;
                                                                --Server.clients[_fromClient].account.inventory.items[_itemID].quantity;
                                                                ServerSend.EditWorldData(_fromClient, _blockID, 0, Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health, new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)(int)Math.Floor((double)_mouseCoordinates.Y)));
                                                                ServerSend.EditInventoryData(_fromClient, _itemID, Server.clients[_fromClient].account.inventory.items[_itemID].quantity);
                                                            }
                                                        }
                                                        else if (Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id == 0)
                                                        {
                                                            int _blockID = _itemID;
                                                            Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id = _blockID;
                                                            Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].isIDChanged = true;
                                                            Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health = GameData.items[_blockID].health;
                                                            ServerSend.EditWorldData(_fromClient, _blockID, 1, Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health, new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)(int)Math.Floor((double)_mouseCoordinates.Y)));
                                                            Server.clients[_fromClient].account.inventory.RemoveItem(_itemID, 1);
                                                            ServerSend.EditInventoryData(_fromClient, _itemID, Server.clients[_fromClient].account.inventory.items[_itemID].quantity);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    if (Server.clients[_fromClient].world == null)
                                                        return;
                                                    if (Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id != 0)
                                                    {
                                                        if (GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].health != -1)
                                                        {
                                                            if (Server.clients[_fromClient].account.clothes[6] != 0)
                                                                Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health -= GameData.items[Server.clients[_fromClient].account.clothes[6]].attack;
                                                            else
                                                                --Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health;
                                                            if (!Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].isChecking)
                                                                new Thread((ThreadStart)(() => Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].CheckForTimeOut(new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)(int)Math.Floor((double)_mouseCoordinates.Y)), 0, _fromClient))).Start();
                                                            else
                                                                Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].milisecondsToDie = 1000;
                                                            if (Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health <= 0)
                                                            {
                                                                if (GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].isSign)
                                                                {
                                                                    Server.clients[_fromClient].world.signs.Remove(new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)(int)Math.Floor((double)_mouseCoordinates.Y)));
                                                                    ServerSend.SignData(_fromClient, new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)(int)Math.Floor((double)_mouseCoordinates.Y)), "<color=red>ERROR, PLEASE REPORT THIS BUG!", 1);
                                                                }
                                                                Server.clients[_fromClient].account.AddCubix(new Random().Next(GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].gemDropRarityMin, GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].gemDropRarityMax));
                                                                Server.clients[_fromClient].account.Xp += new Random().Next(0, GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].rarity);
                                                                Server.clients[_fromClient].world.CreateDroppedItem(_fromClient, GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].itemDropID, new Random().Next(GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].itemDropRarityMin, GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].itemDropRarityMax), new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X) + (float)new Random().Next(-4, 4) / 10f, (float)(int)Math.Floor((double)_mouseCoordinates.Y)));
                                                                Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id = 0;
                                                                Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].isIDChanged = true;
                                                                Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health = GameData.items[0].health;
                                                                ServerSend.EditWorldData(_fromClient, 0, 0, Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y) - 1].health, new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)(int)Math.Floor((double)_mouseCoordinates.Y)));
                                                                if (Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y) - 1].id == 4)
                                                                {
                                                                    Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y) - 1].id = 3;
                                                                    Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].isIDChanged = true;
                                                                    ServerSend.EditWorldData(_fromClient, 3, 0, Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y) - 1].health, new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)((int)Math.Floor((double)_mouseCoordinates.Y) - 1)));
                                                                }
                                                            }
                                                            else
                                                                ServerSend.EditWorldData(_fromClient, Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id, 0, Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health, new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)(int)Math.Floor((double)_mouseCoordinates.Y)));
                                                        }
                                                    }
                                                    else if (Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id != 0)
                                                    {
                                                        if (GameData.items[Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].health != -1)
                                                        {
                                                            if (Server.clients[_fromClient].account.clothes[6] != 0)
                                                                Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health -= GameData.items[Server.clients[_fromClient].account.clothes[6]].attack;
                                                            else
                                                                --Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health;
                                                            if (!Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].isChecking)
                                                                new Thread((ThreadStart)(() => Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].CheckForTimeOut(new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)(int)Math.Floor((double)_mouseCoordinates.Y)), 1, _fromClient))).Start();
                                                            else
                                                                Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].milisecondsToDie = 1000;
                                                            if (Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health <= 0)
                                                            {
                                                                Server.clients[_fromClient].account.AddCubix(new Random().Next(GameData.items[Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].gemDropRarityMin, GameData.items[Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].gemDropRarityMax));
                                                                Server.clients[_fromClient].account.Xp += new Random().Next(0, GameData.items[Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].rarity);
                                                                Server.clients[_fromClient].world.CreateDroppedItem(_fromClient, GameData.items[Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].itemDropID, new Random().Next(GameData.items[Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].itemDropRarityMin, GameData.items[Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id].itemDropRarityMax), new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X) + (float)new Random().Next(-4, 4) / 10f, (float)(int)Math.Floor((double)_mouseCoordinates.Y)));
                                                                Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id = 0;
                                                                Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].isIDChanged = true;
                                                                Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health = GameData.items[0].health;
                                                                ServerSend.EditWorldData(_fromClient, 0, 1, Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y) - 1].health, new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)(int)Math.Floor((double)_mouseCoordinates.Y)));
                                                                if (Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y) - 1].id == 4)
                                                                {
                                                                    Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y) - 1].id = 3;
                                                                    Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].isIDChanged = true;
                                                                    ServerSend.EditWorldData(_fromClient, 3, 1, Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y) - 1].health, new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)((int)Math.Floor((double)_mouseCoordinates.Y) - 1)));
                                                                }
                                                            }
                                                            else
                                                                ServerSend.EditWorldData(_fromClient, Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].id, 1, Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)_mouseCoordinates.X), (int)Math.Floor((double)_mouseCoordinates.Y)].health, new Vector2((float)(int)Math.Floor((double)_mouseCoordinates.X), (float)(int)Math.Floor((double)_mouseCoordinates.Y)));
                                                        }
                                                    }
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string _text = "<color=red>World is not owned by you!</color>";
                                            ServerSend.SendChatBubble(_fromClient, _fromClient, _text, 2f);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Server.Log("An error occured while editing world data!", ex.ToString());
            }
            if (Server.clients[_fromClient].account != null)
                Server.clients[_fromClient].account.Save();
            if (Server.clients[_fromClient].world == null)
                return;
            Server.clients[_fromClient].world.SaveBlockData();
        }

        public static void TakeDroppedItem(int _fromClient, Packet _packet)
        {
            int num = _packet.ReadInt();
            int _itemID = _packet.ReadInt();
            int _quantity = _packet.ReadInt();
            _packet.ReadVector2();
            try
            {
                if (Server.clients[_fromClient].world == null || GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)Server.clients[_fromClient].world.droppedItems[num].positionX + 0.5), (int)Math.Floor((double)Server.clients[_fromClient].world.droppedItems[num].positionY)].id].isSolid || GameData.items[Server.clients[_fromClient].world.worldLayers[1][(int)Math.Floor((double)Server.clients[_fromClient].world.droppedItems[num].positionX + 0.5), (int)Math.Floor((double)Server.clients[_fromClient].world.droppedItems[num].positionY)].id].isSolid)
                    return;
                if (Server.clients[_fromClient].world.droppedItems[num].item.id == _itemID)
                {
                    if (Server.clients[_fromClient].world.droppedItems[num].item.quantity == _quantity)
                    {
                        Server.clients[_fromClient].account.inventory.AddItem(_itemID, _quantity);
                        ServerSend.RemoveDroppedItem(_fromClient, Server.clients[_fromClient].world.droppedItems[num]);
                        Server.clients[_fromClient].world.RemoveDroppedItem(num);
                    }
                }
            }
            catch
            {
            }
            if (Server.clients[_fromClient].world == null)
                return;
            Server.clients[_fromClient].world.SaveDropData();
        }

        public static void GetChatMessage(int _fromClient, Packet _packet)
        {
            string _text = _packet.ReadString();
            Server.Log("Message from " + Server.clients[_fromClient].player.username + ": " + _text, ConsoleColor.DarkYellow);
            if (!(_text != ""))
                return;
            ChatManager.SendMessage(_fromClient, _text, true);
        }

        public static void ItemAction(int _fromClient, Packet _packet)
        {
            int num = _packet.ReadInt();
            int _itemID = _packet.ReadInt();
            int _quantity = _packet.ReadInt();
            switch (num)
            {
                case 0:
                    if (Server.clients[_fromClient].account.inventory.items[_itemID].quantity >= _quantity && _quantity > 0)
                    {
                        if (GameData.items[_itemID].isDropable)
                        {
                            if ((double)Server.clients[_fromClient].player.size.X >= 0.0)
                            {
                                if (!GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)Server.clients[_fromClient].player.position.X + (double)Server.clients[_fromClient].player.size.X - 0.25), (int)Math.Floor((double)Server.clients[_fromClient].player.position.Y)].id].isSolid)
                                {
                                    Server.clients[_fromClient].account.inventory.RemoveItem(_itemID, _quantity);
                                    Server.clients[_fromClient].world.CreateDroppedItem(_fromClient, _itemID, _quantity, new Vector2((float)((double)Server.clients[_fromClient].player.position.X + (double)Server.clients[_fromClient].player.size.X - 0.25), Server.clients[_fromClient].player.position.Y));
                                    break;
                                }
                                string _text = "You cannot drop items inside a block!";
                                ServerSend.SendNotification(_fromClient, "<color=red>Error", _text, 3f);
                                break;
                            }
                            if (!GameData.items[Server.clients[_fromClient].world.worldLayers[0][(int)Math.Floor((double)Server.clients[_fromClient].player.position.X + (double)Server.clients[_fromClient].player.size.X + 0.25), (int)Math.Floor((double)Server.clients[_fromClient].player.position.Y)].id].isSolid)
                            {
                                Server.clients[_fromClient].account.inventory.RemoveItem(_itemID, _quantity);
                                Server.clients[_fromClient].world.CreateDroppedItem(_fromClient, _itemID, _quantity, new Vector2((float)((double)Server.clients[_fromClient].player.position.X + (double)Server.clients[_fromClient].player.size.X + 0.25), Server.clients[_fromClient].player.position.Y));
                                break;
                            }
                            string _text1 = "You cannot drop items inside a block!";
                            ServerSend.SendNotification(_fromClient, "<color=red>Error", _text1, 3f);
                            break;
                        }
                        string _text2 = "Item is not dropable!";
                        ServerSend.SendNotification(_fromClient, "<color=red>Error", _text2, 3f);
                        break;
                    }
                    string _text3 = "You don't have this much item!";
                    ServerSend.SendNotification(_fromClient, "<color=red>Error", _text3, 3f);
                    break;
                case 1:
                    if (Server.clients[_fromClient].account.inventory.items[_itemID].quantity >= _quantity && _quantity > 0)
                    {
                        if (GameData.items[_itemID].isTrashable)
                        {
                            Server.clients[_fromClient].account.inventory.RemoveItem(_itemID, _quantity);
                            break;
                        }
                        string _text4 = "Item is not trashable!";
                        ServerSend.SendNotification(_fromClient, "<color=red>Error", _text4, 3f);
                        break;
                    }
                    string _text5 = "You don't have this much item!";
                    ServerSend.SendNotification(_fromClient, "<color=red>Error", _text5, 3f);
                    break;
                case 2:
                    if (Server.clients[_fromClient].account.inventory.items[_itemID].quantity >= _quantity && _quantity > 0)
                    {
                        if (GameData.items[_itemID].isConvertable)
                        {
                            Server.clients[_fromClient].account.inventory.RemoveItem(_itemID, _quantity);
                            Server.clients[_fromClient].account.AddCubix(GameData.items[_itemID].rarity * _quantity);
                            break;
                        }
                        string _text6 = "Item is not convertable!";
                        ServerSend.SendNotification(_fromClient, "<color=red>Error", _text6, 3f);
                        break;
                    }
                    string _text7 = "You don't have this much item!";
                    ServerSend.SendNotification(_fromClient, "<color=red>Error", _text7, 3f);
                    break;
            }
            if (GameData.items[_itemID].typeID > 2 && Server.clients[_fromClient].account.inventory.items[_itemID].quantity <= 0)
            {
                switch (GameData.items[_itemID].typeID)
                {
                    case 3:
                        if (Server.clients[_fromClient].account.clothes[0] == _itemID)
                        {
                            Server.clients[_fromClient].account.clothes[0] = 0;
                            ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_itemID].typeID - 3);
                            break;
                        }
                        break;
                    case 4:
                        if (Server.clients[_fromClient].account.clothes[1] == _itemID)
                        {
                            Server.clients[_fromClient].account.clothes[1] = 0;
                            ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_itemID].typeID - 3);
                            break;
                        }
                        break;
                    case 5:
                        if (Server.clients[_fromClient].account.clothes[2] == _itemID)
                        {
                            Server.clients[_fromClient].account.clothes[2] = 0;
                            ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_itemID].typeID - 3);
                            break;
                        }
                        break;
                    case 6:
                        if (Server.clients[_fromClient].account.clothes[3] == _itemID)
                        {
                            Server.clients[_fromClient].account.clothes[3] = 0;
                            ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_itemID].typeID - 3);
                            break;
                        }
                        break;
                    case 7:
                        if (Server.clients[_fromClient].account.clothes[4] == _itemID)
                        {
                            Server.clients[_fromClient].account.clothes[4] = 0;
                            ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_itemID].typeID - 3);
                            break;
                        }
                        break;
                    case 8:
                        if (Server.clients[_fromClient].account.clothes[5] == _itemID)
                        {
                            Server.clients[_fromClient].account.clothes[5] = 0;
                            ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_itemID].typeID - 3);
                            break;
                        }
                        break;
                    case 9:
                        if (Server.clients[_fromClient].account.clothes[6] == _itemID)
                        {
                            Server.clients[_fromClient].account.clothes[6] = 0;
                            ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_itemID].typeID - 3);
                            break;
                        }
                        break;
                    case 10:
                        if (Server.clients[_fromClient].account.clothes[7] == _itemID)
                        {
                            Server.clients[_fromClient].account.clothes[7] = 0;
                            ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_itemID].typeID - 3);
                            break;
                        }
                        break;
                    default:
                        string _text8 = "You cannot wear this!";
                        ServerSend.SendNotification(_fromClient, "<color=red>Warning", _text8, 3f);
                        break;
                }
            }
            if (Server.clients[_fromClient].account != null)
                Server.clients[_fromClient].account.Save();
            if (Server.clients[_fromClient].world == null)
                return;
            Server.clients[_fromClient].world.SaveDropData();
        }

        public static void LeaveFromWorld(int _fromClient, Packet _packet)
        {
            _packet.ReadInt();
            ServerSend.UpdateAccountData(_fromClient);
            ServerSend.SendActiveWorlds(_fromClient);
            if (Server.clients[_fromClient].currentTrade != null)
                TradeManager.CancelTrade(Server.clients[_fromClient].currentTrade, false);
            Server.clients[_fromClient].account.inventory.SetItem(28, 0);
            ServerSend.DisconnectUser(_fromClient);
            World world = Server.clients[_fromClient].world;
            world.clients.Remove(Server.clients[_fromClient]);
            if (world.clients.Count == 0)
            {
                Server.Log(world.name + " is removed.", ConsoleColor.Blue);
                world.name = (string)null;
                world.worldLayers[0] = (Block[,])null;
                Server.worlds[world.id] = new World(world.id);
            }
            ServerSend.Reply(_fromClient, 1);
            Server.clients[_fromClient].enteringWorld = false;
        }

        public static void BuyPack(int _fromClient, Packet _packet)
        {
            int index1 = _packet.ReadInt();
            GameData.packs[1] = new Pack(1, 3, "Cubix Coin", string.Format("Buy 1 Cubix Coin for {0} coins! Created for big trades.", (object)Economy.cubixCoin.worth), (int)Economy.cubixCoin.worth, new int[1]
            {
        9
            }, new int[1] { 1 }, 8);
            if (Server.clients[_fromClient].account.cubix >= (long)GameData.packs[index1].cost)
            {
                Server.clients[_fromClient].account.RemoveCubix(GameData.packs[index1].cost);
                for (int index2 = 0; index2 < GameData.packs[index1].itemIDs.Length; ++index2)
                    Server.clients[_fromClient].account.inventory.AddItem(GameData.packs[index1].itemIDs[index2], GameData.packs[index1].itemCounts[index2]);
                if (index1 != 1)
                    return;
                Economy.Buy(1);
            }
            else
            {
                Server.Log(Server.clients[_fromClient].account.cubix.ToString() + ":" + GameData.packs[index1].cost.ToString());
                ServerSend.SendNotification(_fromClient, "<color=red>Error", "You don't have enough cubixs!", 3f);
            }
        }

        public static void ButtonClick(int _fromClient, Packet _packet)
        {
            if (_packet.ReadInt() != 0)
                return;
            Server.clients[_fromClient].player.SetHealth(0);
        }

        public static void TradeAction(int _fromClient, Packet _packet)
        {
            switch (_packet.ReadInt())
            {
                case 0:
                    TradeManager.AcceptTrade(Server.clients[_fromClient].currentTrade, _fromClient);
                    break;
                case 1:
                    TradeManager.CancelTrade(Server.clients[_fromClient].currentTrade, false);
                    break;
            }
        }

        public static void AddItemToTrade(int _fromClient, Packet _packet)
        {
            int _itemID = _packet.ReadInt();
            int _quantity = _packet.ReadInt();
            if (GameData.items[_itemID].isTradeable)
            {
                TradeManager.AddItem(_itemID, _quantity, Server.clients[_fromClient].currentTrade, _fromClient);
            }
            else
            {
                string _text = "You cannot add this item to trade!";
                ServerSend.SendNotification(_fromClient, "<color=red>Warning", _text, 3f);
            }
        }

        public static void EditSignText(int _fromClient, Packet _packet)
        {
            Vector2 vector2 = _packet.ReadVector2();
            string _signText = _packet.ReadString();
            if (_signText.Length > 50)
            {
                ServerSend.SendNotification(_fromClient, "<color=red>Error", "Sign text can't be too long!", 2f);
            }
            else
            {
                if (Server.clients[_fromClient].world == null)
                    return;
                bool flag = false;
                if (Server.clients[_fromClient].world.privateState && Server.clients[_fromClient].world.accessedPlayers.Contains(Server.clients[_fromClient].player.username) || Server.clients[_fromClient].world.ownerName == Server.clients[_fromClient].player.username)
                    flag = true;
                if (!Server.clients[_fromClient].world.privateState)
                    flag = true;
                if (flag)
                {
                    if (!Server.clients[_fromClient].world.signs.ContainsKey(new Vector2((float)Math.Floor((double)vector2.X), (float)Math.Floor((double)vector2.Y))))
                        return;
                    Server.clients[_fromClient].world.signs[new Vector2((float)Math.Floor((double)vector2.X), (float)Math.Floor((double)vector2.Y))] = "<noparse>" + _signText;
                    ServerSend.SignData(_fromClient, new Vector2((float)Math.Floor((double)vector2.X), (float)Math.Floor((double)vector2.Y)), _signText, 2);
                    Server.clients[_fromClient].world.SaveSignsData();
                }
                else
                {
                    string _text = "You cannot edit the sign text!";
                    ServerSend.SendNotification(_fromClient, "<color=red>Warning", _text, 3f);
                }
            }
        }

        public static void WearCostume(int _fromClient, Packet _packet)
        {
            int _costumeID = _packet.ReadInt();
            if (Server.clients[_fromClient].account.inventory.items[_costumeID].quantity > 0)
            {
                switch (GameData.items[_costumeID].typeID)
                {
                    case 3:
                        if (Server.clients[_fromClient].account.clothes[0] != _costumeID)
                        {
                            Server.clients[_fromClient].account.clothes[0] = _costumeID;
                            ServerSend.EditCostumeData(_fromClient, _costumeID, GameData.items[_costumeID].typeID - 3);
                            break;
                        }
                        Server.clients[_fromClient].account.clothes[0] = 0;
                        ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_costumeID].typeID - 3);
                        break;
                    case 4:
                        if (Server.clients[_fromClient].account.clothes[1] != _costumeID)
                        {
                            Server.clients[_fromClient].account.clothes[1] = _costumeID;
                            ServerSend.EditCostumeData(_fromClient, _costumeID, GameData.items[_costumeID].typeID - 3);
                            break;
                        }
                        Server.clients[_fromClient].account.clothes[1] = 0;
                        ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_costumeID].typeID - 3);
                        break;
                    case 5:
                        if (Server.clients[_fromClient].account.clothes[2] != _costumeID)
                        {
                            Server.clients[_fromClient].account.clothes[2] = _costumeID;
                            ServerSend.EditCostumeData(_fromClient, _costumeID, GameData.items[_costumeID].typeID - 3);
                            break;
                        }
                        Server.clients[_fromClient].account.clothes[2] = 0;
                        ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_costumeID].typeID - 3);
                        break;
                    case 6:
                        if (Server.clients[_fromClient].account.clothes[3] != _costumeID)
                        {
                            Server.clients[_fromClient].account.clothes[3] = _costumeID;
                            ServerSend.EditCostumeData(_fromClient, _costumeID, GameData.items[_costumeID].typeID - 3);
                            break;
                        }
                        Server.clients[_fromClient].account.clothes[3] = 0;
                        ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_costumeID].typeID - 3);
                        break;
                    case 7:
                        if (Server.clients[_fromClient].account.clothes[4] != _costumeID)
                        {
                            Server.clients[_fromClient].account.clothes[4] = _costumeID;
                            ServerSend.EditCostumeData(_fromClient, _costumeID, GameData.items[_costumeID].typeID - 3);
                            break;
                        }
                        Server.clients[_fromClient].account.clothes[4] = 0;
                        ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_costumeID].typeID - 3);
                        break;
                    case 8:
                        if (Server.clients[_fromClient].account.clothes[5] != _costumeID)
                        {
                            Server.clients[_fromClient].account.clothes[5] = _costumeID;
                            ServerSend.EditCostumeData(_fromClient, _costumeID, GameData.items[_costumeID].typeID - 3);
                            break;
                        }
                        Server.clients[_fromClient].account.clothes[5] = 0;
                        ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_costumeID].typeID - 3);
                        break;
                    case 9:
                        if (Server.clients[_fromClient].account.clothes[6] != _costumeID)
                        {
                            Server.clients[_fromClient].account.clothes[6] = _costumeID;
                            ServerSend.EditCostumeData(_fromClient, _costumeID, GameData.items[_costumeID].typeID - 3);
                            break;
                        }
                        Server.clients[_fromClient].account.clothes[6] = 0;
                        ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_costumeID].typeID - 3);
                        break;
                    case 10:
                        if (Server.clients[_fromClient].account.clothes[7] != _costumeID)
                        {
                            Server.clients[_fromClient].account.clothes[7] = _costumeID;
                            ServerSend.EditCostumeData(_fromClient, _costumeID, GameData.items[_costumeID].typeID - 3);
                            break;
                        }
                        Server.clients[_fromClient].account.clothes[7] = 0;
                        ServerSend.EditCostumeData(_fromClient, 0, GameData.items[_costumeID].typeID - 3);
                        break;
                    default:
                        string _text1 = "You cannot wear this!";
                        ServerSend.SendNotification(_fromClient, "<color=red>Warning", _text1, 3f);
                        break;
                }
            }
            else
            {
                string _text2 = "You cannot wear this!";
                ServerSend.SendNotification(_fromClient, "<color=red>Warning", _text2, 3f);
            }
            if (Server.clients[_fromClient].account == null)
                return;
            Server.clients[_fromClient].account.Save();
        }

        public static void ChangeAnimation(int _fromClient, Packet _packet)
        {
            int _animationID = _packet.ReadInt();
            if (!Server.clients.ContainsKey(_fromClient) || Server.clients[_fromClient].player == null)
                return;
            Server.clients[_fromClient].player.animationID = _animationID;
            ServerSend.ChangeAnimation(_fromClient, _animationID);
        }

        public static void ChangeBio(int _fromClient, Packet _packet)
        {
            string str = _packet.ReadString();
            if (!Server.clients.ContainsKey(_fromClient) || Server.clients[_fromClient].account == null)
                return;
            Server.clients[_fromClient].account.bio = str;
            ServerSend.SendNotification(_fromClient, "<color=green>Successful", "You have successfully edited your profile!", 2f);
        }

        public static void ChangeAuthData(int _fromClient, Packet _packet)
        {
            string str1 = _packet.ReadString();
            string str2 = _packet.ReadString();
            switch (_packet.ReadInt())
            {
                case 0:
                    if (Server.clients[_fromClient].account == null)
                        break;
                    if (Server.clients[_fromClient].account.password == str1)
                    {
                        if (str2 != "")
                        {
                            Server.clients[_fromClient].account.password = str2;
                            Server.clients[_fromClient].account.Save();
                            ServerSend.SendNotification(_fromClient, "<color=green>Successful", "You have successfully changed your password!", 2f);
                            break;
                        }
                        ServerSend.SendNotification(_fromClient, "<color=red>Error", "Password Input cannot be empty!", 2f);
                        break;
                    }
                    ServerSend.SendNotification(_fromClient, "<color=red>Error", "Old password doesn't match!", 2f);
                    break;
                case 1:
                    if (Server.clients[_fromClient].account == null)
                        break;
                    if (Server.clients[_fromClient].account.password == str1)
                    {
                        if (str2 != "")
                        {
                            Server.clients[_fromClient].account.email = str2;
                            Server.clients[_fromClient].account.Save();
                            ServerSend.SendNotification(_fromClient, "<color=green>Successful", "You have successfully changed your email!", 2f);
                            break;
                        }
                        ServerSend.SendNotification(_fromClient, "<color=red>Error", "Email Input cannot be empty!", 2f);
                        break;
                    }
                    ServerSend.SendNotification(_fromClient, "<color=red>Error", "Old password doesn't match!", 2f);
                    break;
            }
        }
    }
}
