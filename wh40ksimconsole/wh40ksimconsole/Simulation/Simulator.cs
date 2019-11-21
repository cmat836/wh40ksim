using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation
{
    class Simulator
    {
        Dice dice;

        public Simulator()
        {
            dice = new Dice();
        }

        public void Simulate()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(dice.rollD6());
            }
        }
    }
}
