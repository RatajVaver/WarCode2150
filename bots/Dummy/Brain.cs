using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dummy
{
    class Brain
    {
        public Dictionary<int,Enemy> enemies = new Dictionary<int,Enemy>();
        public ushort x;
        public ushort y;

        public void Start(ushort x, ushort y)
        {
            this.x = x;
            this.y = y;

            Think();
        }

        public void Think()
        {
            Thread.Sleep(1000);

            bool shot = false;

            foreach (KeyValuePair<int, Enemy> enemy in enemies)
            {
                if (enemy.Value.hp > 0)
                {
                    if (enemy.Value.x == this.x && enemy.Value.y < this.y)
                    {
                        Shoot(1);
                        shot = true;
                    }
                    else if (enemy.Value.x == this.x && enemy.Value.y > this.y)
                    {
                        Shoot(3);
                        shot = true;
                    }
                    else if (enemy.Value.y == this.y && enemy.Value.x < this.x)
                    {
                        Shoot(4);
                        shot = true;
                    }
                    else if (enemy.Value.y == this.y && enemy.Value.x > this.x)
                    {
                        Shoot(2);
                        shot = true;
                    }
                }

                if(shot) break;
            }

            if(!shot)
                RandomMove();
        }

        public void Move(ushort x, ushort y)
        {
            //Client.SendMessage("MOVE;" + x + ";" + y);
            Console.WriteLine("Moving to: " + x + ", " + y);
            Client.SendMessage( new byte[] { WC2150Shared.Data.MOVE }.Concat(BitConverter.GetBytes(x)).Concat(BitConverter.GetBytes(y)).ToArray() );
        }

        public void Shoot(byte dir)
        {
            //Client.SendMessage("SHOOT;" + dir);
            Client.SendMessage(new byte[] { WC2150Shared.Data.SHOOT }.Concat(BitConverter.GetBytes(dir)).ToArray());
        }

        public void RandomMove()
        {
            Random rnd = new Random();
            Move((ushort)(x + rnd.Next(-1, 2)), (ushort)(y + rnd.Next(-1, 2)));
        }

        public void UpdateEnemy(byte id, ushort x, ushort y)
        {
            if (enemies.ContainsKey(id))
            {
                enemies[id].x = x;
                enemies[id].y = y;
            }
            else
            {
                enemies[id] = new Enemy(x, y);
            }
        }
    }
}
