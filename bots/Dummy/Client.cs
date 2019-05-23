using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PCBClient
{
    class Client
    {
        private TcpClient _client;
        private Brain _brain;
        private int _myId;
        private static StreamReader _sReader;
        private static StreamWriter _sWriter;
        private bool _isConnected;

        public Client(String ipAddress, int portNum, Brain brain)
        {
            _client = new TcpClient();
            _client.Connect(ipAddress, portNum);
            _brain = brain;
            _myId = -1;
            HandleCommunication();
        }

        public void HandleCommunication()
        {
            _sReader = new StreamReader(_client.GetStream(), Encoding.UTF8);
            _sWriter = new StreamWriter(_client.GetStream(), Encoding.UTF8);

            _isConnected = true;
            while (_isConnected)
            {
                string data = _sReader.ReadLine();
                Console.WriteLine("SERVER < " + data);

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
            }
        }

        public static void SendMessage(string data)
        {
            Console.WriteLine("SERVER > " + data);
            _sWriter.WriteLine(data);
            _sWriter.Flush();
        }
    }
}
