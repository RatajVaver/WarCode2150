using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC2150Server
{
    class Projectile
    {
        public byte shooter;
        public ushort x;
        public ushort y;
        public byte dir;
        public bool remove = false;

        public Projectile(byte shooter, ushort x, ushort y, byte dir)
        {
            this.shooter = shooter;
            this.x = x;
            this.y = y;
            this.dir = dir;
        }

        public void Move()
        {
            if (dir == 1) y--;
            if (dir == 2) x++;
            if (dir == 3) y++;
            if (dir == 4) x--;

            bool hit = false;

            foreach (KeyValuePair<byte, Client> client in Server.clients)
            {
                if(x == client.Value.x && y == client.Value.y)
                {
                    client.Value.hp = 0;
                    Server.SendToAll( new byte[] { WC2150Shared.Data.DEATH }.Concat(BitConverter.GetBytes(client.Key)).ToArray() );
                    hit = true;
                    break;
                }
            }

            if (hit)
                remove = true;
        }
    }
}
