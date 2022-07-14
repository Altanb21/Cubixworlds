using System;
using System.Collections.Generic;
using System.Linq;

namespace Shittopia_Server
{
    static class ChatManager
    {
        static Dictionary<string, string> commandUsageTexts = new Dictionary<string, string>(){
    {"/help", "<color=#FFEF89>/help</color> command gives you information about commands, usage is <color=#5CFF75>/help</color>"},
    {"/claim", $"<color=#FFEF89>/claim</color> command makes you claim the world, usage is <color=#5CFF75>/claim</color>"},
    {"/getworldregister", "<color=#FFEF89>/getworldregister</color> command lets you to get world register of the world, usage is <color=#5CFF75>/getworldregister</color>"},
    {"/access", "<color=#FFEF89>/access</color> command lets you to give access someone to the world you own, usage is <color=#5CFF75>/access <Player Username></color>"},
    {"/unaccess", "<color=#FFEF89>/unaccess</color> command lets you to take access from someone from the world you own, usage is <color=#5CFF75>/unaccess <Player Username></color>"},
    {"/world", "<color=#FFEF89>/world</color> command gives information about the world, usage is <color=#5CFF75>/world</color>"},
    {"/everyone", "<color=#FFEF89>/everyone</color> command sends message to everyone in-game, usage is <color=#5CFF75>/everyone <Message></color>"},
    {"/private", "<color=#FFEF89>/private</color> command sends message to player you want in-game, usage is <color=#5CFF75>/private <Player Username> <Message></color>"},
    {"/pull", "<color=#FFEF89>/private</color> command lets you to pull someone in your world, usage is <color=#5CFF75>/pull <Player Username></color>"},
    {"/trade", "<color=#FFEF89>/trade</color> command lets you to trade with someone, usage is <color=#5CFF75>/trade <Player Username></color>"},
    {"/report", "<color=#FFEF89>/report</color> command lets you to report a world or player, usage is <color=#5CFF75>/report <Report Description></color>"},
    {"/kick", "<color=#FFEF89>/kick</color> command lets you to kick someone, usage is <color=#5CFF75>/kick <Player Username></color>"},
    {"/friend", "<color=#FFEF89>/friend</color> command lets you to add someone as friend, usage is <color=#5CFF75>/friend <Player Username></color>"},
    {"/unfriend", "<color=#FFEF89>/unfriend</color> command lets you to remove someone from friends, usage is <color=#5CFF75>/unfriend <Player Username></color>"},
    {"/accept", "<color=#FFEF89>/accept</color> command lets you to accept last friend request, usage is <color=#5CFF75>/accept</color>"},
    {"/refuse", "<color=#FFEF89>/refuse</color> command lets you to refuse last friend request, usage is <color=#5CFF75>/refuse</color>"},
    {"/friends", "<color=#FFEF89>/refuse</color> command lets you to see all your friends, usage is <color=#5CFF75>/friends</color>"},
    {"/warp", "<color=#FFEF89>/warp</color> command lets you to warp to a world, usage is <color=#5CFF75>/warp <World Name></color>"},
    {"/?", "<color=#FFEF89>/?</color> command lets you to know what a command does, usage is <color=#5CFF75>/? <Command Name></color>"},
    {"/online", "<color=#FFEF89>/online</color> command lets you to see online count, usage is <color=#5CFF75>/online></color>"},
        };

        static Dictionary<string, string> modCommandUsageTexts = new Dictionary<string, string>()
        {
    {"/warn", "<color=#FFEF89>/warn</color> command lets you to warn someone, usage is <color=#5CFF75>/warn <Player Username> <Reason></color>"},
    {"/banworld", "<color=#FFEF89>/lock</color> command lets you to lock world, usage is <color=#5CFF75>/banworld</color>"},
    {"/unbanworld", "<color=#FFEF89>/unlock</color> command lets you to unlock world, usage is <color=#5CFF75>/unbanworld</color>"},
    {"/ghost", "<color=#FFEF89>/ghost</color> command lets you to be invisible, usage is <color=#5CFF75>/ghost</color>"},
    {"/noclip", "<color=#FFEF89>/noclip</color> command lets you to noclip, usage is <color=#5CFF75>/noclip</color>"},
    {"/warpto", "<color=#FFEF89>/warpto</color> command lets you to warp to chosen player's world, usage is <color=#5CFF75>/warpto <Player Username></color>"},
    {"/mute", "<color=#FFEF89>/mute</color> command lets you to mute someone, usage is <color=#5CFF75>/mute <Player Username></color>"},
    {"/unmute", "<color=#FFEF89>/unmute</color> command lets you to unmute someone, usage is <color=#5CFF75>/unmute <Player Username></color>"},
    {"/summon", "<color=#FFEF89>/summon</color> command lets you to summon any player to your world, usage is <color=#5CFF75>/summon <Player Username></color>"},
    {"/deviceban", "<color=#FFEF89>/deviceban</color> command lets you device ban users, usage is <color=#5CFF75>/deviceban <username> <days> <hours> <minutes>></color>"},
        };

        static Dictionary<string, string> adminCommandUsageTexts = new Dictionary<string, string>()
        {
    {"/give", "<color=#FFEF89>/give</color> command gives you item you want, usage is <color=#5CFF75>/give <Item ID> <Item Count></color>"},
    {"/find", "<color=#FFEF89>/find</color> command gives a list of items' ids that you are looking for, usage is <color=#5CFF75>/find <Item Name></color>"},
    {"/servermessage", "<color=#FFEF89>/servermessage</color> command lets you to send a warning message to all players in-game, usage is <color=#5CFF75>/servermessage <Text></color>"},
    {"/setpopularworld", "<color=#FFEF89>/setpopularworld</color> command lets you to set popular world, usage is <color=#5CFF75>/setpopularworld <World Name></color>"},
    {"/setrank", "<color=#FFEF89>/setrank</color> command lets you to set rank of someone, usage is <color=#5CFF75>/setrank <Username> <Rank ID></color>"},
    {"/setskin", "<color=#FFEF89>/setskin</color> command lets you to set skin of yourself, usage is <color=#5CFF75>/setskin <Skin ID></color>"},
        };

        static int lineCount;

        public static Dictionary<string, string> coloredTexts = new Dictionary<string, string>()
        {
            {"red", "<color=#FF5C5C>"},
            {"yellow", "<color=#FFEF89>"},
            {"green", "<color=#81FF89>"},
            {"blue", "<color=#7BA1FF>"},
            {"orange", "<color=#FFC97C>"},
            {"white", "<color=#FFFFFF>"},
            {"black", "<color=#000000>"}
        };

