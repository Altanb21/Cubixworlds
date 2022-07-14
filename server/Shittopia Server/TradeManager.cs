
using System;
using System.Collections.Generic;

namespace Shittopia_Server
{
    internal static class TradeManager
    {
        public static List<Trade> currentTrades;

        public static void CreateTrade(Client _client1, Client _client2)
        {
            ServerSend.CreateTrade(_client1.id);
            ServerSend.CreateTrade(_client2.id);
            Trade trade = new Trade();
            trade.client1 = _client1;
            trade.client2 = _client2;
            _client1.currentTrade = trade;
            _client2.currentTrade = trade;
        }

        public static void CancelTrade(Trade _trade, bool _giveItems)
        {
            ServerSend.RemoveTrade(_trade.client1.id);
            ServerSend.RemoveTrade(_trade.client2.id);
            if (_giveItems)
            {
                foreach (GameItem gameItem in _trade.items1)
                {
                    if (gameItem != null)
                        _trade.client1.account.inventory.AddItem(gameItem.id, gameItem.quantity);
                }
                foreach (GameItem gameItem in _trade.items2)
                {
                    if (gameItem != null)
                        _trade.client2.account.inventory.AddItem(gameItem.id, gameItem.quantity);
                }
                string str = "<color=red>Trade is cancelled!</color>";
                ChatManager.SendMessageToClient(_trade.client1.id, str, false, str);
                ChatManager.SendMessageToClient(_trade.client2.id, str, false, str);
            }
            _trade.client1.currentTrade = (Trade)null;
            _trade.client2.currentTrade = (Trade)null;
        }

        public static void AcceptTrade(Trade _trade, int _clientID)
        {
            try
            {
                if (_clientID == _trade.client1.id)
                {
                    _trade.isAcceptedClient1 = true;
                    string str = "<color=#81FF89>" + Server.clients[_clientID].player.username + "</color> has accepted the trade.";
                    ChatManager.SendMessageToClient(_trade.client2.id, str, false, str);
                }
                else if (_clientID == _trade.client2.id)
                {
                    _trade.isAcceptedClient2 = true;
                    string str = "<color=#81FF89>" + Server.clients[_clientID].player.username + "</color> has accepted the trade.";
                    ChatManager.SendMessageToClient(_trade.client1.id, str, false, str);
                }
                if (!_trade.isAcceptedClient1 || !_trade.isAcceptedClient2)
                    return;
                foreach (GameItem gameItem in _trade.items1)
                {
                    if (gameItem != null)
                    {
                        if (gameItem.id != 28)
                        {
                            _trade.client2.account.inventory.AddItem(gameItem.id, gameItem.quantity);
                            _trade.client1.account.inventory.RemoveItem(gameItem.id, gameItem.quantity);
                        }
                        else
                        {
                            _trade.client1.world.ownerName = _trade.client2.player.username;
                            _trade.client1.account.RemoveOwnedWorld(_trade.client1.world.name);
                            _trade.client2.account.AddOwnedWorld(_trade.client1.world.name);
                            string str = "World ownership has been changed, new owner is " + ChatManager.coloredTexts["green"] + _trade.client2.player.username + "</color>";
                            foreach (string accessedPlayer in _trade.client1.world.accessedPlayers)
                                _trade.client1.world.RemoveAccess(accessedPlayer);
                            ChatManager.SendMessageToWorld(_trade.client1.id, str, false, str);
                            _trade.client1.account.inventory.RemoveItem(gameItem.id, gameItem.quantity);
                        }
                    }
                }
                foreach (GameItem gameItem in _trade.items2)
                {
                    if (gameItem != null)
                    {
                        if (gameItem.id != 28)
                        {
                            _trade.client1.account.inventory.AddItem(gameItem.id, gameItem.quantity);
                            _trade.client2.account.inventory.RemoveItem(gameItem.id, gameItem.quantity);
                        }
                        else
                        {
                            _trade.client2.world.ownerName = _trade.client1.player.username;
                            string str = "World ownership has been changed, new owner is " + ChatManager.coloredTexts["green"] + _trade.client1.player.username + "</color>";
                            foreach (string accessedPlayer in _trade.client2.world.accessedPlayers)
                                _trade.client2.world.RemoveAccess(accessedPlayer);
                            ChatManager.SendMessageToWorld(_trade.client2.id, str, false, str);
                            _trade.client2.account.inventory.RemoveItem(gameItem.id, gameItem.quantity);
                        }
                    }
                }
                TradeManager.CancelTrade(_trade, false);
            }
            catch
            {
            }
        }

