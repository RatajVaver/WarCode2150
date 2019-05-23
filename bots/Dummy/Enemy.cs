using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBClient
{
    class Enemy
    {
        public int x;
        public int y;
        public int hp;

        public Enemy(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.hp = 100;
        }
    }
}
