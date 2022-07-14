
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text.Json;
using System.Threading;

namespace Shittopia_Server
{
    internal class Security
    {
        public static long totalConnectionsMadePerMinute = 0;
        public static Dictionary<string, long> loggedConnectionsPerIP = new Dictionary<string, long>();
        public static Dictionary<string, int> connectionPerClient = new Dictionary<string, int>();

        public static BanCheckResult CheckBan(string _IP, string _guid)
        {
            BanCheckResult banCheckResult = new BanCheckResult();
            if (File.Exists(Server.path + "DeviceBans.json"))
            {
                IPBans ipBans = JsonSerializer.Deserialize<IPBans>(File.ReadAllText("C://CubixWorlds/DeviceBans.json"));
                if (ipBans.ipBans.ContainsKey(_IP) && ipBans.ipBans[_IP].Ticks > DateTime.Now.Ticks)
                {
                    banCheckResult.isIPBanned = true;
                    banCheckResult.ipExpireTime = ipBans.ipBans[_IP];
                }
                if (ipBans.guidBans.ContainsKey(_guid) && ipBans.guidBans[_guid].Ticks > DateTime.Now.Ticks)
                {
                    banCheckResult.isGuidBanned = true;
                    banCheckResult.guidExpireTime = ipBans.guidBans[_guid];
                }
            }
            else
            {
                File.WriteAllText(Server.path + "DeviceBans.json", JsonSerializer.Serialize<IPBans>(new IPBans()
                {
                    ipBans = new Dictionary<string, DateTime>(),
                    guidBans = new Dictionary<string, DateTime>()
                }));
                banCheckResult.isIPBanned = false;
                banCheckResult.isGuidBanned = false;
            }
            return banCheckResult;
        }

        public static bool BanIP(string _IP, TimeSpan _Time)
        {
            DateTime dateTime = DateTime.Now.Add(_Time);
            if (File.Exists(Server.path + "DeviceBans.json"))
            {
                IPBans ipBans = JsonSerializer.Deserialize<IPBans>(File.ReadAllText("C://CubixWorlds/DeviceBans.json"));
                if (ipBans.ipBans.ContainsKey(_IP))
                    ipBans.ipBans[_IP] = dateTime;
                else
                    ipBans.ipBans.Add(_IP, dateTime);
                File.WriteAllText(Server.path + "DeviceBans.json", JsonSerializer.Serialize<IPBans>(ipBans));
            }
            else
            {
                IPBans ipBans = new IPBans()
                {
                    ipBans = new Dictionary<string, DateTime>(),
                    guidBans = new Dictionary<string, DateTime>()
                };
                ipBans.ipBans.Add(_IP, dateTime);
                File.WriteAllText(Server.path + "DeviceBans.json", JsonSerializer.Serialize<IPBans>(ipBans));
            }
            return true;
        }

        public static bool BanGuid(string _guid, TimeSpan _Time)
        {
            DateTime dateTime = DateTime.Now.Add(_Time);
            if (File.Exists(Server.path + "DeviceBans.json"))
            {
                IPBans ipBans = JsonSerializer.Deserialize<IPBans>(File.ReadAllText(Server.path + "DeviceBans.json"));
                if (ipBans.guidBans.ContainsKey(_guid))
                    ipBans.guidBans[_guid] = dateTime;
                else
                    ipBans.guidBans.Add(_guid, dateTime);
                File.WriteAllText(Server.path + "DeviceBans.json", JsonSerializer.Serialize<IPBans>(ipBans));
            }
            else
            {
                IPBans ipBans = new IPBans()
                {
                    ipBans = new Dictionary<string, DateTime>(),
                    guidBans = new Dictionary<string, DateTime>()
                };
                ipBans.guidBans.Add(_guid, dateTime);
                File.WriteAllText(Server.path + "DeviceBans.json", JsonSerializer.Serialize<IPBans>(ipBans));
            }
            return true;
        }

        public static bool ClientConnect(string _ip)
        {
            if (Security.connectionPerClient.ContainsKey(_ip))
            {
                if (Security.connectionPerClient[_ip] == 3)
                    return false;
                Security.connectionPerClient[_ip]++;
                return true;
            }
            if (!Security.connectionPerClient.ContainsKey(_ip))
                Security.connectionPerClient.Add(_ip, 1);
            else
                Security.connectionPerClient[_ip] = 1;
            return true;
        }

