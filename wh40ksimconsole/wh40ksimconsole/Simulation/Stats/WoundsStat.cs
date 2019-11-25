using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation.Stats
{
    class WoundsStat : Stat
    {
        int maxvalue;
        int value;
        int upperrange; // The minimum number of wounds to be in the upper range
        int midrange; // The minimum number of wounds to be in the mid range

        public WoundsStat(int value, int upperrange, int midrange)
        {
            this.maxvalue = this.value = value;
            this.upperrange = upperrange;
            this.midrange = midrange;
        }

        public WoundsStat(int value)
        {
            this.maxvalue = this.value = value;
            this.upperrange = this.value;
            this.midrange = this.value;

        }

        public int get()
        {
            return value;
        }

        public WoundRange getWounded()
        {
            return value >= upperrange ? WoundRange.UPPER : (value >= midrange ? WoundRange.MID : WoundRange.LOWER);
        }

        public void giveWounds(int value)
        {
            this.value += value;
            if (this.value > maxvalue) {
                this.value = maxvalue;
            }
        }

        public void removeWounds(int value)
        {
            this.value -= value;
            if (this.value < 0)
            {
                this.value = 0;
            }
        }

        public void reset()
        {
            this.value = this.maxvalue;
        }

        public enum WoundRange
        {
            UPPER,
            MID,
            LOWER
        }
    }
}
