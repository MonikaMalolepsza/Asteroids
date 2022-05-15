using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Model
{
    public class SpaceShip
    {
        private int _x;
        private int _y;
        private int _dx;
        private int _dy;
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        public int Dx { get => _dx; set => _dx = value; }
        public int Dy { get => _dy; set => _dy = value; }

        public SpaceShip()
        {
            Dx = 0;
            Dy = 0;
            X = 0;
            Y = 0;
        }

        public SpaceShip(int dx, int dy, int x, int y)
        {
            Dx = dx;
            Dy = dy;
            X = x;
            Y = y;
        }
    }
}