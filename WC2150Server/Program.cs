using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WC2150Server
{
    class Program
    {
        public static readonly object ConsoleLock = new object();

        static void Main(string[] args)
        {
            Thread t = new Thread(Arena.GameLogic);
            t.Start();

            Server server = new Server(28001);
        }

        public static void LastMessage(string message)
        {
            lock (Program.ConsoleLock)
            {
                Console.SetCursorPosition(0, Arena.height + 3);
                Console.WriteLine(message);
                Console.SetCursorPosition(message.Length, Arena.height + 3);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }
    }
}
