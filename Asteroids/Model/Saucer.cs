using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Model
{
    public class Saucer
    {
        private int _x;
        private int _y;
        private int _size;
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        public int Size { get => _size; set => _size = value; }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Size: {Size}";
        }

        public Saucer()
        {
            X = 0;
            Y = 0;
            Size = 0;
        }

        public Saucer(int x, int y, int size)
        {
            X = x;
            Y = y;
            Size = size;
        }
    }
}