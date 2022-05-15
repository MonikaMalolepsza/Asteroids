using Asteroids.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Asteroids
{
    public class AsteroidsController
    {
        private readonly byte[] _command = { (byte)'c', (byte)'t', (byte)'m', (byte)'a', (byte)'m', (byte)'e', (byte)'@', (byte)'0' };
        private string _adress;
        private int _port;
        private string example1 = "01E210A22DF300800000007000F0D1A293F20080000029C9A8A052E30090000080430005204780C64044E0C62065F8C2B062B0C5E04640C090A0460100700000FFC802A34601007000000DC96DA192F0008000000DC9B5A1D7F1008000001AC936A27CF200800000FFC8A8A2FFF2008000001AC952C86CA36410007000002CCB2CCB2CCB32CBDDCA54A3A0E06DCA6DCA6CA3E001005000002CCB2CCB2ECB41CBDDCA6CA3001300500000FCA1FC11B0B0001300500000FCA1FC11B0B0B0B000500000FCA1FC11B0B0001300500000FCA1FC11B0B0FC11B0B0FC11B0B0FC11B0B0F3CA08CB9BCA8DCAC7CA9BCAD8CA48A230100070000013CB9BCAD8CAD8CA2CCB80CA08CB8DCAB3CAFBCA02CB78CA80CA9BCA2CCBDDCAC7CA2CCBB3CA1FCBE3CA9BCAF3CAFBCAE3CA78CA8DCA9BCA2CCB93CAF3CA08CB9BCA8DCAC7CA9BCAD8CAE4A090210070000078CA72F801F872F801F852C86CA36410007000002CCB2CCB2ECB41CBDDCA6CA3E001005000002CCB2CCB2ECB41CBDDCA6CA30013005000002CCB2CCB2CCBDDCADDCAFCA1FC11B0B0DDCA6CA30013005000002CCB2CCB2CCBDDCADDCAFCA1FC11B0B0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002930";

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

        private GameField LoadField(byte[] ram)
        {
            int gsf = 0;
            int labsX = 0;
            int labsY = 0;
            int vctrX = 0;
            int vctrY = 0;
            int vctrZ = 0;
            int v1x = 0;
            int v1y = 0;
            GameField result = new GameField();
            for (int i = 2; i < ram.Length - 2; i += 2)
            {
                int[] res = new int[1];
                RightShift(new BitArray(new byte[] { ram[i], ram[i + 1] }), 12).CopyTo(res, 0);
                int operation = res[0];
                switch (operation)
                {
                    case 10: //LABS
                        //Console.WriteLine("LABS");
                        BitArray tmpLabs = new BitArray(new byte[] { ram[i], ram[i + 1] });
                        tmpLabs.And(new BitArray(new bool[] { true, true, true, true, true, true, true, true, true, true, false, false, false, false, false, false }));
                        tmpLabs.CopyTo(res, 0);
                        labsY = res[0];
                        tmpLabs = new BitArray(new byte[] { ram[i + 2], ram[i + 3] });
                        tmpLabs.And(new BitArray(new bool[] { true, true, true, true, true, true, true, true, true, true, false, false, false, false, false, false }));
                        tmpLabs.CopyTo(res, 0);
                        labsX = res[0];
                        RightShift(new BitArray(new byte[] { ram[i + 2], ram[i + 3] }), 12).CopyTo(res, 0);
                        gsf = res[0];
                        i += 2;
                        break;

                    case 11: //HALT
                        //Console.WriteLine("HALT");
                        break;

                    case 12: //JSRL
                        //Console.WriteLine("JSRL");
                        new BitArray(new byte[] { ram[i], ram[i + 1] }).And(new BitArray(new bool[] { true, true, true, true, true, true, true, true, true, true, true, true, false, false, false, false })).CopyTo(res, 0);
                        if (res[0] == 0x8F3)
                            result.Asteroids.Add(new Asteroid(1, gsf, labsX, labsY));
                        else if (res[0] == 0x8FF)
                            result.Asteroids.Add(new Asteroid(2, gsf, labsX, labsY));
                        else if (res[0] == 0x90D)
                            result.Asteroids.Add(new Asteroid(3, gsf, labsX, labsY));
                        else if (res[0] == 0x91A)
                            result.Asteroids.Add(new Asteroid(4, gsf, labsX, labsY));
                        else if (res[0] == 0x929)
                            result.Saucer = new Saucer(labsX, labsY, gsf);
                        break;

                    case 13: //RTSL
                        //Console.WriteLine("RTSL");
                        break;

                    case 14: //JMPL
                        //Console.WriteLine("JMPL");
                        break;

                    case 15: //SVEC
                        //Console.WriteLine("SVEC");
                        break;

                    default: //VCTR
                        //Console.WriteLine("VCTR");
                        BitArray tmpVctr = new BitArray(new byte[] { ram[i], ram[i + 1] });
                        tmpVctr.And(new BitArray(new bool[] { true, true, true, true, true, true, true, true, true, true, false, false, false, false, false, false })).CopyTo(res, 0);
                        vctrY = res[0];

                        tmpVctr = new BitArray(new byte[] { ram[i + 2], ram[i + 3] });
                        tmpVctr.And(new BitArray(new bool[] { true, true, true, true, true, true, true, true, true, true, false, false, false, false, false, false })).CopyTo(res, 0);
                        vctrX = res[0];

                        tmpVctr = new BitArray(new byte[] { ram[i + 2], ram[i + 3] });
                        RightShift(tmpVctr, 12).CopyTo(res, 0);
                        vctrZ = res[0];

                        vctrY = new BitArray(new byte[] { ram[i], ram[i + 1] }).Get(10) ? vctrY * -1 : vctrY;
                        vctrX = new BitArray(new byte[] { ram[i + 2], ram[i + 3] }).Get(10) ? vctrX * -1 : vctrX;

                        switch (operation + gsf)
                        {
                            case 0:
                                vctrY = vctrY / 512;
                                vctrX = vctrX / 512;
                                break;

                            case 1:
                                vctrY = vctrY / 256;
                                vctrX = vctrX / 256;
                                break;

                            case 2:
                                vctrY = vctrY / 128;
                                vctrX = vctrX / 128;
                                break;

                            case 3:
                                vctrY = vctrY / 64;
                                vctrX = vctrX / 64;
                                break;

                            case 4:
                                vctrY = vctrY / 32;
                                vctrX = vctrX / 32;
                                break;

                            case 5:
                                vctrY = vctrY / 16;
                                vctrX = vctrX / 16;
                                break;

                            case 6:
                                vctrY = vctrY / 8;
                                vctrX = vctrX / 8;
                                break;

                            case 7:
                                vctrY = vctrY / 4;
                                vctrX = vctrX / 4;
                                break;

                            case 8:
                                vctrY = vctrY / 2;
                                vctrX = vctrX / 2;
                                break;

                            case 9:
                                vctrY = vctrY / 1;
                                vctrX = vctrX / 1;
                                break;
                        }

                        //Shot
                        if (vctrX == 0 && vctrY == 0 && vctrZ == 15)
                        {
                            result.Shots.Add(new Shot(labsX, labsY));
                        }
                        //Ship
                        else if (operation == 6 && vctrZ == 12 && vctrX != 0 && vctrY != 0)
                        {
                            if (v1x == 0 && v1y == 0)
                            {
                                v1x = vctrX;
                                v1y = vctrY;
                            }
                            else
                            {
                                result.SpaceShip = new SpaceShip(v1x - vctrX, v1y - vctrY, labsX, labsY);
                            }
                        }
                        i += 2;
                        break;
                }
            }
            return result;
        }

        private double GetDistanceBetweenVectors(int x1, int x2, int y1, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        private int AnalyseField(GameField gf)
        {
            double distNearestObject;
            double dirXNearestObject;
            double dirYNearestObject;

            var nearestAsteroid = gf.Asteroids.FirstOrDefault(y => GetDistanceBetweenVectors(gf.SpaceShip.X, y.X, gf.SpaceShip.Y, y.Y) == gf.Asteroids.Min(x => GetDistanceBetweenVectors(gf.SpaceShip.X, x.X, gf.SpaceShip.Y, x.Y)));
            if (nearestAsteroid == null && gf.Saucer == null)
            {
                return 0;
            }
            distNearestObject = GetDistanceBetweenVectors(gf.SpaceShip.X, nearestAsteroid.X, gf.SpaceShip.Y, nearestAsteroid.Y);
            dirXNearestObject = nearestAsteroid.X - gf.SpaceShip.X;
            dirYNearestObject = nearestAsteroid.Y - gf.SpaceShip.Y;

            switch (nearestAsteroid.Type)
            {
                case 1:
                    distNearestObject -= 10;
                    break;

                case 2:
                    distNearestObject -= 10*10;
                    break;

                case 3:
                    distNearestObject -= 15*15;
                    break;

                case 4:
                    distNearestObject -= 20*20;
                    break;
            }

            if (gf.Saucer != null)
            {
                var distSaucer = GetDistanceBetweenVectors(gf.SpaceShip.X, gf.Saucer.X, gf.SpaceShip.Y, gf.Saucer.Y);
                if (distSaucer < distNearestObject)
                {
                    distNearestObject = distSaucer;
                    dirXNearestObject = gf.Saucer.X - gf.SpaceShip.X;
                    dirYNearestObject = gf.Saucer.Y - gf.SpaceShip.Y;
                }
            }

            Vector spaceShipDirection = new Vector(gf.SpaceShip.Dx, gf.SpaceShip.Dy);
            Vector nearestAsteroidDirection = new Vector(dirXNearestObject, dirYNearestObject);

            // "Run for your life" mode
            if (distNearestObject <= 50)
            {
                return 4;
            } else if(distNearestObject>50&&distNearestObject<250)
            {
                if (Vector.Multiply(spaceShipDirection, nearestAsteroidDirection) > 0)
                {
                    return 1;
                }
                else if (Vector.Multiply(spaceShipDirection, nearestAsteroidDirection) < 0)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            } else
            {
                return 3;
            }

            /**
          EXAMPLE:
            Saucer:

            SpaceShip:
                    X: 108, Y: 236, DX: 440, DY:1472
            Field:
            Asteroids:
                    X: 736, Y: 211, ScaleFactor: 0, Typ:1 Distance: 628,4974144736
                    X: 608, Y: 354, ScaleFactor: 15, Typ:4 Distance: 513,735340423452
                    X: 353, Y: 186, ScaleFactor: 0, Typ:2 Distance: 250,049995001
                    X: 772, Y: 134, ScaleFactor: 15, Typ:1 Distance: 671,788657242737
                    X: 60, Y: 530, ScaleFactor: 15, Typ:4 Distance: 297,892598095354
                    X: 565, Y: 238, ScaleFactor: 15, Typ:2 Distance: 457,00437634666
            Shots:
             */

            //return 0;
        }

        public void Test()
        {
            bool IsRunning = true;
            //byte[] b = Enumerable.Range(0, example1.Length)
            //    .Where(x => x % 2 == 0)
            //    .Select(x => Convert.ToByte(example1.Substring(x, 2), 16))
            //    .ToArray();

            //GameField gf = LoadField(b);
            //Console.WriteLine(gf.ToString());
            using (UdpClient client = new UdpClient(_adress, _port))
            {
                while (IsRunning)
                {
                    client.Send(_command, 8);
                    var b = client.ReceiveAsync().Result.Buffer;
                    GameField gf = LoadField(b);
                    if (gf.SpaceShip != null)
                    {
                        int todo = AnalyseField(gf);
                        _command[6] = MapCommand(todo);
                        if (gf.IsEmpty())
                            IsRunning = false;
                        //Console.WriteLine(gf.ToString());
                    }
                }
            }
        }

        private BitArray LeftShift(BitArray b, int shiftSize)
        {
            BitArray res = new BitArray(b);
            int m = res.Length - shiftSize - 1;
            int n = m;
            while (n > 0)
            {
                res.Set(res.Length - 1 - (m - n), res.Get(n));
                n--;
            }
            for (int i = 0; i < shiftSize; i++)
            {
                res.Set(i, false);
            }
            return res;
        }

        private BitArray RightShift(BitArray b, int shiftSize)
        {
            BitArray res = new BitArray(b);
            int m = shiftSize;
            int n = m;
            for (int i = 0; i < b.Length - shiftSize; i++)
            {
                res.Set(i, b.Get(shiftSize + i));
            }
            for (int j = b.Length - 1; j >= b.Length - shiftSize; j--)
            {
                res.Set(j, false);
            }
            return res;
        }
    }
}