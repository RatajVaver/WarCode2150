using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dummy
{
    class Enemy
    {
        public ushort x;
        public ushort y;
        public byte hp;

        public Enemy(ushort x, ushort y)
        {
            this.x = x;
            this.y = y;
            this.hp = 100;
        }
    }
}
