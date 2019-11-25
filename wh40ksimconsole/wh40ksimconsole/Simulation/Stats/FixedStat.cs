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
        int maxvalue;
        int value;

        public FixedStat(int value)
        {
            this.value = this.maxvalue = value;
        }

        public int get()
        {
            return value;
        }

        public void reset()
        {
            this.value = this.maxvalue;
        }
    }
}
