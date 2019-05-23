using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WC2150Server
{
    class Arena
    {
        public static ushort width = 64;
        public static ushort height = 16;
        public static int projectilesCount = 0;
        public static Dictionary<int, Projectile> projectiles = new Dictionary<int, Projectile>();

        public static void GameLogic()
        {
            while (true){

                for(int i = 1; i <= projectilesCount; i++)
                {
                    if (projectiles.ContainsKey(i))
                    {
                        projectiles[i].Move();

                        if (projectiles[i].remove)
                        {
                            projectiles.Remove(i);
                        }
                    }
                }

                DrawArena();

                Thread.Sleep(500);
            }
        }

        public static void DrawArena()
        {
            lock (Program.ConsoleLock)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(new string('#', width+2));

                for (int y = 1; y <= height; y++)
                {
                    Console.Write("#");

                    for (int x = 1; x <= width; x++)
                    {
                        Console.Write(CheckCoords(x, y));
                    }

                    Console.WriteLine("#");
                }

                Console.WriteLine(new string('#', width+2));
            }
        }

        public static char CheckCoords(int x, int y)
        {
            foreach (KeyValuePair<byte, Client> client in Server.clients)
            {
                if(client.Value.x == x && client.Value.y == y)
                {
                    if (client.Value.hp <= 0)
                    {
                        return 'X';
                    }
                    else
                    {
                        return Convert.ToChar(client.Key.ToString()[0]);
                    }
                }
            }

            foreach (KeyValuePair<int, Projectile> projectile in projectiles)
            {
                if(projectile.Value.x == x && projectile.Value.y == y)
                {
                    return '*';
                }
            }

            return '-';
        }
    }
}
