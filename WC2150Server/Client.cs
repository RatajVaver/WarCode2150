using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WC2150Server
{
    class Client
    {
        public byte id;
        public Socket socket;
        public ushort x;
        public ushort y;
        public byte hp;
        
        public Client(byte id, Socket socket)
        {
            this.id = id;
            this.socket = socket;
            this.hp = 100;
        }

        public void Spawn()
        {
            Random rand = new Random();
            x = (ushort)rand.Next(1, Arena.width + 1);
            y = (ushort)rand.Next(1, Arena.height + 1);
            SendMessage( new byte[]{ WC2150Shared.Data.START }.Concat(BitConverter.GetBytes(x)).Concat(BitConverter.GetBytes(y)).ToArray());
        }

        public void SendMessage(byte[] data)
        {
            socket.Send(data);
        }
    }
}
