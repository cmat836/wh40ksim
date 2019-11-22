using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation.Stats
{
    // A fixed stat that should not change
    class FixedStat : Stat 
    {
        int value;

        public FixedStat(int value)
        {
            this.value = value;
        }

        public int get()
        {
            return value;
        }
    }
}
