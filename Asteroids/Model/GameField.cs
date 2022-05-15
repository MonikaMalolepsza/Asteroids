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

        public GameField()
        {
            Asteroids = new List<Asteroid>();
            Shots = new List<Shot>();
            Saucer = null;
            SpaceShip = null;
        }

        public
    }
}