
using System;
using System.Numerics;
using System.Threading;

namespace Shittopia_Server
{
    internal class Player
    {
        public int id;
        public string username;
        public int animationID;
        public Vector2 position;
        public Vector2 size;
        public Vector2 collisionSize;
        public int health;
        public int damageTake;
        public bool isGrounded;
        public float maxY;
        public float triangleY;
        public bool isInWater;
        public bool isGround;

        public Player(int _id, string _username, Vector2 _spawnPosition, int _health)
        {
            this.id = _id;
            this.username = _username;
            this.position = _spawnPosition;
            this.size = new Vector2(1f, 1f);
            this.collisionSize = new Vector2(0.5f, 0.9f);
            this.health = _health;
            new Thread(new ThreadStart(this.UpdateDamage)).Start();
        }

        public void Update()
        {
        }

        public void UpdateDamage()
        {
            while (true)
            {
                do
                {
                    Thread.Sleep(1000);
                }
                while (this.damageTake == 0);
                this.SetHealth(this.health - this.damageTake);
            }
        }

        public void SetPosition(Vector2 _position)
        {
            this.position = _position;
            ServerSend.SetPosition(this.id);
        }

        public void SetHealth(int _health)
        {
            this.health = _health;
            if (this.health == 0)
            {
                this.health = 100;
                this.SetPosition(Server.clients[this.id].world.whiteDoorPosition);
            }
            ServerSend.SetHealth(this.id, this.health);
        }

        public void OnPlayerUnGrounded()
        {
            this.isGround = false;
            float num1 = this.CheckSolidBlocksUnderPlayer();
            int num2 = this.CheckBlocksOnPlayer();
            if (Server.clients[this.id].account.clothes[4] != 0)
            {
                this.maxY = (double)MathF.Abs(num1 - this.position.Y) <= 2.0 ? num1 + 8f : this.position.Y + 8f;
                this.triangleY = this.position.Y + 5.5f;
                if (num2 == -1 || num2 > 2)
                {
                    new Thread(new ParameterizedThreadStart(this.DeceaseYPos)).Start((object)1400);
                }
                else
                {
                    switch (num2)
                    {
                        case 1:
                            new Thread(new ParameterizedThreadStart(this.DeceaseYPos)).Start((object)500);
                            break;
                        case 2:
                            new Thread(new ParameterizedThreadStart(this.DeceaseYPos)).Start((object)800);
                            break;
                        case 3:
                            new Thread(new ParameterizedThreadStart(this.DeceaseYPos)).Start((object)900);
                            break;
                        case 5:
                            new Thread(new ParameterizedThreadStart(this.DeceaseYPos)).Start((object)1100);
                            break;
                        case 6:
                            new Thread(new ParameterizedThreadStart(this.DeceaseYPos)).Start((object)1100);
                            break;
                    }
                }
            }
            else
            {
                this.maxY = (double)MathF.Abs(num1 - this.position.Y) <= 2.0 ? num1 + 4.5f : this.position.Y + 4.5f;
                this.triangleY = this.position.Y + 3f;
                if (num2 == -1 || num2 > 2)
                    new Thread(new ParameterizedThreadStart(this.DeceaseYPos)).Start((object)700);
                else if (num2 == 1)
                {
                    new Thread(new ParameterizedThreadStart(this.DeceaseYPos)).Start((object)270);
                }
                else
                {
                    if (num2 != 2)
                        return;
                    new Thread(new ParameterizedThreadStart(this.DeceaseYPos)).Start((object)400);
                }
            }
        }

        public void DeceaseYPos(object _ms)
        {
            try
            {
                Thread.Sleep((int)_ms + 30);
                while (!this.isGround)
                {
                    Thread.Sleep(66);
                    this.maxY -= 0.25f;
                    if ((double)this.maxY < (double)this.position.Y && !this.isInWater)
                    {
                        this.SetPosition(new Vector2(this.position.X, this.maxY));
                        this.position = new Vector2(this.position.X, this.maxY);
                    }
                    if (Server.clients[this.id].world == null)
                        break;
                    if ((GameData.items[Server.clients[this.id].world.worldLayers[0][(int)Math.Round((double)this.position.X + 0.200000002980232), (int)Math.Round((double)this.position.Y - 0.550000011920929)].id].isSolid || GameData.items[Server.clients[this.id].world.worldLayers[0][(int)Math.Round((double)this.position.X - 0.200000002980232), (int)Math.Round((double)this.position.Y - 0.550000011920929)].id].isSolid) && Server.clients[this.id].player != null && !Server.clients[this.id].player.isGrounded)
                    {
                        Server.clients[this.id].player.isGrounded = true;
                        Server.clients[this.id].player.OnPlayerGrounded();
                    }
                }
            }
            catch
            {
            }
        }

        public float CheckSolidBlocksUnderPlayer()
        {
            int num = (int)MathF.Round(this.position.Y);
            for (int x = num; x > num - 10; --x)
            {
                if (GameData.items[Server.clients[this.id].world.worldLayers[0][(int)MathF.Round(this.position.X + 0.25f), (int)MathF.Round((float)x)].id].isSolid || GameData.items[Server.clients[this.id].world.worldLayers[0][(int)MathF.Round(this.position.X - 0.25f), (int)MathF.Round((float)x)].id].isSolid || GameData.items[Server.clients[this.id].world.worldLayers[0][(int)MathF.Round(this.position.X), (int)MathF.Round((float)x)].id].isSolid)
                    return MathF.Abs((float)x) + 0.5f;
            }
            return (float)num;
        }

        public int CheckBlocksOnPlayer()
        {
            int num1 = (int)MathF.Round(this.position.Y);
            int num2 = 0;
            for (int x = num1; x < num1 + 10; ++x)
            {
                if (GameData.items[Server.clients[this.id].world.worldLayers[0][(int)MathF.Round(this.position.X), (int)MathF.Round((float)x)].id].isSolid)
                    return num2;
                ++num2;
            }
            return -1;
        }

        public void OnPlayerGrounded()
        {
            this.maxY = float.MaxValue;
            this.isGround = true;
        }
    }
}
