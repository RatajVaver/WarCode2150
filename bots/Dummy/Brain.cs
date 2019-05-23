using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PCBClient
{
    class Brain
    {
        public Dictionary<int,Enemy> enemies = new Dictionary<int,Enemy>();
        public int x;
        public int y;

        public void Start(int x, int y)
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

        public void Move(int x, int y)
        {
            Client.SendMessage("MOVE;" + x + ";" + y);
        }

        public void Shoot(int dir)
        {
            Client.SendMessage("SHOOT;" + dir);
        }

        public void RandomMove()
        {
            Random rnd = new Random();
            Move(x + rnd.Next(-1, 2), y + rnd.Next(-1, 2));
        }

        public void UpdateEnemy(int id, int x, int y)
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
