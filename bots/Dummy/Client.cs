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
        //private TcpClient _client;
        private static Socket _client;
        private Brain _brain;
        private int _myId;
        //private static StreamReader _sReader;
        //private static StreamWriter _sWriter;
        private bool _isConnected;

        public Client(String ipAddress, int portNum, Brain brain)
        {
            //_client = new TcpClient();
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _client.Connect(ipAddress, portNum);
            _brain = brain;
            _myId = -1;
            HandleCommunication();
        }

        public void HandleCommunication()
        {
            //_sReader = new StreamReader(_client.GetStream(), Encoding.UTF8);
            //_sWriter = new StreamWriter(_client.GetStream(), Encoding.UTF8);

            byte[] data = null;
            _isConnected = true;

            while (_isConnected)
            {
                //string data = _sReader.ReadLine();
                data = new byte[WC2150Shared.Data.BUFFER_SIZE];
                int bytes = _client.Receive(data);
                //Console.WriteLine("SERVER > " + data);
                Console.WriteLine("SERVER > " + data[0].ToString());

                HandleMessage(data);

                /*
                string[] args = data.Split(';');
                if (args.Length > 0)
                {
                    if(args[0] == "WELCOME")
                    {
                        Int32.TryParse(args[1], out int id);
                        _myId = id;
                    }
                    else if (args[0] == "START")
                    {
                        Int32.TryParse(args[1], out int x);
                        Int32.TryParse(args[2], out int y);
                        _brain.Start(x, y);
                    }
                    else if (args[0] == "MOVE")
                    {
                        Int32.TryParse(args[1], out int id);
                        Int32.TryParse(args[2], out int x);
                        Int32.TryParse(args[3], out int y);

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
                    else if (args[0] == "BAD_MOVE")
                    {
                        _brain.Think();
                    }
                    else if (args[0] == "SHOOT")
                    {
                        Int32.TryParse(args[1], out int id);
                        Int32.TryParse(args[2], out int dir);

                        if (id == _myId)
                        {
                            _brain.Think();
                        }
                    }
                    else if (args[0] == "DEATH")
                    {
                        Int32.TryParse(args[1], out int id);

                        if (id != _myId)
                        {
                            _brain.enemies[id].hp = 0;
                            _brain.Think();
                        }
                    }
                }
                */
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
                    _brain.enemies[id].hp = 0;
                    _brain.Think();
                }
            }

        }

        //public static void SendMessage(string data)
        public static void SendMessage(byte[] data)
        {
            //Console.WriteLine("CLIENT > " + data);
            Console.WriteLine("CLIENT > " + data[0].ToString());
            _client.Send(data);
            //_sWriter.WriteLine(data);
            //_sWriter.Flush();
        }
    }
}
