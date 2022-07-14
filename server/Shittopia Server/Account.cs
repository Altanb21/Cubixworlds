
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Shittopia_Server
{
    internal class Account
    {
        public int id;

        public string username { get; set; }

        public string bio { get; set; } = "Hello, I'm a player!";

        public string[] ownedWorlds { get; set; }

        public int xp { get; set; }

        public int Xp
        {
            get => this.xp;
            set
            {
                int level = this.level;
                this.xp = value;
                this.level = (int)Math.Floor((double)this.xp / 100.0);
                if (this.level == level || !Server.clients.ContainsKey(this.id) || Server.clients[this.id].player == null)
                    return;
                ChatManager.SendMessageToClient(this.id, string.Format("You have leveled up to {0}{1}</color>!", (object)ChatManager.coloredTexts["green"], (object)this.level), true, string.Format("I have leveled up to {0}{1}</color>!", (object)ChatManager.coloredTexts["green"], (object)this.level));
                ServerSend.UpdateClientData(this.id);
            }
        }

        public int level { get; set; }

        public long cubix { get; set; }

        public int accountLevel { get; set; }

        public string password { get; set; }

        public string email { get; set; }

        public Inventory inventory { get; set; }

        public string[] friendList { get; set; }

        public bool isMuted { get; set; }

        public DateTime muteExpireTime { get; set; }

        public int[] clothes { get; set; } = new int[8];

        public void SetCubix(int _cubixCount)
        {
            this.cubix = (long)_cubixCount;
            ServerSend.SendCubixCount(this.id);
        }

        public void AddCubix(int _cubixCount)
        {
            this.cubix += (long)_cubixCount;
            ServerSend.SendCubixCount(this.id);
        }

        public void RemoveCubix(int _cubixCount)
        {
            this.cubix -= (long)_cubixCount;
            ServerSend.SendCubixCount(this.id);
        }

        public void AddOwnedWorld(string _worldName)
        {
            List<string> stringList = this.ownedWorlds == null ? new List<string>() : ((IEnumerable<string>)this.ownedWorlds).ToList<string>();
            stringList.Add(_worldName);
            this.ownedWorlds = new string[stringList.Count];
            this.ownedWorlds = stringList.ToArray();
            this.Save();
        }

        public void RemoveOwnedWorld(string _worldName)
        {
            List<string> stringList = this.ownedWorlds == null ? new List<string>() : ((IEnumerable<string>)this.ownedWorlds).ToList<string>();
            if (stringList.Contains(_worldName))
                stringList.Remove(_worldName);
            this.ownedWorlds = new string[stringList.Count];
            this.ownedWorlds = stringList.ToArray();
            this.Save();
        }

        public void AddFriend(string _friendName)
        {
            List<string> stringList = this.friendList == null ? new List<string>() : ((IEnumerable<string>)this.friendList).ToList<string>();
            stringList.Add(_friendName);
            this.friendList = new string[stringList.Count];
            this.friendList = stringList.ToArray();
            this.Save();
        }

        public void RemoveFriend(string _friendName)
        {
            List<string> stringList = this.friendList == null ? new List<string>() : ((IEnumerable<string>)this.friendList).ToList<string>();
            if (stringList.Contains(_friendName))
                stringList.Remove(_friendName);
            this.friendList = new string[stringList.Count];
            this.friendList = stringList.ToArray();
            this.Save();
        }

        public void Mute(int _minutes)
        {
            this.isMuted = true;
            this.muteExpireTime = DateTime.Now.AddMinutes((double)_minutes);
            this.Save();
        }

        public void Unmute()
        {
            this.isMuted = false;
            this.Save();
        }

        public bool CheckForMute()
        {
            if (DateTime.Now >= this.muteExpireTime)
                this.isMuted = false;
            this.Save();
            return this.isMuted;
        }

        public void Save()
        {
            try
            {
                string contents = JsonSerializer.Serialize<Account>(this);
                File.WriteAllText(Server.path + "\\Accounts\\" + this.username, contents);
            }
            catch (Exception ex)
            {
                Server.Log("An error ocurred", ex.ToString());
            }
        }
    }
}
