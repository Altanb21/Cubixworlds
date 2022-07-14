
using System;
using System.IO;
using System.Text.Json;

namespace Shittopia_Server
{
    internal static class Auth
    {
        public static string allowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public static int Login(string _username, string _password, Client _client)
        {
            try
            {
                if (!File.Exists(Server.path + "\\Accounts\\" + _username))
                    return 2;
                Account account = JsonSerializer.Deserialize<Account>(File.ReadAllText(Server.path + "\\Accounts\\" + _username));
                account.id = _client.id;
                Inventory inventory = new Inventory();
                inventory.Initialize();
                for (int index = 0; index < account.inventory.items.Length; ++index)
                    inventory.items[index] = account.inventory.items[index];
                account.inventory = inventory;
                int[] clothes = account.clothes;
                account.clothes = new int[8];
                for (int index = 0; index < clothes.Length; ++index)
                    account.clothes[index] = clothes[index];
                if (!(account.password == _password))
                    return 3;
                if (Server.FindIDFromUsernameNonPlayer(_username) != 0)
                    return 6;
                ServerSend.SendActiveWorlds(_client.id);
                _client.account = account;
                _client.isActive = true;
                return 0;
            }
            catch (Exception ex)
            {
                Server.Log("An error occured while trying to login an account!", ex.ToString());
                return 4;
            }
        }

        public static int Register(string _username, string _password, string _email, Client _client)
        {
            try
            {
                foreach (char ch in _username)
                {
                    bool flag = false;
                    foreach (char allowedCharacter in Auth.allowedCharacters)
                    {
                        if (ch.ToString().ToLower() == allowedCharacter.ToString().ToLower())
                            flag = true;
                    }
                    if (!flag)
                        return 4;
                }
                if (_username.Length > 12 || _username.Contains(' '))
                    return 4;
                if (_username == null || _password == null || _email == null)
                    return 5;
                if (File.Exists(Server.path + "/Accounts/" + _username))
                    return 1;
                Account account = new Account();
                account.id = _client.id;
                account.username = _username;
                account.password = _password;
                account.email = _email;
                account.cubix = 1000L;
                Inventory inventory = new Inventory();
                inventory.maxSlots = 1000;
                inventory.Initialize();
                account.inventory = inventory;
                Server.Log("Account is created successfully!", ConsoleColor.Green);
                _client.account = account;
                _client.isActive = true;
                return 0;
            }
            catch (Exception ex)
            {
                Server.Log("An error occured while trying to register an account!", ex.ToString());
                return 4;
            }
        }
    }
}
