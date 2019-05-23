using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Dummy
{
    class Client
    {
        private static Socket _client;
        private Brain _brain;
        private int _myId;
        private bool _isConnected;

        public Client(String ipAddress, int portNum, Brain brain)
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _client.Connect(ipAddress, portNum);
            _brain = brain;
            _myId = -1;
            HandleCommunication();
        }

        public void HandleCommunication()
        {
            byte[] data = null;
            _isConnected = true;

            while (_isConnected)
            {
                data = new byte[WC2150Shared.Data.BUFFER_SIZE];
                int bytes = _client.Receive(data);
                Console.WriteLine("SERVER > " + data[0].ToString());

                HandleMessage(data);
            }
        }

        public void HandleMessage(byte[] data)
        {

            if (data[0] == WC2150Shared.Data.WELCOME) {
                byte id = data[1];
                _myId = id;
                Console.WriteLine("ID: " + id);
            }

            if (data[0] == WC2150Shared.Data.START) {
                ushort x = BitConverter.ToUInt16(data, 1);
                ushort y = BitConverter.ToUInt16(data, 3);
                Console.WriteLine("Spawn: " + x + ", " + y);
                _brain.Start(x, y);
            }

            if (data[0] == WC2150Shared.Data.MOVE) {
                byte id = data[1];
                // TODO: bugfix - byte data[2] je z nějakýho důvodu vždy 0
                ushort x = BitConverter.ToUInt16(data, 3);
                ushort y = BitConverter.ToUInt16(data, 5);

                if (id == _myId)
                {
                    _brain.x = x;
                    _brain.y = y;
                    _brain.Think();
                }
                else
                {
                    _brain.UpdateEnemy(id, x, y);
                }
            }

            if (data[0] == WC2150Shared.Data.BAD_MOVE) {
                _brain.Think();
            }

            if (data[0] == WC2150Shared.Data.SHOOT) {
                byte id = data[1];
                byte dir = data[2];

                if (id == _myId)
                {
                    _brain.Think();
                }
            }

            if (data[0] == WC2150Shared.Data.DEATH) {
                byte id = data[1];

                if (id != _myId)
                {
                    if (_brain.enemies.ContainsKey(id))
                    {
                        _brain.enemies[id].hp = 0;
                    }

                    _brain.Think();
                }
            }

        }

        public static void SendMessage(byte[] data)
        {
            Console.WriteLine("CLIENT > " + data[0].ToString());
            _client.Send(data);
        }
    }
}