        public static void SendMessage(int _clientID, string _text, bool _isBubble)
        {
            if (Server.clients[_clientID].chatCooldown > DateTime.Now)
            {
                SendMessageToClient(_clientID, $"{ChatManager.coloredTexts["red"]}You are sending messages too fast, please wait.</color>", false, "");
                Server.clients[_clientID].chatCooldown = DateTime.Now.AddSeconds(1.5);

                return;
            }

            string[] _texts = _text.Split(' ');

            string _message = "";
            if (_text[0] == '/')
            {
                SendMessageToClient(_clientID, $"{coloredTexts["orange"]}" + _text + "</color>", false, _text);
                switch (_texts[0])
                {
                    case "/help":
                        if (_texts.Length == 1)
                        {
                            try
                            {
                                _message = $"Player Commands: {ChatManager.coloredTexts["green"]}";
                                foreach (string _commandUsage in commandUsageTexts.Keys)
                                {
                                    if (_message == $"Player Commands: {ChatManager.coloredTexts["green"]}")
                                    {
                                        _message += _commandUsage;
                                    }
                                    else
                                    {
                                        _message += $"</color>, {ChatManager.coloredTexts["green"]}" + _commandUsage;
                                    }
                                }

                                _message += "</color>";

                                if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5 || Server.clients[_clientID].account.accountLevel == 4)
                                {
                                    _message += $"\nModerator Commands: {ChatManager.coloredTexts["green"]}";
                                    foreach (string _commandUsage in modCommandUsageTexts.Keys)
                                    {
                                        if (_message[_message.Length - 1] == '>')
                                        {
                                            _message += _commandUsage;
                                        }
                                        else
                                        {
                                            _message += $"</color>, {ChatManager.coloredTexts["green"]}" + _commandUsage;
                                        }
                                    }

                                    _message += "</color>";
                                }

                                _message += "</color>";

                                if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5)
                                {
                                    _message += $"\nAdmin Commands: {ChatManager.coloredTexts["green"]}";
                                    foreach (string _commandUsage in adminCommandUsageTexts.Keys)
                                    {
                                        if (_message[_message.Length - 1] == '>')
                                        {
                                            _message += _commandUsage;
                                        }
                                        else
                                        {
                                            _message += $"</color>, {ChatManager.coloredTexts["green"]}" + _commandUsage;
                                        }
                                    }

                                    _message += "</color>";
                                }

                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                            catch (Exception _ex)
                            {
                                Server.Log("An error occured while applying the command.", _ex.ToString());
                                SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                            }
                        }
                        else
                        {
                            SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                        }
                        break;

                    case "/everyone":
                        if (_texts.Length >= 2)
                        {
                            try
                            {
                                _message += $"{ChatManager.coloredTexts["yellow"]}Message to everyone from <color=#5CFF75>" + Server.clients[_clientID].player.username + $"</color> in {ChatManager.coloredTexts["red"]}" + Server.clients[_clientID].world.name + "</color>:</color>";
                                for (int i = 1; i < _texts.Length; i++)
                                {
                                    _message += " " + _texts[i];
                                }

                                SendMessageToEveryone(_message);
                            }
                            catch
                            {
                                Server.Log("An error occured while applying the command.", "Item ID doesn't exist");
                                SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                            }
                        }
                        else
                        {
                            SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                        }
                        break;

                    case "/private":
                        if (_texts.Length >= 3)
                        {
                            int _id = Server.FindIDFromUsername(_texts[1]);

                            if (_id != 0)
                            {
                                try
                                {
                                    _message += $"{ChatManager.coloredTexts["yellow"]}Message to you from {ChatManager.coloredTexts["green"]}" + Server.clients[_clientID].player.username + $"</color> in {ChatManager.coloredTexts["red"]}" + Server.clients[_clientID].world.name + "</color>:</color>";
                                    for (int i = 2; i < _texts.Length; i++)
                                    {
                                        _message += " " + _texts[i];
                                    }

                                    string _selfMessage = $"{ChatManager.coloredTexts["yellow"]}Message is sent to {ChatManager.coloredTexts["green"]}" + Server.clients[_id].player.username + "</color>:</color>";
                                    for (int i = 2; i < _texts.Length; i++)
                                    {
                                        _selfMessage += " " + _texts[i];
                                    }

                                    SendMessageToClient(_id, _message, false, _text);
                                    SendMessageToClient(_clientID, _selfMessage, false, _text);
                                }
                                catch
                                {
                                    Server.Log("An error occured while applying the command.", "Item ID doesn't exist");
                                    SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Player is not online");
                                _message = $"{coloredTexts["red"]}Player is not online!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        else
                        {
                            SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                        }
                        break;

                    case "/give":
                        if (_texts.Length == 3)
                        {
                            if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5 || Server.clients[_clientID].account.accountLevel == 4)
                            {
                                try
                                {
                                    Server.clients[_clientID].account.inventory.AddItem(int.Parse(_texts[1]), int.Parse(_texts[2]));
                                    _message = $"{ChatManager.coloredTexts["green"]}" + _texts[2] + $"x</color> {ChatManager.coloredTexts["yellow"]}" + GameData.items[int.Parse(_texts[1])].name + "</color> is added to your inventory.";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                                catch
                                {
                                    Server.Log("An error occured while applying the command.", "Item ID doesn't exist");
                                    SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be staff!");
                                _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/find":
                        if (_texts.Length == 2)
                        {
                            if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5)
                            {
                                try
                                {
                                    _message = $"Result(s) for {coloredTexts["yellow"]}" + _texts[1] + "</color>:";
                                    foreach (Item _item in GameData.items)
                                    {
                                        if (_item.name.Contains(_texts[1]))
                                        {
                                            _message += $"\n{ChatManager.coloredTexts["blue"]}ID " + _item.id + $"</color> is {ChatManager.coloredTexts["yellow"]}" + _item.name + "</color>";
                                        }
                                    }

                                    if (_message == $"Result(s) for {coloredTexts["yellow"]}" + _texts[1] + ":")
                                    {
                                        string _errorMessage = $"{ChatManager.coloredTexts["red"]}No items found!</color>";
                                        SendMessageToClient(_clientID, _errorMessage, false, _text);
                                    }
                                    else
                                    {
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }

                                }
                                catch
                                {
                                    Server.Log("An error occured while applying the command.", "Item ID doesn't exist");
                                    SendMessageToClient(_clientID, adminCommandUsageTexts[_texts[0]], false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be staff!");
                                _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        else
                        {
                            SendMessageToClient(_clientID, adminCommandUsageTexts[_texts[0]], false, _text);
                        }
                        break;

                    case "/claim":
                        if (!Server.clients[_clientID].world.privateState)
                        {
                            if (Server.clients[_clientID].world.ownerName == null)
                            {
                                if (Server.clients[_clientID].world.ownerName != Server.clients[_clientID].player.username)
                                {
                                    if (Server.clients[_clientID].account.cubix >= Server.clients[_clientID].world.worth)
                                    {
                                        Server.clients[_clientID].account.RemoveCubix(Server.clients[_clientID].world.worth);
                                        Server.clients[_clientID].world.ownerName = Server.clients[_clientID].player.username;
                                        Server.clients[_clientID].account.AddOwnedWorld(Server.clients[_clientID].world.name);
                                        Server.clients[_clientID].world.privateState = true;

                                        Server.clients[_clientID].world.SaveAccessData();

                                        _message = $"{ChatManager.coloredTexts["green"]}Successfully claimed the world!</color>";

                                        ServerSend.Reply(_clientID, 2);
                                        string _worldMessage = $"{ChatManager.coloredTexts["green"]}" + Server.clients[_clientID].player.username + "</color> now owns the world!";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                        SendMessageToWorld(_clientID, _worldMessage, false, _worldMessage);
                                    }
                                    else
                                    {
                                        Server.Log("An error occured while applying the command.", "Not enough cubixs");
                                        _message = $"{coloredTexts["red"]}You don't have {Server.clients[_clientID].world.worth} cubixs!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "World is owned by client already");
                                    _message = $"{coloredTexts["red"]}You own this world already!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "World is owned already");
                                _message = $"{coloredTexts["red"]}World is already owned!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        else
                        {
                            Server.Log("An error occured while applying the command.", "World is owned already");
                            _message = $"{coloredTexts["red"]}World is already owned!</color>";
                            SendMessageToClient(_clientID, _message, false, _text);
                        }
                        break;

                    case "/getworldregister":
                        if (Server.clients[_clientID].world.privateState)
                        {
                            if (Server.clients[_clientID].world.ownerName == Server.clients[_clientID].player.username)
                            {
                                if (Server.clients[_clientID].account.inventory.items[28].quantity == 0)
                                {
                                    Server.clients[_clientID].account.inventory.AddItem(28, 1);
                                    _message = $"{ChatManager.coloredTexts["green"]}Successfully got the world register!</color>";
                                    string _worldMessage = $"{ChatManager.coloredTexts["green"]}" + Server.clients[_clientID].player.username + "</color> now got the world register!";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                    SendMessageToWorld(_clientID, _worldMessage, false, _worldMessage);
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Owner already has world register");
                                    _message = $"{coloredTexts["red"]}You own this world's register already!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Doesn't own the world");
                                _message = $"{coloredTexts["red"]}You must own this world!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/access":
                        if (Server.clients[_clientID].world.privateState && _texts.Length >= 2)
                        {
                            if (Server.clients[_clientID].world.ownerName == Server.clients[_clientID].player.username)
                            {
                                try
                                {
                                    if (!Server.clients[_clientID].world.accessedPlayers.Contains(_texts[1]))
                                    {
                                        Server.clients[_clientID].world.AddAccess(_texts[1]);

                                        _message = $"{ChatManager.coloredTexts["green"]}Successfully given access to " + _texts[1] + "!</color>";
                                        string _worldMessage = $"{ChatManager.coloredTexts["green"]}" + _texts[1] + "</color> now has access in the world!";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                        SendMessageToWorld(_clientID, _worldMessage, false, _worldMessage);
                                    }
                                    else
                                    {
                                        Server.Log("An error occured while applying the command.", "World is owned by client already");
                                        _message = $"{coloredTexts["red"]}Player owns this world already!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                }
                                catch
                                {
                                    Server.Log("An error occured while applying the command.", "Command is not usen correctly");
                                    SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Doesn't own the world");
                                _message = $"{coloredTexts["red"]}You must own this world!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/unaccess":
                        if (Server.clients[_clientID].world.privateState && _texts.Length >= 2)
                        {
                            if (Server.clients[_clientID].world.ownerName == Server.clients[_clientID].player.username)
                            {
                                try
                                {
                                    if (Server.clients[_clientID].world.accessedPlayers.Contains(_texts[1]))
                                    {
                                        Server.clients[_clientID].world.RemoveAccess(_texts[1]);

                                        _message = $"{ChatManager.coloredTexts["green"]}Successfully taken access from " + _texts[1] + "!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                    else
                                    {
                                        Server.Log("An error occured while applying the command.", "World is owned by client already");
                                        _message = $"{coloredTexts["red"]}Player doesn't have access in this world already!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                }
                                catch
                                {
                                    Server.Log("An error occured while applying the command.", "Command is not usen correctly");
                                    SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Doesn't own the world");
                                _message = $"{coloredTexts["red"]}You must own this world!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/world":
                        try
                        {
                            string _accessedPlayersText = "";

                            foreach (string _accessed in Server.clients[_clientID].world.accessedPlayers)
                            {
                                if (_accessedPlayersText == "")
                                {
                                    _accessedPlayersText += _accessed;
                                }
                                else
                                {
                                    _accessedPlayersText += ", " + _accessed;
                                }
                            }

                            _message = $"World Name: {ChatManager.coloredTexts["yellow"]}" + Server.clients[_clientID].world.name + "</color>" + "\n" + $"Owner: {ChatManager.coloredTexts["yellow"]}" + Server.clients[_clientID].world.ownerName + "</color>" + "\n" + $"Accessed Players: {ChatManager.coloredTexts["yellow"]}" + _accessedPlayersText + "</color>" + "\n" + $"World State: {ChatManager.coloredTexts["green"]}" + Server.clients[_clientID].world.worldState.ToString() + "</color>";
                            SendMessageToClient(_clientID, _message, false, _text);
                        }
                        catch
                        {
                            Server.Log("An error occured while applying the command.", ConsoleColor.Red);
                            SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                        }
                        break;

                    default:
                        _message = $"{coloredTexts["red"]}This command doesn't exist!</color>";
                        SendMessageToClient(_clientID, _message, false, _text);
                        break;

                    case "/pull":
                        if (_texts.Length >= 2)
                        {
                            try
                            {
                                if (Server.clients[_clientID].world.accessedPlayers.Contains(Server.clients[_clientID].player.username) || Server.clients[_clientID].world.ownerName == Server.clients[_clientID].player.username)
                                {
                                    if (Server.clients[_clientID].world.clients.Contains(Server.clients[Server.clients[_clientID].world.ContainsPlayer(_texts[1])]))
                                    {
                                        try
                                        {
                                            Server.clients[Server.clients[_clientID].world.ContainsPlayer(_texts[1])].player.SetPosition(Server.clients[_clientID].player.position);

                                            string _worldMessage = $"{ChatManager.coloredTexts["green"]}" + _texts[1] + $"</color> is pulled by {ChatManager.coloredTexts["green"]}" + Server.clients[_clientID].player.username + "</color>";
                                            SendMessageToWorld(_clientID, _worldMessage, false, _worldMessage);
                                        }
                                        catch
                                        {
                                            Server.Log("An error occured while applying the command.", "Command is not usen correctly");
                                            SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                                        }
                                    }
                                    else
                                    {
                                        Server.Log("An error occured while applying the command.", "Client doesn't exist");
                                        _message = $"{coloredTexts["red"]}Player must be in same world!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client doesn't have access");
                                    _message = $"{coloredTexts["red"]}You must have access to do this!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            catch
                            {
                                Server.Log("An error occured while applying the command.", "Client doesn't exist");
                                _message = $"{coloredTexts["red"]}Player must be in same world!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/trade":
                        if (_texts.Length >= 2)
                        {
                            try
                            {
                                if (Server.FindIDFromUsername(_texts[1]) != 0)
                                {
                                    if (Server.clients[Server.FindIDFromUsername(_texts[1])].world != null)
                                    {
                                        if (Server.clients[_clientID].currentTrade == null)
                                        {
                                            if (Server.clients[Server.FindIDFromUsername(_texts[1])].currentTrade == null)
                                            {
                                                if (Server.FindIDFromUsername(_texts[1]) != _clientID)
                                                {
                                                    try
                                                    {
                                                        TradeManager.CreateTrade(Server.clients[_clientID], Server.clients[Server.FindIDFromUsername(_texts[1])]);

                                                        string _tradeMessage = $"{ChatManager.coloredTexts["green"]}" + Server.clients[_clientID].player.username + "</color> has created a trade.";
                                                        string _tradeMessage1 = $"{ChatManager.coloredTexts["green"]}" + Server.clients[_clientID].player.username + "</color> is now trading with you.";
                                                        string _tradeMessage2 = $"{ChatManager.coloredTexts["green"]}" + _texts[1] + "</color> is now trading with you.";
                                                        SendMessageToClient(_clientID, _tradeMessage, false, _text);
                                                        SendMessageToClient(Server.FindIDFromUsername(_texts[1]), _tradeMessage, false, _text);

                                                        SendMessageToClient(_clientID, _tradeMessage2, false, _text);
                                                        SendMessageToClient(Server.FindIDFromUsername(_texts[1]), _tradeMessage1, false, _text);
                                                    }
                                                    catch (Exception _ex)
                                                    {
                                                        Server.Log("An error occured while applying the command.", "Command is not usen correctly");
                                                        SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                                                    }
                                                }
                                                else
                                                {
                                                    Server.Log("An error occured while applying the command.", "Client is trying to trade with itself");
                                                    _message = $"{coloredTexts["red"]}You cannot trade with yourself!</color>";
                                                    SendMessageToClient(_clientID, _message, false, _text);
                                                }
                                            }
                                            else
                                            {
                                                Server.Log("An error occured while applying the command.", "Client is already trading");
                                                _message = $"{coloredTexts["red"]}Player is already trading with someone!</color>";
                                                SendMessageToClient(_clientID, _message, false, _text);
                                            }
                                        }
                                        else
                                        {
                                            Server.Log("An error occured while applying the command.", "Client is already trading");
                                            _message = $"{coloredTexts["red"]}You are already trading with someone!</color>";
                                            SendMessageToClient(_clientID, _message, false, _text);
                                        }
                                    }
                                    else
                                    {
                                        Server.Log("An error occured while applying the command.", "Client isn't in a world");
                                        _message = $"{coloredTexts["red"]}Player must be in a world!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client doesn't exist");
                                    _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            catch
                            {
                                Server.Log("An error occured while applying the command.", "Client doesn't exist");
                                _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/banworld":
                        if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5 || Server.clients[_clientID].account.accountLevel == 4)
                        {
                            Client[] clients = new Client[Server.clients[_clientID].world.clients.Count];

                            for (int i = 0; i < Server.clients[_clientID].world.clients.Count; i++)
                            {
                                clients[i] = Server.clients[_clientID].world.clients[i];
                            }

                            foreach (Client _client in clients)
                            {
                                if (Server.clients[_client.id].account.accountLevel == 7 || Server.clients[_client.id].account.accountLevel == 5 || Server.clients[_client.id].account.accountLevel == 4)
                                {

                                }
                                else
                                {
                                    Server.clients[_client.id].LeaveFromWorld();
                                    ServerSend.SendNotification(_client.id, "<color=red>Warning", "World has been banned", 2.5f);
                                }
                            }

                            Server.clients[_clientID].world.worldState = 1;
                            Server.worlds[Server.clients[_clientID].world.id].worldState = 1;

                            _message = $"{ChatManager.coloredTexts["green"]}Successfully banned the world!</color>";
                            string _worldMessage = $"{ChatManager.coloredTexts["green"]}" + Server.clients[_clientID].player.username + "</color> banned the world!";
                            SendMessageToClient(_clientID, _message, false, _text);
                            SendMessageToWorld(_clientID, _worldMessage, false, _worldMessage);
                        }
                        else
                        {
                            Server.Log("An error occured while applying the command.", "Client must be staff!");
                            _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                            SendMessageToClient(_clientID, _message, false, _text);
                        }
                        break;

                    case "/unbanworld":
                        if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5 || Server.clients[_clientID].account.accountLevel == 4)
                        {
                            Server.clients[_clientID].world.worldState = 0;
                            Server.worlds[Server.clients[_clientID].world.id].worldState = 0;

                            _message = $"{ChatManager.coloredTexts["green"]}Successfully unbanned the world!</color>";
                            string _worldMessage = $"{ChatManager.coloredTexts["green"]}" + Server.clients[_clientID].player.username + "</color> unbanned the world!";
                            SendMessageToClient(_clientID, _message, false, _text);
                            SendMessageToWorld(_clientID, _worldMessage, false, _worldMessage);
                        }
                        else
                        {
                            Server.Log("An error occured while applying the command.", "Client must be staff!");
                            _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                            SendMessageToClient(_clientID, _message, false, _text);
                        }
                        break;

                    case "/report":
                        if (_texts.Length >= 2)
                        {
                            try
                            {
                                _message += $"{ChatManager.coloredTexts["green"]}Successfully reported!</color>";
                                string _rtext = "";
                                for (int i = 1; i < _texts.Length; i++)
                                {
                                    if (_rtext == "")
                                    {
                                        _rtext += _texts[i];
                                    }
                                    else
                                    {
                                        _rtext += " " + _texts[i];
                                    }
                                }

                                SendMessageToClient(_clientID, _message, false, _message);
                                ReportManager.CreateReport(_clientID, _rtext, 0);
                            }
                            catch (Exception _ex)
                            {
                                Server.Log("An error occured while applying the command.", _ex.ToString());
                                SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                            }
                        }
                        else
                        {
                            SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                        }
                        break;

                    case "/servermessage":
                        if (_texts.Length >= 2)
                        {
                            if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5)
                            {
                                try
                                {
                                    _message += $"{ChatManager.coloredTexts["green"]}Successfully sent!</color>";
                                    string _wtext = "";
                                    for (int i = 1; i < _texts.Length; i++)
                                    {
                                        if (_wtext == "")
                                        {
                                            _wtext += _texts[i];
                                        }
                                        else
                                        {
                                            _wtext += " " + _texts[i];
                                        }
                                    }

                                    SendMessageToClient(_clientID, _message, false, _message);
                                    SendMessageToEveryone($"{ChatManager.coloredTexts["red"]}Server Message</color>: " + _wtext);

                                    foreach (Client _client in Server.clients.Values)
                                    {
                                        if (_client.player != null)
                                        {
                                            ServerSend.SendNotification(_client.id, "<color=red>Server Message", _wtext, 5f);
                                        }
                                    }
                                }
                                catch (Exception _ex)
                                {
                                    Server.Log("An error occured while applying the command.", _ex.ToString());
                                    SendMessageToClient(_clientID, adminCommandUsageTexts[_texts[0]], false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be staff!");
                                _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        else
                        {
                            SendMessageToClient(_clientID, adminCommandUsageTexts[_texts[0]], false, _text);
                        }
                        break;

                    case "/warn":
                        if (_texts.Length >= 3)
                        {
                            if (Server.clients.ContainsKey(_clientID))
                            {
                                if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5 || Server.clients[_clientID].account.accountLevel == 4)
                                {
                                    if (Server.FindIDFromUsername(_texts[1]) != 0)
                                    {
                                        try
                                        {
                                            string _warningText = "";
                                            for (int i = 2; i < _texts.Length; i++)
                                            {
                                                if (_warningText == "")
                                                {
                                                    _warningText += _texts[i];
                                                }
                                                else
                                                {
                                                    _warningText += " " + _texts[i];
                                                }
                                            }

                                            _message = $"{ChatManager.coloredTexts["green"]}Successfully warned " + _texts[1] + "!</color>";
                                            SendMessageToClient(_clientID, _message, false, _text);

                                            string _warningMessage = $"{ChatManager.coloredTexts["red"]}Warning From Staff</color>: " + _warningText;
                                            SendMessageToClient(Server.FindIDFromUsername(_texts[1]), _warningMessage, false, _warningMessage);

                                            ServerSend.SendNotification(Server.FindIDFromUsername(_texts[1]), "<color=red>Warning", _warningText, 5f);
                                        }
                                        catch (Exception _ex)
                                        {
                                            Server.Log("An error occured while applying the command.", _ex.ToString());
                                            SendMessageToClient(_clientID, modCommandUsageTexts[_texts[0]], false, _text);
                                        }
                                    }
                                    else
                                    {
                                        Server.Log("An error occured while applying the command.", "Client must be online");
                                        _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client must be staff!");
                                    _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be online");
                                _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/kick":
                        if (_texts.Length >= 2)
                        {
                            if (Server.clients[_clientID].world.accessedPlayers.Contains(Server.clients[_clientID].player.username) || Server.clients[_clientID].world.ownerName == Server.clients[_clientID].player.username)
                            {
                                if (Server.clients[_clientID].world.clients.Contains(Server.clients[Server.clients[_clientID].world.ContainsPlayer(_texts[1])]))
                                {
                                    try
                                    {
                                        _message = $"{ChatManager.coloredTexts["green"]}Successfully kicked " + _texts[1] + "!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);

                                        Server.clients[Server.FindIDFromUsername(_texts[1])].LeaveFromWorld();
                                        ServerSend.SendNotification(Server.FindIDFromUsername(_texts[1]), "<color=red>Server Message", "You are kicked from world by staff", 4f);

                                    }
                                    catch
                                    {
                                        Server.Log("An error occured while applying the command.", "Command is not usen correctly");
                                        SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client must be in same world");
                                    _message = $"{coloredTexts["red"]}Player must be in same world!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be staff!");
                                _message = $"{ChatManager.coloredTexts["red"]}You must have access to do this!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/friend":
                        if (_texts.Length >= 2)
                        {
                            if (_texts[1] != Server.clients[_clientID].player.username)
                            {
                                if (Server.clients.ContainsKey(Server.FindIDFromUsername(_texts[1])))
                                {
                                    if (Server.clients[_clientID].account.friendList == null || !Server.clients[_clientID].account.friendList.ToList().Contains(_texts[1]))
                                    {
                                        try
                                        {
                                            _message = $"{ChatManager.coloredTexts["green"]}Successfully sent friend request to " + _texts[1] + "!</color>";
                                            SendMessageToClient(_clientID, _message, false, _message);

                                            int _requesterID = Server.FindIDFromUsername(_texts[1]);
                                            Server.clients[_requesterID].lastFriendRequestName = Server.clients[_clientID].player.username;
                                            string _friendMessage = $"You got a friend request from {ChatManager.coloredTexts["yellow"]}" + Server.clients[_clientID].player.username + $"</color>! Type {ChatManager.coloredTexts["green"]}/accept</color> to accept friend request and {ChatManager.coloredTexts["red"]}/refuse</color> to refuse friend request!";
                                            SendMessageToClient(_requesterID, _friendMessage, false, _friendMessage);

                                        }
                                        catch (Exception _ex)
                                        {
                                            Server.Log("An error occured while applying the command.", _ex.ToString());
                                            SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                                        }
                                    }
                                    else
                                    {
                                        Server.Log("An error occured while applying the command.", "Clients are already friend");
                                        _message = $"{coloredTexts["red"]}You are already friend with that player!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client must be online");
                                    _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client is trying to add himself as friend");
                                _message = $"{coloredTexts["red"]}You cannot add yourself as friend!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/unfriend":
                        if (_texts.Length >= 2)
                        {
                            if (Server.clients[_clientID].account.friendList == null || Server.clients[_clientID].account.friendList.ToList().Contains(_texts[1]))
                            {
                                try
                                {
                                    Server.clients[_clientID].account.RemoveFriend(_texts[1]);

                                    _message = $"{ChatManager.coloredTexts["green"]}Successfully removed " + _texts[1] + " from your friend list!</color>";
                                    SendMessageToClient(_clientID, _message, false, _message);
                                }
                                catch (Exception _ex)
                                {
                                    Server.Log("An error occured while applying the command.", _ex.ToString());
                                    SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Clients are already not friend");
                                _message = $"{coloredTexts["red"]}You are already not friend with that player!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/accept":
                        if (_texts.Length >= 1)
                        {
                            if (Server.clients.ContainsKey(Server.FindIDFromUsername(Server.clients[_clientID].lastFriendRequestName)))
                            {
                                if (Server.clients[_clientID].lastFriendRequestName != null)
                                {
                                    try
                                    {
                                        string _smessage = $"{ChatManager.coloredTexts["green"]}" + Server.clients[_clientID].player.username + "</color> has accepted your friend request!";
                                        string _message1 = $"You are now friend with {ChatManager.coloredTexts["green"]}" + Server.clients[_clientID].lastFriendRequestName + "!</color>";
                                        string _message2 = $"You are now friend with {ChatManager.coloredTexts["green"]}" + Server.clients[_clientID].player.username + "!</color>";

                                        int _senderID = Server.FindIDFromUsername(Server.clients[_clientID].lastFriendRequestName);

                                        SendMessageToClient(_clientID, _message1, false, _message);

                                        SendMessageToClient(_senderID, _smessage, false, _message);
                                        SendMessageToClient(_senderID, _message2, false, _message);

                                        Server.clients[_senderID].account.AddFriend(Server.clients[_clientID].player.username);
                                        Server.clients[_clientID].account.AddFriend(Server.clients[_clientID].lastFriendRequestName);

                                        Server.clients[_clientID].lastFriendRequestName = null;
                                    }
                                    catch (Exception _ex)
                                    {
                                        Server.Log("An error occured while applying the command.", _ex.ToString());
                                        SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client must have a friend request");
                                    _message = $"{coloredTexts["red"]}You don't have any friend requests!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be online");
                                _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/refuse":
                        if (_texts.Length >= 1)
                        {
                            if (Server.clients.ContainsKey(Server.FindIDFromUsername(Server.clients[_clientID].lastFriendRequestName)))
                            {
                                if (Server.clients[_clientID].lastFriendRequestName != null)
                                {
                                    try
                                    {
                                        string _rmessage1 = $"{ChatManager.coloredTexts["red"]}" + Server.clients[_clientID].player.username + "</color> has refused your friend request!";
                                        string _rmessage2 = $"{ChatManager.coloredTexts["green"]} You successfully refused the friend request!</color>";

                                        int _senderID = Server.FindIDFromUsername(Server.clients[_clientID].lastFriendRequestName);

                                        SendMessageToClient(_senderID, _rmessage1, false, _message);
                                        SendMessageToClient(_clientID, _rmessage2, false, _message);

                                        Server.clients[_clientID].lastFriendRequestName = null;
                                    }
                                    catch
                                    {
                                        Server.Log("An error occured while applying the command.", "Command is not usen correctly");
                                        SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client must have a friend request");
                                    _message = $"{coloredTexts["red"]}You don't have any friend requests!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be online");
                                _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/friends":
                        if (_texts.Length >= 1)
                        {
                            try
                            {
                                _message = "Your Friends: \n";

                                if (Server.clients[_clientID].account.friendList != null)
                                {
                                    foreach (string _friendName in Server.clients[_clientID].account.friendList)
                                    {
                                        if (_message != "Your Friends: \n")
                                        {
                                            if (Server.FindIDFromUsername(_friendName) != 0)
                                            {
                                                _message += $", {coloredTexts["green"]}" + _friendName + $"</color> in {coloredTexts["yellow"]}" + Server.clients[Server.FindIDFromUsername(_friendName)].world.name + "</color>";
                                            }
                                            else
                                            {
                                                _message += $", {coloredTexts["red"]}" + _friendName + "</color>";
                                            }
                                        }
                                        else
                                        {
                                            if (Server.FindIDFromUsername(_friendName) != 0)
                                            {
                                                _message += $"{coloredTexts["green"]}" + _friendName + $"</color> in {coloredTexts["yellow"]}" + Server.clients[Server.FindIDFromUsername(_friendName)].world.name + "</color>";
                                            }
                                            else
                                            {
                                                _message += $"{coloredTexts["red"]}" + _friendName + "</color>";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client doesn't have friends");
                                    _message = $"{coloredTexts["red"]}You don't have any friends!</color>";
                                }

                                SendMessageToClient(_clientID, _message, false, _message);
                            }
                            catch
                            {
                                Server.Log("An error occured while applying the command.", "Command is not usen correctly");
                                SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                            }
                        }
                        break;

                    case "/warp":
                        {
                            if (_texts.Length >= 2)
                            {
                                try
                                {
                                    Server.clients[_clientID].LeaveFromWorld();
                                    ServerSend.JoinWorld(_clientID, _texts[1]);
                                }
                                catch
                                {
                                    Server.Log("An error occured while applying the command.", "Command is not usen correctly");
                                    SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                                }
                            }
                        }
                        break;

                    case "/?":
                        {
                            if (_texts.Length >= 2)
                            {
                                try
                                {
                                    bool _isFound = false;
                                    foreach (string _command in commandUsageTexts.Keys)
                                    {
                                        string _commandToCheck = _command.Substring(1, _command.Length - 1);
                                        if (_texts[1] == _commandToCheck)
                                        {
                                            SendMessageToClient(_clientID, commandUsageTexts["/" + _texts[1]], false, _text);
                                            _isFound = true;
                                            break;
                                        }
                                    }

                                    if (!_isFound)
                                    {
                                        string _errorMessage = $"{coloredTexts["red"]}Command </color>{coloredTexts["yellow"]}{_texts[1]}</color>{coloredTexts["red"]} doesn't exist!</color>";
                                        SendMessageToClient(_clientID, _errorMessage, false, _text);
                                    }
                                }
                                catch (Exception _ex)
                                {
                                    Server.Log("An error occured while applying the command.", _ex.ToString());
                                    SendMessageToClient(_clientID, commandUsageTexts[_texts[0]], false, _text);
                                }
                            }
                        }
                        break;

                    case "/ghost":
                        {
                            if (_texts.Length >= 1)
                            {
                                try
                                {
                                    if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5 || Server.clients[_clientID].account.accountLevel == 4)
                                    {
                                        Server.clients[_clientID].isGhost = !Server.clients[_clientID].isGhost;

                                        if (Server.clients[_clientID].isGhost)
                                        {
                                            _message = $"{ChatManager.coloredTexts["green"]}Ghost mode is enabled!</color>";
                                            SendMessageToClient(_clientID, _message, false, _text);
                                        }
                                        else
                                        {
                                            _message = $"{ChatManager.coloredTexts["red"]}Ghost mode is disabled!</color>";
                                            SendMessageToClient(_clientID, _message, false, _text);
                                        }
                                    }
                                    else
                                    {
                                        Server.Log("An error occured while applying the command.", "Client must be staff!");
                                        _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                }
                                catch
                                {
                                    Server.Log("An error occured while applying the command.", "Command is not usen correctly");
                                    SendMessageToClient(_clientID, modCommandUsageTexts[_texts[0]], false, _text);
                                }
                            }
                        }
                        break;

                    case "/online":
                        _message = $"{ChatManager.coloredTexts["green"]}" + Server.clients.Count(t => t.Value.player != null) + "</color> online player(s) are in-game right now.";
                        SendMessageToClient(_clientID, _message, false, _text);
                        break;

                    case "/deviceban":
                        if (_texts.Length > 4)
                        {
                            if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5 || Server.clients[_clientID].account.accountLevel == 4)
                            {
                                if (Server.clients.ContainsKey(Server.FindIDFromUsername(_texts[1])))
                                {
                                    try
                                    {
                                        Security.BanGuid(Server.clients[Server.FindIDFromUsername(_texts[1])].guidID, new TimeSpan(int.Parse(_texts[2]), int.Parse(_texts[3]), int.Parse(_texts[4]), 0));
                                        Security.BanIP(Server.clients[Server.FindIDFromUsername(_texts[1])].tcp.socket.Client.RemoteEndPoint.ToString().Split(":")[0], new TimeSpan(4, 0, 0));

                                        Server.clients[Server.FindIDFromUsername(_texts[1])].Disconnect();

                                        _message = $"{ChatManager.coloredTexts["green"]}Successfully device banned " + _texts[1] + "!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);

                                        _message = $"{ChatManager.coloredTexts["red"]}" + _texts[1] + " is BANNED for breaking the game rules, play safe everyone!</color>";
                                        SendMessageToEveryone(_message);
                                    }
                                    catch (Exception _ex)
                                    {
                                        Server.Log("An error occured while applying the command.", _ex.ToString());
                                        SendMessageToClient(_clientID, modCommandUsageTexts[_texts[0]], false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client must be online");
                                    _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be staff!");
                                _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        else
                        {
                            Server.Log("An error occured while applying the command.", "Command is not usen correctly");
                            SendMessageToClient(_clientID, modCommandUsageTexts[_texts[0]], false, _text);
                        }
                        break;

                    case "/noclip":
                        {
                            if (_texts.Length >= 1)
                            {
                                try
                                {
                                    if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5 || Server.clients[_clientID].account.accountLevel == 4)
                                    {
                                        Server.clients[_clientID].isNoclip = !Server.clients[_clientID].isNoclip;

                                        if (Server.clients[_clientID].isNoclip)
                                        {
                                            ServerSend.SetStatus(_clientID, 0, Server.clients[_clientID].isNoclip);
                                            ServerSend.UpdateClientData(_clientID);

                                            _message = $"{ChatManager.coloredTexts["green"]}Noclip mode is enabled!</color>";
                                            SendMessageToClient(_clientID, _message, false, _text);
                                        }
                                        else
                                        {
                                            ServerSend.SetStatus(_clientID, 0, Server.clients[_clientID].isNoclip);
                                            ServerSend.UpdateClientData(_clientID);

                                            _message = $"{ChatManager.coloredTexts["red"]}Noclip mode is disabled!</color>";
                                            SendMessageToClient(_clientID, _message, false, _text);
                                        }
                                    }
                                    else
                                    {
                                        Server.Log("An error occured while applying the command.", "Client must be staff!");
                                        _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                }
                                catch
                                {
                                    Server.Log("An error occured while applying the command.", "Command is not usen correctly");
                                    SendMessageToClient(_clientID, modCommandUsageTexts[_texts[0]], false, _text);
                                }
                            }
                        }
                        break;

                    case "/warpto":
                        if (_texts.Length >= 2)
                        {
                            if (Server.clients.ContainsKey(_clientID))
                            {
                                if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5 || Server.clients[_clientID].account.accountLevel == 4)
                                {
                                    if (Server.FindIDFromUsername(_texts[1]) != 0)
                                    {
                                        try
                                        {
                                            string _worldName = Server.clients[Server.FindIDFromUsername(_texts[1])].world.name;

                                            Server.clients[_clientID].LeaveFromWorld();
                                            ServerSend.JoinWorld(_clientID, _worldName);
                                        }
                                        catch (Exception _ex)
                                        {
                                            Server.Log("An error occured while applying the command.", _ex.ToString());
                                            SendMessageToClient(_clientID, modCommandUsageTexts[_texts[0]], false, _text);
                                        }
                                    }
                                    else
                                    {
                                        Server.Log("An error occured while applying the command.", "Client must be online");
                                        _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client must be staff!");
                                    _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be online");
                                _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/summon":
                        if (_texts.Length >= 2)
                        {
                            if (Server.clients.ContainsKey(_clientID))
                            {
                                if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5 || Server.clients[_clientID].account.accountLevel == 4)
                                {
                                    if (Server.FindIDFromUsername(_texts[1]) != 0)
                                    {
                                        try
                                        {
                                            Server.clients[Server.FindIDFromUsername(_texts[1])].LeaveFromWorld();
                                            ServerSend.JoinWorld(Server.FindIDFromUsername(_texts[1]), Server.clients[_clientID].world.name);

                                            ServerSend.SendNotification(Server.FindIDFromUsername(_texts[1]), "<color=red>Summon", "You got summoned by a staff", 4f);
                                        }
                                        catch (Exception _ex)
                                        {
                                            Server.Log("An error occured while applying the command.", _ex.ToString());
                                            SendMessageToClient(_clientID, modCommandUsageTexts[_texts[0]], false, _text);
                                        }
                                    }
                                    else
                                    {
                                        Server.Log("An error occured while applying the command.", "Client must be online");
                                        _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client must be staff!");
                                    _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be online");
                                _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/mute":
                        if (_texts.Length >= 3)
                        {
                            if (Server.clients.ContainsKey(_clientID))
                            {
                                if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5 || Server.clients[_clientID].account.accountLevel == 4)
                                {
                                    if (Server.FindIDFromUsername(_texts[1]) != 0)
                                    {
                                        try
                                        {
                                            Server.clients[Server.FindIDFromUsername(_texts[1])].account.Mute(Convert.ToInt32(_texts[2]));

                                            ServerSend.SendNotification(Server.FindIDFromUsername(_texts[1]), "<color=red>Mute", "You got muted by a staff", 4f);

                                            _message = $"{ChatManager.coloredTexts["red"]}" + _texts[1] + " is MUTED for breaking the game rules, play safe everyone!</color>";
                                            SendMessageToEveryone(_message);
                                        }
                                        catch (Exception _ex)
                                        {
                                            Server.Log("An error occured while applying the command.", _ex.ToString());
                                            SendMessageToClient(_clientID, modCommandUsageTexts[_texts[0]], false, _text);
                                        }
                                    }
                                    else
                                    {
                                        Server.Log("An error occured while applying the command.", "Client must be online");
                                        _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client must be staff!");
                                    _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be online");
                                _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/unmute":
                        if (_texts.Length >= 1)
                        {
                            if (Server.clients.ContainsKey(_clientID))
                            {
                                if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5 || Server.clients[_clientID].account.accountLevel == 4)
                                {
                                    if (Server.FindIDFromUsername(_texts[1]) != 0)
                                    {
                                        try
                                        {
                                            Server.clients[Server.FindIDFromUsername(_texts[1])].account.Unmute();

                                            ServerSend.SendNotification(Server.FindIDFromUsername(_texts[1]), "<color=red>Mute", "You got unmuted by a staff", 4f);
                                        }
                                        catch (Exception _ex)
                                        {
                                            Server.Log("An error occured while applying the command.", _ex.ToString());
                                            SendMessageToClient(_clientID, modCommandUsageTexts[_texts[0]], false, _text);
                                        }
                                    }
                                    else
                                    {
                                        Server.Log("An error occured while applying the command.", "Client must be online");
                                        _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                        SendMessageToClient(_clientID, _message, false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client must be staff");
                                    _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be online");
                                _message = $"{coloredTexts["red"]}Player must be online!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        break;

                    case "/setpopularworld":
                        if (_texts.Length >= 2)
                        {
                            if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5)
                            {
                                try
                                {
                                    Server.popularWorld = _texts[1];
                                    _message += $"{ChatManager.coloredTexts["green"]}Successfully set the popular world!</color>";

                                    SendMessageToClient(_clientID, _message, false, _message);
                                }
                                catch (Exception _ex)
                                {
                                    Server.Log("An error occured while applying the command.", _ex.ToString());
                                    SendMessageToClient(_clientID, adminCommandUsageTexts[_texts[0]], false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be staff");
                                _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        else
                        {
                            SendMessageToClient(_clientID, adminCommandUsageTexts[_texts[0]], false, _text);
                        }
                        break;

                    case "/setrank":
                        if (_texts.Length >= 3)
                        {
                            if (Server.FindIDFromUsername(_texts[1]) != 0)
                            {
                                if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5)
                                {
                                    try
                                    {
                                        Server.clients[Server.FindIDFromUsername(_texts[1])].account.accountLevel = Convert.ToInt32(_texts[2]);
                                        _message += $"{ChatManager.coloredTexts["green"]}Successfully set the rank of the player!</color>";

                                        ServerSend.UpdateClientData(Server.FindIDFromUsername(_texts[1]));

                                        SendMessageToClient(_clientID, _message, false, _message);
                                    }
                                    catch (Exception _ex)
                                    {
                                        Server.Log("An error occured while applying the command.", _ex.ToString());
                                        SendMessageToClient(_clientID, adminCommandUsageTexts[_texts[0]], false, _text);
                                    }
                                }
                                else
                                {
                                    Server.Log("An error occured while applying the command.", "Client must be staff");
                                    _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                    SendMessageToClient(_clientID, _message, false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be online");
                                _message = $"{ChatManager.coloredTexts["red"]}Player must be online!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        else
                        {
                            SendMessageToClient(_clientID, adminCommandUsageTexts[_texts[0]], false, _text);
                        }
                        break;

                    case "/setskin":
                        if (_texts.Length >= 2)
                        {
                            if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5)
                            {
                                try
                                {
                                    _message += $"{ChatManager.coloredTexts["green"]}Successfully set the skin of yourself!</color>";

                                    ServerSend.UpdateClientData(_clientID);

                                    SendMessageToClient(_clientID, _message, false, _message);
                                }
                                catch (Exception _ex)
                                {
                                    Server.Log("An error occured while applying the command.", _ex.ToString());
                                    SendMessageToClient(_clientID, adminCommandUsageTexts[_texts[0]], false, _text);
                                }
                            }
                            else
                            {
                                Server.Log("An error occured while applying the command.", "Client must be staff");
                                _message = $"{ChatManager.coloredTexts["red"]}You must be a staff to do this!</color>";
                                SendMessageToClient(_clientID, _message, false, _text);
                            }
                        }
                        else
                        {
                            SendMessageToClient(_clientID, adminCommandUsageTexts[_texts[0]], false, _text);
                        }
                        break;
                }
            }
            else
            {
                if (!Server.clients[_clientID].account.isMuted)
                {
                    foreach (string _badWord in Server.badWords)
                    {
                        string[] _words = _text.Split(' ');
                        List<string> _newWords = new List<string>();

                        string _newText = "";

                        foreach (string _word in _words)
                        {
                            if (_word.ToLower().Contains(_badWord))
                            {
                                string _nWord = _word.ToLower().Replace(_badWord, new String('#', _badWord.Length));
                                _newWords.Add(_nWord);
                            }
                            else
                            {
                                _newWords.Add(_word);
                            }
                        }

                        foreach (string _newWord in _newWords)
                        {
                            if (_newText == "")
                            {
                                _newText += _newWord;
                            }
                            else
                            {
                                _newText += " " + _newWord;
                            }
                        }

                        _text = _newText;
                    }

                    _message = $"{ChatManager.coloredTexts["orange"]}<{ChatManager.coloredTexts["white"]}" + Server.clients[_clientID].player.username + "</color>></color> " + "<noparse>" + _text + "</noparse>";
                    SendMessageToWorld(_clientID, _message, _isBubble, wrapString("<noparse>" + _text + "</noparse>", 55));
                }
                else
                {
                    if (!Server.clients[_clientID].account.CheckForMute())
                    {
                        foreach (string _badWord in Server.badWords)
                        {
                            _text = _text.Replace(_badWord, new String('#', _badWord.Length));
                        }

                        _message = $"{ChatManager.coloredTexts["orange"]}<{ChatManager.coloredTexts["white"]}" + Server.clients[_clientID].player.username + "</color>></color> " + "<noparse>" + _text + "</noparse>";
                        SendMessageToWorld(_clientID, _message, _isBubble, wrapString("<noparse>" + _text + "</noparse>", 55));
                    }
                    else
                    {
                        string _mText = "";
                        for (int i = 0; i < _text.Length; i++)
                        {
                            int _c = new Random().Next(0, 6);

                            if (_c <= 2)
                            {
                                _mText += 'm';
                            }
                            else if (_c <= 4)
                            {
                                _mText += 'h';
                            }
                            else if ((_c <= 5))
                            {
                                _mText += 'f';
                            }
                        }

                        _message = $"{ChatManager.coloredTexts["orange"]}<{ChatManager.coloredTexts["white"]}" + Server.clients[_clientID].player.username + $"</color>></color> " + _mText;

                        SendMessageToWorld(_clientID, _message, _isBubble, wrapString(_mText, 55));
                    }
                }
            }

            Server.clients[_clientID].chatCooldown = DateTime.Now.AddSeconds(1.5);
        }

        public static void SendMessageToClient(int _clientID, string _text, bool _isBubble, string _bubbleText)
        {
            ServerSend.SendChatMessage(_clientID, _text);

            if (_isBubble)
            {
                ServerSend.SendChatBubble(_clientID, _clientID, _bubbleText, 2f);
            }
        }

        public static void SendMessageToWorld(int _clientID, string _text, bool _isBubble, string _bubbleText)
        {
            foreach (Client _client in Server.clients[_clientID].world.clients)
            {
                ServerSend.SendChatMessage(_client.id, _text);

                if (_isBubble)
                {
                    ServerSend.SendChatBubble(_client.id, _clientID, _bubbleText, 2f);
                }
            }
        }

        public static void SendMessageToEveryone(string _text)
        {
            for (int i = 1; i < Server.clients.Count; i++)
            {
                if (Server.clients[i].player != null)
                {
                    ServerSend.SendChatMessage(i, _text);
                }
            }
        }

        public static string wrapString(string msg, int width)
        {
            lineCount = 0;
            string[] words = msg.Split(" "[0]);
            string retVal = ""; //returning string 
            string NLstr = "";  //leftover string on new line
            for (int index = 0; index < words.Length; index++)
            {
                string word = words[index].Trim();
                //if word exceeds width
                if (words[index].Length >= width + 2)
                {
                    string[] temp = new string[5];
                    int i = 0;
                    while (words[index].Length > width)
                    { //word exceeds width, cut it at widrh
                        temp[i] = words[index].Substring(0, width) + "\n"; //cut the word at width
                        lineCount++;
                        words[index] = words[index].Substring(width);     //keep remaining word
                        i++;
                        if (words[index].Length <= width)
                        { //the balance is smaller than width
                            temp[i] = words[index];
                            NLstr = temp[i];
                        }
                    }
                    retVal += "\n";
                    lineCount++;
                    for (int x = 0; x < i + 1; x++)
                    { //loops through temp array
                        retVal = retVal + temp[x];
                    }
                }
                else if (index == 0)
                {
                    retVal = words[0];
                    NLstr = retVal;
                }
                else if (index > 0)
                {
                    if (NLstr.Length + words[index].Length <= width)
                    {
                        retVal = retVal + " " + words[index];
                        NLstr = NLstr + " " + words[index]; //add the current line length
                    }
                    else if (NLstr.Length + words[index].Length > width)
                    {
                        retVal = retVal + "\n" + words[index];
                        lineCount++;
                        NLstr = words[index]; //reset the line length
                    }
                }
            }
            if (retVal[0] == '\n')
                retVal = retVal.Substring(1, retVal.Length - 1);
            return retVal;
        }
    }
}
