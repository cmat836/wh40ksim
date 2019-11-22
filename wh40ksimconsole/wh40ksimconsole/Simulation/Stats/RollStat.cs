using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation.Stats
{
    class RollStat : Stat
    {
        int numberofdice; // Only handles 1 d3
        bool d6; // is the dice a d6, otherwise it will be a d3

        public RollStat(bool d6, int dicenumber)
        {
            numberofdice = dicenumber;
            this.d6 = d6;
        }

        public int get()
        {
            return d6 ? Simulator.instance.dice.rollD6(numberofdice) : Simulator.instance.dice.rollD3();
        }
    }
}
