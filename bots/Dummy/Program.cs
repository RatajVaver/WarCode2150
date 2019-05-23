using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dummy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("IP: ");
            string ip = Console.ReadLine();
            Brain brain = new Brain();
            Client client = new Client(ip, 28001, brain);
        }
    }
}
