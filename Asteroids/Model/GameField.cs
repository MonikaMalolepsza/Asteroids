using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Model
{
    public class GameField
    {
        private List<Asteroid> _asteroids;
        private List<Shot> _shots;
        private Saucer _saucer;
        private SpaceShip _spaceShip;
        public List<Asteroid> Asteroids { get => _asteroids; set => _asteroids = value; }
        public List<Shot> Shots { get => _shots; set => _shots = value; }
        public Saucer Saucer { get => _saucer; set => _saucer = value; }
        public SpaceShip SpaceShip { get => _spaceShip; set => _spaceShip = value; }

        public bool IsEmpty()
        {
            return Asteroids.Count == 0 && Shots.Count == 0 && Saucer == null && SpaceShip == null;
        }

        public GameField()
        {
            Asteroids = new List<Asteroid>();
            Shots = new List<Shot>();
            Saucer = null;
            SpaceShip = null;
        }

        private double GetDistanceBetweenVectors(int x1, int x2, int y1, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public override string ToString()
        {
            if (SpaceShip != null)
            {
                string saucer = "";
                if (Saucer != null)
                    saucer = Saucer.ToString() + $" Distance: {GetDistanceBetweenVectors(SpaceShip.X, Saucer.X, SpaceShip.Y, Saucer.Y)}";
                return "Field:"
                    + "\r\n\tAsteroids:\r\n\t\t" + string.Join("\r\n\t\t", Asteroids.Select(x => x.ToString() + $" Distance: {GetDistanceBetweenVectors(SpaceShip.X, x.X, SpaceShip.Y, x.Y)}"))
                    + "\r\n\tShots:\r\n\t\t" + string.Join("\r\n\t\t", Shots.Select(x => x.ToString() + $" Distance: {GetDistanceBetweenVectors(SpaceShip.X, x.X, SpaceShip.Y, x.Y)}"))
                    + "\r\n\tSaucer:\r\n\t\t" + saucer
                    + "\r\n\tSpaceShip:\r\n\t\t" + SpaceShip.ToString();
            }
            return "";
        }
    }
}