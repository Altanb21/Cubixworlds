
using System.Numerics;
using System.Threading;

namespace Shittopia_Server
{
    internal class Block
    {
        private int Health;
        public int milisecondsToDie = 4000;
        public bool isIDChanged;
        public bool isChecking;

        public int id { get; set; }

        public int health
        {
            get => this.Health;
            set => this.Health = value;
        }

        public void CheckForTimeOut(Vector2 _position, int _layer, int _clientID)
        {
            this.isChecking = true;
            if (this.isIDChanged)
            {
                this.milisecondsToDie = 4000;
                this.isIDChanged = false;
                this.isChecking = false;
            }
            else if (this.id == 0)
            {
                this.milisecondsToDie = 4000;
                this.isIDChanged = false;
                this.isChecking = false;
            }
            else
            {
                System.Threading.Thread.Sleep(100);
                this.milisecondsToDie -= 100;
                if (this.health != GameData.items[this.id].health && this.health != 0 && this.milisecondsToDie == 0)
                {
                    this.health = GameData.items[this.id].health;
                    this.milisecondsToDie = 4000;
                    ServerSend.EditWorldData(_clientID, this.id, _layer, this.health, _position);
                    this.isChecking = false;
                }
                else
                    this.CheckForTimeOut(_position, _layer, _clientID);
            }
        }
    }
}
