using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var controller = new AsteroidsController();

            bool isRunning = true;

            while (isRunning)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.W:
                        controller.SendCommand(4);
                        break;

                    case ConsoleKey.A:
                        controller.SendCommand(1);
                        break;

                    case ConsoleKey.D:
                        controller.SendCommand(2);
                        break;

                    case ConsoleKey.P:
                        controller.SendCommand(3);
                        break;

                    case ConsoleKey.Spacebar:
                        controller.SendCommand(5);
                        break;

                    case ConsoleKey.Escape:
                        isRunning = false;
                        break;
                }
            }
        }
    }
}