        public static void ClientDisconnect(string _ip)
        {
            if (!Security.connectionPerClient.ContainsKey(_ip))
                return;
            Security.connectionPerClient[_ip]--;
        }

        public static bool CheckLoadBalancer(string _ip)
        {
            ++Security.totalConnectionsMadePerMinute;
            if (Security.loggedConnectionsPerIP.ContainsKey(_ip))
            {
                Security.loggedConnectionsPerIP[_ip]++;
                if (Security.loggedConnectionsPerIP[_ip] > 20L)
                {
                    Security.BanIP(_ip, new TimeSpan(2, 30, 0));
                    return false;
                }
            }
            else
                Security.loggedConnectionsPerIP.Add(_ip, 1L);
            return Security.totalConnectionsMadePerMinute > 90L;
        }

        public static void UpdateLoadBalancer()
        {
        label_0:
            Thread.Sleep(6000);
            Security.totalConnectionsMadePerMinute = 0L;
            using (Dictionary<string, long>.Enumerator enumerator = Security.loggedConnectionsPerIP.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, long> current = enumerator.Current;
                    Security.loggedConnectionsPerIP.Remove(current.Key);
                }
                goto label_0;
            }
        }

        public static void LogError(string _error)
        {
            if (File.Exists("ErrorLog.txt"))
            {
                File.ReadAllText("ErrorLog.txt");
                File.WriteAllText("ErrorLog.txt", Environment.NewLine + string.Format("[{0}] {1}", (object)DateTime.Now, (object)_error));
            }
            else
                File.WriteAllText("ErrorLog.txt", string.Format("[{0}] {1}", (object)DateTime.Now, (object)_error));
        }

        public static bool CheckNoclip(Vector2 _oldPosition, Vector2 _newPosition, int _clientID)
        {
            if ((double)MathF.Abs(_newPosition.X - _oldPosition.X) > 0.300000011920929 || (double)MathF.Abs(_newPosition.Y - _oldPosition.Y) > 0.5)
                return true;
            if (Server.clients[_clientID].account.accountLevel == 7 || Server.clients[_clientID].account.accountLevel == 5 || Server.clients[_clientID].account.accountLevel == 4 || Server.clients[_clientID].world == null || Server.clients[_clientID].world.worldLayers[0] == null)
                return false;
            bool flag = false;
            try
            {
                if ((int)Math.Round((double)_newPosition.X) >= 0)
                {
                    if ((int)Math.Round((double)_newPosition.X) <= Server.clients[_clientID].world.worldLayers[0].GetLength(0))
                    {
                        if ((int)Math.Round((double)_newPosition.Y) >= 0)
                        {
                            if ((int)Math.Round((double)_newPosition.Y) <= Server.clients[_clientID].world.worldLayers[0].GetLength(1))
                            {
                                if ((double)Vector2.Distance(_oldPosition, _newPosition) > 2.0 || GameData.items[Server.clients[_clientID].world.worldLayers[0][(int)Math.Floor((double)_newPosition.X - 0.200000002980232 + 0.5), (int)Math.Floor((double)_newPosition.Y + 0.5)].id].isSolid || GameData.items[Server.clients[_clientID].world.worldLayers[0][(int)Math.Floor((double)_newPosition.X + 0.200000002980232 + 0.5), (int)Math.Floor((double)_newPosition.Y + 0.5)].id].isSolid)
                                    return true;
                                Vector2[] vector2Array = new Vector2[4];
                                for (int index = 0; index < vector2Array.Length; ++index)
                                {
                                    vector2Array[index] = new Vector2((float)(((double)_newPosition.X - (double)_oldPosition.X) / 3.0) * (float)index + _oldPosition.X, (float)(((double)_newPosition.Y - (double)_oldPosition.Y) / 3.0) * (float)index + _oldPosition.Y);
                                    if (GameData.items[Server.clients[_clientID].world.worldLayers[0][(int)Math.Floor((double)vector2Array[index].X + 0.5), (int)Math.Floor((double)vector2Array[index].Y + 0.5)].id].isSolid)
                                        return true;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return true;
            }
            return flag;
        }
    }
}
