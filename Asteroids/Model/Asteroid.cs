using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Model
{
    public class Asteroid
    {
        private int _type;
        private int _scaleFactor;
        private int _x;
        private int _y;

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, ScaleFactor: {ScaleFactor}, Typ:{Type}";
        }

        public Asteroid()
        {
            Type = 0;
            ScaleFactor = 0;
            X = 0;
            Y = 0;
        }

        public Asteroid(int type, int scaleFactor, int x, int y)
        {
            Type = type;
            ScaleFactor = scaleFactor;
            X = x;
            Y = y;
        }

        public int Type { get => _type; set => _type = value; }
        public int ScaleFactor { get => _scaleFactor; set => _scaleFactor = value; }
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
    }
}