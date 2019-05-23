using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WC2150Server
{
    class Server
    {
        private Socket _server;
        private bool _isRunning;
        private byte _lastId = 0;
        public static Dictionary<byte, Client> clients = new Dictionary<byte, Client>();

        public Server(int port)
        {
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.Bind(new IPEndPoint(IPAddress.Any, port));
            _server.Listen(255);
            _isRunning = true;

            AcceptClients();
        }

        public void AcceptClients()
        {
            while (_isRunning)
            {
                Socket newClient = _server.Accept();
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient);
            }
        }

        public void HandleClient(object obj)
        {
            Socket client = (Socket)obj;

            byte id = ++_lastId;
            clients[id] = new Client(id, client);

            bool bClientConnected = true;
            byte[] data = null;

            SendToId( id, new byte[]{ WC2150Shared.Data.WELCOME }.Concat(BitConverter.GetBytes(id)).ToArray() );

            clients[id].Spawn();

            while (bClientConnected)
            {
                try
                {
                    data = new byte[WC2150Shared.Data.BUFFER_SIZE];
                    int bytes = client.Receive(data);
                    byte type = data[0];

                    HandleMessage(id, type, data);

                    Program.LastMessage("BOT #" + id + " > " + type.ToString());

                    Thread.Sleep(1000);
                }
                catch
                {
                    bClientConnected = false;
                }
            }

            client.Close();
            clients.Remove(id);
        }

        public void HandleMessage(byte id, byte type, byte[] data)
        {
            if(clients[id].hp <= 0)
            {
                SendToId( id, new byte[]{ WC2150Shared.Data.YOU_DEAD } );
            }

            switch(type){
                case WC2150Shared.Data.MOVE:
                    ushort x = BitConverter.ToUInt16(data, 1);
                    ushort y = BitConverter.ToUInt16(data, 3);
                    HandleMove(id, x, y);
                    break;
                case WC2150Shared.Data.SHOOT:
                    byte dir = data[1];
                    HandleShoot(id, dir);
                    break;
            }
        }

        public void HandleMove(byte id, ushort x, ushort y)
        {
            if (Math.Abs(clients[id].x - x) <= 1 && Math.Abs(clients[id].y - y) <= 1)
            {
                if (x >= 1 && x <= Arena.width && y >= 1 && y <= Arena.height)
                {
                    clients[id].x = x;
                    clients[id].y = y;
                    SendToAll( new byte[] { WC2150Shared.Data.MOVE }.Concat(BitConverter.GetBytes(id)).Concat(BitConverter.GetBytes(x)).Concat(BitConverter.GetBytes(y)).ToArray() );
                }
                else
                {
                    SendToId( id, new byte[] { WC2150Shared.Data.BAD_MOVE } );
                }
            }
            else
            {
                SendToId( id, new byte[] { WC2150Shared.Data.BAD_MOVE } );
            }
        }

        public void HandleShoot(byte id, byte dir)
        {
            if(dir >= 1 && dir <= 4)
            {
                Projectile _projectile = new Projectile(id, clients[id].x, clients[id].y, dir);
                Arena.projectiles.Add(++Arena.projectilesCount, _projectile);
                SendToAll( new byte[] { WC2150Shared.Data.SHOOT }.Concat(BitConverter.GetBytes(id)).Concat(BitConverter.GetBytes(dir)).ToArray() );
            }
        }

        public static void SendToId(byte id, byte[] data)
        {
            if (clients.ContainsKey(id))
            {
                Program.LastMessage("BOT #" + id + " < " + data[0].ToString());
                clients[id].SendMessage(data);
            }
        }

        public static void SendToAll(byte[] data)
        {
            foreach (KeyValuePair<byte, Client> client in clients)
            {
                SendToId(client.Key, data);
            }
        }
    }
}
