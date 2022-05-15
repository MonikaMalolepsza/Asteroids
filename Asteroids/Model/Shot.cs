using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Model
{
    public class Shot
    {
        private int _x;
        private int _y;
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }

        public Shot()
        {
            X = 0;
            Y = 0;
        }

        public Shot(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}