using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation.Stats
{
    class VariStat : Stat
    {
        int statLevelUpper;
        int statLevelMid;
        int statLevelLower; // Some stats change as the model is damaged

        public Model parent;

        public VariStat(int levelUpper, int levelMid, int levelLower, Model parent)
        {
            statLevelUpper = levelUpper;
            statLevelMid = levelMid;
            statLevelLower = levelLower;
            this.parent = parent;
        }

        public int get()
        {
            if (!(parent.wounds is WoundsStat))
            {
                return statLevelUpper;
            }
            switch (((WoundsStat)parent.wounds).getWounded())
            {
                case WoundsStat.WoundRange.UPPER:
                    return statLevelUpper;                  
                case WoundsStat.WoundRange.MID:
                    return statLevelMid;
                case WoundsStat.WoundRange.LOWER:
                    return statLevelLower;
                default:
                    return statLevelUpper;
            }
        }

    }
}
