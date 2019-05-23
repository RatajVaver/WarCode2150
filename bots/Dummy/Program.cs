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
            string ip = "127.0.0.1";

            if(args.Length > 0)
            {
                ip = args[0];
            }

            Brain brain = new Brain();
            Client client = new Client(ip, 28001, brain);
        }
    }
}
