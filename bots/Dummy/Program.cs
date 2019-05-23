using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Brain brain = new Brain();
            Client client = new Client("127.0.0.1", 28001, brain);
        }
    }
}