        public static void AddItem(int _itemID, int _quantity, Trade _trade, int _clientID)
        {
            try
            {
                if (Server.clients[_clientID].account.inventory.items[_itemID].quantity < _quantity || _quantity < 0)
                    return;
                if (_clientID == _trade.client1.id)
                {
                    for (int _row = 0; _row < _trade.items1.Length; ++_row)
                    {
                        if (_trade.items1[_row] == null)
                        {
                            if (_quantity != 0)
                            {
                                _trade.items1[_row] = new GameItem()
                                {
                                    id = _itemID,
                                    quantity = _quantity
                                };
                                ServerSend.AddItemToTrade(_trade.client1.id, _itemID, _trade.items1[_row].quantity, _row, _clientID);
                                ServerSend.AddItemToTrade(_trade.client2.id, _itemID, _trade.items1[_row].quantity, _row, _clientID);
                                string str = "<color=#81FF89>" + Server.clients[_clientID].player.username + "</color> has added an item to the trade.";
                                ChatManager.SendMessageToClient(_trade.client1.id, str, false, str);
                                ChatManager.SendMessageToClient(_trade.client2.id, str, false, str);
                                if (_trade.items1[_row].quantity != 0)
                                    break;
                                _trade.items1[_row] = (GameItem)null;
                                TradeManager.ReRowTrade(_trade);
                                break;
                            }
                        }
                        else if (_trade.items1[_row].id == _itemID)
                        {
                            if (_trade.items1[_row].quantity != 0)
                            {
                                _trade.items1[_row].quantity = _quantity;
                                ServerSend.AddItemToTrade(_trade.client1.id, _itemID, _trade.items1[_row].quantity, _row, _clientID);
                                ServerSend.AddItemToTrade(_trade.client2.id, _itemID, _trade.items1[_row].quantity, _row, _clientID);
                                string str = "<color=#81FF89>" + Server.clients[_clientID].player.username + "</color> has added an item to the trade.";
                                ChatManager.SendMessageToClient(_trade.client1.id, str, false, str);
                                ChatManager.SendMessageToClient(_trade.client2.id, str, false, str);
                                if (_trade.items1[_row].quantity != 0)
                                    break;
                                _trade.items1[_row] = (GameItem)null;
                                TradeManager.ReRowTrade(_trade);
                                break;
                            }
                            _trade.items1[_row] = (GameItem)null;
                        }
                    }
                }
                else
                {
                    if (_clientID != _trade.client2.id)
                        return;
                    for (int _row = 0; _row < _trade.items2.Length; ++_row)
                    {
                        if (_trade.items2[_row] == null)
                        {
                            if (_quantity != 0)
                            {
                                _trade.items2[_row] = new GameItem()
                                {
                                    id = _itemID,
                                    quantity = _quantity
                                };
                                ServerSend.AddItemToTrade(_trade.client1.id, _itemID, _trade.items2[_row].quantity, _row, _clientID);
                                ServerSend.AddItemToTrade(_trade.client2.id, _itemID, _trade.items2[_row].quantity, _row, _clientID);
                                string str = "<color=#81FF89>" + Server.clients[_clientID].player.username + "</color> has added an item to the trade.";
                                ChatManager.SendMessageToClient(_trade.client1.id, str, false, str);
                                ChatManager.SendMessageToClient(_trade.client2.id, str, false, str);
                                if (_trade.items2[_row].quantity != 0)
                                    break;
                                _trade.items2[_row] = (GameItem)null;
                                TradeManager.ReRowTrade(_trade);
                                break;
                            }
                        }
                        else if (_trade.items2[_row].id == _itemID)
                        {
                            if (_trade.items2[_row].quantity != 0)
                            {
                                GameItem gameItem = new GameItem();
                                _trade.items2[_row].quantity = _quantity;
                                ServerSend.AddItemToTrade(_trade.client1.id, _itemID, _trade.items2[_row].quantity, _row, _clientID);
                                ServerSend.AddItemToTrade(_trade.client2.id, _itemID, _trade.items2[_row].quantity, _row, _clientID);
                                string str = "<color=#81FF89>" + Server.clients[_clientID].player.username + "</color> has added an item to the trade.";
                                ChatManager.SendMessageToClient(_trade.client1.id, str, false, str);
                                ChatManager.SendMessageToClient(_trade.client2.id, str, false, str);
                                if (_trade.items2[_row].quantity != 0)
                                    break;
                                _trade.items2[_row] = (GameItem)null;
                                TradeManager.ReRowTrade(_trade);
                                break;
                            }
                            _trade.items2[_row] = (GameItem)null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Server.Log("An error has occured", ex.ToString());
            }
        }

        public static void ReRowTrade(Trade _trade)
        {
            Server.Log("asd");
            List<GameItem> gameItemList1 = new List<GameItem>();
            List<GameItem> gameItemList2 = new List<GameItem>();
            for (int _row = 0; _row < _trade.items1.Length; ++_row)
            {
                if (_trade.items1[_row] != null)
                {
                    gameItemList1.Add(_trade.items1[_row]);
                    ServerSend.AddItemToTrade(_trade.client1.id, _trade.items1[_row].id, 0, _row, _trade.client1.id);
                    ServerSend.AddItemToTrade(_trade.client2.id, _trade.items1[_row].id, 0, _row, _trade.client1.id);
                    _trade.items1[_row] = (GameItem)null;
                }
            }
            for (int _row = 0; _row < _trade.items2.Length; ++_row)
            {
                if (_trade.items2[_row] != null)
                {
                    gameItemList2.Add(_trade.items2[_row]);
                    ServerSend.AddItemToTrade(_trade.client1.id, _trade.items2[_row].id, 0, _row, _trade.client2.id);
                    ServerSend.AddItemToTrade(_trade.client2.id, _trade.items2[_row].id, 0, _row, _trade.client2.id);
                    _trade.items2[_row] = (GameItem)null;
                }
            }
            for (int index = 0; index < gameItemList1.Count; ++index)
                TradeManager.AddItem(gameItemList1[index].id, gameItemList1[index].quantity, _trade, _trade.client1.id);
            for (int index = 0; index < gameItemList2.Count; ++index)
                TradeManager.AddItem(gameItemList2[index].id, gameItemList2[index].quantity, _trade, _trade.client2.id);
        }
    }
}
