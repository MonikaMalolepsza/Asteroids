using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Asteroids
{
    public class AsteroidsController
    {
        private readonly byte[] _command = { (byte)'c', (byte)'t', (byte)'m', (byte)'a', (byte)'m', (byte)'e', (byte)'@', (byte)'0' };
        private string _adress;
        private int _port;

        public AsteroidsController()
        {
            _adress = "127.0.0.1";
            _port = 1979;
        }

        public AsteroidsController(string adress, int port)
        {
            _adress = adress;
            _port = port;
        }

        public byte[] SendCommand(int cmd)
        {
            _command[6] = MapCommand(cmd);
            using (UdpClient client = new UdpClient(_adress, _port))
            {
                client.Send(_command, 8);
                if (cmd == 5 || cmd == 3)
                {
                    Thread.Sleep(10);
                    _command[6] = MapCommand(0);
                    client.Send(_command, 8);
                }

                var b = client.ReceiveAsync().Result.Buffer;
                return b;
            }
        }

        private byte MapCommand(int cmd)
        {
            switch (cmd)
            {
                case 0:
                    return (byte)0x40;

                case 1:
                    return (byte)0x50;

                case 2:
                    return (byte)0x48;

                case 3:
                    return (byte)0x42;

                case 4:
                    return (byte)0x44;

                case 5:
                    return (byte)0x41;
            }
            throw new Exception($"Invalid command {cmd}");
        }
    }
}